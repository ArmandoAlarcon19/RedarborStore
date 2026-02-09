FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY RedarborStore.slnx ./

COPY src/RedarborStore.Domain/RedarborStore.Domain.csproj                 src/RedarborStore.Domain/
COPY src/RedarborStore.Application/RedarborStore.Application.csproj       src/RedarborStore.Application/
COPY src/RedarborStore.Infrastructure/RedarborStore.Infrastructure.csproj src/RedarborStore.Infrastructure/
COPY src/RedarborStore.Api/RedarborStore.Api.csproj                       src/RedarborStore.Api/
COPY tests/RedarborStore.Domain.Tests/RedarborStore.Domain.Tests.csproj   tests/RedarborStore.Domain.Tests/

RUN dotnet restore

COPY src/ src/
COPY tests/ tests/

WORKDIR /src/src/RedarborStore.Api
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

USER app

EXPOSE 8080

ENTRYPOINT ["dotnet", "RedarborStore.Api.dll"]