FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app/WebApiUser
COPY . .

RUN dotnet publish -c Release -o /app/WebApiUser/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app/Release
COPY --from=build /app/WebApiUser/publish .
ENTRYPOINT [ "dotnet", "WebApiUser.dll" ]