using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;


namespace ChartApp.Messages
{
    public class AddSeries
    {
        public Series Series { get; private set; }

        public AddSeries(Series series )
        {
            Series = series;
        }
    }

    public class InitializeChart
    {
        public InitializeChart(Dictionary<string, Series> initialSeries)
        {
            InitialSeries = initialSeries;
        }

        public Dictionary<string, Series> InitialSeries { get; private set; }
    }
}
