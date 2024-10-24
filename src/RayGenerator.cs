using _3d_Rendering_Engine.src;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace _3DRayTracingEngine.src
{
    internal class RayGenerator
    {
        private readonly Camera _camera;
        private readonly int _width;
        private readonly int _height;
        private readonly Vector3 _right;
        private readonly Vector3 _up;
        private readonly float _viewHeight;
        private readonly float _viewWidth;

        public RayGenerator(Camera camera, int width, int height)
        {
            _camera = camera;
            _width = width;
            _height = height;

            // Calculating vectors that are right and up from camera for east coordinate manipulation later. camera direction is already forwards.
            _right = Vector3.Normalize(Vector3.Cross(_camera.direction, _camera.Up));
            _up = Vector3.Normalize(Vector3.Cross(_right, _camera.direction));

            float aspectRatio = (float)width / (float)height;

            // https://www.edmundoptics.com/knowledge-center/application-notes/imaging/understanding-focal-length-and-field-of-view/
            // Camera FOV will be the vertical field of view. We can use this to calculate the view height using 2tan(FOV/2)
            _viewHeight = 2.0f * MathF.Tan(Common.ToRadians(_camera.FieldOfView) / 2.0f);
            _viewWidth = _viewHeight * aspectRatio;
        }

        // Initializes a new ray for each pixel in the image by setting up its origin and direction.
        public Ray[,] GenerateRays()
        {
            Ray[,] rays = new Ray[_height, _width];

            for (int j = 0; j < _height; j++)
            {
                for (int i = 0; i < _width; i++)
                {
                    rays[j, i] = GenerateRayForPixel(i, j);
                }
            }

            return rays;
        }

        // Handles actually creating the 
        private Ray GenerateRayForPixel(int i, int j)
        {
            // center the coordinates
            float pixelCenterX = i + 0.5f;
            float pixelCenterY = j + 0.5f;

            // Normalize the coordinates
            float normalizedX = pixelCenterX / _width;
            float normalizedY = pixelCenterY / _height;

            // convert to [-1,1] space so that [0,0] is centered.
            float ndcX = (2.0f * normalizedX - 1.0f);
            // for y we subtract instead since y starts at top and moves downwards.
            float ndcY = (1.0f - 2.0f * normalizedY);

            // Now we can scale them to the view width and height
            float worldX = ndcX * _viewWidth / 2.0f;  // Half width because NDCX is -1 to +1
            float worldY = ndcY * _viewHeight / 2.0f; // Half height because NDCY is -1 to +1

            // Calculate the point on the view plane (the view plane is one unit away from the camera)
            Vector3 pointOnPlane = _camera.direction +
                                 worldX * _right +
                                 worldY * _up;

            Ray ray = new Ray()
            {
                Origin = _camera.position,
                Direction = pointOnPlane
            };

            return ray;
        }
    }
}
