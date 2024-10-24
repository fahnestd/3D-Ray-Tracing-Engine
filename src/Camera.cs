
using System;
using System.Numerics;

namespace _3d_Rendering_Engine.src
{
    public class Camera
    (
        Vector3 position,
        Vector3 direction,
        Vector3 up,
        float fieldOfView
    )
    {
        public Vector3 position { get; set; } = position;
        public Vector3 direction { get; set; } = direction;
        public Vector3 Up { get; set; } = up;
        public float fieldOfView = fieldOfView;

        public float FieldOfView
        {
            get { return fieldOfView; }
            set { fieldOfView = Math.Clamp(fieldOfView, 0, 180); }
        }

       

    }
}