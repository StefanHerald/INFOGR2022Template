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
        internal static Camera camera;
        internal static Scene scene;
        //manages if the debug should be drawn
        internal bool debugMode;
        //this manages the gloss factor
        int gloss = 1;
        /// <summary>
        /// initialize
        /// </summary>
        public void Init()
        {
            //setting the scene, the camera and the ray tracer 
            scene = new Scene();
            scene.objects.Add(new Sphere(new Vector3(4, 8, 5), 1, new Vector3(1f, 0f, 0f), Primitives.materials.glossy));
            scene.objects.Add(new Sphere(new Vector3(1, 8, 5.5f), 1f, new Vector3(0f, 1f, 0f), Primitives.materials.mirror));
            scene.objects.Add(new Sphere(new Vector3(-5, 8, 5), 0.4f, new Vector3(0f, 0f, 1f),Primitives.materials.glossy));
          //  Plane p = new Plane(new Vector3(4,8,5), new Vector3(1, 0, 0), new Vector3(1f, 1f, 0f));
           // p.getTexture = true;
           // scene.objects.Add(p);
            //set the lights, first the position, then the color, then the watt strength
            scene.lights.Add(new Light(new Vector3(4, 10, 6), new Vector3(1, 1, 1), 0.8f, gloss));
          //  scene.lights.Add(new Light(new Vector3(4, 8, 3), new Vector3(1, 1, 1), 0.1f, gloss));

            camera = new Camera(new Vector3(0), new Vector3(0, 0, 1), new Vector3(0, 1, 0), 100);
            raytracer = new Raytracer(scene, camera);
        }

        /// <summary>
        /// tick: renders one frame
        /// </summary>
        public void Tick()
        {
            screen.Clear(0x000000);
            OpenTKApp.Debug.Tick();
            raytracer.Render();
            if (debugMode)
            {
                //this is the only way to get this to work properly
                //shows the debug screen
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