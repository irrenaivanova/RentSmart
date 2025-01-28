# Use the official .NET SDK image to build the solution
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory
WORKDIR /src

# Copy the solution file and restore the dependencies
COPY ["RentSmart.sln", "./"]
COPY ["Data/RentSmart.Data/", "Data/RentSmart.Data/"]
COPY ["Data/RentSmart.Data.Common/", "Data/RentSmart.Data.Common/"]
COPY ["Data/RentSmart.Data.Models/", "Data/RentSmart.Data.Models/"]
COPY ["RentSmart.Common/", "RentSmart.Common/"]
COPY ["Services/RentSmart.Services/", "Services/RentSmart.Services/"]
COPY ["Services/RentSmart.Services.Data/", "Services/RentSmart.Services.Data/"]
COPY ["Services/RentSmart.Services.Mapping/", "Services/RentSmart.Services.Mapping/"]
COPY ["Services/RentSmart.Services.Messaging/", "Services/RentSmart.Services.Messaging/"]
COPY ["Tests/RentSmart.Services.Data.Tests/", "Tests/RentSmart.Services.Data.Tests/"]
COPY ["Tests/RentSmart.Web.Tests/", "Tests/RentSmart.Web.Tests/"]
COPY ["Web/RentSmart.Web.Infrastructure/", "Web/RentSmart.Web.Infrastructure/"]
COPY ["Web/RentSmart.Web.ViewModels/", "Web/RentSmart.Web.ViewModels/"]
COPY ["Web/RentSmart.Web/", "Web/RentSmart.Web/"]
COPY ["Rules.ruleset", "./"]
COPY ["stylecop.json", "./"]
COPY ["Web/RentSmart.Web/libman.json", "Web/RentSmart.Web/"]
COPY ["Web/RentSmart.Web/appsettings.json", "Web/RentSmart.Web/"]
COPY ["Web/RentSmart.Web/bundleconfig.json", "Web/RentSmart.Web/"]

# Restore NuGet dependencies for the entire solution
RUN dotnet restore "RentSmart.sln"

RUN dotnet build "Web/RentSmart.Web/RentSmart.Web.csproj" -c Release -o /app/build

# Publish the app to the /app directory
RUN dotnet publish "Web/RentSmart.Web/RentSmart.Web.csproj" -c Release -o /app/publish

# Define the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

# Set the working directory in the container
WORKDIR /app

# Copy the published app from the previous stage
COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "RentSmart.Web.dll"]

