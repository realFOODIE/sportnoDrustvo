#izberi osnovno sliko za .NET 6.0 ASP.NET, ki bo služila kot temelj za naš container.
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
#nastavi delovni direktorij znotraj containerja na /app.
WORKDIR /app
#nastavi container, da bo poslušal na portih 80 in 443 (HTTP in HTTPS).
EXPOSE 80
EXPOSE 443


#izberi osnovno sliko za .NET 6.0 SDK, ki bo uporabljena za gradnjo aplikacije.
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
#nastavi delovni direktorij znotraj containerja na /src.
WORKDIR /src
#kopiraj csproj datoteko projekta v /src direktorij in obnovi vse odvisnosti.
COPY ["sportnoDrustvo.csproj", "./"]
RUN dotnet restore "sportnoDrustvo.csproj"
#kopiraj vse ostale datoteke projekta v /src direktorij.
COPY . .
#ponovno nastavi delovni direktorij na /src.
WORKDIR "/src"
#zgradi aplikacijo v release načinu in izhod shranjuj v /app/build.
RUN dotnet build "sportnoDrustvo.csproj" -c Release -o /app/build

#naredi novo sliko za objavo aplikacije.
FROM build AS publish
#objavi aplikacijo v release načinu in izhod shranjuj v /app/publish.
RUN dotnet publish "sportnoDrustvo.csproj" -c Release -o /app/publish

#uporabi osnovno sliko, ki smo jo definirali na začetku.
FROM base AS final
#nastavi delovni direktorij na /app.
WORKDIR /app
#kopiraj iz /app/publish v trenutni direktorij, ki je zopet /app.
COPY --from=publish /app/publish .
#ko zaženemo container, bo ta zagnal našo .NET aplikacijo.
ENTRYPOINT ["dotnet", "sportnoDrustvo.dll"]
