using Engine.Components;
using Engine.Renderer;
using Engine.Tracers;
using System.Diagnostics;

namespace Viewer
{
    public partial class Viewer : Form
    {
        private Scene? scene;

        private Tracer? Tracer;

        private Bitmap? bitmap;

        public Viewer()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Black);

            if (bitmap == null) return;

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

            e.Graphics.DrawImage(bitmap, new Rectangle(drawX, drawY, drawWidth, drawHeight), new Rectangle(0, 0, Tracer.Width, Tracer.Height), GraphicsUnit.Pixel);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Create a new Scene and load in a sample
            scene = SampleScenes.PlantOBJ();

            // Calculates lighting for the scene
            scene.PreCalculateLighting();

            Tracer = new BVHTracer(scene);

            // Set Tracer width and height to match the screen size
            Tracer.Width = Width;
            Tracer.Height = Height;

            var sw = Stopwatch.StartNew();
            Tracer.GenerateCollisionBuffer();
            sw.Stop();
            Debug.WriteLine($"Successfully traced scene in {sw.ElapsedMilliseconds} ms");

            bitmap = BitmapRenderer.RenderBitmap(Tracer, Width, Height);

            Invalidate();
        }

        private void Viewer_Resize(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void Viewer_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Control.ModifierKeys == Keys.Control) && e.KeyChar == '\u0013')
            {
                if (bitmap == null)
                {
                    return;
                }
                e.Handled = true; // Mark the event as handled

                // Create and configure SaveFileDialog
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Bitmap Files (*.bmp)|*.bmp";
                saveFileDialog.Title = "Save Bitmap";
                saveFileDialog.DefaultExt = "bmp";

                // Show dialog and get file path
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;
                    bitmap.Save(filePath);
                }
            }
        }
    }
}
