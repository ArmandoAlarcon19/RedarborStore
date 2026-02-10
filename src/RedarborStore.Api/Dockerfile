FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY RedarborStore.sln ./

COPY src/RedarborStore.Domain/RedarborStore.Domain.csproj                 src/RedarborStore.Domain/
COPY src/RedarborStore.Application/RedarborStore.Application.csproj       src/RedarborStore.Application/
COPY src/RedarborStore.Infrastructure/RedarborStore.Infrastructure.csproj src/RedarborStore.Infrastructure/
COPY src/RedarborStore.Api/RedarborStore.Api.csproj                       src/RedarborStore.Api/
COPY tests/RedarborStore.Domain.Tests/RedarborStore.Domain.Tests.csproj   tests/RedarborStore.Domain.Tests/
COPY tests/RedarborStore.Application.Tests/RedarborStore.Application.Tests.csproj   tests/RedarborStore.Application.Tests/
RUN dotnet restore

COPY src/ src/
COPY tests/ tests/

WORKDIR /src/src/RedarborStore.Api
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app

RUN adduser --disabled-password --gecos "" app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

RUN chown -R app:app /app
USER app


ENTRYPOINT ["dotnet", "RedarborStore.Api.dll"]