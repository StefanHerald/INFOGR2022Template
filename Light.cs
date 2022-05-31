using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOGR2022Template
{
    internal class Light : Primitives
    {
        //position of the light
        internal Vector3 position;
        internal float watt;
        int gloss = 1;
        public Light(Vector3 position, Vector3 RGB, float watt, int gloss) : base(RGB)
        {
            this.gloss = gloss;
            this.position = position;
            this.watt = watt;
        }

        /// <summary>
        /// returns the color at a specific point in accordance to a material
        /// </summary>
        /// <param name="normal"></param>
        /// <param name="lightDirection"></param>
        /// <param name="colour"></param>
        /// <param name="lookAtDirection"></param>
        /// <param name="material"></param>
        /// <returns></returns>
        internal Vector3 returnColor(Vector3 normal, Vector3 lightDirection, Vector3 colour, Vector3 lookAtDirection, Primitives.materials material)
        {
            Vector3 reflected = new Vector3();
            switch (material)
            {
                case Primitives.materials.diffuse:
                    reflected = (1f / lightDirection.LengthSquared) * watt * RGB * Math.Max(0, Vector3.Dot(normal, lightDirection));
                    reflected = Vector3.Clamp(reflected, new Vector3(0), colour);
                    break;
                case Primitives.materials.glossy:
                    Vector3 R = lightDirection - 2 * (lightDirection * normal) * normal;
                    reflected = returnColor(normal,lightDirection,colour,lookAtDirection,Primitives.materials.diffuse) + (1f / lightDirection.LengthSquared) * watt * RGB * (float)Math.Pow(Math.Max(0, Vector3.Dot(R, lookAtDirection)), gloss);
                    break;
            }
            return Vector3.Clamp(new Vector3(0.07f),reflected,new Vector3(1));
        }
    }
}
