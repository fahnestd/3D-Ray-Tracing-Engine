using Engine.BVH;
using Engine.Util;
using System.Numerics;

namespace Engine.Components
{


    public class Mesh()
    {
        public Vector3 Position { get; set; } = Vector3.Zero;
        public List<Face> Faces = [];
        public List<Vector3> Vertices { get; private set; } = [];
        public List<Vector3> Normals { get; } = [];

        public Scene Scene { get; set; }

        public Mode FaceMode { get; set; } = Mode.TRIANGLE_STRIPS;
        public BVHNode BVHTree { get; set; }
        public float Reflectivity { get; set; } = 0f;


        public enum Mode
        {
            TRIANGLE_STRIPS,
            TRIANGLE_FAN,
            LOAD
        }

        // for vertex 3 and greater, we start performing a line strip, so the last 2 vertices are connected to the new one.
        public void AddVertex(float x, float y, float z)
        {
            Vertices.Add(new Vector3(x, y, z));

            switch (FaceMode)
            {
                case Mode.TRIANGLE_STRIPS:
                    TriangleStrip();
                    break;
                case Mode.TRIANGLE_FAN:
                    TriangleFan();
                    break;
            }
        }

        public void TriangleStrip()
        {
            // Start creating a triangle strip
            if (Vertices.Count > 2)
            {
                Face face = new Face(this)
                {
                    Vertex1 = Vertices.Count - 3,
                    Vertex2 = Vertices.Count - 2,
                    Vertex3 = Vertices.Count - 1
                };
                Faces.Add(face);
            }
        }

        public void TriangleFan()
        {
            // Start creating a triangle fan
            if (Vertices.Count > 2)
            {
                Face face = new Face(this)
                {
                    Vertex1 = 0,
                    Vertex2 = Vertices.Count - 2,
                    Vertex3 = Vertices.Count - 1
                };
                Faces.Add(face);
            }
        }

        public void AddFace(Face face)
        {
            Faces.Add(face);
        }

        public void CalculateNormalsFromVertices()
        {
            foreach (Face face in Faces)
            {
                Vector3 U = Vertices[face.Vertex2] - Vertices[face.Vertex1];
                Vector3 V = Vertices[face.Vertex3] - Vertices[face.Vertex1];
                Vector3 Normal = Vector3.Cross(U, V);
                Vector3 UnitNormal = Vector3.Normalize(Normal);
                face.SetNormal(UnitNormal);
            }
        }

        public void CalculateBoundingBox()
        {
            BVH.BVH bvh = new BVH.BVH(this);
            BVHTree = bvh.Root;
        }

        public void Scale(float Amount)
        {
            for (int i = 0; i < Vertices.Count; i++)
            {
                Vertices[i] *= Amount;
            }
        }

        public void Transform(Vector3 Amount)
        {
            for (int i = 0; i < Vertices.Count; i++)
            {
                Vertices[i] += Amount;
            }

        }

        public void TransformX(float Amount)
        {
            List<Vector3> NewVertices = [];
            foreach (var Vertex in Vertices)
            {
                NewVertices.Add(new Vector3(Vertex.X + Amount, Vertex.Y, Vertex.Z));
            }
            Vertices = NewVertices;
        }

        public void TransformY(float Amount)
        {
            List<Vector3> NewVertices = [];
            foreach (var Vertex in Vertices)
            {
                NewVertices.Add(new Vector3(Vertex.X, Vertex.Y + Amount, Vertex.Z));
            }
            Vertices = NewVertices;
        }

        public void TransformZ(float Amount)
        {
            List<Vector3> NewVertices = [];
            foreach (var Vertex in Vertices)
            {
                NewVertices.Add(new Vector3(Vertex.X, Vertex.Y, Vertex.Z + Amount));
            }
            Vertices = NewVertices;
        }

        public void SetColor(PixelColor Color)
        {
            List<Face> NewFaces = [];
            foreach (var Face in Faces)
            {
                Face.color = Color;
                NewFaces.Add(Face);
            }
            Faces = NewFaces;
        }
    }
}