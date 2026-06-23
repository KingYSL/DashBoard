# DashBoard

A Windows desktop app that pulls live **Binance candlestick (kline) data** and renders it as an interactive candlestick chart. Pick a timeframe, and the app fetches the most recent data available and plots it with classic green/red candles, scroll-to-zoom, and hover tooltips.

> One of my earlier projects — built to learn the Binance API, charting, and C# WinForms. It's a chart viewer.

![Language](https://img.shields.io/badge/language-C%23-178600)
![Framework](https://img.shields.io/badge/.NET%20Framework-4.7.2-512BD4)
![Platform](https://img.shields.io/badge/platform-Windows-0078D6)

---

## Features

- 📈 **Live candlestick charts** sourced directly from the Binance API via [Binance.Net](https://github.com/JKorf/Binance.Net)
- ⏱️ **Selectable timeframes** — 15 minute, 1 hour, 4 hour, and 24 hour candles
- 🔢 **Automatic range** — on each load it walks back from the current time and pulls up to the Binance limit of 1,000 candles
- 🟢🔴 **Standard candle styling** — green for up, red for down, with open/close triangle marks
- 🔍 **Mouse-wheel zoom** on the time axis
- 💬 **Hover tooltips** showing price and timestamp at the cursor
- 🔐 **Simple login gate** before the chart window opens

## Tech stack

| Area | Used |
|------|------|
| Language | C# |
| Runtime | .NET Framework 4.7.2 |
| UI | Windows Forms |
| Charting | `System.Windows.Forms.DataVisualization.Charting`, OxyPlot |
| Exchange API | Binance.Net / CryptoExchange.Net |
| Serialization | Newtonsoft.Json |

## Getting started

### Prerequisites

- Windows
- Visual Studio (2017 or later) with the **.NET desktop development** workload
- .NET Framework 4.7.2 developer pack

### Build & run

```bash
git clone https://github.com/KingYSL/DashBoard.git
```

1. Open `DashBoard.csproj` in Visual Studio.
2. Restore NuGet packages (`packages.config` is used — Visual Studio will prompt, or run *Restore NuGet Packages* from the Solution menu).
3. Press **F5** to build and run.

### First launch

The app opens to a login window. Enter the default credentials (`Admin` / `Admin`) to reach the chart, then choose a timeframe from the dropdown to load data.

## Configuration

A few things are currently set in code:

- **API key** — `Form1.cs` initializes the Binance client with placeholder credentials:
  ```csharp
  ApiCredentials = new ApiCredentials("INSERT KEY", "INSERT KEY")
  ```
  Public market data (klines) works without a key, but adding your own enables authenticated endpoints if you extend the app.
- **Trading pair** — the symbol is hardcoded to `BTCUSDT` inside `GetKlines()`. Change it there to chart a different market.
- **Candle limit** — capped at `1000` to match the Binance API's per-request maximum.

## How it works

The dropdown selection maps to a Binance kline interval. The app computes a start time by stepping back from `DateTime.Now` far enough to fill ~1,000 candles at that interval, requests the range from Binance, and feeds each candle's open / high / low / close into the chart series.

## Notes & caveats

This is a learning project, so a couple of things are intentionally rough:

- The login uses hardcoded credentials
- Don't commit a real API key; treat the placeholder as a reminder to load secrets from config or environment variables.
- Nothing here is financial advice or a recommendation to trade.



**Tags:** `C#` · `WinForms` · `Binance` · `Binance.Net` · `candlestick` · `trading` · `API`
