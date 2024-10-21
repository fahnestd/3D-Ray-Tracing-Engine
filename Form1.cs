using _3d_Rendering_Engine.src;
using System.Numerics;

namespace _3DRayTracingEngine
{
    public partial class Form1 : Form
    {
        private Scene scene;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Brush brush = new SolidBrush(Color.Black);
            Pen pen = new Pen(brush);
            foreach (Mesh mesh in scene.Meshes)
            {
                foreach (Vector3 Vertex in mesh.Vertices)
                {
                    // Testing drawing.
                    g.FillRectangle(brush, new Rectangle(new Point((int)Vertex.X, (int)Vertex.Y), new Size(1, 1)));
                }
            }
            g.Flush();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Camera camera = new Camera(new Vector3(0,0,0));
            scene = new Scene(camera);
        }
    }
}
