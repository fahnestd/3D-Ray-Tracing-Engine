using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DRayTracingEngine.src
{
    internal class Common
    {
        public static float ToRadians(float degrees)
        {
            return degrees * MathF.PI / 180.0f;
        }
    }
}
