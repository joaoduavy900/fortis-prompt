FROM mcr.microsoft.com/dotnet/sdk:8.0

# Install system dependencies for Playwright
RUN apt-get update && apt-get install -y \
    libglib2.0-0 \
    libnspr4 \
    libnss3 \
    libdbus-1-3 \
    libatk1.0-0 \
    libatk-bridge2.0-0 \
    libcups2 \
    libxcb1 \
    libxkbcommon0 \
    libatspi2.0-0 \
    libx11-6 \
    libxcomposite1 \
    libxdamage1 \
    libxext6 \
    libxfixes3 \
    libxrandr2 \
    libgbm1 \
    libcairo2 \
    libpango-1.0-0 \
    libasound2 \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app

COPY . .

RUN dotnet restore

RUN dotnet build

RUN dotnet tool install --global Microsoft.Playwright.CLI

ENV PATH="$PATH:/root/.dotnet/tools"

RUN playwright install chromium

CMD ["dotnet", "test", "--logger", "console;verbosity=normal", "--logger", "trx;LogFileName=test_results.trx"]
