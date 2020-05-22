#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["PIoT2020/Server/PIoT2020.Server.csproj", "PIoT2020/Server/"]
COPY ["PIoT2020/Client/PIoT2020.Client.csproj", "PIoT2020/Client/"]
COPY ["PIoT2020/Shared/PIoT2020.Shared.csproj", "PIoT2020/Shared/"]
COPY ["PIoT2020.BIZ/PIoT2020.BIZ.csproj", "PIoT2020.BIZ/"]
COPY ["PIoT2020.DAL.API/PIoT2020.DAL.API.csproj", "PIoT2020.DAL.API/"]
COPY ["PIoT2020.COMMON/PIoT2020.COMMON.csproj", "PIoT2020.COMMON/"]
COPY ["PIoT2020.DAL/PIoT2020.DAL.csproj", "PIoT2020.DAL/"]
RUN dotnet restore "PIoT2020/Server/PIoT2020.Server.csproj"
COPY . .
WORKDIR "/src/PIoT2020/Server"
RUN dotnet build "PIoT2020.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PIoT2020.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PIoT2020.dll"]