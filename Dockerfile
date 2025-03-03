#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/AttendanceSystem.API/AttendanceSystem.API.csproj", "src/AttendanceSystem.API/"]
COPY ["src/AttendanceSystem.Infrastructure/AttendanceSystem.Infrastructure.csproj", "src/AttendanceSystem.Infrastructure/"]
COPY ["src/AttendanceSystem.Application/AttendanceSystem.Application.csproj", "src/AttendanceSystem.Application/"]
COPY ["src/AttendanceSystem.Domain/AttendanceSystem.Domain.csproj", "src/AttendanceSystem.Domain/"]
COPY ["src/AttendanceSystem.Persistence/AttendanceSystem.Persistence.csproj", "src/AttendanceSystem.Persistence/"]
RUN dotnet restore "src/AttendanceSystem.API/AttendanceSystem.API.csproj"
COPY . .
WORKDIR "/src/src/AttendanceSystem.API"
RUN dotnet build "AttendanceSystem.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AttendanceSystem.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AttendanceSystem.API.dll"]