using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Volo.Abp.Users;
using System.Security.Claims;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;
using Volo.Abp.Users;

namespace TravelBuddy.EntityFrameworkCore;


public class FakePrincipalAccessor : ICurrentPrincipalAccessor, ISingletonDependency
{
    public ClaimsPrincipal Principal { get; set; } = new ClaimsPrincipal();

    public IDisposable Change(ClaimsPrincipal principal)
    {
        Principal = principal;
        return new NullDisposable();
    }

    private class NullDisposable : IDisposable
    {
        public void Dispose()
        {
        }
    }
}

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class TravelBuddyDbContextFactory : IDesignTimeDbContextFactory<TravelBuddyDbContext>
{

    public TravelBuddyDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        TravelBuddyEfCoreEntityExtensionMappings.Configure();

        var builder = new DbContextOptionsBuilder<TravelBuddyDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));

        // Crear accessor falso para design-time
        var fakeAccessor = new FakePrincipalAccessor();

        var currentUser = new CurrentUser(fakeAccessor);

        return new TravelBuddyDbContext(builder.Options, currentUser);
    }




    /* public TravelBuddyDbContext CreateDbContext(string[] args)
     {
         var configuration = BuildConfiguration();

         TravelBuddyEfCoreEntityExtensionMappings.Configure();

         var builder = new DbContextOptionsBuilder<TravelBuddyDbContext>()
             .UseSqlServer(configuration.GetConnectionString("Default"));

         return new TravelBuddyDbContext(builder.Options);

     }*/



    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../TravelBuddy.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}




