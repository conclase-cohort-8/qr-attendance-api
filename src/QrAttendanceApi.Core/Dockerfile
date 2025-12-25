FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/QrAttendanceApi.Core/QrAttendanceApi.Core.csproj", "QrAttendanceApi.Core/"]
COPY ["src/QrAttendanceApi.Domain/QrAttendanceApi.Domain.csproj", "QrAttendanceApi.Domain/"]
COPY ["src/QrAttendanceApi.Infrastructure/QrAttendanceApi.Infrastructure.csproj", "QrAttendanceApi.Infrastructure/"]
COPY ["src/QrAttendanceApi.Application/QrAttendanceApi.Application.csproj", "QrAttendanceApi.Application/"]

RUN dotnet restore "QrAttendanceApi.Core/QrAttendanceApi.Core.csproj"
COPY /src .
WORKDIR "/src/QrAttendanceApi.Core"
RUN dotnet build "QrAttendanceApi.Core.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "QrAttendanceApi.Core.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QrAttendanceApi.Core.dll"]