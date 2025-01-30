using Engine.Components;
using Engine.Tracers;
using System.Diagnostics;
using System.Numerics;

namespace Viewer
{
    public partial class Viewer : Form
    {
        private Scene ?scene;

        private Tracer ?Tracer;

        private const float MINBRIGHTNESS = 0.05f;

        public Viewer()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
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
            e.Graphics.DrawImage(bitmap, new Point(0, 0));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Create a new Scene and load in a sample
            scene = SampleScenes.TeapotOBJ();
            // Create a new camera facing the positive Z direction
            Camera camera = new Camera(new Vector3(0,0,-4), Vector3.UnitZ, Vector3.UnitY, 60.0f);
            scene.AddCamera(camera);

            // Calculates lighting for the scene
            scene.PreCalculateLighting();
            Tracer = new BVHTracer(scene);
            var sw = Stopwatch.StartNew();
            Tracer.GetCollisionBuffer();
            sw.Stop();
            Debug.WriteLine($"Successfully traced scene in {sw.ElapsedMilliseconds} ms");

            Invalidate();
        }
    }
}
