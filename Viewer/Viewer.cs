using Engine.Components;
using Engine.Tracers;
using System.Diagnostics;
using System.Numerics;

namespace Viewer
{
    public partial class Viewer : Form
    {
        private Scene? scene;

        private Tracer? Tracer;

        private const float MINBRIGHTNESS = 0.05f;

        public Viewer()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.Clear(Color.Black);

            if (Tracer == null || Tracer.CollisionBuffer == null) return;

            Bitmap bitmap = new Bitmap(Width, Height);
            for (int x = 0; x < Tracer.Width; x++)
            {
                for (int y = 0; y < Tracer.Height; y++)
                {
                    if (!Tracer.CollisionBuffer[x, y].DidCollide)
                    {
                        continue;
                    }
                    const int MIN_RGB = (int)(MINBRIGHTNESS * 255);
                    Brush pixelBrush = new SolidBrush(Color.FromArgb(Math.Max(MIN_RGB, (int)(Tracer.CollisionBuffer[x, y].Color.R)), Math.Max(MIN_RGB, (int)(Tracer.CollisionBuffer[x, y].Color.G)), Math.Max(MIN_RGB, (int)(Tracer.CollisionBuffer[x, y].Color.B))));
                    bitmap.SetPixel(x, y, Color.FromArgb(Math.Max(MIN_RGB, (int)(Tracer.CollisionBuffer[x, y].Color.R)), Math.Max(MIN_RGB, (int)(Tracer.CollisionBuffer[x, y].Color.G)), Math.Max(MIN_RGB, (int)(Tracer.CollisionBuffer[x, y].Color.B))));
                }
            }

            float imageAspect = (float)Tracer.Width / Tracer.Height;
            float formAspect = (float)Width / Height;

            int drawWidth, drawHeight, drawX, drawY;
            if (imageAspect > formAspect)
            {
                // Image is wider than the form
                drawWidth = Width;
                drawHeight = (int)(Width / imageAspect);
                drawX = 0;
                drawY = (Height - drawHeight) / 2; // Center vertically
            }
            else
            {
                // Image is taller than the form
                drawHeight = Height;
                drawWidth = (int)(Height * imageAspect);
                drawX = (Width - drawWidth) / 2; // Center horizontally
                drawY = 0;
            }
            drawHeight = Math.Min(Height, drawHeight);
            drawWidth = Math.Min(Width, drawWidth);
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            e.Graphics.DrawImage(bitmap, new Rectangle(drawX, drawY, drawWidth, drawHeight), new Rectangle(0, 0, Tracer.Width, Tracer.Height), GraphicsUnit.Pixel);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Create a new Scene and load in a sample
            scene = SampleScenes.PlantOBJ();
            // Create a new camera facing the positive Z direction
            Camera camera = new Camera(new Vector3(0, 0, -4), Vector3.UnitZ, Vector3.UnitY, 60.0f);
            scene.AddCamera(camera);

            // Calculates lighting for the scene
            scene.PreCalculateLighting();
            Tracer = new BVHTracer(scene);
            Tracer.Width = Width;
            Tracer.Height = Height;
            var sw = Stopwatch.StartNew();
            Tracer.GetCollisionBuffer();
            sw.Stop();
            Debug.WriteLine($"Successfully traced scene in {sw.ElapsedMilliseconds} ms");

            Invalidate();
        }

        private void Viewer_Resize(object sender, EventArgs e)
        {
            Invalidate();
        }
    }
}
