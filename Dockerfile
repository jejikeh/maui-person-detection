FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /
COPY ["/Neural/Neural.Defaults/Neural.Defaults.csproj", "/Neural/Neural.Defaults/"]
COPY ["/Neural/Neural.Core/Neural.Core.csproj", "/Neural/Neural.Core/"]
COPY ["/Neural/Neural.Onnx/Neural.Onnx.csproj", "/Neural/Neural.Onnx/"]
COPY ["/Backend/PersonDetection.Backend.Web/PersonDetection.Backend.Web.csproj", "/Backend/PersonDetection.Backend.Web/"]
COPY ["/Backend/PersonDetection.Backend.Infrastructure/PersonDetection.Backend.Infrastructure.csproj", "/Backend/PersonDetection.Backend.Infrastructure/"]
COPY ["/Backend/PersonDetection.Backend.Application/PersonDetection.Backend.Application.csproj", "/Backend/PersonDetection.Backend.Application/"]
RUN dotnet restore "/Backend/PersonDetection.Backend.Web/PersonDetection.Backend.Web.csproj"
COPY . .
WORKDIR "/Backend/PersonDetection.Backend.Web/"
RUN dotnet build "PersonDetection.Backend.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PersonDetection.Backend.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# COPY localhost.crt /usr/local/share/ca-certificates
# RUN chmod 644 /usr/local/share/ca-certificates/localhost.crt && update-ca-certificates
ENTRYPOINT ["dotnet", "PersonDetection.Backend.Web.dll"]

RUN echo "deb http://deb.debian.org/debian/ bookworm main contrib" > /etc/apt/sources.list && \
    echo "deb-src http://deb.debian.org/debian/ bookworm main contrib" >> /etc/apt/sources.list && \
    echo "deb http://security.debian.org/ bookworm-security main contrib" >> /etc/apt/sources.list && \
    echo "deb-src http://security.debian.org/ bookworm-security main contrib" >> /etc/apt/sources.list
RUN sed -i'.bak' 's/$/ contrib/' /etc/apt/sources.list
RUN apt-get update; apt-get install -y ttf-mscorefonts-installer fontconfig