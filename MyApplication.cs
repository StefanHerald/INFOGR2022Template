using INFOGR2022Template;
using OpenTK;
using System;

namespace Template
{
    class MyApplication
    {
        // member variables
        internal Surface screen;
        Raytracer raytracer;
        Camera camera;
        internal static Scene scene;
        //manages if the debug should be drawn
        internal bool debugMode;
        //this manages the gloss factor
        int gloss = 10;
        // initialize
        public void Init()
        {
            //setting the scene, the camera and the ray tracer (not sure if the values are correct)
            scene = new Scene();
         //   scene.objects.Add(new Sphere(new Vector3(4, 16, 10), 2, new Vector3(1f, 0f, 0f)));
         //   scene.objects.Add(new Sphere(new Vector3(-3, 8, 5), 0.9f, new Vector3(0f, 1f, 0f)));
            scene.objects.Add(new Sphere(new Vector3(-1, 8, 5), 4, new Vector3(0f, 0f, 1f),Primitives.materials.glossy));
            //scene.objects.Add(new Plane(new Vector3(100, 0, 0), new Vector3(0, 0, -1000000000000), new Vector3(1f, 1f, 0f)));
            //set the lights, first the position, then the color, then the watt strength
            scene.lights.Add(new Light(new Vector3(-1, 8, 6), new Vector3(1, 1, 1), 0.7f, gloss));
         //   scene.lights.Add(new Light(new Vector3(10, 0, 10), new Vector3(1, 1, 1), 100f, gloss));

            //scene.lights.Add(new Light(new Vector3(10, 5, 2), new Vector3(1, 1, 1), 1f));
            camera = new Camera(new Vector3(0), new Vector3(0, 0, 1), new Vector3(0, 1, 0));
            raytracer = new Raytracer(scene, camera);
        }

        // tick: renders one frame
        public void Tick()
        {
            screen.Clear(0x000000);
            OpenTKApp.Debug.Tick();
            raytracer.Render();
            if (debugMode)
            {
                //this is the only way to get this to work properly
                int s = 0;
                foreach (int p in OpenTKApp.Debug.debugScreen.pixels)
                {
                    screen.pixels[s] = p;
                    s++;
                }
            }
        }
    }
}