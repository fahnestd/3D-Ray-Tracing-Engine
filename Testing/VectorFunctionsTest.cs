using Engine.Components;
using Engine.Tracers;
using Engine.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Testing
{
    public class VectorFunctionsTest
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void TestBarycentricCoordinatesAtEdge()
        {
            // Arrange
            Vector3 v1 = new Vector3(0, 0, 0);
            Vector3 v2 = new Vector3(1, 0, 0);
            Vector3 v3 = new Vector3(0, 1, 0);
            Vector3 p = new Vector3(0.5f, 0.5f, 0);

            // Act
            Vector3 result = VectorFunctions.CalculateBarycentricCoordinates(v1, v2, v3, p);

            // Assert
            Assert.That(result, Is.EqualTo(new Vector3(0, 0.5f, 0.5f)));
        }

        [Test]
        public void TestBarycentricCoordinatesAtVertex2()
        {
            // Arrange
            Vector3 v1 = new Vector3(0, 0, 0);
            Vector3 v2 = new Vector3(1, 0, 0);
            Vector3 v3 = new Vector3(0, 1, 0);
            Vector3 p = new Vector3(1, 0, 0);

            // Act
            Vector3 result = VectorFunctions.CalculateBarycentricCoordinates(v1, v2, v3, p);

            // Assert
            Assert.That(result, Is.EqualTo(new Vector3(0, 1, 0)));
        }

        [Test]
        public void TestBarycentricCoordinatesNearCenter()
        {
            // Arrange
            Vector3 v1 = new Vector3(0, 0, 0);
            Vector3 v2 = new Vector3(1, 0, 0);
            Vector3 v3 = new Vector3(0, 1, 0);
            Vector3 p = new Vector3(0.33f, 0.33f, 0);

            // Act
            Vector3 result = VectorFunctions.CalculateBarycentricCoordinates(v1, v2, v3, p);

            // Assert
            Assert.IsTrue(Vector3Comparer(result, new Vector3(0.34f, 0.33f, 0.33f)));
        }

        // Custom comparer for Vector3 allowing tolerances for floating point imprecision
        private static bool Vector3Comparer(Vector3 a, Vector3 b)
        {
            const float tolerance = 0.01f;
            return Math.Abs(a.X - b.X) < tolerance &&
                   Math.Abs(a.Y - b.Y) < tolerance &&
                   Math.Abs(a.Z - b.Z) < tolerance;
        }
    }
}
