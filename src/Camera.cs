
using System;
using System.Numerics;

namespace _3d_Rendering_Engine.src
{
    public class Camera
    (
        Vector3 position
    )
    {
        public Vector3 position { get; set; } = position;
        public Vector3 LookDirection { get; set; }

        private float fieldOfView = 60;

        public float FieldOfView
        {
            get { return fieldOfView; }
            set { fieldOfView = Math.Clamp(fieldOfView, 0, 180); }
        }
    }
}