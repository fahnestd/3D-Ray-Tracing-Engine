using Engine.Components;
using Engine.Tracers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Renderer
{
    public class BitmapRenderer
    {
        private const float MINBRIGHTNESS = 0.05f;

        public static Bitmap RenderBitmap(Tracer Tracer, int Width, int Height)
        {
            Bitmap bitmap = new Bitmap(Width, Height);
            for (int x = 0; x < Tracer.Width; x++)
            {
                for (int y = 0; y < Tracer.Height; y++)
                {
                    if (!Tracer.CollisionBuffer[x, y].DidCollide)
                    {
                        bitmap.SetPixel(x, y, Color.Black);
                        continue;
                    }
                    const int MIN_RGB = (int)(MINBRIGHTNESS * 255);
                    bitmap.SetPixel(x, y, Color.FromArgb(Math.Max(MIN_RGB, (int)(Tracer.CollisionBuffer[x, y].Color.R)), Math.Max(MIN_RGB, (int)(Tracer.CollisionBuffer[x, y].Color.G)), Math.Max(MIN_RGB, (int)(Tracer.CollisionBuffer[x, y].Color.B))));
                }
            }
            return bitmap;
        }
    }
}
