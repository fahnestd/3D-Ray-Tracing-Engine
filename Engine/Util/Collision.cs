using System.Numerics;

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
    }
}