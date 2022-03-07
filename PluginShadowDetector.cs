using GameReaderCommon;
using SimHub.Plugins;
using System;
using System.Drawing;
using System.Windows.Media;
using Color = System.Drawing.Color;

namespace User.PluginShadowDetector
{
    [PluginDescription("Plugin to detect if driving in shadows")]
    [PluginAuthor("Gmasil")]
    [PluginName("Shadow Detector")]
    public class PluginShadowDetector : IPlugin, IDataPlugin, IWPFSettingsV2
    {
        public PluginShadowDetectorSettings Settings;
        public ScreenUtils Screen;

        /// <summary>
        /// Instance of the current plugin manager
        /// </summary>
        public PluginManager PluginManager { get; set; }

        /// <summary>
        /// Gets the left menu icon. Icon must be 24x24 and compatible with black and white display.
        /// </summary>
        public ImageSource PictureIcon => this.ToIcon(Properties.Resources.sdkmenuicon);

        /// <summary>
        /// Gets a short plugin title to show in left menu. Return null if you want to use the title as defined in PluginName attribute.
        /// </summary>
        public string LeftMenuTitle => null;

        /// <summary>
        /// Called one time per game data update, contains all normalized game data,
        /// raw data are intentionnally "hidden" under a generic object type (A plugin SHOULD NOT USE IT)
        ///
        /// This method is on the critical path, it must execute as fast as possible and avoid throwing any error
        ///
        /// </summary>
        /// <param name="pluginManager"></param>
        /// <param name="data">Current game data, including current and previous data frame.</param>
        public void DataUpdate(PluginManager pluginManager, ref GameData data)
        {
        }

        /// <summary>
        /// Called at plugin manager stop, close/dispose anything needed here !
        /// Plugins are rebuilt at game change
        /// </summary>
        /// <param name="pluginManager"></param>
        public void End(PluginManager pluginManager)
        {
            // Save settings
            this.SaveCommonSettings("GeneralSettings", Settings);
            Screen.CleanUp();
        }

        /// <summary>
        /// Returns the settings control, return null if no settings control is required
        /// </summary>
        /// <param name="pluginManager"></param>
        /// <returns></returns>
        public System.Windows.Controls.Control GetWPFSettingsControl(PluginManager pluginManager)
        {
            return new PluginShadowDetectorSettingsControl(this);
        }

        /// <summary>
        /// Called once after plugins startup
        /// Plugins are rebuilt at game change
        /// </summary>
        /// <param name="pluginManager"></param>
        public void Init(PluginManager pluginManager)
        {
            SimHub.Logging.Current.Info("Starting Shadow Detector Plugin");

            // Load settings
            Settings = this.ReadCommonSettings("GeneralSettings", () => new PluginShadowDetectorSettings());
            Screen = new ScreenUtils();

            this.AttachDelegate("IsInShadow", IsInShadow);
            this.AttachDelegate("BrightnessValue", GetBrightnessValue);
            this.AttachDelegate("BrightnessThreshold", GetBrightnessThreshold);

            this.AddAction("AddCursorPositionToReferencePoints", (a, b) =>
            {
                Settings.Points.Add(Screen.GetCursorPosition());
            });

            this.AddAction("IncreaseBrightnessThreshold", (a, b) =>
            {
                Settings.BrightnessThreshold++;
            });

            this.AddAction("DecreaseBrightnessThreshold", (a, b) =>
            {
                Settings.BrightnessThreshold--;
            });
        }

        private Boolean IsInShadow()
        {
            PluginManager pluginManager = PluginManager.GetInstance();
            int brightnessValue = (int)pluginManager.GetPropertyValue("PluginShadowDetector.BrightnessValue");
            return brightnessValue < GetBrightnessThreshold();
        }

        private int GetBrightnessThreshold()
        {
            return Settings.BrightnessThreshold;
        }

        private int GetBrightnessValue() {
            if(Settings.Points.Count == 0)
            {
                return -1;
            }
            int totalBrightness = 0;
            foreach(Point p in Settings.Points)
            {
                Color color = Screen.GetPixelColor(p);
                int value = (color.R + color.G + color.B) / 3;
                totalBrightness += (value / Settings.Points.Count);
            }
            return totalBrightness;
        }
    }
}
