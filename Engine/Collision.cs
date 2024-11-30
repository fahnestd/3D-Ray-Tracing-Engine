using System.Numerics;

namespace Engine
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

        public Vector3 getIncidentVector()
        {
            Vector3 reflectionVector = Ray - (2 * Vector3.Dot(Ray, CollisionNormal)/ Vector3.Dot(CollisionNormal, CollisionNormal) * CollisionNormal);
            return reflectionVector;
        }
    }
}