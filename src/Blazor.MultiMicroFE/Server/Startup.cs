using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Blazor.MultiMicroFE.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.MapWhen(ctx => ctx.Request.Path.StartsWithSegments("/FirstApp"), first =>
            {
                first.UseBlazorFrameworkFiles("/FirstApp");
                first.UseStaticFiles();

                first.UseRouting();
                first.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapFallbackToFile("FirstApp/{*path:nonfile}", "FirstApp/index.html");
                });
            });

            app.MapWhen(ctx => ctx.Request.Path.StartsWithSegments("/SecondApp"), second =>
            {
                second.UseBlazorFrameworkFiles("/SecondApp");
                second.UseStaticFiles();

                second.UseRouting();
                second.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapFallbackToFile("SecondApp/{*path:nonfile}", "SecondApp/index.html");
                });
            });

            app.MapWhen(ctx => ctx.Request.Path.StartsWithSegments("/ThirdApp"), third =>
            {
                third.UseBlazorFrameworkFiles("/ThirdApp");
                third.UseStaticFiles();

                third.UseRouting();
                third.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapFallbackToFile("ThirdApp/{*path:nonfile}", "ThirdApp/index.html");
                });
            });
        }
    }
}