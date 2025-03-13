using Engine.Components;
using Engine.Util;
using System.Numerics;

namespace Viewer
{
    internal class SampleScenes
    {
        public static Scene BasicMeshCollision()
        {
            Scene scene = new Scene();

            //A basic square shape tilted towards the screen
            Face.CurrentColor = PixelColor.FromRGB(255, 0, 0);

            Mesh mesh1 = new Mesh();
            mesh1.AddVertex(2, 2, 6);
            mesh1.AddVertex(-2, 2, 6);
            mesh1.AddVertex(2, -2, 2);
            mesh1.AddVertex(-2, -2, 2);
            mesh1.CalculateNormalsFromVertices();

            // A basic square shape laying flat, cutting through mesh1
            Face.CurrentColor = PixelColor.FromRGB(0, 255, 0);

            Mesh mesh2 = new Mesh();
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

            Light light1 = new Light
            {
                Position = new Vector3(0, 15, 0),
                Direction = Vector3.Normalize(new Vector3(0, 1, -1))
            };

            scene.AddLight(light1);

            return scene;
        }

        public static Scene PawnOBJ()
        {
            Scene scene = new Scene();
            Mesh? mesh = Import.FromObjectFile("../../../assets/obj/pawn.obj");
            if (mesh != null)
            {
                mesh.Scale(.5f);
                scene.AddMesh(mesh);
            }

            Light light1 = new Light
            {
                Position = new Vector3(0, 15, 0),
                Direction = Vector3.Normalize(new Vector3(0.5f, 1, -1))
            };

            scene.AddLight(light1);

            return scene;
        }
        public static Scene TeapotOBJ()
        {
            Scene scene = new Scene();
            Mesh? mesh = Import.FromObjectFile("../../../assets/obj/teapot.obj");
            if (mesh != null)
            {
                mesh.SetColor(PixelColor.FromRGB(255, 0, 0));
                mesh.Reflectivity = .3f;
                scene.AddMesh(mesh);
            }

            // Import a square object and set reflectivity to 100% like a mirror
            Mesh? mesh2 = Import.FromObjectFile("../../../assets/obj/reflector.obj");
            if (mesh2 != null)
            {
                mesh2.SetColor(PixelColor.FromRGB(100, 255, 100));
                mesh2.Reflectivity = 1f;
                scene.AddMesh(mesh2);
            }

            Light light1 = new Light
            {
                Position = new Vector3(0, 15, 0),
                Direction = Vector3.Normalize(new Vector3(0.5f, 1, -1))
            };

            scene.AddLight(light1);

            Light light2 = new Light
            {
                Position = new Vector3(0, -15, 0),
                Direction = Vector3.Normalize(new Vector3(-0.5f, 1, 1))
            };

            scene.AddLight(light2);

            return scene;
        }

        public static Scene PlantOBJ()
        {
            Scene scene = new Scene();
            Mesh? mesh = Import.FromObjectFile("../../../assets/obj/plant.obj");
            if (mesh != null)
            {
                mesh.SetColor(PixelColor.FromRGB(200, 200, 200));
                mesh.Reflectivity = 0f;
                mesh.Scale(.5f);
                mesh.Transform(new Vector3(0, -1.5f, 0));
                scene.AddMesh(mesh);
            }

            // Import a square object and set reflectivity to 100% like a mirror
            Mesh? mesh2 = Import.FromObjectFile("../../../assets/obj/reflector.obj");
            if (mesh2 != null)
            {
                //mesh2.CalculateNormalsFromVertices();
                mesh2.SetColor(PixelColor.FromRGB(10, 20, 10));
                mesh2.Reflectivity = 1f;
                scene.AddMesh(mesh2);
            }

            Light light1 = new Light
            {
                Position = new Vector3(3, 2, -4),
                Direction = VectorFunctions.PointAt(new Vector3(3, 2, -4), Vector3.Zero),
            };
            scene.AddLight(light1);

            Light light2 = new Light
            {
                Position = new Vector3(-8, 2, -4),
                Direction = VectorFunctions.PointAt(new Vector3(-8, 2, -4), Vector3.Zero),
            };

            scene.AddLight(light2);

            return scene;
        }
    }
}
