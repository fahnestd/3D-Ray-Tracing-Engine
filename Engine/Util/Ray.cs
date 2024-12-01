using System.Numerics;

namespace Engine.Util
{
    public record Ray
    {
        public Vector3 Origin { get; set; }
        public Vector3 Direction { get; set; }
    }
}