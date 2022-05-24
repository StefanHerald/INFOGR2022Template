using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template;

namespace INFOGR2022Template
{
    internal class Raytracer
    {
        Scene scene;
        Camera camera;
        Surface screen;

        public Raytracer(Scene scene, Camera camera, Surface screen)
        {
            this.scene = scene;
            this.camera = camera;
            this.screen = screen;
        }

        internal void Render()
        {
            
        }
    }
}
