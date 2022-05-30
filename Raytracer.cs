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
            //set the pos of the camera
            primaryRay.position = camera.position;
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
                    if((float)x / (float)OpenTKApp.app.screen.width == 0.5 && y/(float)OpenTKApp.app.screen.height == 0.5)
                    {
                        ;
                    }
                    primaryRay.direction = upLeft + ((float)x / (float)OpenTKApp.app.screen.width) * horizon + ((float)y / (float)OpenTKApp.app.screen.height) * vertical;
                    primaryRay.direction.Normalize();
                    if(Math.Abs(primaryRay.direction.X) <= 0.5 && Math.Abs(primaryRay.direction.Y) <= 0.5 && primaryRay.direction.Z <= 3)
                    {
                        Console.WriteLine("found one: " + x + " " + y);
                    }
                    //for every object
                    foreach (Primitives p in scene.objects)
                    {
                        //if it is a sphere
                        if (p is Sphere)
                        {
                            primaryRay.scalar = 1;
                            //intersect the ray with the sphere, storing the result in the scalar of the primary ray
                            intersection = new Intersection();
                            //if it hits something
                            if (IntersectSphere((Sphere)p, primaryRay))
                            {
                                
                                //create a shadow ray and set its position
                                Ray shadowRay = new Ray();
                                shadowRay.position = primaryRay.position + primaryRay.direction * primaryRay.scalar;
                                //then for every light
                                foreach (Light l in scene.lights)
                                {
                                    //reset the scalar and set the normal direction
                                    shadowRay.scalar = 0;
                                    shadowRay.direction = l.position - shadowRay.position;
                                    shadowRay.direction.Normalize();
                                    bool hitAny = false;
                                    foreach (Primitives toCheck in scene.objects)
                                    {
                                        intersection = new Intersection();
                                        if (toCheck is Sphere) 
                                        {
                                            hitAny = IntersectSphere((Sphere)toCheck, shadowRay);
                                        }
                                        if (hitAny)
                                            break;
                                    }
                                    //if it doesn't hit any, set the color
                                    if (!hitAny)
                                    {

                                        primaryRay.RGB = l.returnColor(p.ReturnNormal(primaryRay.position + primaryRay.direction * primaryRay.scalar), shadowRay.direction);
                                    }
                                }
                            }
                            if(y == OpenTKApp.app.screen.height/2 && x%10 == 0)
                            { 
                                //draw the line between the camera and the screen

                                OpenTKApp.Debug.debugScreen.Line(
                                     OpenTKApp.Debug.debugScreen.width/2,
                                     OpenTKApp.Debug.debugScreen.height,
                                    (int)primaryRay.direction.X * -OpenTKApp.Debug.debugScreen.width / 10 + OpenTKApp.Debug.debugScreen.width / 2,
                                    (int)primaryRay.direction.Z * -OpenTKApp.Debug.debugScreen.height / 10 + OpenTKApp.Debug.debugScreen.height,
                                    0x00FF00);
                            }
                        }
                    }
                    //using MixColor store the colour of the ray into the pixel
                    OpenTKApp.app.screen.Plot(x, y, MixColor((int)(primaryRay.RGB.X * 255),
                        (int)(primaryRay.RGB.Y * 255),
                        (int)(primaryRay.RGB.Z * 255)));
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
            if ((t < ray.scalar) && (t > 0))
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
