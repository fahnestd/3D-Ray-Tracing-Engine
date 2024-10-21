

using System.Numerics;

namespace _3d_Rendering_Engine.src
{
    public record Collision
    {
        float Distance { get; set; }
        bool DidCollide { get; set; }
        Vector3 CollisionPoint { get; set; }
        Vector3 CollisionNormal { get; set; }
    }
}