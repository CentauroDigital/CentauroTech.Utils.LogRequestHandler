using CentauroTech.Utils.Http;
using log4net;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CentauroTech.Utils.LogRequestHandler
{
    /// <summary>
    /// Request handler that log the request and response messages.
    /// </summary>
    public class RequestHandler : DelegatingHandler
    {
        private ILog _logger;

        private ILog Logger
        {
            get { return _logger ?? (_logger = LogManager.GetLogger(this.GetType())); }
            set { _logger = value; }
        }

        private Encoding DefaultEncoding
        {
            get
            {
                string encodingType = System.Configuration.ConfigurationManager.AppSettings["CentauroTech.Utils.LogRequestHandler.DefaultEncoding"];
                Encoding encoding;
                try
                {
                    encoding = Encoding.GetEncoding(encodingType);
                }
                catch
                {
                    encoding = Encoding.UTF8;
                }
                return encoding;
            }
        }

        /// <summary>
        /// Creates a new instance of the CentauroTech.Utils.LogRequestHandler.RequestHandler class.
        /// </summary>
        public RequestHandler() : base() { }

        /// <summary>
        /// Creates a new instance of the System.Net.Http.DelegatingHandler class with a specific inner handler.
        /// </summary>
        /// <param name="httpMessageHandler">The inner handler which is responsible for processing the HTTP response messages.</param>
        public RequestHandler(HttpMessageHandler httpMessageHandler) : base(httpMessageHandler)
        {
            InnerHandler = httpMessageHandler;
        }

        /// <summary>
        /// Creates a new instance of the System.Net.Http.DelegatingHandler class with a specific logger and inner handler.
        /// </summary>
        /// <param name="logger">The logger that will be used to log information</param>
        /// <param name="httpMessageHandler">The inner handler which is responsible for processing the HTTP response messages.</param>
        public RequestHandler(ILog logger, HttpMessageHandler httpMessageHandler) : base(httpMessageHandler)
        {
            Logger = logger;
        }

        /// <summary>
        /// Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.
        /// </summary>
        /// <param name="request"> The HTTP request message to send to the server.</param>
        /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
        /// <returns>Returns System.Threading.Tasks.Task. The task object representing the asynchronous operation</returns>
        /// <exception cref="ArgumentNullException">The request was null.</exception>
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (InnerHandler == null)
                InnerHandler = new HttpClientHandler();

            if (request == null)
                throw new ArgumentNullException("request");

            var requestUid = Guid.NewGuid();

            var centauroTechMessage = new CentauroTechMessage(request);

            try
            {
                centauroTechMessage.StartTime = DateTime.Now;
                centauroTechMessage.ResponseMessage = await base.SendAsync(centauroTechMessage.RequestMessage, cancellationToken);
                centauroTechMessage.EndTime = DateTime.Now;

                if (Logger.IsDebugEnabled)
                {
                    try
                    {
                        if (centauroTechMessage.ResponseMessage != null)
                        {

                            await LogRequestResponseAsync(centauroTechMessage, requestUid);
                        }
                    }
                    catch (InvalidOperationException)
                    {
                        Logger.Warn("Unable to parse content! Now trying with the default encoding...");
                        try
                        {
                            await LogRequestResponseAsync(centauroTechMessage, requestUid, true);
                        }
                        catch
                        {
                            throw;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex is AggregateException)
                            ex = ex.InnerException;

                        Logger.Error("Error getting the response message: " + ex.Message, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is AggregateException)
                    ex = ex.InnerException;

                Logger.Error("Error generating the log of the request: " + ex.Message, ex);

                throw ex;
            }

            return centauroTechMessage.ResponseMessage;
        }

        private int GetSatsusCodeNumber(HttpStatusCode httpStatusCode)
        {
            return (int)httpStatusCode;
        }

        private async Task LogRequestAsync(HttpRequestMessage request, Guid messageUid, bool useDefaultEncoding = false)
        {
            Logger.Debug(
            new
            {
                LogMessage = $"Request object",
                HttpMessageUid = messageUid,
                Uri = request?.RequestUri.ToString(),
                Method = request?.Method.ToString(),
                Headers = request?.Headers,
                Content = await GetContentAsync(request.Content, useDefaultEncoding)
            });
        }

        private async Task LogResponseAsync(HttpResponseMessage response, Guid messageUid, bool useDefaultEncoding = false)
        {
            Logger.Debug(
            new
            {
                LogMessage = "Response object",
                RequestUid = messageUid,
                StatusCode = GetSatsusCodeNumber(response.StatusCode),
                ReasonPhrase = response.ReasonPhrase,
                Headers = response?.Headers,
                Content = await GetContentAsync(response.Content, useDefaultEncoding)
            });
        }

        private async Task LogRequestResponseAsync(CentauroTechMessage centauroMessage, Guid messageUid, bool useDefaultEncoding = false)
        {


            Logger.Debug(
            new
            {
                StartTime = centauroMessage.StartTime,
                EndTime = centauroMessage.EndTime,
                TotalTime = centauroMessage.TotalTime,

                request = new
                {
                    LogMessage = $"Request object",
                    HttpMessageUid = messageUid,
                    Uri = centauroMessage.RequestMessage?.RequestUri.ToString(),
                    Method = centauroMessage.RequestMessage?.Method.ToString(),
                    Headers = centauroMessage.RequestMessage?.Headers,
                    Content = await GetContentAsync(centauroMessage.RequestMessage.Content, useDefaultEncoding),
                    RequestSize = centauroMessage.RequestMessage.Content.ReadAsByteArrayAsync().Result.Length
                },

                response = new
                {
                    LogMessage = "Response object",
                    RequestUid = messageUid,
                    StatusCode = GetSatsusCodeNumber(centauroMessage.ResponseMessage.StatusCode),
                    ReasonPhrase = centauroMessage.ResponseMessage.ReasonPhrase,
                    Headers = centauroMessage.ResponseMessage?.Headers,
                    Content = await GetContentAsync(centauroMessage.ResponseMessage.Content, useDefaultEncoding),
                    ResponseSize = centauroMessage.ResponseMessage.Content.ReadAsByteArrayAsync().Result.Length
                }
            });
        }

        private async Task<string> GetContentAsync(HttpContent httpContent, bool useDefaultEncoding)
        {
            var content = string.Empty;
            if (httpContent != null)
            {
                content = useDefaultEncoding ? DefaultEncoding.GetString(await httpContent.ReadAsByteArrayAsync()) : await httpContent.ReadAsStringAsync();
            }
            return content;
        }
    }
}
