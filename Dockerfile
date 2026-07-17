
# ---------------------------------------------------------------------------
# Stage 1 — Build the Angular frontend
# ---------------------------------------------------------------------------
# We use a Node.js image to install dependencies and run the Angular CLI
# production build. The output is static HTML/CSS/JS files.
FROM node:22-alpine AS frontend-build

WORKDIR /app/client

# Copy only the package files first — this lets Docker cache the npm install
# step. If package.json hasn't changed, Docker skips reinstalling everything.
COPY StoreManagementSystem.Client/package.json StoreManagementSystem.Client/package-lock.json ./

# Install all dependencies (including devDependencies needed for the build)
RUN npm ci

# Now copy the rest of the Angular source code
COPY StoreManagementSystem.Client/ ./

# Build the Angular app in production mode
# This uses environment.prod.ts which sets apiUrl to '/api' (relative path)
RUN npx ng build --configuration production


# ---------------------------------------------------------------------------
# Stage 2 — Build the .NET API backend
# ---------------------------------------------------------------------------
# We use the .NET 10 SDK image to restore NuGet packages and publish the API.
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS backend-build

WORKDIR /src

# Copy only the .csproj and solution files first for cached restore
COPY StoreManagementSystem.slnx ./
COPY StoreManagementSystem.API/StoreManagementSystem.API.csproj ./StoreManagementSystem.API/

# Restore NuGet packages (cached if .csproj hasn't changed)
RUN dotnet restore StoreManagementSystem.API/StoreManagementSystem.API.csproj

# Copy the full API source code
COPY StoreManagementSystem.API/ ./StoreManagementSystem.API/

# Publish the API in Release mode to /app/publish
RUN dotnet publish StoreManagementSystem.API/StoreManagementSystem.API.csproj \
    -c Release \
    -o /app/publish \
    --no-restore


# ---------------------------------------------------------------------------
# Stage 3 — Runtime image (final, lightweight)
# ---------------------------------------------------------------------------
# The ASP.NET runtime image is much smaller than the SDK — it only contains
# what's needed to RUN the app, not build it.
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app

# Copy the published .NET API from stage 2
COPY --from=backend-build /app/publish ./

# Copy the Angular production build into the API's wwwroot folder.
# The .NET API serves these static files via UseStaticFiles().
# Angular 19 outputs to: dist/<project-name>/browser/
COPY --from=frontend-build /app/client/dist/store-management-system.client/browser/ ./wwwroot/

# Set the ASP.NET environment to "Docker" so it loads appsettings.Docker.json
ENV ASPNETCORE_ENVIRONMENT=Docker

# ASP.NET Core 10 listens on port 8080 by default in containers
EXPOSE 8080

# Start the API (which also serves the Angular frontend)
ENTRYPOINT ["dotnet", "StoreManagementSystem.API.dll"]
