using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Controls;

namespace User.PluginShadowDetector
{
    /// <summary>
    /// Logique d'interaction pour SettingsControlDemo.xaml
    /// </summary>
    public partial class PluginShadowDetectorSettingsControl : UserControl
    {
        public PluginShadowDetector Plugin { get; }

        public PluginShadowDetectorSettingsControl()
        {
            InitializeComponent();
        }

        public PluginShadowDetectorSettingsControl(PluginShadowDetector plugin) : this()
        {
            this.Plugin = plugin;
        }

        private void BtnSave_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Plugin.Settings.BrightnessThreshold = (int)BrightnessThresholdSlider.Value;
            Plugin.Settings.Points = new List<Point>();
            foreach (var Item in LstPoints.Items)
            {
                string[] split = Item.ToString().Split(',');
                Plugin.Settings.Points.Add(new Point(Int32.Parse(split[0]), Int32.Parse(split[1])));
            }
        }

        private void BtnDeletePoint_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (LstPoints.SelectedIndex == -1)
            {
                return;
            }
            List<string> selected = new List<string>();
            foreach(var item in LstPoints.SelectedItems)
            {
                selected.Add(item.ToString());
            }
            for (int i = selected.Count - 1; i >= 0; i--) {
                LstPoints.Items.Remove(selected[i]);
            }
        }

        private void BtnAddPoint_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            LblAddError.Visibility = System.Windows.Visibility.Hidden;
            string points = TxtPoint.Text;
            if (!points.Contains(","))
            {
                LblAddError.Visibility = System.Windows.Visibility.Visible;
                LblAddError.Content = "Invalid form, must contain comma seperator";
                return;
            }
            string[] split = points.Split(',');
            if (split.Length != 2)
            {
                LblAddError.Visibility = System.Windows.Visibility.Visible;
                LblAddError.Content = "Invalid form, must not contain more then one comma seperator";
                return;
            }
            if (!Int32.TryParse(split[0], out int x))
            {
                LblAddError.Visibility = System.Windows.Visibility.Visible;
                LblAddError.Content = "Invalid argument, could not parse " + split[0] + " as integer";
                return;
            }
            if (!Int32.TryParse(split[1], out int y))
            {
                LblAddError.Visibility = System.Windows.Visibility.Visible;
                LblAddError.Content = "Invalid argument, could not parse " + split[1] + " as integer";
                return;
            }
            LstPoints.Items.Add(x + ", " + y);
            TxtPoint.Text = "";
        }

        private void BrightnessThresholdSlider_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            BrightnessThresholdSlider.Value = Plugin.Settings.BrightnessThreshold;
        }

        private void TxtPoint_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            TxtPoint.Text = "";
        }

        private void LstPoints_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            LstPoints.Items.Clear();
            foreach (Point p in Plugin.Settings.Points)
            {
                LstPoints.Items.Add(p.X + ", " + p.Y);
            }
        }
    }
}
