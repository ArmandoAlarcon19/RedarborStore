using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using RedarborStore.Domain.Interfaces.Commands;
using RedarborStore.Domain.Interfaces.Queries;
using RedarborStore.Infrastructure.Auth;
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
        services.AddAuth0Authentication(configuration);

        return services;
    }
    private static IServiceCollection AddAuth0Authentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Bind settings
        var auth0Settings = new Auth0Settings();
        configuration.Bind(Auth0Settings.SectionName, auth0Settings);
        services.Configure<Auth0Settings>(configuration.GetSection(Auth0Settings.SectionName));

        // ============================
        // JWT Bearer Authentication
        // ============================
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            // Auth0 uses https://{domain}/ as the authority
            options.Authority = auth0Settings.Authority;

            // The audience is the API identifier configured in Auth0
            options.Audience = auth0Settings.Audience;

            // Auth0 always uses HTTPS
            options.RequireHttpsMetadata = true;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = auth0Settings.Authority,
                ValidateAudience = true,
                ValidAudience = auth0Settings.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(1),
                NameClaimType = "name",
                RoleClaimType = $"{auth0Settings.RolesNamespace}/roles"
            };

            options.Events = new JwtBearerEvents
            {
                OnTokenValidated = context =>
                {
                    MapAuth0RolesToClaims(context, auth0Settings.RolesNamespace);
                    return Task.CompletedTask;
                },
                OnAuthenticationFailed = context =>
                {
                    if (context.Exception is SecurityTokenExpiredException)
                    {
                        context.Response.Headers["Token-Expired"] = "true";
                    }
                    return Task.CompletedTask;
                }
            };
        });

        services.AddAuthorizationBuilder()
            .AddPolicy("AdminOnly", policy =>
                policy.RequireRole("admin"))
            .AddPolicy("ManagerOrAdmin", policy =>
                policy.RequireRole("admin", "manager"))
            .AddPolicy("AllAuthenticated", policy =>
                policy.RequireAuthenticatedUser())

            .AddPolicy("CanRead", policy =>
                policy.RequireAssertion(ctx =>
                    ctx.User.HasClaim("permissions", "inventory:read") ||
                    ctx.User.HasClaim("scope", "inventory:read") ||
                    ctx.User.IsInRole("admin") ||
                    ctx.User.IsInRole("manager") ||
                    ctx.User.IsInRole("viewer")))
            .AddPolicy("CanWrite", policy =>
                policy.RequireAssertion(ctx =>
                    ctx.User.HasClaim("permissions", "inventory:write") ||
                    ctx.User.HasClaim("scope", "inventory:write") ||
                    ctx.User.IsInRole("admin") ||
                    ctx.User.IsInRole("manager")))
            .AddPolicy("CanDelete", policy =>
                policy.RequireAssertion(ctx =>
                    ctx.User.HasClaim("permissions", "inventory:delete") ||
                    ctx.User.HasClaim("scope", "inventory:delete") ||
                    ctx.User.IsInRole("admin")));

        return services;
    }

    private static void MapAuth0RolesToClaims(
        TokenValidatedContext context,
        string rolesNamespace)
    {
        if (context.Principal?.Identity is not ClaimsIdentity identity) return;

        // Map custom namespaced roles → ClaimTypes.Role
        var roleClaims = identity.FindAll($"{rolesNamespace}/roles").ToList();
        foreach (var roleClaim in roleClaims)
        {
            if (!identity.HasClaim(ClaimTypes.Role, roleClaim.Value))
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, roleClaim.Value));
            }
        }

        // Also map Auth0 "permissions" claim (from RBAC)
        var permissionClaims = identity.FindAll("permissions").ToList();
        foreach (var perm in permissionClaims)
        {
            if (!identity.HasClaim("permissions", perm.Value))
            {
                identity.AddClaim(new Claim("permissions", perm.Value));
            }
        }
    }

}