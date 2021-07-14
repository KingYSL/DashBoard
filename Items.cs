using System;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Binance.Net;
using Binance.Net.Objects.Spot;
using CryptoExchange.Net.Authentication;
using DashBoard;
using OxyPlot;
namespace DashBoard{
    class Items
    {
        public int ID { set; get; }

        public string Text { set; get; }


        public override string ToString()
        {
            return Text;
        }

    }
    
    
}

