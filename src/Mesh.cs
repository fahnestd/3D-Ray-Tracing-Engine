using System;
using System.Numerics;

namespace _3d_Rendering_Engine.src
{
    public class Mesh() {
        public List<Vector3> Vertices { get; } = [];
        public Vector3 Position { get; set; } = Vector3.Zero;
        
        public void AddVertex(float x, float y, float z) { 
            Vertices.Add(new Vector3(x, y, z));
        }
    }
}