FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY HomeLocalStorage.Core/ HomeLocalStorage.Core/
COPY HomeLocalStorage.DataAccess/ HomeLocalStorage.DataAccess/
COPY HomeLocalStorage.Services/ HomeLocalStorage.Services/
COPY HomeLocalStorage/ HomeLocalStorage/

RUN dotnet publish HomeLocalStorage/HomeLocalStorage.csproj -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "HomeLocalStorage.dll"]