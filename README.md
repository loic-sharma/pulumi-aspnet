# Pulumi ASP.NET

This example shows how Pulumi could integrate into an ASP.NET Core app:

* Resources are declared through the [`MyServiceResources`](https://github.com/loic-sharma/pulumi-aspnet/blob/master/MyServiceResources.cs) resource provider
* The resource provider is registered to Dependency Injection container in [`Startup.ConfigureServices`](https://github.com/loic-sharma/pulumi-aspnet/blob/master/Startup.cs#L30)

The `pulumi update` command should behavior similarly to Entity Framework Core's migrations: it should invoke [`Program.BuildWebHost()`](https://github.com/loic-sharma/pulumi-aspnet/blob/master/Program.cs#L20), access the `IWebHost.Services` property, and resolve the `Pulumi.ServiceProvider`. For more information on how EF Core creates its context at design-time, refer to [this documentation](https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dbcontext-creation).