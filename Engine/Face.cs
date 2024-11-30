
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

        // https://codeplea.com/triangular-interpolation
        public Vector3 Lerp(Vector3 point)
        {
            Vector3 v1 = Mesh.Vertices[Vertex1];
            Vector3 v2 = Mesh.Vertices[Vertex2];
            Vector3 v3 = Mesh.Vertices[Vertex3];

            // Calculate vectors from point to vertices
            Vector3 f1 = v1 - point;
            Vector3 f2 = v2 - point;
            Vector3 f3 = v3 - point;

            // Calculate areas using cross product
            Vector3 a = Vector3.Cross(v1 - v2, v1 - v3);
            Vector3 a1 = Vector3.Cross(f2, f3);
            Vector3 a2 = Vector3.Cross(f3, f1);
            Vector3 a3 = Vector3.Cross(f1, f2);

            float areaTotal = a.Length();

            // Calculate barycentric coordinates
            float w1 = a1.Length() / areaTotal;
            float w2 = a2.Length() / areaTotal;
            float w3 = a3.Length() / areaTotal;

            // Interpolate the point using barycentric coordinates
            return new Vector3(
                w1 * v1.X + w2 * v2.X + w3 * v3.X,
                w1 * v1.Y + w2 * v2.Y + w3 * v3.Y,
                w1 * v1.Z + w2 * v2.Z + w3 * v3.Z
            );
        }
    }
}
