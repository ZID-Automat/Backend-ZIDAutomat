FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
USER root
WORKDIR /app

RUN apt-get update && apt-get install -y git

COPY . . 
WORKDIR /app/ZID.Automat
RUN dotnet restore ZID.Automat.sln
RUN dotnet publish ZID.Automat.sln -c Release -o out



FROM mcr.microsoft.com/dotnet/sdk:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/ZID.Automat/out .






ENTRYPOINT ["dotnet", "ZID.Automat.Api.dll"]