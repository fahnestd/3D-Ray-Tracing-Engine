using Engine.Components;
using Engine.Geometry;
using Engine.Tracers;
using Engine.Util;
using System.Numerics;

namespace Testing
{
    public class TracerTest
    {
        Scene scene;
        [SetUp]
        public void Setup()
        {
            scene = new Scene();

        }

        [Test]
        public void Test1()
        {

            // Test case 1: Ray intersects triangle
            Mesh mesh1 = new Mesh();
            mesh1.AddVertex(0, 0, 0);
            mesh1.AddVertex(1, 0, 0);
            mesh1.AddVertex(0, 1, 0);
            scene.AddMesh(mesh1);
            Ray ray1 = new Ray();
            ray1.Origin = new Vector3(0.2f, 0.2f, 1);
            ray1.Direction = new Vector3(0, 0, -1);
            Tracer tracer = new StandardTracer(scene);
            Collision intersected1 = tracer.RayTrace(ray1);
            scene.RemoveMesh(mesh1);

            Assert.IsTrue(intersected1.DidCollide);
        }

        [Test]
        public void Test2()
        {

            // Test case 2: Ray misses triangle
            Mesh mesh2 = new Mesh();
            mesh2.AddVertex(2.5f, -1.3f, 4.7f);
            mesh2.AddVertex(-0.8f, 3.2f, 1.6f);
            mesh2.AddVertex(1.9f, 2.5f, -3.1f);
            scene.AddMesh(mesh2);
            Ray ray2 = new Ray();
            ray2.Origin = new Vector3(5, 5, 5);
            ray2.Direction = new Vector3(1, 0, 0);
            Tracer tracer = new StandardTracer(scene);
            Collision intersected2 = tracer.RayTrace(ray2);
            scene.RemoveMesh(mesh2);

            Assert.IsFalse(intersected2.DidCollide);
        }

        [Test]
        public void Test3()
        {
            // Test case 3: Ray parallel to triangle
            Mesh mesh3 = new Mesh();
            mesh3.AddVertex(0, 0, 0);
            mesh3.AddVertex(2, 0, 0);
            mesh3.AddVertex(1, 2, 0);
            scene.AddMesh(mesh3);
            Ray ray3 = new Ray();
            ray3.Origin = new Vector3(0, 0, 1);
            ray3.Direction = new Vector3(1, 0, 0);
            Tracer tracer = new StandardTracer(scene);
            Collision intersected3 = tracer.RayTrace(ray3);
            scene.RemoveMesh(mesh3);

            Assert.IsFalse(intersected3.DidCollide);
        }

        [Test]
        public void Test4()
        {
            // Test case 4: Ray intersects triangle edge
            Mesh mesh4 = new Mesh();
            mesh4.AddVertex(-1, 0, 0);
            mesh4.AddVertex(1, 0, 0);
            mesh4.AddVertex(0, 1, 0);
            scene.AddMesh(mesh4);
            Ray ray4 = new Ray();
            ray4.Origin = new Vector3(0, 0, 1);
            ray4.Direction = new Vector3(0, 0, -1);
            Tracer tracer = new StandardTracer(scene);
            Collision intersected4 = tracer.RayTrace(ray4);
            scene.RemoveMesh(mesh4);

            Assert.IsTrue(intersected4.DidCollide);
        }

        [Test]
        public void Test5()
        {
            // Test case 5: Ray intersects triangle vertex
            Mesh mesh5 = new Mesh();
            mesh5.AddVertex(0, 0, 0);
            mesh5.AddVertex(1, 1, 0);
            mesh5.AddVertex(-1, 1, 0);
            scene.AddMesh(mesh5);
            Ray ray5 = new Ray();
            ray5.Origin = new Vector3(0, 0, 1);
            ray5.Direction = new Vector3(0, 0, -1);
            Tracer tracer = new StandardTracer(scene);
            Collision intersected5 = tracer.RayTrace(ray5);
            scene.RemoveMesh(mesh5);

            Assert.IsTrue(intersected5.DidCollide);
        }

        [Test]
        public void Test6()
        {
            // Test case 6: Ray originates inside triangle
            Mesh mesh6 = new Mesh();
            mesh6.AddVertex(-1, -1, 0);
            mesh6.AddVertex(1, -1, 0);
            mesh6.AddVertex(0, 1, 0);
            scene.AddMesh(mesh6);
            Ray ray6 = new Ray();
            ray6.Origin = new Vector3(0, 0, 0);
            ray6.Direction = new Vector3(0, 0, 1);
            Tracer tracer = new StandardTracer(scene);
            Collision intersected6 = tracer.RayTrace(ray6);
            scene.RemoveMesh(mesh6);

            Assert.IsTrue(intersected6.DidCollide);
        }

        [Test]
        public void Test7()
        {
            // Test case 7: Ray intersects large triangle
            Mesh mesh7 = new Mesh();
            mesh7.AddVertex(-100, -100, 10);
            mesh7.AddVertex(100, -100, 10);
            mesh7.AddVertex(0, 100, 10);
            scene.AddMesh(mesh7);
            Ray ray7 = new Ray();
            ray7.Origin = new Vector3(0, 0, 0);
            ray7.Direction = new Vector3(0.1f, 0.1f, 1);
            Tracer tracer = new StandardTracer(scene);
            Collision intersected7 = tracer.RayTrace(ray7);
            scene.RemoveMesh(mesh7);
            
            Assert.IsTrue(intersected7.DidCollide);
        }

        [Test]
        public void Test8()
        {
            // Test case 8: Ray barely misses triangle corner
            Mesh mesh8 = new Mesh();
            mesh8.AddVertex(0, 0, 0);
            mesh8.AddVertex(1, 0, 0);
            mesh8.AddVertex(0, 1, 0);
            scene.AddMesh(mesh8);
            Ray ray8 = new Ray();
            ray8.Origin = new Vector3(1.1f, 1.1f, 1);
            ray8.Direction = new Vector3(0, 0, -1);
            Tracer tracer = new StandardTracer(scene);
            Collision intersected8 = tracer.RayTrace(ray8);
            scene.RemoveMesh(mesh8);

            Assert.IsFalse(intersected8.DidCollide);
        }

        [Test]
        public void Test9()
        {
            // Test case 9: Ray intersects very small triangle
            Mesh mesh9 = new Mesh();
            mesh9.AddVertex(0, 0, 0);
            mesh9.AddVertex(0.001f, 0, 0);
            mesh9.AddVertex(0, 0.001f, 0);
            scene.AddMesh(mesh9);
            Ray ray9 = new Ray();
            ray9.Origin = new Vector3(0.0005f, 0.0005f, 1);
            ray9.Direction = new Vector3(0, 0, -1);
            Tracer tracer = new StandardTracer(scene);
            Collision intersected9 = tracer.RayTrace(ray9);
            scene.RemoveMesh(mesh9);

            Assert.IsTrue(intersected9.DidCollide);
        }

        [Test]
        public void Test10()
        {
            // Test case 10: Ray intersects triangle from behind
            Mesh mesh10 = new Mesh();
            mesh10.AddVertex(-1, -1, 0);
            mesh10.AddVertex(1, -1, 0);
            mesh10.AddVertex(0, 1, 0);
            scene.AddMesh(mesh10);
            Ray ray10 = new Ray();
            ray10.Origin = new Vector3(0, 0, -1);
            ray10.Direction = new Vector3(0, 0, 1);
            Tracer tracer = new StandardTracer(scene);
            Collision intersected10 = tracer.RayTrace(ray10);
            scene.RemoveMesh(mesh10);

            Assert.IsTrue(intersected10.DidCollide);
        }
    }
}