using Microsoft.OpenApi;
using Scalar.AspNetCore;
using RedarborStore.Application.Features.Categories.Commands.CreateCategory;
using RedarborStore.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<CreateCategoryCommand>());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var auth0Domain = builder.Configuration["Auth0:Domain"]
                  ?? "dev-to137li8vk1740xb.us.auth0.com/api/v2/";

builder.Services.AddAuthorization(options => {
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("admin"));
});


builder.Services.AddOpenApi("v1", options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info = new OpenApiInfo
        {
            Title = "RedarborStore API",
            Version = "v1",
            Description = "Inventory Management API secured with Auth0 OAuth2",
            Contact = new OpenApiContact
            {
                Name = "RedarborStore Team",
                Email = "support@redarborstore.com"
            }
        };
        return Task.CompletedTask;
    });

    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
        document.Components.SecuritySchemes["OAuth2"] = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,
            Description = "Auth0 OAuth2 Authorization Code Flow with PKCE",
            Flows = new OpenApiOAuthFlows
            {
                AuthorizationCode = new OpenApiOAuthFlow
                {
                    AuthorizationUrl = new Uri($"https://{auth0Domain}/authorize"),
                    TokenUrl = new Uri($"https://{auth0Domain}/oauth/token"),
                    Scopes = new Dictionary<string, string>
                    {
                        { "openid", "OpenID Connect" },
                        { "profile", "User profile information" },
                        { "email", "User email" },
                        { "inventory:read", "Read inventory data" },
                        { "inventory:write", "Create and modify inventory data" },
                        { "inventory:delete", "Delete inventory data" }
                    }
                }
            }
        };

        return Task.CompletedTask;
    });

    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        var requirement = new OpenApiSecurityRequirement();
        var scheme = new OpenApiSecuritySchemeReference("OAuth2");
       
        requirement.Add(scheme, new List<string>
        {
            "openid", "profile", "email", "inventory:read"
        });

        document.Security ??= new List<OpenApiSecurityRequirement>();
        document.Security.Add(requirement);

        return Task.CompletedTask;
    });

    options.AddOperationTransformer((operation, context, cancellationToken) =>
    {
        var metadata = context.Description.ActionDescriptor.EndpointMetadata;
        var hasAuthorize = metadata
            .Any(m => m is Microsoft.AspNetCore.Authorization.AuthorizeAttribute);
        var hasAllowAnonymous = metadata
            .Any(m => m is Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute);

        if (hasAuthorize && !hasAllowAnonymous)
        {
            operation.Security ??= new List<OpenApiSecurityRequirement>();

            var requirement = new OpenApiSecurityRequirement();
            var scheme = new OpenApiSecuritySchemeReference("OAuth2");

            requirement.Add(scheme, new List<string>
            {
                "openid", "profile", "inventory:read"
            });
            operation.Security.Add(requirement);

            operation.Responses.TryAdd("401", new OpenApiResponse
            {
                Description = "Unauthorized — Token missing or invalid"
            });
            operation.Responses.TryAdd("403", new OpenApiResponse
            {
                Description = "Forbidden — Insufficient permissions"
            });
        }

        return Task.CompletedTask;
    });
});

builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowSwagger", policy =>
        {
            policy
                .WithOrigins(
                    "http://localhost:5001",
                    "http://localhost:8081")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    });


var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference(options =>
        {
            options
                .WithTitle("RedarborStore API")
                .WithTheme(ScalarTheme.Moon)
                .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
                .AddPreferredSecuritySchemes("OAuth2")
                .WithOAuth2Authentication(oauth =>
                {
                    oauth.ClientId = builder.Configuration["Auth0:ClientId"];
                });
        });
    }

    app.UseCors("AllowSwagger");
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
