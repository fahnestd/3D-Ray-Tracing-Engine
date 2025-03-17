using Engine.Geometry;
using System.Numerics;

namespace Engine.Components
{
    public class Light
    {
        public Vector3 Position;
        public Vector3 Direction;

        public void ApplyFaceLighting(Face face)
        {
            face.lightness += Math.Abs(Vector3.Dot(face.Normal, Direction));
            if (face.HasVertexNormals)
            {
                face.Vertex1Lightness += Math.Abs(Vector3.Dot(face.Mesh.Normals[face.Vertex1Normal], Direction));
                face.Vertex2Lightness += Math.Abs(Vector3.Dot(face.Mesh.Normals[face.Vertex2Normal], Direction));
                face.Vertex3Lightness += Math.Abs(Vector3.Dot(face.Mesh.Normals[face.Vertex3Normal], Direction));
            }
        }
    }
}
