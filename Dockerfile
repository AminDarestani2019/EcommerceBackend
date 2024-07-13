#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
ENV ASPNETCORE_ENVIRONMENT=Development
WORKDIR /app
EXPOSE 5085

# Add this line to copy the PFX file
COPY saelectronics.pfx /app/saelectronics.pfx

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Web/Web.csproj", "src/Web/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]
COPY ["src/Application/Application.csproj", "src/Application/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]

RUN dotnet dev-certs https --clean && dotnet dev-certs https --export-path /app/cert.pfx -p Sha@341401
RUN chmod 644 /app/cert.pfx

RUN dotnet restore "src/Web/Web.csproj"
COPY . .
RUN dotnet build "src/Web/Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "src/Web/Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Web.dll","--environment = Development"]