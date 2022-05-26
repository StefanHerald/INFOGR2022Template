
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
		Scene scene;

		// initialize
		public void Init()
		{
			//setting the scene, the camera and the ray tracer (not sure if the values are correct)
			scene = new Scene();


			scene.objects.Add(new Sphere(new Vector3(0,0,4), 1, new Vector3(0f,0.5f,0.5f)));
			scene.lights.Add(new Light(new Vector3(0,1,0), new Vector3(1, 1, 1)));
			camera = new Camera(new Vector3(0), new Vector3(0,0, 1), new Vector3(0,1,0));
			raytracer = new Raytracer(scene, camera);
			screen.Line(0, 0, 100, 100, 0x00FF00);

		}

		// tick: renders one frame
		public void Tick()
		{
			raytracer.Render();
		}
		int TX(float x, int origin = 320, int scale = 4)
        {
            x += scale/ 2;
            x *= screen.width / scale;
			x += origin - screen.width/2;
            return (int)x;
        }

		int TY(float y, int origin = 200, int scale = 4)
        {
			y *= -1;
			y += scale/2;
			y *= screen.height/scale;
			y += origin - screen.height/2 ;
			return (int)y;
		}
    }
}