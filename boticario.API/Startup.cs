using System;
using System.IO;
using System.Reflection;
using System.Text;
using boticario.Helpers;
using boticario.Helpers.Security;
using boticario.Models;
using boticario.Options;
using boticario.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace boticario.API
{
    public class Startup
    {
        private readonly string corsPolicy = "MyPolicy";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddCors(options => options.AddPolicy(corsPolicy, builder =>
            {
                builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
            }));

            IConfigurationSection settingsOptionsSection = Configuration.GetSection(nameof(SettingsOptions));
            services.Configure<SettingsOptions>(settingsOptionsSection);

            SettingsOptions settingsOptions = settingsOptionsSection.Get<SettingsOptions>();
            byte[] key = Encoding.ASCII.GetBytes(settingsOptions.Secret);
            services.AddAuthentication(item =>
            {
                item.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                item.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(item =>
            {
                item.RequireHttpsMetadata = false;
                item.SaveToken = true;
                item.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddDbContext<AppDbContext>(options => 
                options.UseSqlServer(
                    Configuration.GetConnectionString("ConnectionSqlServer"), 
                    connection => connection.MigrationsAssembly("boticario.API")
                )
            );

            services.AddSwaggerGen(item =>
            {
                item.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "o'Boticário - Desafio",
                        Description = "API de Cashback para Revendedores em .Net Core 3.1 com Swagger",
                        Contact = new OpenApiContact
                        {
                            Name = "Luiz Anthero Gama",
                            Email = "luizanthero@icloud.com",
                            Url = new Uri("https://github.com/luizanthero")
                        }
                    });

                item.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });

                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                item.IncludeXmlComments(xmlPath);

                item.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                item.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            services.AddScoped<SettingsOptions>();
            services.AddScoped<HelperService>();

            services.AddScoped<CompraService>();
            services.AddScoped<HistoricoService>();
            services.AddScoped<ParametroSistemaService>();
            services.AddScoped<RegraCashbackService>();
            services.AddScoped<RevendedorService>();
            services.AddScoped<StatusCompraService>();
            services.AddScoped<TipoHistoricoService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            SwaggerOptions swagger = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swagger);

            app.UseSwagger(options => { options.RouteTemplate = swagger.JsonRoute; });

            app.UseSwaggerUI(options => { options.SwaggerEndpoint(swagger.UIEndpoint, swagger.Description); });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors(corsPolicy);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Run(async context => { context.Response.Redirect("/swagger/index.html"); });
        }
    }
}
