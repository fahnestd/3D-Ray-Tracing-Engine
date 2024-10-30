using System;
using System.Collections.Generic;

namespace Engine
{
    public class Scene
    {
        public List<Mesh> Meshes { get; } = [];

        public void AddMesh(Mesh mesh)
        {
            Meshes.Add(mesh);
        }

        public void RemoveMesh(Mesh mesh)
        {
            Meshes.Remove(mesh);
        }
    }

}