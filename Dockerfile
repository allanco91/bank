FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build-env
WORKDIR /src
COPY ["*.csproj", ""]
RUN dotnet restore 
COPY . .
WORKDIR /src
RUN dotnet build -c Release -o /app/build

FROM build-env AS publish
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS deploy
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApplication3.dll"]