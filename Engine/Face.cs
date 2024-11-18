
using System.Numerics;

namespace Engine
{
    public class Face
    (
        Mesh mesh    
    )
    {
        public int Vertex1;
        public int Vertex2;
        public int Vertex3;
        public Vector3 Normal = Vector3.Zero;
        public PixelColor color = CurrentColor;
        public float shininess = 0f;
        public float lightness = 0f;
        public Mesh Mesh { get; set; } = mesh;

        public static PixelColor CurrentColor { get; set; } = PixelColor.FromRGB(255, 255, 255);

        public void SetNormal(Vector3 normal)
        {
           Normal = normal;
        }

        /**
         *  Calculates the face normal given 3 vertex normals
         */
        public void SetFaceNormal(Vector3 v1Normal, Vector3 v2Normal, Vector3 v3Normal)
        {

            Normal = (v1Normal + v2Normal + v3Normal) / 3;
        }

        public void CalculateLightEffect(Light light)
        {
            lightness += Vector3.Dot(Normal, light.Direction);
        }
       
        public Vector3 Center
        {
            get { return (Mesh.Vertices[Vertex1] + Mesh.Vertices[Vertex2] + Mesh.Vertices[Vertex3]) / 3; }
        }
    }
}
