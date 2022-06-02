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

ENV CORECLR_ENABLE_PROFILING=1
ENV CORECLR_PROFILER={846F5F1C-F9AE-4B07-969E-05C26BC060D8}
ENV CORECLR_PROFILER_PATH=/opt/datadog/linux-x64/Datadog.Trace.ClrProfiler.Native.so
ENV DD_DOTNET_TRACER_HOME=/opt/datadog

# Run the createLogPath script on Linux to ensure the automatic instrumentation logs are genereated without permission isues
RUN /opt/datadog/createLogPath.sh

CMD ["dotnet", "TodoApi.dll"]

