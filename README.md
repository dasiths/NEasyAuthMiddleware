# NEasyAuthMiddleware  [![Build status](https://ci.appveyor.com/api/projects/status/5e9kb8rd3egstvkb?svg=true)](https://ci.appveyor.com/project/dasiths/neasyauthmiddleware) [![NuGet](https://img.shields.io/nuget/v/NEasyAuthMiddleware.svg)](https://www.nuget.org/packages/NEasyAuthMiddleware) [![Downloads](https://img.shields.io/nuget/dt/NEasyAuthMiddleware.svg)](https://www.nuget.org/packages/NEasyAuthMiddleware/)

Azure App Service Authentication (EasyAuth) middleware for ASP.NET CORE with fully customizable components with support for local debugging.

## What is `EasyAuth`?

Azure `App Service` has a feature to turn on Authentication on top of your application code. This is useful if you don't want to handle the nitty gritty of auth. It's meant to be a quick and easy way to put an authentication layer above an application hosted on an app service. More details can be found here https://docs.microsoft.com/en-us/azure/app-service/overview-authentication-authorization.

There is a how to get started tutorial [here](https://www.benday.com/2018/05/17/walkthrough-part-2-configure-app-service-authentication-for-your-azure-web-app/).

## The problem

Although we don't have to worry about things like `OAuth` or `OpenIdConnect` when we use EasyAuth, there are instances where we still need to know information about the logged in user. For example you might want to allow only users who have a certain role to access a part of your system.

ASP.NET applications built for the full .NET Framework have the `HttpContext.User` already populated by EasyAuth. But AspNetCore web applications don't get that due to the fact that IIS EasyAuth modules not integrating with AspNetCore.

Due to this limitation, If you want to make decisions based on the current user, you have to use your own logic to construct the user principal by looking at HTTP headers in the request.

To add insult to injury, when you want to debug this on your local machine, these HTTP headers will not be present as these are added by the App Service. So you will have to use some form of mocking to populate the user principal.

## Solution

NEasyAuthMiddleware does all of this complicated logic for you and keeps your authentication concerns simple. It hydrates the `HttpContext.User` by registering a custom authentication handler. To make things easier when running locally, it even has the ability to use a `json` file to load mocked claims.

## Using it

- Version 1.X targets AspNet Core 2.x
- Version 2.X targets AspNet Core 3.x

Install the package using NuGet Package Manager Console

```
Install-Package NEasyAuthMiddleware
```

Just add the following to your `Startup.cs`

```csharp
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddEasyAuth();

            if (_hostingEnvironment.IsDevelopment()) // Use the mock json file when not running in an app service
            {
                var mockFile = $"{_hostingEnvironment.ContentRootPath}\\mock_user.json";
                services.UseJsonFileToMockEasyAuth(mockFile);
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication(); // Indicate we are using the Authenticaiton middleware
            app.UseRouting();
            app.UseAuthorization(); // This has to come between routing and endoints

            app.UseEndpoints(endpoints =>
            {
                // Your endpoints are defined here
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
```

In your controller, use the `Authorize` attribute as you would with any other authentication provider.

```csharp
        [Authorize(Roles = "Resource.Read")]
        public ActionResult<string> Get()
        {
            return "authorization worked...";
        }
```

See the [sample app](https://github.com/dasiths/NEasyAuthMiddleware/tree/master/NEasyAuthMiddleware.Sample) for more.

## Customizing it

The library already maps most of the claims coming in the http headers. If you find a custom header that you would like to map to the claims of the current user, all you have to do is implement the `IClaimMapper` interface below and register it in your DI container.

```csharp
    public interface IClaimMapper
    {
        ClaimMapResult Map(IHeaderDictionary headers);
    }
```

If you require mutating the `IHeaderDictionary` prior to being consumed by the mappers, implement the `IHeaderDictionaryTransformer` interface and register it in your DI container.

```csharp
    public interface IHeaderDictionaryTransformer
    {
        IHeaderDictionary Transform(IHeaderDictionary headerDictionary);
    }
```

If you want to filter or mutate the mapped claims prior to being part of a `ClaimsIdentity`, implement the `IClaimsTransformer` interface and register it in your DI container.

```csharp
    public interface IClaimsTransformer
    {
        List<Claim> Transform(List<Claim> claims);
    }
```

Note: The main focus is to support AzureAD as the identity provider and most of the testing has been done against it. Please raise an issue if you find any bugs. Thank you.
