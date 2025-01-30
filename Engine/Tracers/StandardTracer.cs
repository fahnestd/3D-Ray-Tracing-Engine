using Engine.Components;
using Engine.Util;
using System.Numerics;

namespace Engine.Tracers
{
    public class StandardTracer(Scene scene): Tracer(scene)
    {
        public override Collision RayTrace(Ray ray)
        {
            Collision collision = new Collision();
            collision.Distance = float.PositiveInfinity;

            foreach (Mesh mesh in Scene.Meshes)
            {
                foreach (Face face in mesh.Faces)
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

                    // If the ray is parallel or close to, we skip it and assume it doesnt hit
                    if (Math.Abs(rayDirectionDotNormal) < 1e-7) // Ray is parallel to the plane
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
            if (collision.DidCollide)
            {
                return collision;
            }

            collision.DidCollide = false;
            return collision;
        }
        protected static bool InsideOutsideEdgeTest(Vector3 v1, Vector3 v2, Vector3 intersectionPoint, Vector3 normal)
        {
            Vector3 edge1 = v1 - v2;
            Vector3 vertexToIntersection = intersectionPoint - v2;

            Vector3 crossProduct = Vector3.Cross(edge1, vertexToIntersection);
            return Vector3.Dot(normal, crossProduct) < 0;
        }

    }
}