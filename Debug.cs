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
			camera = new Camera(new Vector3(0), new Vector3(0, 0, 1), new Vector3(0, 1, 0));
		//	debugScreen.Line(0, 0, 100, 100, 0x00FF00);
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
			//For every circle, the position is calculated 100 times using a formula
			foreach (var c in circles)
			{
				for (double i = 0; i < 2 * Math.PI; i+= Math.PI / 50)
				{
					if (i == 0)
                    {
						x1 = (c.Position.X + (c.Radius * (float)Math.Cos(i))) * debugScreen.width / 10 + c.Position.X;
						y1 = (c.Position.Z + (c.Radius * (float)Math.Sin(i))) * debugScreen.width / 10 + c.Position.Z;
					}
					//x2 and y2 are the previous positions, they make the lines possible
					y2 = y1;
					x2 = x1;
                    x1 = (c.Position.X + (c.Radius * (float)Math.Cos(i))) * debugScreen.width / 10 + c.Position.X;
                    y1 = (c.Position.Z + (c.Radius * (float)Math.Sin(i))) * debugScreen.width / 10 + c.Position.Z;
                    debugScreen.Line((int)x1, (int)y1, (int)x2, (int)y2, 0xFFFFFF);
				}
			}
		}

	}
}
