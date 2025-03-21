﻿using Engine.Components;
using Engine.Geometry;
using System.Numerics;

namespace Engine.BVH
{
    public class BoundingBox
    (
        Mesh mesh
    )
    {
        public Vector3 Min = Vector3.One * float.PositiveInfinity;
        public Vector3 Max = Vector3.One * float.NegativeInfinity;
      

        public Vector3 GetCenter()
        {
            return (Min + Max) / 2f;
        }

        public void Expand(Vector3 vertex)
        {
            Min = Vector3.Min(Min, vertex);
            Max = Vector3.Max(Max, vertex);
        }

        public void Expand(Face face)
        {
            Expand(mesh.Vertices[face.Vertex1]);
            Expand(mesh.Vertices[face.Vertex2]);
            Expand(mesh.Vertices[face.Vertex3]);
        }

    }
}
