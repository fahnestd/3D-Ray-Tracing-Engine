
using Engine.Components;
using System.Numerics;

namespace Engine.Util
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
            Normal = Vector3.Normalize(normal);
        }

        /**
         *  Calculates the face normal given 3 vertex normals
         */
        public void SetFaceNormal(Vector3 v1Normal, Vector3 v2Normal, Vector3 v3Normal)
        {

            Normal = Vector3.Normalize((v1Normal + v2Normal + v3Normal) / 3);
        }

        public void CalculateLightEffect(Light light)
        {
            lightness += Math.Abs(Vector3.Dot(Normal, light.Direction));
        }

        public Vector3 Center
        {
            get { return (Mesh.Vertices[Vertex1] + Mesh.Vertices[Vertex2] + Mesh.Vertices[Vertex3]) / 3; }
        }

        public Vector3 CalculateNormalFromVertices(bool invert = false)
        {
            Vector3 U = Mesh.Vertices[Vertex2] - Mesh.Vertices[Vertex1];
            Vector3 V = Mesh.Vertices[Vertex3] - Mesh.Vertices[Vertex1];
            Vector3 Normal = Vector3.Cross(U, V);
            Vector3 UnitNormal = Vector3.Normalize(Normal);
            if (invert)
            {
                UnitNormal *= -1;
            }
            SetNormal(UnitNormal);
            return UnitNormal;
        }
    }
}
