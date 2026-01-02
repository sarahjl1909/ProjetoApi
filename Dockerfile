# Estágio de Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ProjetoApi.csproj", "."]
RUN dotnet restore "./ProjetoApi.csproj"
COPY . .
RUN dotnet publish "ProjetoApi.csproj" -c Release -o /app/publish

# Estágio de Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expor a porta que a aplicação usa internamente
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "ProjetoApi.dll"]