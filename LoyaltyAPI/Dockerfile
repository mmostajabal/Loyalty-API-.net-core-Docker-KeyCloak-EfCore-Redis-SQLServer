# Use the ASP.NET Core base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the csproj files and restore dependencies
COPY ["LoyaltyAPI/LoyaltyAPI.csproj", "LoyaltyAPI/"]
COPY ["LoyaltyModels/LoyaltyModels.csproj", "LoyaltyModels/"]
COPY ["LoyaltyServices/LoyaltyServices.csproj", "LoyaltyServices/"]
COPY ["LoyaltyShared/LoyaltyShared.csproj", "LoyaltyShared/"]
RUN dotnet restore "LoyaltyAPI/LoyaltyAPI.csproj"

# Copy the rest of the source code
COPY . .

# Build the application
WORKDIR "/src/LoyaltyAPI"
RUN dotnet build "LoyaltyAPI.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "LoyaltyAPI.csproj" -c Release -o /app/publish

# Final stage/image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LoyaltyAPI.dll"]
