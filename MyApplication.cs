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
		// initialize
		public void Init()
		{
			//setting the scene, the camera and the ray tracer (not sure if the values are correct)
			scene = new Scene();
			//scene.objects.Add(new Sphere(new Vector3(3,0,4), 2, new Vector3(0f,0.5f,0.5f)));
			scene.objects.Add(new Sphere(new Vector3(0, 0, 4), 1, new Vector3(0f, 0.5f, 0.5f)));
			scene.lights.Add(new Light(new Vector3(0,1,0), new Vector3(1, 1, 1)));
			camera = new Camera(new Vector3(0), new Vector3(0,0, 1), new Vector3(0,1,0));
			raytracer = new Raytracer(scene, camera);
		}

		// tick: renders one frame
		public void Tick()
		{
			screen.Clear(0x000000);
			raytracer.Render();
			OpenTKApp.Debug.Tick();
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