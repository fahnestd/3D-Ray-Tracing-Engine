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
    }
}
