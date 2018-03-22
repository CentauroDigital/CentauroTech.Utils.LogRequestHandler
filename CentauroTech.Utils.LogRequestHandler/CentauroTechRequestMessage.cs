
using System;
using System.Net.Http;


namespace CentauroTech.Utils.Http
{
    /// <summary>
    /// Message aggregating request and response messages 
    /// </summary>
    public class CentauroTechMessage 
    {
        private HttpResponseMessage _responseMessage { get; set; }
        private HttpRequestMessage _requestMessage { get; set; }

        /// <summary>
        /// response message for the current request
        /// </summary>
        public HttpResponseMessage ResponseMessage
        {
            get { return _responseMessage; }
            set { _responseMessage = value; }
        }
        /// <summary>
        /// request message for current request
        /// </summary>
        public HttpRequestMessage RequestMessage
        {
            get { return _requestMessage; }
            set { _requestMessage = value; }
        }

        public CentauroTechMessage()
        {
            _responseMessage = new HttpResponseMessage { };
        }

        public CentauroTechMessage(HttpRequestMessage requestMessage)
        {
            _requestMessage = requestMessage;            
        }

        /// <summary>
        ///  time when request is triggered
        /// </summary>       
        public DateTime StartTime { get; set; }
        /// <summary>
        /// time when response is returned
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// total time between request e response
        /// </summary>
        public double TotalTime { get { return (EndTime - StartTime).TotalMilliseconds; } }
        /// <summary>
        /// size in bytes of request content
        /// </summary>
        public int RequestSize { get; set; }
        /// <summary>
        /// size  of response content
        /// </summary>
        public int ResponseSize { get; set; }



    }

}