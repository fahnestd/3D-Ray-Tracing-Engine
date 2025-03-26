using Engine.Components;
using Engine.Geometry;
using Engine.Tracers;
using Engine.Util;
using System.Numerics;

namespace Testing
{
    public class TracerTest
    {
        private Scene scene;
        private Tracer tracer;

        [SetUp]
        public void Setup()
        {
            scene = new Scene();
            tracer = new BVHTracer(scene);
        }

        [Test]
        public void TestRayIntersectsTriangle()
        {
            // Arrange
            Mesh mesh = new Mesh();
            mesh.AddVertex(0, 0, 0);
            mesh.AddVertex(12, 0, 0);
            mesh.AddVertex(0, 12, 0);
            mesh.CalculateNormalsFromVertices();
            scene.AddMesh(mesh);

            Ray ray = new Ray();
            ray.Origin = new Vector3(0.2f, 0.2f, 1);
            ray.Direction = new Vector3(0, 0, -1);

            // Act
            Collision result = tracer.RayTrace(ray);

            // Assert
            Assert.IsTrue(result.DidCollide);

            scene.RemoveMesh(mesh);
        }

        [Test]
        public void TestRayMissesTriangle()
        {
            // Arrange
            Mesh mesh = new Mesh();
            mesh.AddVertex(2.5f, -1.3f, 4.7f);
            mesh.AddVertex(-0.8f, 3.2f, 1.6f);
            mesh.AddVertex(1.9f, 2.5f, -3.1f);
            mesh.CalculateNormalsFromVertices();
            scene.AddMesh(mesh);

            Ray ray = new Ray();
            ray.Origin = new Vector3(5, 5, 5);
            ray.Direction = new Vector3(1, 0, 0);

            // Act
            Collision result = tracer.RayTrace(ray);

            // Assert
            Assert.IsFalse(result.DidCollide);

            scene.RemoveMesh(mesh);
        }

        [Test]
        public void TestRayParallelToTriangle()
        {
            // Arrange
            Mesh mesh = new Mesh();
            mesh.AddVertex(0, 0, 0);
            mesh.AddVertex(2, 0, 0);
            mesh.AddVertex(1, 2, 0);
            mesh.CalculateNormalsFromVertices();
            scene.AddMesh(mesh);

            Ray ray = new Ray();
            ray.Origin = new Vector3(0, 0, 1);
            ray.Direction = new Vector3(1, 0, 0);

            // Act
            Collision result = tracer.RayTrace(ray);

            // Assert
            Assert.IsFalse(result.DidCollide);

            scene.RemoveMesh(mesh);
        }

        [Test]
        public void TestRayIntersectsTriangleEdge()
        {
            // Arrange
            Mesh mesh = new Mesh();
            mesh.AddVertex(-1, 0, 0);
            mesh.AddVertex(1, 0, 0);
            mesh.AddVertex(0, 1, 0);
            mesh.CalculateNormalsFromVertices();
            scene.AddMesh(mesh);

            Ray ray = new Ray();
            ray.Origin = new Vector3(0, 0, 1);
            ray.Direction = new Vector3(0, 0, -1);

            // Act
            Collision result = tracer.RayTrace(ray);

            // Assert
            Assert.IsTrue(result.DidCollide);

            scene.RemoveMesh(mesh);
        }

        [Test]
        public void TestRayIntersectsTriangleVertex()
        {
            // Arrange
            Mesh mesh = new Mesh();
            mesh.AddVertex(0, 0, 0);
            mesh.AddVertex(1, 1, 0);
            mesh.AddVertex(-1, 1, 0);
            mesh.CalculateNormalsFromVertices();
            scene.AddMesh(mesh);

            Ray ray = new Ray();
            ray.Origin = new Vector3(0, 0, 1);
            ray.Direction = new Vector3(0, 0, -1);

            // Act
            Collision result = tracer.RayTrace(ray);

            // Assert
            Assert.IsTrue(result.DidCollide);

            scene.RemoveMesh(mesh);
        }

        [Test]
        public void TestRayOriginatesInsideTriangle()
        {
            // Arrange
            Mesh mesh = new Mesh();
            mesh.AddVertex(-1, -1, 0);
            mesh.AddVertex(1, -1, 0);
            mesh.AddVertex(0, 1, 0);
            mesh.CalculateNormalsFromVertices();
            scene.AddMesh(mesh);

            Ray ray = new Ray();
            ray.Origin = new Vector3(0, 0, 0);
            ray.Direction = new Vector3(0, 0, 1);

            // Act
            Collision result = tracer.RayTrace(ray);

            // Assert
            Assert.IsTrue(result.DidCollide);

            scene.RemoveMesh(mesh);
        }

        [Test]
        public void TestRayIntersectsLargeTriangle()
        {
            // Arrange
            Mesh mesh = new Mesh();
            mesh.AddVertex(-100, -100, 10);
            mesh.AddVertex(100, -100, 10);
            mesh.AddVertex(0, 100, 10);
            mesh.CalculateNormalsFromVertices();
            scene.AddMesh(mesh);

            Ray ray = new Ray();
            ray.Origin = new Vector3(0, 0, 0);
            ray.Direction = new Vector3(0.1f, 0.1f, 1);

            // Act
            Collision result = tracer.RayTrace(ray);

            // Assert
            Assert.IsTrue(result.DidCollide);

            scene.RemoveMesh(mesh);
        }

        [Test]
        public void TestRayBarelyMissesTriangleCorner()
        {
            // Arrange
            Mesh mesh = new Mesh();
            mesh.AddVertex(0, 0, 0);
            mesh.AddVertex(1, 0, 0);
            mesh.AddVertex(0, 1, 0);
            mesh.CalculateNormalsFromVertices();
            scene.AddMesh(mesh);

            Ray ray = new Ray();
            ray.Origin = new Vector3(1.1f, 1.1f, 1);
            ray.Direction = new Vector3(0, 0, -1);

            // Act
            Collision result = tracer.RayTrace(ray);

            // Assert
            Assert.IsFalse(result.DidCollide);

            scene.RemoveMesh(mesh);
        }

        [Test]
        public void TestRayIntersectsVerySmallTriangle()
        {
            // Arrange
            Mesh mesh = new Mesh();
            mesh.AddVertex(0, 0, 0);
            mesh.AddVertex(0.001f, 0, 0);
            mesh.AddVertex(0, 0.001f, 0);
            mesh.CalculateNormalsFromVertices();
            scene.AddMesh(mesh);

            Ray ray = new Ray();
            ray.Origin = new Vector3(0.0005f, 0.0005f, 1);
            ray.Direction = new Vector3(0, 0, -1);

            // Act
            Collision result = tracer.RayTrace(ray);

            // Assert
            Assert.IsTrue(result.DidCollide);

            scene.RemoveMesh(mesh);
        }

        [Test]
        public void TestRayIntersectsTriangleFromBehind()
        {
            // Arrange
            Mesh mesh = new Mesh();
            mesh.AddVertex(-1, -1, 0);
            mesh.AddVertex(1, -1, 0);
            mesh.AddVertex(0, 1, 0);
            mesh.CalculateNormalsFromVertices();
            scene.AddMesh(mesh);

            Ray ray = new Ray();
            ray.Origin = new Vector3(0, 0, -1);
            ray.Direction = new Vector3(0, 0, 1);

            // Act
            Collision result = tracer.RayTrace(ray);

            // Assert
            Assert.IsTrue(result.DidCollide);

            scene.RemoveMesh(mesh);
        }
    }
}