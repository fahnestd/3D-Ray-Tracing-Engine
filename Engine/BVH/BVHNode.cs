﻿using Engine.Geometry;

namespace Engine.BVH
{
    public class BVHNode
    (
        BoundingBox boundingBox
    )
    {
        public BoundingBox BoundingBox = boundingBox;
        public List<Face> Faces = [];
        public BVHNode ?ChildA;
        public BVHNode ?ChildB;
    }
}
