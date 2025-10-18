FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app
COPY . ./

# Restore dependencies
RUN dotnet restore
# Build the application
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/out .
# Expose the port the app runs on
EXPOSE 80
ENTRYPOINT ["dotnet", "AlkaShoes.dll"]


