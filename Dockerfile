# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Copy the project file and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the entire project and publish it
COPY . ./
RUN dotnet publish -c Release -o /app/out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

# Copy published files from the build stage
COPY --from=build /app/out .

# Serve static files (ensure app.UseStaticFiles() is in Program.cs)
EXPOSE 80

ENTRYPOINT ["dotnet", "ManyRoomStudio.dll"]
