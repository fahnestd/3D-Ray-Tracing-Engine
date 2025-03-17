using System.Numerics;
using Engine.Geometry;

namespace Engine.Util
{
    public record Collision
    {
        public Vector3 Ray { get; set; }
        public float Distance { get; set; }
        public bool DidCollide { get; set; }
        public Vector3 CollisionPoint { get; set; }
        public Vector3 CollisionNormal { get; set; }
        public Face Face { get; set; }
        public PixelColor Color { get; set; } = PixelColor.FromRGB(0, 0, 0);

        public Vector3 GetReflectionVector()
        {
            Vector3 reflectionVector = Ray - 2 * Vector3.Dot(Ray, CollisionNormal) * CollisionNormal;
            return reflectionVector;
        }

        public float GetLightness()
        {
            if (!DidCollide || Face == null)
                return 0f;

            // Get the three vertices of the face and their lightness values
            Vector3 v1 = Face.Mesh.Vertices[Face.Vertex1];
            Vector3 v2 = Face.Mesh.Vertices[Face.Vertex2];
            Vector3 v3 = Face.Mesh.Vertices[Face.Vertex3];

            // Calculate barycentric coordinates
            Vector3 baryCoords = VectorFunctions.CalculateBarycentricCoordinates(v1, v2, v3, CollisionPoint);

            // Use barycentric coordinates to interpolate lightness
            float interpolatedLight = baryCoords.X * Face.Vertex1Lightness + baryCoords.Y * Face.Vertex2Lightness + baryCoords.Z * Face.Vertex3Lightness;

            return interpolatedLight;
        }

       
    }
}