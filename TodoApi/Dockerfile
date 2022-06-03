FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY . .
# RUN dotnet restore "TodoApi.csproj"
# RUN dotnet build "TodoApi.csproj" -c Release -o /app/build
RUN dotnet publish TodoApi.csproj \
  --output /publish  \
  --runtime linux-x64 \
  --self-contained=true \
  -p:UseAppHost=true \
  -p:PublishReadyToRun=true

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS publish
WORKDIR /app
COPY --from=build /publish .

COPY dd-dotnet-apm.deb .
RUN mkdir -p /var/log/datadog && \
  mkdir -p /opt/datadog && \
  dpkg -i dd-dotnet-apm.deb

ENV ASPNETCORE_ENVIRONMENT=Development

# Run the createLogPath script on Linux to ensure the automatic instrumentation logs are genereated without permission isues
RUN /opt/datadog/createLogPath.sh

CMD ["dotnet", "TodoApi.dll"]

