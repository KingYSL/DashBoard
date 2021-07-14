using Binance.Net;
using Binance.Net.Objects.Spot;
using CryptoExchange.Net.Authentication;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dashboard_1._1
{

    public class ChartData
    {
        public static PlotModel Candlestick()
        {
            BinanceClient client = new BinanceClient();

            BinanceClient.SetDefaultOptions(new BinanceClientOptions()
            {
                ApiCredentials = new ApiCredentials("R18P8TOnbJhqmGlHFWUGPy5fIU2gjv2DXuajSJC0DLEmd0Pme157p9x2EU3DhViD", "bnSTYTPofdZyhcjH40cjJ0XtI1mwtdOJrkMcpPV9hL8scL7ZLMab1kx3aIPeNNi0"),
                // LogVerbosity = LogVerbosity.Debug,
                //   LogWriters = new List<TextWriter> { Console.Out }

            });
            var candles = client.Spot.Market.GetKlines(symbol: "SXPUSDT", interval: Binance.Net.Enums.KlineInterval.OneDay, startTime: new DateTime(2020, 03, 25), endTime: new DateTime(2021, 07, 12), limit: 1000).Data.ToList();
            var pm = new PlotModel { Title = "SXPUSDT DAILY" };
            DateTime dateTime = new DateTime(2021, 03, 25);
            DateTime dateTime1 = new DateTime(2021, 4, 24);
            var timeSpanAxis1 = new OxyPlot.Axes.DateTimeAxis { Position = AxisPosition.Bottom };
            pm.Axes.Add(timeSpanAxis1);
            var linearAxis1 = new OxyPlot.Axes.LinearAxis { Position = AxisPosition.Left };
            pm.Axes.Add(linearAxis1);
            var n = 5;
            //var items = HighLowItemGenerator.MRProcess(n).ToArray();



            var series = new CandleStickSeries
            {
                Color = OxyColors.Black,
                IncreasingColor = OxyColors.DarkGreen,
                DecreasingColor = OxyColors.Red,
                DataFieldX = "Time",
                DataFieldHigh = "H",
                DataFieldLow = "L",
                DataFieldOpen = "O",
                DataFieldClose = "C",
                TrackerFormatString =
                                     "High: {2:0.00}\nLow: {3:0.00}\nOpen: {4:0.00}\nClose: {5:0.00}",
                //ItemsSource = candles
            };



            for (int i = 0; i < candles.Count; i++)
            {

                series.Items.Add(new HighLowItem((double)candles[i].OpenTime.ToOADate(), (double)candles[i].High, (double)candles[i].Low, (double)candles[i].Open, (double)candles[i].Close));
            }



            timeSpanAxis1.Minimum = dateTime.ToOADate();
            timeSpanAxis1.Maximum = dateTime1.ToOADate();

            linearAxis1.Minimum = (double)candles.Skip(0).Take(1).Select(x => x.Low).Min();
            linearAxis1.Maximum = (double)candles.Skip(0).Take(1).Select(x => x.High).Max();

            pm.Series.Add(series);
            timeSpanAxis1.AxisChanged += (sender, e) => AdjustYExtent(series, timeSpanAxis1, linearAxis1);

            var controller = new PlotController();
            controller.UnbindAll();
            controller.BindMouseDown(OxyMouseButton.Left, OxyPlot.PlotCommands.PanAt);
            return pm;
        }



        private static void AdjustYExtent(CandleStickSeries series, OxyPlot.Axes.DateTimeAxis xaxis, OxyPlot.Axes.LinearAxis yaxis)
        {
            var xmin = xaxis.ActualMinimum;
            var xmax = xaxis.ActualMaximum;

            var istart = series.FindByX(xmin);
            var iend = series.FindByX(xmax, istart);

            var ymin = double.MaxValue;
            var ymax = double.MinValue;
            for (int i = istart; i <= iend; i++)
            {
                var bar = series.Items[i];
                ymin = Math.Min(ymin, bar.Low);
                ymax = Math.Max(ymax, bar.High);
            }

            var extent = ymax - ymin;
            var margin = extent * 0.10;

            yaxis.Zoom(ymin - margin, ymax + margin);
        }


        public static PlotModel PlotModel1(VolumeStyle style, bool naturalY = false,
            bool naturalV = false)

        {
            BinanceClient client = new BinanceClient();

            BinanceClient.SetDefaultOptions(new BinanceClientOptions()
            {
                ApiCredentials = new ApiCredentials("", ""),
                // LogVerbosity = LogVerbosity.Debug,
                //   LogWriters = new List<TextWriter> { Console.Out }

            });
            var candles = client.Spot.Market.GetKlines(symbol: "SXPUSDT", interval: Binance.Net.Enums.KlineInterval.OneDay, startTime: new DateTime(2020, 03, 25), endTime: new DateTime(2021, 07, 12), limit: 1000).Data.ToList();
            var pm = new PlotModel { Title = "SXPUSDT DAILY" };
            var series = new CandleStickAndVolumeSeries
            {
                PositiveColor = OxyColors.DarkGreen,
                NegativeColor = OxyColors.Red,
                PositiveHollow = false,
                NegativeHollow = false,
                SeparatorColor = OxyColors.Gray,
                SeparatorLineStyle = LineStyle.Dash,
                VolumeStyle = style
            };
            List<double> VolumeBar = new List<double>();
            for (int i = 0; i < candles.Count; i++)
            {



                series.Append(new OhlcvItem((double)candles[i].OpenTime.ToOADate(), (double)candles[i].High, (double)candles[i].Low, (double)candles[i].Open, (double)candles[i].Close, (double)candles[i].TakerBuyBaseVolume, (double)candles[i].QuoteVolume));
            }
            var Istart = 0;
            var Iend = 100 ;
            var Ymin = series.Items.Skip(Istart).Take(Iend - Istart + 1).Select(x => x.Low).Min();
            var Ymax = series.Items.Skip(Istart).Take(Iend - Istart + 1).Select(x => x.High).Max();
            var Xmin = series.Items[Istart].X;
            var Xmax = series.Items[Iend].X;
            var timeAxis = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = Xmin,
                Maximum = Xmax
            };
            var barAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Key = series.BarAxisKey,
                StartPosition = 0.25,
                EndPosition = 1.0,
                Minimum = naturalY ? double.NaN : Ymin,
                Maximum = naturalY ? double.NaN : Ymax
            };
            var volAxis = new OxyPlot.Axes.LinearAxis
            {
                Position = AxisPosition.Left,
                Key = series.VolumeAxisKey,
                StartPosition = 0.0,
                EndPosition = 0.22,
                Minimum = naturalV ? double.NaN : 0,
                Maximum = naturalV ? double.NaN : 5000
            };

            switch (style)
            {
                case VolumeStyle.None:
                    barAxis.Key = null;
                    barAxis.StartPosition = 0.0;
                    pm.Axes.Add(timeAxis);
                    pm.Axes.Add(barAxis);
                    break;

                case VolumeStyle.Combined:
                case VolumeStyle.Stacked:
                    pm.Axes.Add(timeAxis);
                    pm.Axes.Add(barAxis);
                    pm.Axes.Add(volAxis);
                    break;

                case VolumeStyle.PositiveNegative:
                    volAxis.Minimum = naturalV ? double.NaN : -5000;
                    pm.Axes.Add(timeAxis);
                    pm.Axes.Add(barAxis);
                    pm.Axes.Add(volAxis);
                    break;


            }
            pm.Series.Add(series);

            if (naturalY == false)
            {
                timeAxis.AxisChanged += (sender, e) => AdjustYExtent(series, timeAxis, barAxis);
            }

            var controller = new PlotController();
            controller.UnbindAll();
            controller.BindMouseDown(OxyMouseButton.Left, PlotCommands.PanAt);
            return pm;
        }

        private static void AdjustYExtent(CandleStickAndVolumeSeries series, DateTimeAxis timeAxis, LinearAxis barAxis)
        {
            var xmin = timeAxis.ActualMinimum;
            var xmax = timeAxis.ActualMaximum;

            var istart = series.FindByX(xmin);
            var iend = series.FindByX(xmax, istart);

            var ymin = double.MaxValue;
            var ymax = double.MinValue;
            for (int i = istart; i <= iend; i++)
            {
                var bar = series.Items[i];
                ymin = Math.Min(ymin, bar.Low);
                ymax = Math.Max(ymax, bar.High);
            }

            var extent = ymax - ymin;
            var margin = extent * 0.10;

            barAxis.Zoom(ymin - margin, ymax + margin);
        }
    }





}






