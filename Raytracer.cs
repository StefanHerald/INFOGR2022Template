using OpenTK;
using System;
using Template;

namespace INFOGR2022Template
{
    internal class Raytracer
    {
        Scene scene;
        Camera camera;
        Surface screen;
        //the ray that will be set through the the camera origin and the screen plane
        Ray primaryRay = new Ray();

        //upleft corner
        Vector3 upLeft;
        //upRight
        Vector3 upRight;
        //downRight corner
        Vector3 downleft;
        public Raytracer(Scene scene, Camera camera, Surface screen)
        {
            this.scene = scene;
            this.camera = camera;
            this.screen = screen;
            primaryRay.position = camera.position;

            //calculating the edge points of the plane
            Vector3 center = camera.position + camera.lookAtDirection * camera.FOV;
            Vector3 rightDirection = -CrossProduct(camera.upDirection, camera.lookAtDirection);
            upLeft = center + camera.upDirection - rightDirection;
            upRight = center + camera.upDirection + rightDirection;
            downleft = center - camera.upDirection - rightDirection;
        }

        internal void Render()
        {
            Vector3 horizon = upLeft - upRight;
            Vector3 vertical = downleft - upLeft;
            for(int x = 0; x < screen.width; x++)
            {
                for(int y = 0; y < screen.height; y++)
                {
                    primaryRay.direction = upLeft + (x / screen.width) * horizon + (y/screen.height) * vertical;
                    primaryRay.direction.Normalize();
                    foreach (Primitives p in scene.objects)
                    {
                        if(p is Sphere)
                        {
                            IntersectSphere((Sphere)p, primaryRay);
                            if(primaryRay.scalar > 0)
                            {
                                Ray shadowRay = new Ray();
                                shadowRay.position = primaryRay.position * primaryRay.scalar;
                                foreach(Light l in scene.lights)
                                {
                                    
                                }
                            }
                        }
                    }
                }
            }
        }

        internal void IntersectSphere(Sphere sphere, Ray ray)
        {
            Vector3 c = sphere.Position - ray.position; 
            float t = Vector3.Dot(c, ray.direction); 
            Vector3 q = c - t * ray.direction; 
            float p2 = q.LengthSquared; 
            if (p2 > sphere.Radius) 
                return; 
            t -= (float)Math.Sqrt(sphere.Radius - p2); 
            if ((t < ray.scalar) && (t > 0)) 
                ray.scalar = t;
        }        

        //a ray is defined by a position and a (normalized) direction
        internal struct Ray
        {
            internal Vector3 RGB;
            internal Vector3 position;
            internal float scalar;
            internal Vector3 direction;
        }
        //takes the cross product of two vectors to calulate a thrid, orthogonal to both
        internal Vector3 CrossProduct(Vector3 A, Vector3 B)
        {
            return new Vector3(A.Y * B.Z - A.Z * B.Y,
                A.Z * B.X - A.X * B.Z,
                A.X * B.Y - A.Y * B.X);
        }
    }
}
