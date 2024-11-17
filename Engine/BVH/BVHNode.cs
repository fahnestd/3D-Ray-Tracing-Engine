using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.BVH
{
    public class BVHNode
    (
        BoundingBox boundingBox
    )
    {
        public BoundingBox BoundingBox = boundingBox;
        public List<Face> Faces = [];
        public BVHNode ChildA;
        public BVHNode ChildB;
    }
}
