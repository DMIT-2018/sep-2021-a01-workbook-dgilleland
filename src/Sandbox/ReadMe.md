# Dependency Injection and Internal Database Models

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

