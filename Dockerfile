# Use the official .NET SDK image to build the solution
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Set the working directory
WORKDIR /src

# Copy the solution file and restore the dependencies
COPY ["Web/Web.sln", "Web/"]
COPY ["Data/Data.csproj", "Data/"]
COPY ["Services/Services.csproj", "Services/"]
COPY ["Tests/Tests.csproj", "Tests/"]
COPY ["RentSmart.Web/RentSmart.Web.csproj", "RentSmart.Web/"]
COPY ["RentSmart.Data.Models/RentSmart.Data.Models.csproj", "RentSmart.Data.Models/"]
COPY ["RentSmart.Services/RentSmart.Services.csproj", "RentSmart.Services/"]
COPY ["RentSmart.Services.Messaging/RentSmart.Services.Messaging.csproj", "RentSmart.Services.Messaging/"]
COPY ["RentSmart.Data/RentSmart.Data.csproj", "RentSmart.Data/"]
COPY ["RentSmart.Data.Common/RentSmart.Data.Common.csproj", "RentSmart.Data.Common/"]
COPY ["RentSmart.Common/RentSmart.Common.csproj", "RentSmart.Common/"]
COPY ["RentSmart.Web.Infrastructure/RentSmart.Web.Infrastructure.csproj", "RentSmart.Web.Infrastructure/"]
COPY ["RentSmart.Services.Data/RentSmart.Services.Data.csproj", "RentSmart.Services.Data/"]
COPY ["RentSmart.Services.Mapping/RentSmart.Services.Mapping.csproj", "RentSmart.Services.Mapping/"]
COPY ["Z_ImotScraper/Z_ImotScraper.csproj", "Z_ImotScraper/"]

# Restore NuGet dependencies for the entire solution
RUN dotnet restore "Web/Web.sln"

# Publish the app to the /app directory
FROM build AS publish
RUN dotnet publish "Web/Web.sln" -c Release -o /app

# Define the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final

# Set the working directory in the container
WORKDIR /app

# Copy the published app from the previous stage
COPY --from=publish /app .

# Expose the port that your application will run on
EXPOSE 80

# Set the entry point for the container to run the application
ENTRYPOINT ["dotnet", "RentSmart.Web.dll"]
