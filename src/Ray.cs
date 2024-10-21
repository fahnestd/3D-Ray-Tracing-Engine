

using System.Numerics;

namespace _3d_Rendering_Engine.src
{
    public record Ray
    {
        public Vector3 Origin { get; set; }
        public Vector3 Direction { get; set; }
    }
}