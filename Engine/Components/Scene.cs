using Engine.Geometry;
using System.Diagnostics;
using System.Numerics;

namespace Engine.Components
{
    public class Scene
    {
        public List<Mesh> Meshes { get; } = [];
        public List<Face> Faces { get; } = [];
        public List<Light> Lights { get; } = [];
        public List<Camera> Cameras { get; private set; } = [new Camera(Vector3.Zero, Vector3.UnitX, Vector3.UnitY, 60)];
        public int ActiveCamera { get; set; } = 0;

        public void AddMesh(Mesh mesh)
        {
            mesh.GenerateBVHTree();
            Meshes.Add(mesh);
        }

        public void RemoveMesh(Mesh mesh)
        {
            Meshes.Remove(mesh);
        }

        public void AddLight(Light light)
        {
            Lights.Add(light);
        }

        public void RemoveLight(Light light)
        {
            Lights.Remove(light);
        }

        public void PreCalculateLighting()
        {
            foreach (Mesh mesh in Meshes)
            {
                foreach (Face face in mesh.Faces)
                {
                    foreach (Light light in Lights)
                    {
                        light.ApplyFaceLighting(face);
                    }
                }
            }
        }

        public void AddCamera(Camera camera, bool setAsActive = true)
        {
            Cameras.Add(camera);
            if (setAsActive)
            {
                ActiveCamera = Cameras.IndexOf(camera);
            }
        }
        
        public void RemoveCamera(Camera camera)
        {
            if (Cameras.Count == 1)
            {
                Debug.WriteLine("Error attempting to remove the last remaining camera");
                return;
            }
            Cameras.Remove(camera);
        }

        public void SetActiveCamera(Camera camera)
        {
            var activeCamera = Cameras.IndexOf(camera);
            if (activeCamera != -1)
            {
                ActiveCamera = activeCamera;
            }
        }

        public void SetActiveCamera(int cameraIndex)
        {
            if (cameraIndex > 0 && cameraIndex < Cameras.Count)
            {
                ActiveCamera = cameraIndex;
            }
        }
    }

}