using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Engine.Tracers
{
    public abstract class Tracer
    (
        Scene scene
    )
    {
        public int Width { get; set; } = 640;
        public int Height { get; set; } = 480;
        public Scene Scene { get; set; } = scene;
        public Collision[,] CollisionBuffer {  get; set; }

        public abstract Collision RayTrace(Ray ray);

        public Collision[,] GetCollisionBuffer()
        {
            // Create a new RayGenerator and specify the camera, height, and width in pixels
            RayGenerator rayGenerator = new RayGenerator(scene.Cameras[Scene.ActiveCamera], Width, Height);

            // Generate rays for the scene
            Ray[,] rays = rayGenerator.GenerateRays();

            // Loop through pixels and store collisions
            Collision[,] collisionBuffer = new Collision[Width, Height];

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    collisionBuffer[x, y] = RayTrace(rays[x, y]);
                }
            }

            CollisionBuffer = collisionBuffer;
            return collisionBuffer;
        }
    }
}
