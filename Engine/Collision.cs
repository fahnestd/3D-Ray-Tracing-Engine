using System.Numerics;

namespace Engine
{
    public record Collision
    {
        public float Distance { get; set; }
        public bool DidCollide { get; set; }
        public Vector3 CollisionPoint { get; set; }
        public Vector3 CollisionNormal { get; set; }
        public Face Face { get; set; }
    }
}