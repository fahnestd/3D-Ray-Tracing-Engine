using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DRayTracingEngine.src
{
    public class Face
    {
        public int Vertex1;
        public int Vertex2;
        public int Vertex3;
        public int[] color = CurrentColor;

        public static int[] CurrentColor { get; set; } = [255, 255, 255];
       
    }
}
