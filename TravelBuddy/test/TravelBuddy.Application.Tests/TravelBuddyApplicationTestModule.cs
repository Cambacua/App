using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
//using TravelBuddy.EntityFrameworkCore;
using System;

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

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        // No se hace seeding ni permisos, para simplificar las pruebas
    }
}

public static class PermissionManagementDisabler
{
    public static IServiceCollection AddAlwaysDisablePermissionManagementForTests(this IServiceCollection services)
    {
        // Evita cargar módulos de permisos dinámicos de
        // 
        services.Configure<PermissionManagementOptions>(options =>
        {
            options.IsDynamicPermissionStoreEnabled = false;
        });

        // Registra una implementacion vacia para evitar resolucion de dependencias
        services.AddTransient<IPermissionManagementProvider, EmptyPermissionManagementProvider>();
        services.AddTransient<IDataSeedContributor, EmptyPermissionDataSeedContributor>();
        return services;
    }
}

// Implementación vacía de IPermissionManagementProvider
public class EmptyPermissionManagementProvider : IPermissionManagementProvider
{
    public string Name => "EmptyProvider";
    // Método original
    public Task<PermissionValueProviderGrantInfo> CheckAsync(string name, string providerName, string providerKey)
    {
        return Task.FromResult(new PermissionValueProviderGrantInfo(false, null));
    }
    // Sobrecarga para múltiples permisos (con tipo de retorno correcto)
    public Task<MultiplePermissionValueProviderGrantInfo> CheckAsync(string[] names, string providerName, string providerKey)
    {
        // Devuelve un objeto vacío (asume constructor por defecto o propiedades)
        return Task.FromResult(new MultiplePermissionValueProviderGrantInfo());
    }

    // Otros métodos requeridos
    public Task<PermissionValueProviderGrantInfo[]> GetAllAsync(string providerName, string providerKey)
    {
        return Task.FromResult(Array.Empty<PermissionValueProviderGrantInfo>());
    }
    // Método original de SetAsync
    public Task SetAsync(string name, string providerName, string providerKey, string value)
    {
        return Task.CompletedTask;
    }
    // Sobrecarga de SetAsync con bool
    public Task SetAsync(string name, string providerName, bool isGranted)
    {
        return Task.CompletedTask;
    }
}

// Implementación vacía de IDataSeedContributor (corrige la interfaz)
public class EmptyPermissionDataSeedContributor : IDataSeedContributor
{
    public Task SeedAsync(DataSeedContext context)
    {
        // No hace nada
        return Task.CompletedTask;
    }
}



