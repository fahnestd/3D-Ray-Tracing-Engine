

using System.Numerics;

namespace _3DRayTracingEngine.src
{
    public record Ray
    {
        public Vector3 Origin { get; set; }
        public Vector3 Direction { get; set; }
    }
}