FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore

FROM build AS publish
RUN dotnet publish src/app/Edelstein.Application.Server/ -c Release -o /publish

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS runtime
WORKDIR /app
COPY --from=publish /publish .

ENTRYPOINT ["./Edelstein.Application.Server"]