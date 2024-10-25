using _3DRayTracingEngine.src;
using System;
using System.Numerics;

namespace _3d_Rendering_Engine.src
{
    public class Mesh() {
        public List<Vector3> Vertices { get; } = [];
        public Vector3 Position { get; set; } = Vector3.Zero;
        public List<Face> Faces = [];

        // for vertex 3 and greater, we start performing a line strip, so the last 2 vertices are connected to the new one.
        public void AddVertex(float x, float y, float z) { 
            Vertices.Add(new Vector3(x, y, z));
            
            // Start creating a triangle strip
            if (Vertices.Count > 2)
            {
                Face face = new Face();
                face.Vertex1 = Vertices.Count - 1;
                face.Vertex2 = Vertices.Count - 2;
                face.Vertex3 = Vertices.Count - 3;
                Faces.Add(face);
            }
        }
    }
}