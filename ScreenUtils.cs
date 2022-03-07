using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace User.PluginShadowDetector
{
    public class ScreenUtils
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);
        
        [DllImport("user32.dll")]
        static extern Int32 ReleaseDC(IntPtr hwnd, IntPtr hdc);
        
        [DllImport("gdi32.dll")]
        static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point lpPoint);

        private IntPtr hdc;

        public ScreenUtils()
        {
            hdc = GetDC(IntPtr.Zero);
        }

        public void CleanUp()
        {
            ReleaseDC(IntPtr.Zero, hdc);
        }

        public Point GetCursorPosition()
        {
            Point cursor = new Point();
            GetCursorPos(ref cursor);
            return cursor;
        }

        public Color GetPixelColorAtCursor()
        {
            return GetPixelColor(GetCursorPosition());
        }

        public Color GetPixelColor(Point P)
        {
            return GetPixelColor(P.X, P.Y);
        }

        public Color GetPixelColor(int x, int y)
        {
            uint pixel = GetPixel(this.hdc, x, y);
            Color color = Color.FromArgb((int)(pixel & 0x000000FF), (int)(pixel & 0x0000FF00) >> 8, (int)(pixel & 0x00FF0000) >> 16);
            return color;
        }
    }
}
