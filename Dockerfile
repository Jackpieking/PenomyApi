###################
# Build stage
###################

# Pull dotnet sdk
FROM bitnami/dotnet-sdk:8.0.403-debian-12-r4 AS build

# Set working directory
WORKDIR /penomy

# Copy in essential files
COPY ./PenomyApi.sln .
COPY ./global.json .
COPY ./sync_csproj/src ./src/

# Try to restore the project
RUN dotnet restore

# Copy the rest
COPY . .

# Try to build the project
RUN dotnet build --no-restore -c Release

# Try to publish the project into publish folder
RUN dotnet publish --no-restore --no-build -c Release -o publish

###################
# Final stage
###################

# Pull aspnet core runtime
FROM bitnami/aspnet-core:8.0.10-debian-12-r2

EXPOSE 8700

# Set working directory
WORKDIR /app

# Copy publish folder from build stage
COPY --from=build /penomy/publish ./

# Run the application.
ENTRYPOINT ["dotnet", "PenomyAPI.Presentation.FastEndpointBasedApi.dll"]

