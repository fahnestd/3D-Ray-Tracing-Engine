# Basic Ray Tracing Engine
A simple ray tracing engine implemented in C# using Windows Forms for visualization. This project demonstrates fundamental concepts of ray tracing including ray generation, camera systems, lighting, reflections, and basic geometry intersection.

![image](https://github.com/user-attachments/assets/43ee579f-17ee-42e5-a7ec-c68c53bd73dc)

# Technologies
The engine is written in C# using the .NET framework. Otherwise, everything is written from scratch!

# How To Use
1. Create a new scene
2. Create a new camera
3. Create a mesh
4. Add verticies to the mesh
5. Add the mesh to the scene
6. Run the tracing algorithm and handle your results

# Example
```C#
// Create a new Scene
scene = new Scene();
// Create a new camera facing the positive Z direction
Camera camera = new Camera(new Vector3(0,0,-10), Vector3.UnitZ, Vector3.UnitY, 60.0f);

// A basic square shape, but two opposing corners are 1 unit closer to the screen
Mesh mesh1 = new Mesh();
mesh1.AddVertex(2, 2, 3);
mesh1.AddVertex(-2, 2, 4);

Face.CurrentColor = PixelColor.FromRGB(255, 0, 0);
mesh1.AddVertex(2, -2, 4);

Face.CurrentColor = PixelColor.FromRGB(0, 0, 255);
mesh1.AddVertex(-2, -2, 3);

// Add the mesh to the scene
scene.AddMesh(mesh1);

// Create a new RayGenerator and specify the camera, height, and width in pixels
rayGenerator = new RayGenerator(camera, 640, 480);

// Generate rays for the scene
Ray[,] rays = rayGenerator.GenerateRays();

// Loop through pixels and store collisions
collisionBuffer = new Collision[HEIGHT, WIDTH];

for (int y = 0; y < HEIGHT; y++)
{
    for (int x = 0; x < WIDTH; x++)
    {
        collisionBuffer[y, x] = Tracer.Intersect(scene, rays[y, x]);
    }
}
```
See the **[demo viewer](https://github.com/fahnestd/3D-Ray-Tracing-Engine/blob/b03e744/Viewer/Viewer.cs)** for more about drawing to a window 

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
![image](https://github.com/user-attachments/assets/e9eadc32-b1ff-4a37-98fa-05f078656538)
Loading in an OBJ file with vertex normals.

![image](https://github.com/user-attachments/assets/2270d065-7c5e-41ff-9279-0d6b554ac3a1)

Drawing some basic shapes using verticies (No lighting)

![image](https://github.com/user-attachments/assets/c276c71a-a990-4c8e-891d-bc7d70002734)

Testing out using distance based fog, we can see a line strip being rendered from 1 mesh with 4 vertices. the 2 opposing corners of the square are closer to the camera; creating the gradient effect we see. The white represents areas closer to the camera.
