using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Binance.Net;
using Binance.Net.Objects.Spot;
using CryptoExchange.Net.Authentication;
using OxyPlot;

namespace DashBoard
{
    public partial class Form1 : Form
    {

       

        public Form1()
        {
            this.ShowInTaskbar = false;
            InitializeComponent();

            BinanceClient.SetDefaultOptions(new BinanceClientOptions()
            {
                ApiCredentials = new ApiCredentials("INSERT KEY", "INSERT KEY"),
                // LogVerbosity = LogVerbosity.Debug,
                //   LogWriters = new List<TextWriter> { Console.Out }

            });

           
            comboBox1.DataSource = new Items[]
            {

                new Items{ ID = 0, Text = "15Min" },

                new Items{ ID = 1, Text = "1Hr" },

                new Items{ ID = 2, Text = "4Hr" },

                new Items{ ID = 3, Text = "24Hr"}
            };

           
            comboBox1.SelectedIndexChanged += new System.EventHandler(comboBox1_SelectedIndexChanged);
            //comboBox1.SelectedIndex = -1;
           // chart1.MouseWheel += chart1_MouseWheel;

            
        }


    

        

        

        public void CandleStick()
        {
            
            

        }
        private void chart1_MouseWheel(object sender, MouseEventArgs e)
        {
            try
            {
                Axis xAxis = chart1.ChartAreas[0].AxisX;
                double xMin = xAxis.ScaleView.ViewMinimum;
                double xMax = xAxis.ScaleView.ViewMaximum;
                double xPixelPos = xAxis.PixelPositionToValue(e.Location.X);

                if (e.Delta < 0)//0 && FZoomLevel > 0)
                {
                  
                    var posXStart = xAxis.PixelPositionToValue(e.Location.X) - (xMax - xMin) / 0.75;
                    var posXFinish = xAxis.PixelPositionToValue(e.Location.X) + (xMax - xMin) / 1;
                    //var posYStart = yAxis.PixelPositionToValue(e.Location.Y) - (yMax - yMin) / 0.75;
                   // var posYFinish = yAxis.PixelPositionToValue(e.Location.Y) + (yMax - yMin) / 1;

                    xAxis.ScaleView.Zoom(posXStart, posXFinish);
                    // yAxis.ScaleView.Zoom(posYStart, posYFinish);
                }
                else if (e.Delta > 0)
                {
                    // Scrolled up, meaning zoom in
                    var posXStart = xAxis.PixelPositionToValue(e.Location.X) - (xMax - xMin) * 0.75;
                    var posXFinish = xAxis.PixelPositionToValue(e.Location.X) + (xMax - xMin) * 1;
                    xAxis.ScaleView.Zoom(posXStart, posXFinish);
                    
                }
            }
            catch { }
        }


        

        private void chart1_DoubleClick(object sender, EventArgs e)
        {
            //CandleStick();
        }

       

        

        

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            const int LIMIT = 1000;
            int day = 24, halfDay = 12, quarterDay = 6, FourHr = 4, hour = 1;
            double fifteenMin = 0.25;
            
            
            

            DateTime START, END;
           
            //int id = (int)comboBox1.SelectedIndex;
            //comboBox1.SelectedIndex = id;
           
            if(comboBox1.SelectedIndex==0)
            {
                chart1.ChartAreas[0].AxisY2.ScaleView.ZoomReset();
                chart1.ChartAreas[0].AxisX.ScaleView.ZoomReset();
                chart1.Series.Clear();
                END = DateTime.Now;
            
                END.Date.ToShortDateString();
            
                double dat = LIMIT * fifteenMin / 24;
           
                DateTime date = END.AddDays(-(dat));
           
                START = date;
            
                START.Date.ToShortDateString();
               
                GetKlines(START, END, (Interval)3);
               



            }

            if (comboBox1.SelectedIndex == 1)
            {
                chart1.ChartAreas[0].AxisY2.ScaleView.ZoomReset();
                chart1.ChartAreas[0].AxisX.ScaleView.ZoomReset();
                chart1.Series.Clear();
                END = DateTime.Now;

                END.Date.ToShortDateString();

                int dat = LIMIT * hour / 24;

                DateTime date = END.AddDays(-(dat));

                START = date;

                START.Date.ToShortDateString();

                GetKlines(START, END, (Interval)5);
            }

            if (comboBox1.SelectedIndex == 2)
            {
                chart1.ChartAreas[0].AxisY2.ScaleView.ZoomReset();
                chart1.ChartAreas[0].AxisX.ScaleView.ZoomReset();
                chart1.Series.Clear();
                END = DateTime.Now;

                END.Date.ToShortDateString();

                double dat = LIMIT * FourHr / 24;

                DateTime date = END.AddDays(-(dat));

                START = date;

                START.Date.ToShortDateString();

                GetKlines(START, END, (Interval)7);
            }

            if (comboBox1.SelectedIndex == 3)
            {
                chart1.ChartAreas[0].AxisY2.ScaleView.ZoomReset();
                chart1.ChartAreas[0].AxisX.ScaleView.ZoomReset();
                chart1.Series.Clear();
                END = DateTime.Now;

                END.Date.ToShortDateString();

                double dat = LIMIT * day / 24;

                DateTime date = END.AddDays(-(dat));

                START = date;

                START.Date.ToShortDateString();

                GetKlines(START, END, (Interval)11);
            }


        }


        private void GetKlines(DateTime start, DateTime end, Interval inter)
        {
            //Interval inter = new Interval();
            BinanceClient client = new BinanceClient();
            var candles = client.Spot.Market.GetKlines(symbol: "BTCUSDT", interval: (Binance.Net.Enums.KlineInterval)inter, startTime: start, endTime: end, limit: 1000).Data.ToList();
            int Curr = 0;
            Series Price15 = new Series("Price15"); // <<== make sure to name the series "price"
            chart1.Series.Add(Price15);
            chart1.Series["Price15"].ChartType = SeriesChartType.Candlestick;

            // Set the style of the open-close marks
            chart1.Series["Price15"]["OpenCloseStyle"] = "Triangle";

            // Show both open and close marks
            chart1.Series["Price15"]["ShowOpenClose"] = "Both";

            // Set point width
            chart1.Series["Price15"]["PointWidth"] = "0.5";

            // Set colors bars
            chart1.Series["Price15"]["PriceUpColor"] = "Green"; // <<== use text indexer for series
            chart1.Series["Price15"]["PriceDownColor"] = "Red"; // <<== use text indexer for series
            if (inter == (Interval)3)
            {
                Curr = 160;
            }    
            
            for (int i = 0; i <candles.Count; i++)
            {
                
                    // adding date and high 
                    chart1.Series["Price15"].Points.AddXY(candles[i].OpenTime, candles[i].High);
                    // adding low
                    chart1.Series["Price15"].Points[i].YValues[1] = (double)candles[i].Low;
                    //adding open
                    chart1.Series["Price15"].Points[i].YValues[2] = (double)candles[i].Open;
                    // adding close
                    chart1.Series["Price15"].Points[i].YValues[3] = (double)candles[i].Close;
                    //chart1.ChartAreas[0].RecalculateAxesScale();
                }
            
           

            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].AxisY2.ScaleView.Zoomable = true;
            chart1.MouseWheel += chart1_MouseWheel;
            //chart1.MouseMove += chart1_MouseMove;
            
            //chart1.GetToolTipText += chart1_GetToolTipText;
            chart1.Series[0].XValueType = ChartValueType.DateTime; 
            chart1.ChartAreas[0].AxisX.IntervalType = (DateTimeIntervalType)DateRangeType.DayOfMonth;
           // chart1.ChartAreas[0].AxisX.LabelStyle.IntervalOffsetType = DateTimeIntervalType.Days;
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "MM-dd-hh";

            label1.Text = candles.Count.ToString();

        }





        Point? prevPosition = null;
        ToolTip tooltip = new ToolTip();
        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            var pos = e.Location;
            if (prevPosition.HasValue && pos == prevPosition.Value)
                return;
            tooltip.RemoveAll();
            prevPosition = pos;
            var results = chart1.HitTest(pos.X, pos.Y, false, ChartElementType.DataPoint); // set ChartElementType.PlottingArea for full area, not only DataPoints
            foreach (var result in results)
            {
                if (result.ChartElementType == ChartElementType.DataPoint) // set ChartElementType.PlottingArea for full area, not only DataPoints
                {
                    double yVal = result.ChartArea.AxisY.PixelPositionToValue(pos.Y);
                    var XVal = result.ChartArea.AxisX.PixelPositionToValue(pos.X);
                    tooltip.Show(yVal.ToString()+DateTime.FromOADate(XVal).ToString(), chart1, pos.X, pos.Y - 15);
                   // tooltip.Show(DateTime.FromOADate(XVal).ToString(), chart1, pos.X, pos.Y );


                }



            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }
    }
    }

       
    

    


    public enum Interval
    {
        //
        // Summary:
        //     1m
        OneMinute = 0,
        //
        // Summary:
        //     3m
        ThreeMinutes = 1,
        //
        // Summary:
        //     5m
        FiveMinutes = 2,
        //
        // Summary:
        //     15m
        FifteenMinutes = 3,
        //
        // Summary:
        //     30m
        ThirtyMinutes = 4,
        //
        // Summary:
        //     1h
        OneHour = 5,
        //
        // Summary:
        //     2h
        TwoHour = 6,
        //
        // Summary:
        //     4h
        FourHour = 7,
        //
        // Summary:
        //     6h
        SixHour = 8,
        //
        // Summary:
        //     8h
        EightHour = 9,
        //
        // Summary:
        //     12h
        TwelveHour = 10,
        //
        // Summary:
        //     1d
        OneDay = 11,
        //
        // Summary:
        //     3d
        ThreeDay = 12,
        //
        // Summary:
        //     1w
        OneWeek = 13,
        //
        // Summary:
        //     1M
        OneMonth = 14
    }





