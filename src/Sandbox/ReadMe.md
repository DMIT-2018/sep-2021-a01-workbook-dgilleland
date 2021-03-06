Key information

## User Secrets

> ### Update-Database
>
> Make sure you run **`Update-Database -Context ApplicationDbContext`** in the Package Manager Console before running the application. This will make sure that the database tables for logins have been correctly set up.

The latest user secrets for my example include some key **application settings** for the default
*SuperUser* who is acting as a web-master for the website.

```json
{
    "ConnectionStrings": {
        "Capstone": "Server=.;Database=Capstone;Integrated Security=true;"
    },
    "Setup": {
        "SuperUser": "Super@user.local",
        "SuperUserEmail": "Super@user.local",
        "SuperUserPassword": "D3fault.user"
    }
}
```

## Dependency Injection and Internal Database Models

Part of the reason to have an internal database context and entities is to **make it hard to do the *wrong* thing**. In other words, we don't want to expose our database model to the Presentation Layer because this "leaking" could cause tight coupling between the backend and the frontend. We want the freedom to modify our backend while minimizing any effect on our frontend should we need to change the database model. We also want to have loose coupling for our services, and this means leveraging **dependency injection** in creating our services.

In our sample, we have an **`AboutService`** service that requires an instance of the **`CapstoneContext`** in order to provide the **`DatabaseVersion`**.

```cs
public class AboutService
{
    // ...

    public DatabaseVersion GetDatabaseVersion()
    {
        var versionInfo = _context.DbVersions.Single();
        return new DatabaseVersion(new Version(versionInfo.Major, versionInfo.Minor, versionInfo.Build), versionInfo.ReleaseDate);
    }
}
```

```cs
public record DatabaseVersion(Version Version, DateTime ReleaseDate);
```

But how do we inject something that is internal to our backend? Our `CapstoneContext` is not visible to the web application!

```cs
internal partial class CapstoneContext : DbContext
{
    // ...
}
```

Because the `CapstoneContext` class is `internal`, the constructor for `AboutService` needs to be internal as well.

```cs
namespace Backend.BLL
{
    public class AboutService
    {
        private readonly CapstoneContext _context;
        internal AboutService(CapstoneContext context)
        {
            _context = context;
        }

        public DatabaseVersion GetDatabaseVersion()
        {
            var versionInfo = _context.DbVersions.Single();
            return new DatabaseVersion(new Version(versionInfo.Major, versionInfo.Minor, versionInfo.Build), versionInfo.ReleaseDate);
        }
    }
}
```

One solution is to create an *extension method* in the backend project. Here's an example that uses an *implementation factory* for the **`AboutService`**

```cs
namespace Backend
{
    public static class StartupExtensions
    {
        public static void AddBackendDependencies(this IServiceCollection services, Action<DbContextOptionsBuilder> options)
        {
            services.AddDbContext<CapstoneContext>(options);
            services.AddTransient<AboutService>((serviceProvider) => 
            {
                var context = serviceProvider.GetRequiredService<CapstoneContext>();
                return new AboutService(context);
            });
        }
    }
}
```

From the web application's **`Startup`** class, we can configure the services accordingly.

```cs
services.AddBackendDependencies(options =>
    options.UseSqlServer(
        Configuration.GetConnectionString("Capstone")));
```

Don't forget that you still have to set up your connection string (in the `appSettings.json` file).

```js
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=aspnet-WebApp-53bc9b9d-9d6a-45d4-8429-2a2761773502;Trusted_Connection=True;MultipleActiveResultSets=true",
    "Capstone": "Server=.;Database=Capstone;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*"
}
```

