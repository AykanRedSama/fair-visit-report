# Use the full .NET SDK to restore, build and publish the API.
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

# Set the working directory for the build stage.
WORKDIR /src

# Copy the project file separately to reuse the restored packages
# when only application source files have changed.
COPY FairVisitReport.Api.csproj .
RUN dotnet restore

# Copy the remaining source code and publish a release build.
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Use the smaller ASP.NET Core runtime for the final container.
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

# Set the working directory of the running API.
WORKDIR /app

# Configure the API to listen on port 8080 inside the container.
ENV ASPNETCORE_URLS=http://+:8080

# Document the port used by the API.
EXPOSE 8080

# Copy only the published application from the build stage.
COPY --from=build /app/publish .

# Start the .NET Web API when the container starts.
ENTRYPOINT ["dotnet", "FairVisitReport.Api.dll"]