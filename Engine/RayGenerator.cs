using System.Numerics;

namespace Engine
{
    public class RayGenerator
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
            _right = Vector3.Normalize(Vector3.Cross(_camera.Direction, _camera.Up));
            _up = Vector3.Normalize(Vector3.Cross(_right, _camera.Direction));

            float aspectRatio = (float)width / (float)height;

            // https://www.edmundoptics.com/knowledge-center/application-notes/imaging/understanding-focal-length-and-field-of-view/
            // Camera FOV will be the vertical field of view. We can use this to calculate the view height using 2tan(FOV/2)
            _viewHeight = 2.0f * MathF.Tan(Common.ToRadians(_camera.FieldOfView) / 2.0f);
            _viewWidth = _viewHeight * aspectRatio;
        }

        // Initializes a new ray for each pixel in the image by setting up its origin and direction.
        public Ray[,] GenerateRays()
        {
            Ray[,] rays = new Ray[_width, _height];

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    rays[x, y] = GenerateRayForPixel(x, y);
                }
            }

            return rays;
        }

        // Handles creating the ray for a given x, y pixel
        private Ray GenerateRayForPixel(int x, int y)
        {
            // center the coordinates
            float pixelCenterX = x + 0.5f;
            float pixelCenterY = y + 0.5f;

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
            Vector3 pointOnPlane = _camera.Direction +
                                 worldX * _right +
                                 worldY * _up;

            Ray ray = new Ray()
            {
                Origin = _camera.Position,
                Direction = pointOnPlane
            };

            return ray;
        }
    }
}
