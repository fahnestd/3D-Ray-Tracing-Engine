using _3d_Rendering_Engine.src;
using _3DRayTracingEngine.src;
using System.Numerics;

namespace _3DRayTracingEngine
{
    public partial class Form1 : Form
    {
        private Scene scene;
        private RayGenerator rayGenerator;



        private Collision[,] collisionBuffer;

        private const int WIDTH = 640;
        private const int HEIGHT = 480;

        private const int MAXVIEWDISTANCE = 5;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (collisionBuffer == null) return;

            Graphics g = e.Graphics;

            Brush blackBrush = new SolidBrush(Color.Black);
            Brush whiteBrush = new SolidBrush(Color.White);
            
            for (int y = 0; y < HEIGHT; y++)
            {
                for (int x = 0; x < WIDTH; x++)
                {
                    if (!collisionBuffer[y, x].DidCollide)
                    {
                        continue;
                    }

                    int brushIntensity = (int)(Math.Max(0, MAXVIEWDISTANCE - collisionBuffer[y, x].Distance) / MAXVIEWDISTANCE * 255);
                    Brush pixelBrush = new SolidBrush(Color.FromArgb(brushIntensity, brushIntensity, brushIntensity));

                    // Draw a 1x1 rectangle for each pixel
                    g.FillRectangle(pixelBrush, x, y, 1, 1);
                }
            }
            g.Flush();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // New camera facing the positive Z direction
            Camera camera = new Camera(new Vector3(0,0,0), Vector3.UnitZ, Vector3.UnitY, 80.0f);
            scene = new Scene(camera);

            // A basic triangle tilted towards the camera at the top
            Mesh mesh1 = new Mesh();
            mesh1.AddVertex(-2, -2, 6);
            mesh1.AddVertex(2, -2, 6);
            mesh1.AddVertex(0, 1, 2);
            scene.AddMesh(mesh1);

            rayGenerator = new RayGenerator(camera, WIDTH, HEIGHT);
            Ray[,] rays = rayGenerator.GenerateRays();

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
