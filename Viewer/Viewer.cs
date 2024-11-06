using _3DRayTracingEngine;
using Engine;
using System.Globalization;
using System.Numerics;

namespace Viewer
{
    public partial class Viewer : Form
    {
        private Scene scene;
        private RayGenerator rayGenerator;

        private Collision[,] collisionBuffer;

        private const int WIDTH = 640;
        private const int HEIGHT = 480;

        private const int MAXVIEWDISTANCE = 25;

        private const float MINBRIGHTNESS = 0.1f;

        public Viewer()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (collisionBuffer == null) return;

            Graphics g = e.Graphics;
            
            for (int y = 0; y < HEIGHT; y++)
            {
                for (int x = 0; x < WIDTH; x++)
                {
                    if (!collisionBuffer[y, x].DidCollide)
                    {
                        continue;
                    }

                    // Draw a 1x1 rectangle for each pixel
                    float brushIntensity = Math.Max(MINBRIGHTNESS, collisionBuffer[y, x].Face.lightness);

                    Brush pixelBrush = new SolidBrush(Color.FromArgb((int)(collisionBuffer[y, x].Face.color.R * brushIntensity), (int)(collisionBuffer[y, x].Face.color.G * brushIntensity), (int)(collisionBuffer[y, x].Face.color.B * brushIntensity)));
                    g.FillRectangle(pixelBrush, x, y, 1, 1);

                }
            }
            g.Flush();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Create a new Scene and load in a sample
            scene = SampleScenes.PawnOBJ();
            // Create a new camera facing the positive Z direction
            Camera camera = new Camera(new Vector3(0,0,-10), Vector3.UnitZ, Vector3.UnitY, 60.0f);

            // Calculates lighting for the scene
            scene.Bake();

            // Create a new RayGenerator and specify the camera, height, and width in pixels
            rayGenerator = new RayGenerator(camera, WIDTH, HEIGHT);

            // Generate rays for the scene
            Ray[,] rays = rayGenerator.GenerateRays();

            // Loop through pixels and store collisions
            collisionBuffer = new Collision[HEIGHT, WIDTH];

            for (int y = 0; y < HEIGHT; y++)
            {
                for (int x = 0; x < WIDTH; x++)
                {
                    collisionBuffer[y, x] = Tracer.Intersect(scene, rays[y, x]);
                }
            }

            Invalidate();
        }
    }
}
