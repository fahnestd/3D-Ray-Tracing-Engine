using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Viewer
{
    internal class SampleScenes
    {
        public static Scene BasicMeshCollision()
        {
            Scene scene = new Scene();

            //A basic square shape tilted towards the screen
            Mesh mesh1 = new Mesh();

            Face.CurrentColor = PixelColor.FromRGB(255, 0, 0);

            mesh1.AddVertex(2, 2, 6);
            mesh1.AddVertex(-2, 2, 6);
            mesh1.AddVertex(2, -2, 2);
            mesh1.AddVertex(-2, -2, 2);
            mesh1.CalculateNormalsFromVertices();

            // A basic square shape laying flat, cutting through mesh1
            Mesh mesh2 = new Mesh();

            Face.CurrentColor = PixelColor.FromRGB(0, 255, 0);

            mesh2.AddVertex(2, 2, 4);
            mesh2.AddVertex(-2, 2, 4);
            mesh2.AddVertex(2, -2, 4);
            mesh2.AddVertex(-2, -2, 4);
            mesh2.CalculateNormalsFromVertices();


            Mesh mesh3 = new Mesh();

            mesh3.AddVertex(2, -2, 2);
            mesh3.AddVertex(2, -2, -2);
            mesh3.AddVertex(-2, -2, 2);
            mesh3.AddVertex(-2, -2, -2);
            mesh3.CalculateNormalsFromVertices();


            // Add the mesh to the scene
            scene.AddMesh(mesh1);
            scene.AddMesh(mesh2);
            scene.AddMesh(mesh3);

            Light light1 = new Light();
            light1.Position = new Vector3(0, 15, 0);
            light1.Direction = Vector3.Normalize(new Vector3(0, 1, -1));

            scene.AddLight(light1);

            return scene;
        }

        public static Scene PawnOBJ()
        {
            Scene scene = new Scene();
            Mesh? mesh = Import.fromObjectFile("../../../assets/obj/pawn.obj");
            if (mesh != null)
            {
                scene.AddMesh(mesh);
            }

            Light light1 = new Light();
            light1.Position = new Vector3(0, 15, 0);
            light1.Direction = Vector3.Normalize(new Vector3(0.5f, 1, -1));

            scene.AddLight(light1);

            return scene;
        }
    }
}
