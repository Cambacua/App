using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using OpenIddict.Server.AspNetCore;
using OpenIddict.Validation.AspNetCore;
using System;
using System.IO;
using System.Linq;
using System.Text;
using TravelBuddy.EntityFrameworkCore;
using TravelBuddy.HealthChecks;
using TravelBuddy.MultiTenancy;
using Volo.Abp;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.Studio.Client.AspNetCore;
using Volo.Abp.Swashbuckle;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;

namespace TravelBuddy
{
   
    [DependsOn(typeof(TravelBuddyHttpApiModule),
        typeof(AbpStudioClientAspNetCoreModule), 
        typeof(AbpAspNetCoreMvcUiLeptonXLiteThemeModule),
        typeof(AbpAutofacModule), 
        typeof(AbpAspNetCoreMultiTenancyModule),
        typeof(TravelBuddyApplicationModule), 
        typeof(TravelBuddyEntityFrameworkCoreModule),
        typeof(AbpAccountWebOpenIddictModule),
        typeof(AbpSwashbuckleModule),
        typeof(AbpAspNetCoreSerilogModule))]

    public class TravelBuddyHttpApiHostModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var hostingEnvironment = context.Services.GetHostingEnvironment();

            
            PreConfigure<OpenIddictBuilder>(builder =>
            {
                builder.AddValidation(options =>
                {
                   
                    options.AddAudiences("TravelBuddy");

                   
                    options.UseLocalServer();

                    // Integración con ASP.NET Core
                    options.UseAspNetCore();
                });
            });

           
            PreConfigure<OpenIddictServerBuilder>(builder =>
            {
              
                builder.SetTokenEndpointUris("/connect/token");

              
                builder.AllowPasswordFlow()
                       .AcceptAnonymousClients(); 

                builder.UseAspNetCore()
                       .EnableTokenEndpointPassthrough();

               
                builder.DisableAccessTokenEncryption();

                if (hostingEnvironment.IsDevelopment())
                {
                   
                    builder.AddDevelopmentEncryptionCertificate()
                           .AddDevelopmentSigningCertificate();
                }
                else
                {
                 
                }
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            ConfigureUrls(configuration);
            ConfigureCors(context, configuration);
            ConfigureSwagger(context, configuration);

          
            ConfigureVirtualFileSystem(context);
            ConfigureHealthChecks(context);
        }

        private void ConfigureUrls(IConfiguration configuration)
        {
            Configure<AppUrlOptions>(options =>
            {
                options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
                options.Applications["Angular"].RootUrl = configuration["App:AngularUrl"];
                options.RedirectAllowedUrls.AddRange(
                    configuration["App:RedirectAllowedUrls"]?.Split(',') ?? Array.Empty<string>());
            });
        }

        private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    var origins = configuration["App:CorsOrigins"]?
                        .Split(",", StringSplitOptions.RemoveEmptyEntries)
                        .Select(o => o.Trim().TrimEnd('/'))
                        .ToArray() ?? Array.Empty<string>();

                    policy.WithOrigins(origins)
                          .WithAbpExposedHeaders()
                          .SetIsOriginAllowedToAllowWildcardSubdomains()
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });
        }

        private static void ConfigureSwagger(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddAbpSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "TravelBuddy API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);

               
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Ingrese el token JWT en el formato: Bearer {token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
                        Array.Empty<string>()
                    }
                });
            });
        }

        private void ConfigureVirtualFileSystem(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();

            if (hostingEnvironment.IsDevelopment())
            {
                Configure<AbpVirtualFileSystemOptions>(options =>
                {
                    options.FileSets.ReplaceEmbeddedByPhysical<TravelBuddyDomainSharedModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}TravelBuddy.Domain.Shared"));
                    options.FileSets.ReplaceEmbeddedByPhysical<TravelBuddyDomainModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}TravelBuddy.Domain"));
                    options.FileSets.ReplaceEmbeddedByPhysical<TravelBuddyApplicationContractsModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}TravelBuddy.Application.Contracts"));
                    options.FileSets.ReplaceEmbeddedByPhysical<TravelBuddyApplicationModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}TravelBuddy.Application"));
                });
            }
        }

        private void ConfigureHealthChecks(ServiceConfigurationContext context)
        {
            context.Services.AddTravelBuddyHealthChecks();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedProto
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAbpRequestLocalization();

            app.UseRouting();
            app.MapAbpStaticAssets();
           
            app.UseAbpSecurityHeaders();
            app.UseCors();

           
            app.UseAuthentication();

            
            app.UseAbpOpenIddictValidation(); 

            if (MultiTenancyConsts.IsEnabled)
            {
                app.UseMultiTenancy();
            }

            app.UseUnitOfWork();
            app.UseDynamicClaims();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseAbpSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "TravelBuddy API v1");
                // Si querés probar con OAuth2/OpenIddict en Swagger UI, configurá client aquí:
                // var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
                // options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
            });

            app.UseAuditing();
            app.UseAbpSerilogEnrichers();
            app.UseConfiguredEndpoints();
        }
    }
}
