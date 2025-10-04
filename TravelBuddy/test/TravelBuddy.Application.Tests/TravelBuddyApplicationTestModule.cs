using Microsoft.Extensions.DependencyInjection;
using TravelBuddy.Destinations;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace TravelBuddy;

[DependsOn(
    typeof(TravelBuddyApplicationModule),
    typeof(TravelBuddyDomainTestModule)
)]
public class TravelBuddyApplicationTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTransient<IDestinationAppService, DestinationAppService>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<TravelBuddyApplicationModule>();
        });
    }

}
