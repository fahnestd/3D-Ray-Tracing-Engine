
using System.Numerics;

namespace Engine
{
    public class Face
    {
        public int Vertex1;
        public int Vertex2;
        public int Vertex3;
        public Vector3 Normal = Vector3.Zero;
        public PixelColor color = CurrentColor;

        public static PixelColor CurrentColor { get; set; } = PixelColor.FromRGB(255, 255, 255);

        public void SetNormal(Vector3 normal)
        {
            Normal = normal;
        }
       
    }
}
