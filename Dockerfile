# Use the official .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copy the solution file and restore dependencies
COPY *.sln ./
COPY Web/RentSmart.Web/*.csproj Web/RentSmart.Web/
COPY Web/RentSmart.Web.Infrastructure/*.csproj Web/RentSmart.Web.Infrastructure/
COPY Services/RentSmart.Services/*.csproj Services/RentSmart.Services/
COPY Data/RentSmart.Data/*.csproj Data/RentSmart.Data/
COPY Data/RentSmart.Data.Common/*.csproj Data/RentSmart.Data.Common/
COPY RentSmart.Common/*.csproj RentSmart.Common/

# Restore dependencies
RUN dotnet restore Web/RentSmart.Web/RentSmart.Web.csproj

# Copy the entire project directory
COPY . .

# Publish the application as a self-contained release
RUN dotnet publish Web/RentSmart.Web/RentSmart.Web.csproj -c Release -o /publish

# Use a lightweight runtime image for running the application
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /publish .

# Expose port 8080 for Railway
EXPOSE 8080

# Specify the command to run the application
ENTRYPOINT ["dotnet", "RentSmart.Web.dll"]