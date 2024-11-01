using Engine;
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
                    float brushIntensity = (Math.Max(0, MAXVIEWDISTANCE - collisionBuffer[y, x].Distance) / MAXVIEWDISTANCE);
                   
                    Brush pixelBrush = new SolidBrush(Color.FromArgb((int)(collisionBuffer[y, x].Face.color.R * brushIntensity), (int)(collisionBuffer[y, x].Face.color.G * brushIntensity), (int)(collisionBuffer[y, x].Face.color.B * brushIntensity)));
                    g.FillRectangle(pixelBrush, x, y, 1, 1);

                }
            }
            g.Flush();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Create a new Scene
            scene = new Scene();
            // Create a new camera facing the positive Z direction
            Camera camera = new Camera(new Vector3(0,0,-10), Vector3.UnitZ, Vector3.UnitY, 60.0f);

            // A basic square shape, but two opposing corners are 1 unit closer to the screen
            Mesh mesh1 = new Mesh();
            mesh1.AddVertex(2, 2, 3);
            mesh1.AddVertex(-2, 2, 4);

            Face.CurrentColor = PixelColor.FromRGB(255, 0, 0);
            mesh1.AddVertex(2, -2, 4);
            
            Face.CurrentColor = PixelColor.FromRGB(0, 0, 255);
            mesh1.AddVertex(-2, -2, 3);
            
            // Add the mesh to the scene
            scene.AddMesh(mesh1);

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
