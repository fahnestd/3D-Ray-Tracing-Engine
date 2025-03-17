using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Util
{
    public class VectorFunctions
    {
        public static Vector3 PointAt(Vector3 source, Vector3 target)
        {
            return Vector3.Normalize(source - target);
        }

        public static Vector3 CalculateBarycentricCoordinates(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 p)
        {
            // Create vectors
            Vector3 v0 = v2 - v1;
            Vector3 v1v = v3 - v1;
            Vector3 v2v = p - v1;

            // Compute dot products
            float d00 = Vector3.Dot(v0, v0);
            float d01 = Vector3.Dot(v0, v1v);
            float d11 = Vector3.Dot(v1v, v1v);
            float d20 = Vector3.Dot(v2v, v0);
            float d21 = Vector3.Dot(v2v, v1v);

            // Calculate barycentric coordinates
            float denom = d00 * d11 - d01 * d01;
            float beta = (d11 * d20 - d01 * d21) / denom;
            float gamma = (d00 * d21 - d01 * d20) / denom;
            float alpha = 1.0f - beta - gamma;

            return new Vector3(alpha, beta, gamma);
        }
    }
}
