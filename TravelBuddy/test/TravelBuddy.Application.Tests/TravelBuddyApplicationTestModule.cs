using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;

namespace TravelBuddy;

[DependsOn(
    typeof(TravelBuddyApplicationModule),
    typeof(TravelBuddyDomainTestModule)
)]
public class TravelBuddyApplicationTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAlwaysDisablePermissionManagementForTests();
    }
}

public static class PermissionManagementDisabler
{
    public static IServiceCollection AddAlwaysDisablePermissionManagementForTests(this IServiceCollection services)
    {
        services.Configure<PermissionManagementOptions>(options =>
        {
            options.IsDynamicPermissionStoreEnabled = false;
        });

        services.AddTransient<IPermissionManagementProvider, EmptyPermissionManagementProvider>();
        services.AddTransient<IDataSeedContributor, EmptyPermissionDataSeedContributor>();
        return services;
    }
}

public class EmptyPermissionManagementProvider : IPermissionManagementProvider
{
    public string Name => "EmptyProvider";

    public Task<PermissionValueProviderGrantInfo> CheckAsync(string name, string providerName, string providerKey)
    {
        return Task.FromResult(new PermissionValueProviderGrantInfo(false, null));
    }

    public Task<MultiplePermissionValueProviderGrantInfo> CheckAsync(string[] names, string providerName, string providerKey)
    {
        return Task.FromResult(new MultiplePermissionValueProviderGrantInfo());
    }

    public Task<PermissionValueProviderGrantInfo[]> GetAllAsync(string providerName, string providerKey)
    {
        return Task.FromResult(Array.Empty<PermissionValueProviderGrantInfo>());
    }

    public Task SetAsync(string name, string providerName, string providerKey, string value)
    {
        return Task.CompletedTask;
    }

    public Task SetAsync(string name, string providerName, bool isGranted)
    {
        return Task.CompletedTask;
    }
}

public class EmptyPermissionDataSeedContributor : IDataSeedContributor
{
    public Task SeedAsync(DataSeedContext context)
    {
        return Task.CompletedTask;
    }
}