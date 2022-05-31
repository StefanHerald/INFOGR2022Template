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
					if (i == 0)
                    {
						x1 = (Position.X + (c.Radius * (float)Math.Cos(i))) * debugScreen.width / 16;
						y1 = (Position.Z + (c.Radius * (float)Math.Sin(i))) * debugScreen.height  / 10;
					}
					//x2 and y2 are the previous positions, they make the lines possible
					y2 = y1;
					x2 = x1;
                    x1 = (Position.X + (c.Radius * (float)Math.Cos(i))) * debugScreen.width / 16;
                    y1 = (Position.Z + (c.Radius * (float)Math.Sin(i))) * debugScreen.height / 10;
                    debugScreen.Line((int)x1, (int)y1, (int)x2, (int)y2, 0xFFFFFF);
					if (new Vector2(x1 - camera.position.X, y1 - camera.position.Y).Length < near.Length)
					{
						near = new Vector2(x1 - camera.position.X, y1 - camera.position.Y);
						j = i;
					}
					if (new Vector2(x1 - camera.position.X, y1 - camera.position.Y).Length > far.Length)
					{
						far = new Vector2(x1 - camera.position.X, y1 - camera.position.Y);
						k = i;
					}

				}
				if(j < k)
                {
					double var = j;
					j = k;
					k = var;
                }
				for (double i = j + Math.PI / 2; i > k + Math.PI / 2; i -= Math.PI / 10)
                {
					x1 = (Position.X + (c.Radius * (float)Math.Cos(i))) * debugScreen.width / 16;
					y1 = (Position.Z + (c.Radius * (float)Math.Sin(i ))) * debugScreen.height / 10;
					debugScreen.Line((int)camera.position.X, (int)camera.position.Z, (int)x1, (int)y1, 0xFFFF00);
                }
			}
		}

	}
}
