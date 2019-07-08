using Android.App;
using Android.Graphics;
using Android.Views;
using MikePhil.Charting.Charts;
using MikePhil.Charting.Components;
using MikePhil.Charting.Data;

namespace DigiTrailApp.Helpers
{
    /// <summary>
    /// Model and Manager for ChartManager
    /// </summary>
    static class ChartManager
    {
        /// <summary>
        /// LineChart object to draw a line chart from LineData object
        /// </summary>
        public static LineChart LineChart { get; set; }

        /// <summary>
        /// LineData uses LineDataSet object(s) to create a line data for LineChart
        /// </summary>
        public static LineData LineData { get; set; }

        /// <summary>
        /// Main LineDataSet used to populate y and x axes
        /// </summary>
        public static LineDataSet ElevationSet { get; set; }

        /// <summary>
        /// Data set with a single entry used to show device's location on chart
        /// </summary>
        public static LineDataSet LocationSet { get; set; }

        /// <summary>
        /// Left side Y axis
        /// </summary>
        public static YAxis LeftAxis { get; set; }

        /// <summary>
        /// Right side Y axis
        /// </summary>
        public static YAxis RightAxis { get; set; }

        /// <summary>
        /// X axis
        /// </summary>
        public static XAxis XAxis { get; set; }

        /// <summary>
        /// An integer used to determ mininum width of the chart
        /// </summary>
        public static int Width { get; private set; }

        /// <summary>
        /// An integer used to determ mininum height of the chart
        /// </summary>
        public static int Height { get; private set; }

        /// <summary>
        /// A boolean to track if the chart is expanded
        /// </summary>
        private static bool IsExpanded { set; get; }

        /// <summary>
        /// Sets styling to line chart
        /// </summary>
        public static void InitChartStyling()
        {
            LineChart.SetNoDataText(Application.Context.GetString(Resource.String.errorNoChartData));
            LineChart.SetNoDataTextColor(Color.White);
            LineChart.Legend.Enabled = false;
            LineChart.Description.Enabled = false;
            LineChart.SetTouchEnabled(false);
        }

        /// <summary>
        /// Sets styling to line data sets
        /// </summary>
        public static void InitDataSetStylings()
        {
            ElevationSet.LineWidth = 2;
            ElevationSet.Color = Color.DarkOrange;
            ElevationSet.SetDrawCircles(false);
            ElevationSet.HighlightEnabled = false;

            LocationSet.SetDrawCircles(true);
            LocationSet.CircleRadius = 3;
            LocationSet.SetCircleColor(Color.Red);
            LocationSet.HighlightEnabled = false;
        }

        /// <summary>
        /// Sets styling to left and right y-axis and x-axis
        /// </summary>
        public static void InitAxisStyling()
        {
            LeftAxis.TextColor = Color.White;
            LeftAxis.GridColor = Color.White;
            LeftAxis.SetLabelCount(4, false);

            RightAxis.Enabled = false;

            XAxis.Position = XAxis.XAxisPosition.Bottom;
            XAxis.TextColor = Color.White;
            XAxis.GridColor = Color.White;
        }

        /// <summary>
        /// Set width and height of line chart
        /// </summary>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        public static void SetChartWidthAndHeight(int width, int height)
        {
            Width = width;
            Height = height;

            LineChart.SetMinimumHeight(Height);
            LineChart.SetMinimumWidth(Width);
        }

        /// <summary>
        /// Set visibility state of location circle
        /// </summary>
        /// <param name="state">Entry is visible if true</param>
        public static void SetLocationEntryVisibilityState(bool state)
        {
            LocationSet.SetDrawCircles(state);
            RefreshChart();
        }

        /// <summary>
        /// Set visivility state of line chart
        /// </summary>
        /// <param name="state">Chart is visible if true</param>
        public static void SetChartVisibilityState(bool state)
        {
            if (!state)
            {
                LineChart.Visibility = ViewStates.Invisible;
            }
            else
            {
                LineChart.Visibility = ViewStates.Visible;
            }
        }
        
        /// <summary>
        /// Expanded and shrink LineChart
        /// </summary>
        public static void SetExpandedState(bool state)
        {
            if (state && !IsExpanded)
            {
                float width = Width * 1.5F;
                float height = Height * 1.5F;

                LineChart.SetMinimumWidth((int)width);
                LineChart.SetMinimumHeight((int)height);
                IsExpanded = true;
            }
            else if (!state && IsExpanded)
            {
                LineChart.SetMinimumWidth(Width);
                LineChart.SetMinimumHeight(Height);
                IsExpanded = false;
            }

            RefreshChart();
        }

        /// <summary>
        /// Creates new lineData object from provided LineDataSet array and set it as data of LineChart 
        /// </summary>
        /// <param name="lineDataSets">Array of LineDataSets</param>
        public static void CreateAndSetLineChartData(LineDataSet[] lineDataSets)
        {
            LineChart.Data = new LineData(lineDataSets);
        }

        /// <summary>
        /// Updates value of first index of location set
        /// </summary>
        /// <param name="xPosition">new x coordinate</param>
        /// <param name="yPosition">new y coordinate</param>
        public static void UpdateLocationDataSet(float xPosition, float yPosition)
        {
            if (LocationSet != null && LocationSet.GetEntryForIndex(0) != null)
            {
                // Remove the first and only entry from set
                LocationSet.RemoveFirst();
                // Add new entry to set
                LocationSet.AddEntry(new Entry(xPosition, yPosition));

                RefreshChart();
            }
        }

        /// <summary>
        /// Call when chart data has been changed and must be redrawn
        /// </summary>
        public static void RefreshChart()
        {
            LineChart.NotifyDataSetChanged();
            LineChart.Invalidate();
        }

        /// <summary>
        /// Call to clear line chart
        /// </summary>
        public static void ClearChart()
        {
            LineChart.Clear();
            RefreshChart();
        }
    }
}