using Microsoft.Extensions.DependencyInjection;
using TravelBuddy.Destinations;
using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;

namespace TravelBuddy;

[DependsOn(
    typeof(TravelBuddyDomainModule),
    typeof(AbpAutoMapperModule),
    typeof(TravelBuddyApplicationContractsModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpAccountApplicationModule),
    typeof(AbpSettingManagementApplicationModule)
    )]
public class TravelBuddyApplicationModule : AbpModule
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

/*public class TravelBuddyApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<TravelBuddyApplicationModule>();
        });
    }
}*/