using System.Numerics;

namespace Engine.Geometry
{
    public record Ray
    {
        public Vector3 Origin { get; set; }
        public Vector3 Direction { get; set; }
    }
}