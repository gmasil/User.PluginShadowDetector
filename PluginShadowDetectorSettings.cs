using System.Drawing;
using System.Collections.Generic;

namespace User.PluginShadowDetector
{
    /// <summary>
    /// Settings class, make sure it can be correctly serialized using JSON.net
    /// </summary>
    public class PluginShadowDetectorSettings
    {
        public int BrightnessThreshold = 128;
        public List<Point> Points = new List<Point>();
    }
}
