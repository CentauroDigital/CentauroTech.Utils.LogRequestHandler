# CentauroTech.Utils.LogRequestHandler
Nuget package to log requests made during HTTP request and responses on ASP.net web applications

#### Status

Branches: &nbsp;&nbsp;&nbsp; [![Build status](https://ci.appveyor.com/api/projects/status/2t8nit05e6n7sx6p?svg=true)](https://ci.appveyor.com/project/jmtvms/centaurotech-utils-logrequesthandler)

Master: &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; [![Build status](https://ci.appveyor.com/api/projects/status/2t8nit05e6n7sx6p/branch/master?svg=true)](https://ci.appveyor.com/project/jmtvms/centaurotech-utils-logrequesthandler/branch/master)

#### Nuget package installation:
To install CentauroTech.Utils.RegexReplacementAppender, run the following command in the Package Manager Console

	PM> Install-Package CentauroTech.Utils.LogRequestHandler
	
More information about the package, please visit:
https://www.nuget.org/packages/CentauroTech.Utils.LogRequestHandler/

#### Usage:
Add the message handler to the HttpConfiguration of you web site. The logs will appear wherever your log4net logs are being presented by your app.

    public static class WebApiConfig
    {
      public static void Register(HttpConfiguration config)
      {
        //Add the log request handler to que configuration of you asp.net web application.
        //It works fine with web api and web apps.
        config.MessageHandlers.Add(new CentauroTech.Utils.LogRequestHandler());
  
        config.MapHttpAttributeRoutes();
  
        config.Routes.MapHttpRoute(
          name: "DefaultApi",
            routeTemplate: "api/{controller}/{id}",
            defaults: new { id = RouteParameter.Optional }
        );
      }
    }

#### Config (Optional):
Add a key CentauroTech.Utils.LogRequestHandler.DefaultEncoding in AppSettings to define a default encoding in case that the Content-Type header cannot be read or is invalid.
```
<appSettings>
  <add key="CentauroTech.Utils.LogRequestHandler.DefaultEncoding " value="UTF-8" />
</appSettings>
```
##### log4net reference:
https://logging.apache.org/log4net/
