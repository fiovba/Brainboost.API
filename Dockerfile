FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 10000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["BrainBoost.API/BrainBoost.API.csproj", "BrainBoost.API/"]
RUN dotnet restore "BrainBoost.API/BrainBoost.API.csproj"
COPY . .
WORKDIR "/src/BrainBoost.API"
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BrainBoost.API.dll"]