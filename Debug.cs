using OpenTK;
using System;
using Template;
using System.Linq;

namespace INFOGR2022Template
{
    internal class Debug
	{
		// member variables
		Camera camera;
		Scene debugscene;
		Raytracer raytracer;
		internal bool debugMode;
		// initialize
		internal Surface debugScreen;
		float x1;
		float x2;
		float y1;
		float y2;

		public void Init()

		{
			//setting the scene, the camera and the ray tracer (not sure if the values are correct)
			debugscene = new Scene();
			camera = MyApplication.camera;
			//	debugScreen.Line(0, 0, 100, 100, 0x00FF00);
			raytracer = new Raytracer(debugscene, camera);

		}
		public void Tick()
        {
			//updates the camera for the debug
			camera = MyApplication.camera;
			//Clears the debug screen, then renders it
			debugScreen.Clear(0x000000);
			//Draws the lines for the debug screen (could be looked at again, not sure)
			/*for(int i = -debugScreen.width / 5; i <= debugScreen.width * 1.2; i+= 64)
			debugScreen.Line(debugScreen.width / 2, debugScreen.height, i, 0, 0x00FF00);*/
			//Gets all the spheres in the list of primitives
			var circles = from a in MyApplication.scene.objects.OfType<Sphere>() select a;
			raytracer.Render();
			//For every circle, the position is calculated 100 times using a formula
			foreach (var c in circles)
			{
				Vector2 near = new Vector2(1000, 1000);
				Vector2 far = new Vector2(0);
				double j = 0;
				double k = 0;
				float angle = Vector3.Dot(c.Position, camera.lookAtDirection) / (c.Position.Length * camera.lookAtDirection.Length);
				Vector3 Position = c.Position * angle;
				for (double i = 0; i < 2 * Math.PI; i+= Math.PI / 50)
				{
					//x1 and y1 need to be set before x2 and y2 the first time, otherwise it crashes
					if (i == 0)
                    {
						x1 = (Position.X + (c.Radius * (float)Math.Cos(i))) * debugScreen.width / 16;
						y1 = (Position.Z + (c.Radius * (float)Math.Sin(i))) * debugScreen.height  / 10;
					}
					//x2 and y2 are the previous positions, they make the lines possible
					y2 = y1;
					x2 = x1;
					//x1 and y1 are points on the circle, with 100 lines between the points it looks like a circle
                    x1 = (Position.X + (c.Radius * (float)Math.Cos(i))) * debugScreen.width / 16;
                    y1 = (Position.Z + (c.Radius * (float)Math.Sin(i))) * debugScreen.height / 10;
                    debugScreen.Line((int)x1, (int)y1, (int)x2, (int)y2, 0xFFFFFF);
					//this calculates if the current point is the closest or furthest point from the camera, used for the debug raytracing
					if (new Vector2(x1 - camera.position.X, y1 - camera.position.Z).Length < near.Length)
					{
						near = new Vector2(x1 - camera.position.X, y1 - camera.position.Z);
						j = i;
					}
					if (new Vector2(x1 - camera.position.X, y1 - camera.position.Z).Length > far.Length)
					{
						far = new Vector2(x1 - camera.position.X, y1 - camera.position.Z);
						k = i;
					}

				}
				//the next loop depends on k being smaller than j, so it swaps them if needed
				if(j < k)
                {
					double var = j;
					j = k;
					k = var;
                }
				//this draws the lines from the camera to the circles.
				float dot = 0;
				Vector2 line = far - new Vector2(camera.position.X, camera.position.Z);
				Vector2 line1;
				Vector2 line2;
				for (double i = j + Math.PI / 2; i > k + Math.PI / 2; i -= Math.PI / 10)
                {
					if (camera.position.X > near.X && camera.position.X < far.X)
					if (camera.position.Z > near.Y && camera.position.Z < far.Y)
					{
						Console.WriteLine("Inside of primitive, can't debug");
						break;
					}

					x1 = (Position.X + (c.Radius * (float)Math.Cos(i))) * debugScreen.width / 16;
                    y1 = (Position.Z + (c.Radius * (float)Math.Sin(i))) * debugScreen.height / 10;
                    if (i > j)
                    {
                        x2 = (Position.X + (c.Radius * (float)Math.Cos(i - Math.PI / 10))) * debugScreen.width / 16;
                        y2 = (Position.Z + (c.Radius * (float)Math.Sin(i - Math.PI / 10))) * debugScreen.height / 10;
                    }
					else
					{
						x2 = (Position.X + (c.Radius * (float)Math.Cos(i + Math.PI / 10))) * debugScreen.width / 16;
						y2 = (Position.Z + (c.Radius * (float)Math.Sin(i + Math.PI / 10))) * debugScreen.height / 10;
					}
					line1 = new Vector2(x1, y1);
					line2 = new Vector2(x2, y2);
					line1.Normalize();
					line2.Normalize();
					if (i <= j && (line1.Y < line2.Y))
                        break;
                    else if (i >= j && line1.Y > line2.Y)
                        continue;
                    debugScreen.Line((int)camera.position.X, (int)camera.position.Z, (int)x1, (int)y1, 0xFFFF00);
				}
			}
		}

	}
}
