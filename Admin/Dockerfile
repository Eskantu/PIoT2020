#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Admin/Admin.csproj", "Admin/"]
COPY ["PIoT2020.BIZ/PIoT2020.BIZ.csproj", "PIoT2020.BIZ/"]
COPY ["PIoT2020.DAL.API/PIoT2020.DAL.API.csproj", "PIoT2020.DAL.API/"]
COPY ["PIoT2020.COMMON/PIoT2020.COMMON.csproj", "PIoT2020.COMMON/"]
COPY ["PIoT2020.DAL/PIoT2020.DAL.csproj", "PIoT2020.DAL/"]
RUN dotnet restore "Admin/Admin.csproj"
COPY . .
WORKDIR "/src/Admin"
RUN dotnet build "Admin.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Admin.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Admin.dll"]