#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
RUN apt-get update
RUN apt-get install -y curl
RUN apt-get install -y libpng-dev libjpeg-dev curl libxi6 build-essential libgl1-mesa-glx
RUN curl -sL https://deb.nodesource.com/setup_lts.x | bash -
RUN apt-get install -y nodejs

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
RUN apt-get update
RUN apt-get install -y curl
RUN apt-get install -y libpng-dev libjpeg-dev curl libxi6 build-essential libgl1-mesa-glx
RUN curl -sL https://deb.nodesource.com/setup_lts.x | bash -
RUN apt-get install -y nodejs
WORKDIR /src
COPY ["Sample.DigitalNotice.Web/Sample.DigitalNotice.Web.csproj", "Sample.DigitalNotice.Web/"]
COPY ["Sample.DigitalNotice.Bll/Sample.DigitalNotice.Bll.csproj", "Sample.DigitalNotice.Bll/"]
COPY ["Sample.DigitalNotice.Common/Sample.DigitalNotice.Common.csproj", "Sample.DigitalNotice.Common/"]
COPY ["Sample.DigitalNotice.Dal/Sample.DigitalNotice.Dal.csproj", "Sample.DigitalNotice.Dal/"]
COPY ["Sample.DigitalNotice.Di/Sample.DigitalNotice.Di.csproj", "Sample.DigitalNotice.Di/"]
RUN dotnet restore "Sample.DigitalNotice.Web/Sample.DigitalNotice.Web.csproj"
COPY . .
WORKDIR "/src/Sample.DigitalNotice.Web"
RUN dotnet build "Sample.DigitalNotice.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sample.DigitalNotice.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sample.DigitalNotice.Web.dll"]
