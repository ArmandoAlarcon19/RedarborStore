using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using RedarborStore.Domain.Interfaces.Commands;
using RedarborStore.Domain.Interfaces.Queries;
using RedarborStore.Infrastructure.Commands;
using RedarborStore.Infrastructure.Data;
using RedarborStore.Infrastructure.Queries;

namespace RedarborStore.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<InventoryDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null);
                }));
        services.AddScoped<ICategoryQueryRepository, CategoryQueryRepository>();
        services.AddScoped<IProductQueryRepository, ProductQueryRepository>();
        services.AddScoped<IInventoryMovementQueryRepository, InventoryMovementQueryRepository>();
        services.AddScoped<ICategoryCommandRepository, CategoryCommandRepository>();
        services.AddScoped<IProductCommandRepository, ProductCommandRepository>();
        services.AddScoped<IInventoryMovementCommandRepository, InventoryMovementCommandRepository>();

        return services;
    }
}