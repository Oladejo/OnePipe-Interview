#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS base
#FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 5000
#EXPOSE 443

#FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /src
COPY . .
RUN dotnet restore "OnePipe.API/OnePipe.API.csproj"
#COPY . .
#WORKDIR "/src/."
RUN dotnet build "OnePipe.API/OnePipe.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OnePipe.API/OnePipe.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5000  
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OnePipe.API.dll"]