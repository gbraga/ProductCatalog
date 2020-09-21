using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductCatalog.Data;
using ProductCatalog.Repositories;

namespace ProductCatalog
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(option => option.EnableEndpointRouting = false);

            // se quisesse cachear todas as respostas por padrão
            // services.AddResponseCaching();
            services.AddResponseCompression();
            
            services.AddScoped<StoreDataContext, StoreDataContext>();
            services.AddTransient<ProductRepository, ProductRepository>();
            services.AddTransient<CategoryRepository, CategoryRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseMvc();
            app.UseResponseCompression(); // Torna todas as respostas compactadas com gzip
        }
    }
}
