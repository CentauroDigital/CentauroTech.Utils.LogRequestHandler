# CentauroTech.Utils.LogRequestHandler
Nuget package to log requests made during HTTP request and responses on ASP.net web applications

#### Status

Branches: &nbsp;&nbsp;&nbsp; [![Build status](https://ci.appveyor.com/api/projects/status/2t8nit05e6n7sx6p?svg=true)](https://ci.appveyor.com/project/jmtvms/centaurotech-utils-logrequesthandler)

Master: &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; [![Build status](https://ci.appveyor.com/api/projects/status/2t8nit05e6n7sx6p/branch/master?svg=true)](https://ci.appveyor.com/project/jmtvms/centaurotech-utils-logrequesthandler/branch/master)

#### Nuget package installation:
To install CentauroTech.Utils.LogRequestHandler, run the following command in the Package Manager Console

	PM> Install-Package CentauroTech.Utils.LogRequestHandler
	
More information about the package, please visit:
https://www.nuget.org/packages/CentauroTech.Utils.LogRequestHandler/

#### Usage on a server web app:
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


#### Release
To release a new version of the package (based on this document: https://docs.microsoft.com/pt-br/nuget/quickstart/create-and-publish-a-package-using-visual-studio-net-framework)
- Edit the necessary information in `Properties/AssemblyInfo.cs`
- Edit the necessary information in `CentauroTech.Utils.LogRequestHandler.nuspec`
- Create the file `nuget.config` as described in https://dev.azure.com/CentauroDigital/Site%20Omni/_packaging?_a=connect&feed=internal-packages
- Run `nuget pack`. This should create a `.nupkg` file
- Run the publish command as described in https://dev.azure.com/CentauroDigital/Site%20Omni/_packaging?_a=connect&feed=internal-packages
- **Delete the nuget.config file :)**