FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
COPY ["Shuvi/Language", "root/.net/Language"]
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Shuvi/Shuvi.csproj", "Shuvi/"]
RUN dotnet restore "Shuvi/Shuvi.csproj"
COPY . .
WORKDIR "/src/Shuvi"
RUN dotnet build "Shuvi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Shuvi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["./Shuvi"]
