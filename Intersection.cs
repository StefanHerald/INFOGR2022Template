using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOGR2022Template
{
    internal struct Intersection
    {
        //the nearest primitive to check
        internal Primitives nearestPrimitive;
        //the distance to the primitve
        internal Raytracer.Ray distance;
        //the normal from the primitve 
        internal Vector3 normal;
        public Intersection(Primitives nearestPrimitive, Raytracer.Ray distance, Vector3 normal)
        {
            this.nearestPrimitive = nearestPrimitive;
            this.distance = distance;
            this.normal = normal;
        }
    }
}
