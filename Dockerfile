# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o /app/out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Create the keys directory and give permissions
RUN mkdir /keys
RUN chmod -R 777 /keys

COPY --from=build /app/out .

# Expose HTTP port
EXPOSE 80

ENTRYPOINT ["dotnet", "ManyRoomStudio.dll"]
