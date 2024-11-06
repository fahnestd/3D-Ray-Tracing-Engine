using System;
using System.Collections.Generic;

namespace Engine
{
    public class Scene
    {
        public List<Mesh> Meshes { get; } = [];
        public List<Light> Lights { get; } = [];

        public void AddMesh(Mesh mesh)
        {
            Meshes.Add(mesh);
        }

        public void RemoveMesh(Mesh mesh)
        {
            Meshes.Remove(mesh);
        }

        public void AddLight(Light light)
        {
            Lights.Add(light);
        }

        public void RemoveLight(Light light)
        {
            Lights.Remove(light);
        }

        public void Bake()
        {
            foreach (Mesh mesh in Meshes)
            {
                foreach (Face face in mesh.Faces)
                {
                    foreach (Light light in Lights)
                    {
                        face.CalculateLightEffect(light);
                    }

                }
            }
        }
    }

}