using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using TodoApi.Helpers;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi

{
    public class Startup
    {
        //https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-3.1#getvalue
        private readonly IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // requires using Microsoft.Extensions.Options
            services.Configure<BookstoreDatabaseSettings>(
                Configuration.GetSection(nameof(BookstoreDatabaseSettings)));

            services.AddSingleton<IBookstoreDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<BookstoreDatabaseSettings>>().Value);
            //https://stackoverflow.com/questions/58228245/add-all-the-required-services-by-calling-iservicecollection-addhealthchecks
            //services.AddRazorPages();
            // configure strongly typed settings object
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            // configure DI for application services
           // services.AddScoped<IUserService, CustomerService>();

            services.AddSingleton<BookService>();
            services.AddSingleton<CustomerService>();
            services.AddCors();
            services.AddControllers().AddNewtonsoftJson(options => options.UseMemberCasing());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            // custom jwt auth middleware
            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
               // endpoints.MapRazorPages();
                endpoints.MapControllers();
                
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!test...");
                   
                });
                
            });
        }
    }
}
