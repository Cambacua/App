using Localization.Resources.AbpUi;
using TravelBuddy.Localization;
using Volo.Abp.Account;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.SettingManagement;
using TravelBuddy.Destinations;

namespace TravelBuddy;

 [DependsOn(
    typeof(TravelBuddyApplicationContractsModule),
    typeof(AbpPermissionManagementHttpApiModule),
    typeof(AbpSettingManagementHttpApiModule),
    typeof(AbpAccountHttpApiModule),
    typeof(AbpIdentityHttpApiModule),
     typeof(TravelBuddyApplicationContractsModule),
       typeof(AbpAspNetCoreMvcModule),
    typeof(AbpFeatureManagementHttpApiModule)
    )]
public class TravelBuddyHttpApiModule : AbpModule
{

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        ConfigureLocalization();
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(typeof(DestinationAppService).Assembly);
        });
    }
    
    private void ConfigureLocalization()
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<TravelBuddyResource>()
                .AddBaseTypes(
                    typeof(AbpUiResource)
                );
        });
    }
}
