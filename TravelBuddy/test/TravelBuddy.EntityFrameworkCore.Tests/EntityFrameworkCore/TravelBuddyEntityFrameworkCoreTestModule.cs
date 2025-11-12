using Microsoft.Extensions.DependencyInjection;
using System;
using Moq;
using TravelBuddy.Destinations;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
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
        // Repositorio falso de Destination para que el contenedor
        // pueda construir DestinationAppService en los tests de integración.
        var destinationRepoMock = new Mock<IRepository<Destination, Guid>>();
        context.Services.AddSingleton<IRepository<Destination, Guid>>(destinationRepoMock.Object);
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
    }
}
