using Engine.Components;
using Engine.Util;
using System.Numerics;

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
        public Collision[,] ?CollisionBuffer {  get; set; }

        public abstract Collision RayTrace(Ray ray);

        public Collision[,] GetCollisionBuffer()
        {
            // Create a new RayGenerator and specify the camera, height, and width in pixels
            RayGenerator rayGenerator = new RayGenerator(scene.Cameras[Scene.ActiveCamera], Width, Height);

            // Generate rays for the scene
            Ray[,] rays = rayGenerator.GenerateRays();

            // Loop through pixels and store collisions
            Collision[,] collisionBuffer = new Collision[Width, Height];
            Parallel.For(0, Width, x =>
            {
                for (int y = 0; y < Height; y++)
                {
                    collisionBuffer[x, y] = RayTrace(rays[x, y]);
                }
            });
           
            CollisionBuffer = collisionBuffer;
            LayerReflect();
            return CollisionBuffer;
        }

        public void LayerReflect()
        {
            if (CollisionBuffer == null)
            {
                return;
            }

            Parallel.For(0, Width, x =>
            {
                for (int y = 0; y < Height; y++)
                {
                    if (CollisionBuffer[x, y].DidCollide)
                    {
                        Ray ray = new Ray
                        {
                            Direction = CollisionBuffer[x, y].GetReflectionVector()
                        };

                        // Nudge the reflection origin point slightly off the surface to remove flickering from slight inaccuracies in floating points.
                        Vector3 NudgedOrigin = CollisionBuffer[x, y].CollisionPoint + CollisionBuffer[x, y].CollisionNormal * (float)1e-5;
                        ray.Origin = NudgedOrigin;

                        Collision collision = RayTrace(ray);
                        if (collision.DidCollide)
                        {
                            CollisionBuffer[x, y].Color.LayerColor(collision.Face.color * collision.Face.lightness, CollisionBuffer[x, y].Face.Mesh.Reflectivity);
                        }
                    }
                }
            });
        }
    }
}
