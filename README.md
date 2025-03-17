# Basic Ray Tracing Engine
A simple ray tracing engine implemented in C# using Windows Forms for visualization. This project demonstrates fundamental concepts of ray tracing including ray generation, camera systems, lighting, reflections, and basic geometry intersection.

![Plant](https://github.com/user-attachments/assets/c869ed77-216c-4a73-b109-a560222df502)

# Technologies
The engine is written in C# using the .NET framework. Otherwise, everything is written from scratch!

# How To Use
1. Create a new scene
2. Create a new camera
3. Create a mesh or import from OBJ file
5. Add verticies to the mesh
6. Add the mesh to the scene
7. Add lights
8. Run the tracing algorithm and draw your results

# Example
```C#
// Create a new Scene
Scene scene = new Scene();

// Create a new camera facing the positive Z direction
Camera camera = new Camera(new Vector3(0,0,-4), Vector3.UnitZ, Vector3.UnitY, 60.0f);
scene.AddCamera(camera);

// Import a mesh from an OBJ File
Mesh? mesh = Import.FromObjectFile("../../../assets/obj/plant.obj");
if (mesh != null)
{
    mesh.SetColor(PixelColor.FromRGB(200, 200, 200));
    mesh.Reflectivity = 0f;
    mesh.Scale(.5f);
    mesh.Transform(new Vector3(0, -1.5f, 0));
    scene.AddMesh(mesh);
}

Light light1 = new Light
{
    Position = new Vector3(3, 2, -4),
    Direction = VectorFunctions.PointAt(new Vector3(3, 2, -4), Vector3.Zero),
};
scene.AddLight(light1);

// Bakes the lighting for the current camera view
scene.PreCalculateLighting();

Tracer = new BVHTracer(scene);
Tracer.GetCollisionBuffer();
```
See the **[demo viewer](https://github.com/fahnestd/3D-Ray-Tracing-Engine/blob/master/Viewer/Viewer.cs)** for more about drawing to a window 

# Loading from an OBJ file
The engine supports loading a .obj file into the scene. If the obj support vertex normals, those will be loaded as well. The function can return null in the case of the file failing to load.
```
 Mesh? mesh = Import.fromObjectFile("../../../assets/obj/pawn.obj");
 if (mesh != null)
 {
     scene.AddMesh(mesh);
 }
```
# Screenshots

![Hand](https://github.com/user-attachments/assets/391e5e67-d18b-4697-8cbf-372551d1859d)

If no vertex normals are present, the scene renders without smooth shading:
![plant](https://github.com/user-attachments/assets/161ea509-9b34-4aaf-a491-b8ae9fc689f2)



