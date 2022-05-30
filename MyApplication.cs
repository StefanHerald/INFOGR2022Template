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
        internal bool debugMode;
        int a = 0;
        // initialize
        public void Init()
        {
            //setting the scene, the camera and the ray tracer (not sure if the values are correct)
            scene = new Scene();
            scene.objects.Add(new Sphere(new Vector3(3, 8, 5), 2, new Vector3(1f, 0f, 0f)));
            scene.objects.Add(new Sphere(new Vector3(-3, 16, 10), 1, new Vector3(0f, 1f, 0f)));
            scene.lights.Add(new Light(new Vector3(6,4,3), new Vector3(1, 1, 1), 1000));
            camera = new Camera(new Vector3(0), new Vector3(0, 0, 1), new Vector3(0, 1, 0));
            raytracer = new Raytracer(scene, camera);
        }

        // tick: renders one frame
        public void Tick()
        {
            //screen.Clear(0x000000);
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
            a++;
        }
    }
}