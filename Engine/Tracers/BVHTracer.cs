using Engine.BVH;
using Engine.Components;
using Engine.Util;
using System.Numerics;

namespace Engine.Tracers
{
    public class BVHTracer(Scene scene) : Tracer(scene)
    {
        public override Collision RayTrace(Ray ray)
        {
            Collision collision = new Collision();
            collision.Distance = float.PositiveInfinity;

            foreach (Mesh mesh in scene.Meshes)
            {
                if (mesh.BVHTree == null)
                {
                    Console.Error.WriteLine("Error running BVHTracer, BVHTree missing from mesh");
                    continue;
                }
                BVHTreeTest(ref collision, ray, mesh, mesh.BVHTree);
            }
            if (collision.DidCollide)
            {
                return collision;
            }
            collision.DidCollide = false;

            return collision;
        }

        protected static void BVHTreeTest(ref Collision collision, Ray ray, Mesh mesh, BVHNode node)
        {
            bool boundsHit = BoundsHit(ray, node.BoundingBox);

            if (boundsHit)
            {
                if (node.ChildA == null && node.ChildB == null)
                {
                    TriangleTest(ref collision, ray, mesh, node.Faces);
                }
                else
                {
                    if (node.ChildA != null) BVHTreeTest(ref collision, ray, mesh, node.ChildA);
                    if (node.ChildB != null) BVHTreeTest(ref collision, ray, mesh, node.ChildB);
                }
            }
        }


        protected static void TriangleTest(ref Collision collision, Ray ray, Mesh mesh, List<Face> faces)
        {
            foreach (Face face in faces)
            {
                Vector3 v0 = mesh.Vertices[face.Vertex1];
                Vector3 v1 = mesh.Vertices[face.Vertex2];
                Vector3 v2 = mesh.Vertices[face.Vertex3];

                // Normal = (v1 - v0) x (v2 - v0)
                Vector3 normal = Vector3.Cross(v1 - v0, v2 - v0);
                normal = Vector3.Normalize(normal);

                // find the distance from the plane
                float planeDistance = -Vector3.Dot(normal, v0);
                float rayDirectionDotNormal = Vector3.Dot(normal, ray.Direction);

                // If the ray is parallel or close to, we skip it and assume it doesn't hit
                if (Math.Abs(rayDirectionDotNormal) < 1e-10) // Ray is parallel to the plane
                    continue;

                // Intersection Distance = -(N · O + d) / (N · D)
                float intersectionDistance = -(Vector3.Dot(normal, ray.Origin) + planeDistance) / rayDirectionDotNormal;

                // if the intersection happened behind the Origin of the ray, it will be negative. So we need to skip if so
                if (intersectionDistance < 0)
                    continue;

                // Now we know that the ray hit the plane that the triangle sits on,
                // so we need to figure out if the ray actually hit the triangle
                // We can do that with inside-outside testing

                // Intersection Point = ray origin + Distance * ray Direction

                Vector3 intersectionPoint = ray.Origin + intersectionDistance * ray.Direction;

                // Test Each Edge
                if (InsideOutsideEdgeTest(v1, v0, intersectionPoint, normal) || InsideOutsideEdgeTest(v2, v1, intersectionPoint, normal) || InsideOutsideEdgeTest(v0, v2, intersectionPoint, normal))
                {
                    continue;
                }

                // Check if collision is closer than the last closest collision and set face as collision face if so.
                if (collision.Distance > intersectionDistance)
                {
                    collision.Ray = ray.Direction;
                    collision.DidCollide = true;
                    collision.Face = face;
                    collision.Color = collision.Face.color * collision.Face.lightness;
                    collision.CollisionNormal = normal;
                    collision.CollisionPoint = intersectionPoint;
                    collision.Distance = intersectionDistance;
                }


            }
        }

        protected static bool InsideOutsideEdgeTest(Vector3 v1, Vector3 v2, Vector3 intersectionPoint, Vector3 normal)
        {
            Vector3 edge1 = v1 - v2;
            Vector3 vertexToIntersection = intersectionPoint - v2;

            Vector3 crossProduct = Vector3.Cross(edge1, vertexToIntersection);
            return Vector3.Dot(normal, crossProduct) < 0;
        }

        // This provides quick bounds checking for the BVH nodes
        protected static bool BoundsHit(Ray ray, BoundingBox boundingBox)
        {
            double tmin = double.NegativeInfinity;
            double tmax = double.PositiveInfinity;
            Vector3 rayNormal = Vector3.Normalize(ray.Direction);
            
            if (rayNormal.X != 0.0)
            {
                double tx1 = (boundingBox.Min.X - ray.Origin.X) / rayNormal.X;
                double tx2 = (boundingBox.Max.X - ray.Origin.X) / rayNormal.X;

                tmin = double.Max(tmin, double.Min(tx1, tx2));
                tmax = double.Min(tmax, double.Max(tx1, tx2));
            }

            if (rayNormal.Y != 0.0)
            {
                double ty1 = (boundingBox.Min.Y - ray.Origin.Y) / rayNormal.Y;
                double ty2 = (boundingBox.Max.Y - ray.Origin.Y) / rayNormal.Y;

                tmin = double.Max(tmin, double.Min(ty1, ty2));
                tmax = double.Min(tmax, double.Max(ty1, ty2));
            }

            if (rayNormal.Z != 0.0)
            {
                double tz1 = (boundingBox.Min.Z - ray.Origin.Z) / rayNormal.Z;
                double tz2 = (boundingBox.Max.Z - ray.Origin.Z) / rayNormal.Z;

                tmin = double.Max(tmin, double.Min(tz1, tz2));
                tmax = double.Min(tmax, double.Max(tz1, tz2));
            }

            return tmax >= tmin;
        }

    }
}