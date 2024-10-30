using System.Drawing;

namespace Engine
{
    public class PixelColor
    {
        public int r;
        public int R
        {
            get { return r; }
            set { r = Math.Clamp(value, 0, 255); }
        }

        public int g;
        public int G
        {
            get { return g; }
            set { g = Math.Clamp(value, 0, 255); }
        }

        public int b;
        public int B
        {
            get { return b; }
            set { b = Math.Clamp(value, 0, 255); }
        }

        public Color AsColor()
        {
            return Color.FromArgb(R, G, B);
        }

        public static PixelColor FromRGB(int r, int g, int b)
        {
            return new PixelColor() { R = r, G = g, B = b };
        }

        public static PixelColor operator *(PixelColor color, float value)
        {
            return new PixelColor()
            {
                R = (int)(color.R * value),
                G = (int)(color.G * value),
                B = (int)(color.B * value)
            };
        }
    }
}
