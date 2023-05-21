FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
COPY ["Shuvi-Rewrite/Language", "root/.net/Language"]
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Shuvi-Rewrite/Shuvi.csproj", "Shuvi-Rewrite/"]
RUN dotnet restore "Shuvi-Rewrite/Shuvi.csproj"
COPY . .
WORKDIR "/src/ScheduleSender"
RUN dotnet build "ScheduleSender.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ScheduleSender.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["./Shuvi-Rewrite"]
