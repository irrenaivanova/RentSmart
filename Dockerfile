# Use the official .NET SDK image to build the solution
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory
WORKDIR /src

# Copy the solution file and restore the dependencies
COPY ["RentSmart.sln"]
COPY ["Data/RentSmart.Data/RentSmart.Data.csproj", "Data/RentSmart.Data/"]
COPY ["Data/RentSmart.Data.Common/RentSmart.Data.Common.csproj", "Data/RentSmart.Data.Common/"]
COPY ["Data/RentSmart.Data.Models/RentSmart.Data.Models.csproj", "Data/RentSmart.Data.Models/"]
COPY ["RentSmart.Common/RentSmart.Common.csproj", "RentSmart.Common/"]
COPY ["Services/RentSmart.Services/RentSmart.Services.csproj", "Services/RentSmart.Services/"]
COPY ["Servies/RentSmart.Services.Data/RentSmart.Services.Data.csproj", "Servies/RentSmart.Services.Data/"]
COPY ["Servies/RentSmart.Services.Mapping/RentSmart.Services.Mapping.csproj", "Servies/RentSmart.Services.Mapping/"]
COPY ["Servies/RentSmart.Services.Mapping/RentSmart.Services.Messaging.csproj", "Servies/RentSmart.Services.Messaging/"]
COPY ["Tests/RentSmart.Services.Data.Tests/RentSmart.Services.Data.Tests.csproj", "Tests/RentSmart.Services.Data.Tests/"]
COPY ["Tests/RentSmart.Web.Tests/RentSmart.Web.Tests.csproj", "Tests/RentSmart.Web.Tests/"]
COPY ["Web/RentSmart.Web/RentSmart.Web.csproj", "Web/RentSmart.Web/"]
COPY ["Web/RentSmart.Web.Infrastructure/RentSmart.Web.Infrastructure.csproj", "Web/RentSmart.Web.Infrastructure/"]
COPY ["Web/RentSmart.Web.ViewModels/RentSmart.Web.ViewModels.csproj", "Web/RentSmart.Web.ViewModels/"]
COPY ["Z_ImotScraper/Z_ImotScraper.csproj", "Z_ImotScraper/"]

# Restore NuGet dependencies for the entire solution
RUN dotnet restore "RentSmart.sln"

# Publish the app to the /app directory
FROM build AS publish
RUN dotnet publish "RentSmart.sln" -c Release -o /app

# Define the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

# Set the working directory in the container
WORKDIR /app

# Copy the published app from the previous stage
COPY --from=publish /app .

# Expose the port that your application will run on
EXPOSE 8080

# Set the entry point for the container to run the application
ENTRYPOINT ["dotnet", "RentSmart.Web.dll"]
