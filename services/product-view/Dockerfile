# =========================
# Build stage
# =========================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Copy project file first (better caching)
COPY *.sln ./
COPY . .

# Restore dependencies
RUN dotnet restore

# Build + publish
RUN dotnet publish -c Release -o /app/publish --no-restore


# =========================
# Runtime stage
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app

# Copy published output
COPY --from=build /app/publish .

# ASP.NET Core default port
EXPOSE 8080

# Optional: make Kestrel listen on container port
ENV ASPNETCORE_URLS=http://+:8080

# CHANGE THIS to your actual DLL name
ENTRYPOINT ["dotnet", "GraphProductView.dll"]