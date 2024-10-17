# 이 Dockerfile은 Visual Studio에서 빠른 디버깅을 위해 컨테이너를 설정하는 방법을 설명합니다.
# 자세한 내용은 https://aka.ms/customizecontainer를 참조하세요.

# 1단계: 기본 이미지 설정
# 이 이미지는 ASP.NET Core 런타임을 포함합니다.
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# 2단계: 빌드 이미지 설정
# 이 이미지는 .NET SDK를 포함하며, 애플리케이션을 빌드하는 데 사용됩니다.
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["aspnetChat_server.csproj", "."]
RUN dotnet restore "./aspnetChat_server.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./aspnetChat_server.csproj" -c $BUILD_CONFIGURATION -o /app/build

# 3단계: 게시 이미지 설정
# 빌드된 애플리케이션을 게시 폴더로 복사합니다.
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./aspnetChat_server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 4단계: 최종 이미지 설정
# 게시된 애플리케이션을 최종 이미지로 복사하고 실행합니다.
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "aspnetChat_server.dll"]
