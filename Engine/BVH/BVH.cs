using Engine.Components;

namespace Engine.BVH
{
    public class BVH
    {
        const int MaxDepth = 50;
        const int FacesThreshold = 3;

        public BVHNode Root;

        public BVH(Mesh mesh)
        {
            BoundingBox BoundingBox = new BoundingBox(mesh);

            foreach (var vertex in mesh.Vertices)
            {
                BoundingBox.Expand(vertex);
            }

            Root = new BVHNode(BoundingBox);
            Root.Faces = mesh.Faces;
            Split(Root, mesh);
        }

        public static void Split(BVHNode node, Mesh mesh, int depth = 1)
        {
            // Stop recursion at max depth
            if (depth >= MaxDepth || node.Faces.Count < FacesThreshold)
            {
                return;
            }

            // Create a child, splitting the current node in half
            node.ChildA = new BVHNode(new BoundingBox(mesh));
            node.ChildB = new BVHNode(new BoundingBox(mesh));

            // Go through each vertex in the current node and distribute its faces between the two child nodes.
            foreach (var face in node.Faces)
            {
                bool inChildA = face.Center.X < node.BoundingBox.GetCenter().X;
                BVHNode child = inChildA ? node.ChildA : node.ChildB;

                child.Faces.Add(face);
                child.BoundingBox.Expand(face);
            }

            Split(node.ChildA, mesh, depth + 1);
            Split(node.ChildB, mesh, depth + 1);
        }
    }
}
