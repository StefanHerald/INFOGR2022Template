using OpenTK;
using System;
using Template;

namespace INFOGR2022Template
{
    internal class Raytracer
    {
        Scene scene;
        Camera camera;
        //the ray that will be set through the the camera origin and the screen plane
        Ray primaryRay = new Ray();
        //the intersection
        Intersection intersection = new Intersection();
        //upleft corner
        Vector3 upLeft;
        //upRight
        Vector3 upRight;
        //downRight corner
        Vector3 downLeft;
        /// <summary>
        /// initialize the Raytracer using a scene, a camera and a screen.
        /// </summary>
        /// <param name="scene"> the scene containg all the primitives </param> 
        /// <param name="camera">the camera with a position, a updirection and a lookatdirection</param>
        /// <param name="screen">the screen made of pixels that need to be colored according to the pixel</param>
        public Raytracer(Scene scene, Camera camera)
        {
            this.scene = scene;
            this.camera = camera;
            primaryRay.position = camera.position;

            //calculating the edge points of the plane
            Vector3 center = camera.position + camera.lookAtDirection * camera.FOV;
            Vector3 rightDirection = Vector3.Cross(camera.upDirection, camera.lookAtDirection);
            upLeft = center + camera.upDirection - rightDirection;
            upRight = center + camera.upDirection + rightDirection;
            downLeft = center - camera.upDirection - rightDirection;
        }
        /// <summary>
        /// render the scene using raytracing
        /// </summary>
        internal void Render()
        {
            //the width of the plane
            Vector3 horizon = upRight - upLeft;
            //the height of the plane
            Vector3 vertical = upLeft - downLeft;

            //draw the redline, representing the screen to be drawn. We divide by 10, because we assume the 'box' of our scene to be 10 by 10 by 10.
            OpenTKApp.Debug.debugScreen.Line(
                (int)(OpenTKApp.Debug.debugScreen.width / 2 - (horizon.Length / 2) * (OpenTKApp.Debug.debugScreen.width / 10)),
                (int)(OpenTKApp.Debug.debugScreen.height - camera.lookAtDirection.Length * (OpenTKApp.Debug.debugScreen.height / 10) * camera.FOV),
                (int)(OpenTKApp.Debug.debugScreen.width / 2 + (horizon.Length / 2) * (OpenTKApp.Debug.debugScreen.width / 10)),
                (int)(OpenTKApp.Debug.debugScreen.height - camera.lookAtDirection.Length * (OpenTKApp.Debug.debugScreen.height / 10) * camera.FOV),
                0xFF0000);



            for (int y = 0; y < OpenTKApp.app.screen.height; y++)
            {
                for (int x = 0; x < OpenTKApp.app.screen.width; x++)
                {
                    //reset the primary ray and set the direction and normalize it
                    primaryRay.scalar = 0;
                    primaryRay.RGB = new Vector3(0);
                    primaryRay.direction = upLeft + ((float)x / (float)OpenTKApp.app.screen.width) * horizon + ((float)y / (float)OpenTKApp.app.screen.height) * vertical;
                    primaryRay.direction.Normalize();
                    //for every object
                    foreach (Primitives p in scene.objects)
                    {
                        //if it is a sphere
                        if (p is Sphere)
                        {
                            primaryRay.scalar = 0;
                            //intersect the ray with the sphere, storing the result in the scalar of the primary ray
                            intersection = new Intersection();
                            //if it hits something
                            if (IntersectSphere((Sphere)p, primaryRay))
                            {
                                primaryRay.RGB = p.RGB;
                                /*
                                //create a shadow ray and set its position
                                Ray shadowRay = new Ray();
                                shadowRay.position = primaryRay.position * primaryRay.scalar;
                                //then for every light
                                foreach (Light l in scene.lights)
                                {
                                    //reset the scalar and set the normal direction
                                    shadowRay.scalar = 0;
                                    shadowRay.direction = l.position - shadowRay.position;
                                    shadowRay.direction.Normalize();
                                    intersection = new Intersection();
                                    //intersect it with the sphere

                                    //if it doesn't hit it, set the color
                                    if (IntersectSphere((Sphere)p, shadowRay))
                                    {
                                        primaryRay.RGB = p.RGB;
                                    }
                                }*/
                            }
                            if(y == OpenTKApp.app.screen.height/2 && x%10 == 0)
                            {
                                // X = width/2 + (-length/2 + length * (x/width)) * (width/10)  
                                // y = height - length * FOV * (height / 10)
                                //define a vector between the origin of the camera, set to be at half the width and full height of the screen and the virtual screen
                                Vector2 DebugRay = new Vector2((OpenTKApp.Debug.debugScreen.width / 2) + (-(horizon.Length / 2) + horizon.Length  + ((float)x / (float)OpenTKApp.app.screen.width)) * (OpenTKApp.Debug.debugScreen.width / 10),
                                    OpenTKApp.Debug.debugScreen.height - camera.lookAtDirection.Length * (OpenTKApp.Debug.debugScreen.height / 10) * camera.FOV);
                                //draw the line between the camera and the screen

                                OpenTKApp.Debug.debugScreen.Line(
                                     OpenTKApp.Debug.debugScreen.width/2,
                                     OpenTKApp.Debug.debugScreen.height,
                                    (int)DebugRay.X + OpenTKApp.Debug.debugScreen.width / 2,
                                    (int)DebugRay.Y + OpenTKApp.Debug.debugScreen.height,
                                    0x00FF00);
                            }
                        }
                    }
                    //using MixColor store the colour of the ray into the pixel
                    OpenTKApp.app.screen.pixels[x + OpenTKApp.app.screen.width * y] = MixColor((int)(primaryRay.RGB[0] * 256),
                        (int)(primaryRay.RGB[1] * 256),
                        (int)(primaryRay.RGB[2] * 256));
                    //GOD MAKE IT WORK (remove later)
                    if (OpenTKApp.app.screen.pixels[x + OpenTKApp.app.screen.width * y] != 0)
                    {
                        Console.WriteLine("IT WORKS");
                    }
                }

            }
        }

        //intersect a ray with the sphere, storing the scalar if it does
        internal bool IntersectSphere(Sphere sphere, Ray ray)
        {
            Vector3 c = sphere.Position - ray.position;
            float t = Vector3.Dot(c, ray.direction);
            Vector3 q = c - t * ray.direction;
            float p2 = q.LengthSquared;
            if (p2 > sphere.Radius)
            {
                return false;
            }
            t -= (float)Math.Sqrt(sphere.Radius - p2);
            if ((t < ray.direction.Length) && (t > 0))
                ray.scalar = t;
            intersection.nearestPrimitive = sphere;
            intersection.distance = ray;
            return true;
        }

        //a ray is defined by a position and a (normalized) direction
        internal struct Ray
        {
            internal Vector3 RGB;
            internal Vector3 position;
            internal float scalar;
            internal Vector3 direction;
        }
        //mix a color
        int MixColor(int red, int green, int blue)
        {
            return (red << 16) + (green << 8) + blue;
        }

    }
}
