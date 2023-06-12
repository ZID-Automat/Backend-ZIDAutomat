#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/ZID.Automat.Api/ZID.Automat.Api.csproj", "src/ZID.Automat.Api/"]
COPY ["src/ZID.Automat.Application/ZID.Automat.Application.csproj", "src/ZID.Automat.Application/"]
COPY ["src/ZID.Automat.AutoMapper/ZID.Automat.AutoMapper.csproj", "src/ZID.Automat.AutoMapper/"]
COPY ["src/ZID.Automat.Domain/ZID.Automat.Domain.csproj", "src/ZID.Automat.Domain/"]
COPY ["src/ZID.Automat.Dto/ZID.Automat.Dto.csproj", "src/ZID.Automat.Dto/"]
COPY ["src/ZID.Automat.Configuration/ZID.Automat.Configuration.csproj", "src/ZID.Automat.Configuration/"]
COPY ["src/ZID.Automat.Exceptions/ZID.Automat.Exceptions.csproj", "src/ZID.Automat.Exceptions/"]
COPY ["src/ZID.Automat.Repository/ZID.Automat.Repository.csproj", "src/ZID.Automat.Repository/"]
COPY ["src/ZID.Automat.Infrastructure/ZID.Automat.Infrastructure.csproj", "src/ZID.Automat.Infrastructure/"]
COPY ["src/ZID.Automat.Extension/ZID.Automat.DatabaseExtension.csproj", "src/ZID.Automat.Extension/"]
RUN dotnet restore "src/ZID.Automat.Api/ZID.Automat.Api.csproj"
COPY . .
WORKDIR "/src/src/ZID.Automat.Api"
RUN dotnet build "ZID.Automat.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ZID.Automat.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ZID.Automat.Api.dll"]