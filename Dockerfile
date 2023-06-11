FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
USER root
WORKDIR /app

RUN apt-get update && apt-get install -y git

RUN git clone https://github.com/ZID-Automat/Backend-ZIDAutomat
WORKDIR /app/Backend-ZIDAutomat/ZID.Automat
RUN dotnet restore ZID.Automat.sln
RUN dotnet publish ZID.Automat.sln -c Release -o out


FROM mcr.microsoft.com/dotnet/sdk:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/Backend-ZIDAutomat/ZID.Automat/out .
ENTRYPOINT  ["dotnet","ZID.Automat.Api.dll"]