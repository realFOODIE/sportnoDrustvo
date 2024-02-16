# Uporabimo Microsoftov uradni image ASP.NET Core runtime kot osnovo za našo končno sliko
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Uporabimo Microsoftov uradni image SDK za izgradnjo naše aplikacije
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["sportnoDrustvo.csproj", "./"]
RUN dotnet restore "sportnoDrustvo.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "sportnoDrustvo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "sportnoDrustvo.csproj" -c Release -o /app/publish

# Kopiramo zgrajene datoteke iz faze 'publish' v osnovno sliko
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "sportnoDrustvo.dll"]
