###################
# Build stage
###################

# Pull dotnet sdk
FROM mcr.microsoft.com/dotnet/sdk:8.0.403-alpine3.20-amd64 AS build

# Set working directory
WORKDIR /src

COPY . ./

# Try to restore the project
RUN dotnet restore

# Try to build the project
RUN dotnet build --no-restore -c Release

# Try to publish the project into publish folder
RUN dotnet publish --no-restore --no-build -c Release -o publish

###################
# Final stage
###################

# Pull aspnet core runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0.403-alpine3.20-amd64

# Set environment
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:8700

# Set working directory
WORKDIR /app

# Copy publish folder from build stage
COPY --from=build /app/publish ./

# Run the application.
ENTRYPOINT ["dotnet", "PenomyAPI.Presentation.FastEndpointBasedApi.dll"]

