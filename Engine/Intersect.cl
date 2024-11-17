// Uses openCL to perform the tracing calculations to help speed up the program

typedef struct {
    float3 Origin;
    float3 Direction;
} Ray;

typedef struct {
    float3 Position;
} Vertex;

typedef struct {
    int Vertex1;
    int Vertex2;
    int Vertex3;
} Face;

typedef struct {
    float Distance;
    int FaceIndex;
    bool DidCollide;
} Collision;

// Helper function for inside-outside test
bool insideOutsideEdgeTest(float3 v1, float3 v0, float3 point, float3 normal) {
    float3 edge = v1 - v0;
    float3 vp = point - v0;
    float3 c = cross(edge, vp);
    return dot(normal, c) < 0;
}

__kernel void rayTriangleIntersection(
    __global const Vertex* vertices,
    __global const Face* faces,
    __global const Ray* ray,
    __global Collision* collisions,
    const int numFaces
) {
    int faceIdx = get_global_id(0);
    if (faceIdx >= numFaces) return;

    // Initialize local collision data
    Collision localCollision;
    localCollision.Distance = FLT_MAX;
    localCollision.DidCollide = false;
    localCollision.FaceIndex = -1;

    // Get the face
    Face face = faces[faceIdx];

    // Get vertices for the face
    float3 v0 = vertices[face.Vertex1].Position;
    float3 v1 = vertices[face.Vertex2].Position;
    float3 v2 = vertices[face.Vertex3].Position;

    // Calculate normal
    float3 normal = cross(v1 - v0, v2 - v0);
    normal = normalize(normal);

    // Calculate plane distance
    float planeDistance = -dot(normal, v0);
    float rayDirectionDotNormal = dot(normal, ray->Direction);

    // Check if ray is parallel to plane
    if (fabs(rayDirectionDotNormal) < 1e-7f) {
        collisions[faceIdx] = localCollision;
        return;
    }

    // Calculate intersection distance
    float intersectionDistance = -(dot(normal, ray->Origin) + planeDistance) / rayDirectionDotNormal;

    // Check if intersection is behind ray origin
    if (intersectionDistance < 0) {
        collisions[faceIdx] = localCollision;
        return;
    }

    // Calculate intersection point
    float3 intersectionPoint = ray->Origin + intersectionDistance * ray->Direction;

    // Perform inside-outside tests
    if (insideOutsideEdgeTest(v1, v0, intersectionPoint, normal) ||
        insideOutsideEdgeTest(v2, v1, intersectionPoint, normal) ||
        insideOutsideEdgeTest(v0, v2, intersectionPoint, normal)) {
        collisions[faceIdx] = localCollision;
        return;
    }

    // Valid intersection found
    localCollision.Distance = intersectionDistance;
    localCollision.DidCollide = true;
    localCollision.FaceIndex = faceIdx;

    collisions[faceIdx] = localCollision;
}