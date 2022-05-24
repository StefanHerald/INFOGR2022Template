using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOGR2022Template
{
    internal class Intersection
    {
        //the nearest primitive to check
        Primitives nearestPrimitive;
        //the distance to the primitve
        Vector3 distance;
        //the normal from the primitve 
        Vector3 normal;
        public Intersection(Primitives nearestPrimitive, Vector3 distance, Vector3 normal)
        {
            this.nearestPrimitive = nearestPrimitive;
            this.distance = distance;
            this.normal = normal;
        }
    }
}
