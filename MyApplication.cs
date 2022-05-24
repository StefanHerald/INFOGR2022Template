
using INFOGR2022Template;

namespace Template
{
	class MyApplication
	{
		// member variables
		public Surface screen;
		public Raytracer raytracer;
		// initialize
		public void Init()
		{

		}

        public void RenderGL()
        {
			screen.Clear(0x000000);
		}

		// tick: renders one frame
		public void Tick()
		{
			raytracer.Render();
		}


        int MixColor(int red, int green, int blue) 
		{
			return (red << 16) + (green << 8) + blue;
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