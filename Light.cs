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
        public Light(Vector3 position, Vector3 RGB, float watt) : base(RGB)
        {
            this.position = position;
            this.watt = watt;
        }

        internal Vector3 returnColor(Vector3 normal, Vector3 lightDirection, Vector3 colour)
        {
            Vector3 reflected = (1f / lightDirection.LengthSquared) * watt * RGB * Math.Max(0, Vector3.Dot(normal, lightDirection));
            return new Vector3(Math.Min(reflected.X,colour.X), Math.Min(reflected.Y, colour.Y), Math.Min(reflected.Z, colour.Z));
        }
    }
}
