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
        Vector3 position;
        public Light(Vector3 position, Vector3 RGB): base(RGB)
        {
            this.position = position;
        }
    }
}
