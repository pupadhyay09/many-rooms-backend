# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Copy csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and publish
COPY . ./
RUN dotnet publish -c Release -o /app/out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

# Copy published app
COPY --from=build /app/out ./

# Persistent data protection keys directory
VOLUME ["/keys"]

# Set environment variable (optional but useful)
ENV ASPNETCORE_ENVIRONMENT=Production

# Expose port
EXPOSE 80

# Entry point
ENTRYPOINT ["dotnet", "ManyRoomStudio.dll"]
