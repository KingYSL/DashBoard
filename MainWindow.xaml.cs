using Binance.Net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OxyPlot;
using OxyPlot.Wpf;
using OxyPlot.Series;

namespace Dashboard_1._1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            PlotModel pm = new PlotModel();
            InitializeComponent();
           // this.
            ChartData chartData = new ChartData();
            //pm.Series.Add(new FunctionSeries(Math.Cos, 0, 10, 0.1, "cos(x)"));


        }

        //private void InitializeComponent()
       // {
           // throw new NotImplementedException();
       // }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            plotView.Model = ChartData.PlotModel1(VolumeStyle.Stacked);
        }

        
    }
}
