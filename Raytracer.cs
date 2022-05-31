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
        //cap for the mirror recursion
        int recursionCap;
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
                    primaryRay.RGB = new Vector3(0.05f);
                    primaryRay.direction = upLeft + ((float)x / (float)OpenTKApp.app.screen.width) * horizon + ((float)y / (float)OpenTKApp.app.screen.height) * vertical;
                    primaryRay.direction.Normalize();
                    //for every object
                    foreach (Primitives p in scene.objects)
                    {
                        primaryRay.scalar = 1;
                        intersection = new Intersection();
                        //if it is a sphere
                        //intersect the ray with the sphere, storing the result in the scalar of the primary ray
                        //if it hits something
                        if (Intersect(p, primaryRay))
                        {

                            //change the RGB color from background to zero
                            primaryRay.RGB = new Vector3(0);
                            //set the scalar to the scalar calculated for the intersection
                            primaryRay.scalar = intersection.distance.scalar;
                            //if it is mirror material, calculate it differently 
                            if (p.material == Primitives.materials.mirror)
                            {
                                Ray secondaryRay = new Ray();
                                secondaryRay.position = primaryRay.position + primaryRay.direction * primaryRay.scalar;
                                secondaryRay.direction = primaryRay.direction - 2 * (primaryRay.direction * p.ReturnNormal(secondaryRay.position)) * p.ReturnNormal(secondaryRay.position);
                                secondaryRay.direction.Normalize();
                                primaryRay.RGB = CalculateMirror(p, secondaryRay);
                                break;
                            }
                            //calculate the colour according to lights in scene
                            primaryRay.RGB = returnColorLight(p, primaryRay);
                        }
                    }

                    //using MixColor store the colour of the ray into the pixel. ofc, the colour cant be less than 0 or more than 255
                    OpenTKApp.app.screen.Plot(x, y, MixColor((int)(MathHelper.Clamp(primaryRay.RGB.X,0,1) * 255),
                        (int)(MathHelper.Clamp(primaryRay.RGB.Y, 0, 1) * 255),
                        (int)(MathHelper.Clamp(primaryRay.RGB.Z, 0, 1) * 255)));
                }

            }
        }

        Vector3 returnColorLight(Primitives p, Ray calcRay)
        {
            //create a shadow ray and set its position

            Ray shadowRay = new Ray();
            shadowRay.position = calcRay.position + calcRay.direction * calcRay.scalar;
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
                    hitAny = Intersect(toCheck, shadowRay);
                    if (hitAny)
                        break;
                }
                //if it doesn't hit any, set the color
                if (!hitAny)
                {
                    calcRay.RGB += l.returnColor(p.ReturnNormal(calcRay.position + calcRay.direction * calcRay.scalar),
                        shadowRay.direction,
                        p.GetColour(calcRay.position + calcRay.direction * calcRay.scalar),
                        -camera.lookAtDirection,
                        p.material);
                }
                else
                {
                    calcRay.RGB = new Vector3(0.07f);
                }
            }
            return calcRay.RGB;
        }
        /// <summary>
        /// calculates the colour for a pixel on a mirror primitive
        /// </summary>
        /// <param name="toCheck"></param>
        /// <param name="secondaryRay"></param>
        /// <returns></returns>
        Vector3 CalculateMirror(Primitives toCheck, Ray secondaryRay)
        {
            if (secondaryRay.amountOfRecursion++ == recursionCap)
                return new Vector3(0);
            foreach (Primitives p in scene.objects)
            {
                if (Intersect(p, secondaryRay))
                    if (p.material == Primitives.materials.mirror)
                    {
                        secondaryRay.RGB *= toCheck.RGB;
                        secondaryRay.position = secondaryRay.position + secondaryRay.direction * intersection.distance.scalar;
                        secondaryRay.direction = secondaryRay.direction - 2 * (secondaryRay.direction * p.ReturnNormal(secondaryRay.position)) * p.ReturnNormal(secondaryRay.position);
                        secondaryRay.direction.Normalize();
                        return CalculateMirror(p, secondaryRay);
                    }
                    else
                    {
                        secondaryRay.RGB *= returnColorLight(p, secondaryRay);
                    }
            }
            return secondaryRay.RGB;
        }

       
        bool Intersect(Primitives toCheck, Ray ray)
        {
            bool inter = false;
            if (toCheck is Sphere)
                inter = IntersectSphere((Sphere)toCheck, ray);
            if (toCheck is Plane)
                inter = IntersectPlane((Plane)toCheck, ray);
            return inter;

        }
        //intersect a ray with the sphere, storing the scalar if it does
        bool IntersectSphere(Sphere sphere, Ray ray)
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
            if (Math.Abs(t) > 0.001)
            {
                ray.scalar = t;
                intersection.nearestPrimitive = sphere;
                intersection.distance = ray;
                return true;
            }
            return false;
        }

        internal bool IntersectPlane(Plane plane, Ray ray)
        {
            intersection.nearestPrimitive = plane;
            intersection.distance = ray;
            float scalar = Vector3.Dot(plane.normal, ray.direction);
            if (scalar > 0)
            {
                Vector3 intersect = plane.distance - ray.position;
                var t = Vector3.Dot(intersect, plane.normal) / scalar;
                if (t >= 0.0001)
                {
                    intersection.distance.scalar = t;
                    return true;
                }
            }
            return false;
        }

        //a ray is defined by a position and a (normalized) direction
        internal struct Ray
        {
            internal Vector3 RGB;
            internal Vector3 position;
            internal float scalar;
            internal Vector3 direction;
            internal int amountOfRecursion;
        }
        //mix a color
        int MixColor(int red, int green, int blue)
        {
            return (red << 16) + (green << 8) + blue;
        }

    }
}
