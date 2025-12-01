using Microsoft.Extensions.DependencyInjection;
using TravelBuddy.Destinations;
using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using TravelBuddy.Cities;
using Microsoft.Extensions.DependencyInjection;



namespace TravelBuddy
{
    // La clase debe HEREDAR de AbpModule y tener sus llaves de inicio/fin
    [DependsOn(
        typeof(TravelBuddyDomainModule),
        typeof(TravelBuddyApplicationContractsModule),
        typeof(AbpPermissionManagementApplicationModule),
        typeof(AbpFeatureManagementApplicationModule),
        typeof(AbpIdentityApplicationModule),
        typeof(AbpAccountApplicationModule)
    )]
    public class TravelBuddyApplicationModule : AbpModule // <-- Clase de Módulo
    {
        public override void ConfigureServices(ServiceConfigurationContext context) // <-- Método dentro de la clase
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<TravelBuddyApplicationModule>();
            });
            var configuration = context.Services.GetConfiguration();
            context.Services.Configure<GeoDbOptions>(configuration.GetSection("GeoDb"));

            // Esto configura la inyección de dependencia para HttpClient
            context.Services.AddHttpClient<ICitySearchService, GeoDbCitySearchService>();
        }
    }

}



