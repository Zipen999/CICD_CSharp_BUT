
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["OpenLibraryWrapper/OpenLibraryWrapper.csproj", "OpenLibraryWrapper/"]
COPY ["OpenLibraryWrapper_UT/OpenLibraryWrapper_UT.csproj", "OpenLibraryWrapper_UT/"]
COPY ["DtoAbstractLayer/DtoAbstractLayer.csproj", "DtoAbstractLayer/"]
COPY ["JsonReader/JsonReader.csproj", "JsonReader/"]
COPY ["LibraryDTO/LibraryDTO.csproj", "LibraryDTO/"]
COPY ["OpenLibraryClient/OpenLibraryClient.csproj", "OpenLibraryClient/"]
COPY ["StubbedDTO/StubbedDTO.csproj", "StubbedDTO/"]


RUN dotnet restore "OpenLibraryWrapper/OpenLibraryWrapper.csproj"
COPY . .
WORKDIR "/src/OpenLibraryWrapper"
RUN dotnet build "OpenLibraryWrapper.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OpenLibraryWrapper.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OpenLibraryWrapper.dll"]