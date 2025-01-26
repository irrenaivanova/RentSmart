# Use the official .NET SDK image for building the application (using .NET 8)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the solution file and restore dependencies
COPY *.sln ./

# Copy all project files (instead of individual copies, to handle dependencies between projects)
COPY Web/RentSmart.Web/*.csproj Web/RentSmart.Web/
COPY Web/RentSmart.Web.Infrastructure/*.csproj Web/RentSmart.Web.Infrastructure/
COPY Services/RentSmart.Services/*.csproj Services/RentSmart.Services/
COPY Services/RentSmart.Services.Messaging/*.csproj Services/RentSmart.Services.Messaging/
COPY Services/RentSmart.Services.Data/*.csproj Services/RentSmart.Services.Data/
COPY Services/RentSmart.Services.Mapping/*.csproj Services/RentSmart.Services.Mapping/
COPY Data/RentSmart.Data/*.csproj Data/RentSmart.Data/
COPY Data/RentSmart.Data.Models/*.csproj Data/RentSmart.Data.Models/
COPY Data/RentSmart.Data.Common/*.csproj Data/RentSmart.Data.Common/
COPY RentSmart.Common/*.csproj RentSmart.Common/
COPY Tests/Sandbox/*.csproj Tests/Sandbox/
COPY Tests/RentSmart.Services.Data.Tests/*.csproj Tests/RentSmart.Services.Data.Tests/
COPY Tests/RentSmart.Web.Tests/*.csproj Tests/RentSmart.Web.Tests/
COPY Web/RentSmart.Web.ViewModels/*.csproj Web/RentSmart.Web.ViewModels/
COPY Z_ImotScraper/*.csproj Z_ImotScraper/
# Restore dependencies
RUN dotnet restore

# Copy the entire source code
COPY . .

# Publish the application as a self-contained release
RUN dotnet publish Web/RentSmart.Web/RentSmart.Web.csproj -c Release -o /publish

# Use a lightweight runtime image for running the application (using .NET 8)
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /publish .

# Expose port 8080 for Railway
EXPOSE 8080

# Specify the command to run the application
ENTRYPOINT ["dotnet", "RentSmart.Web.dll"]
