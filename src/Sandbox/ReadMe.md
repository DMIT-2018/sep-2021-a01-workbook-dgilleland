# Dependency Injection and Internal Database Models

Part of the reason to have an internal database context and entities is to **make it hard to do the *wrong* thing**. In other words, we don't want to expose our database model to the Presentation Layer because this "leaking" could cause tight coupling between the backend and the frontend.

However, we also want to have loose coupling for our services, and this means leveraging **dependency injection**. But how do we inject something that is internal to our backend? Our `CapstoneContext` is not visible to the web application!

One solution is to create an *extension method* in the backend project.

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