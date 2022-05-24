using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOGR2022Template
{
    internal class Camera
    {
        //this stores the position relative to the updirection of the camera
        static Vector3 position;
        //this vector stores the direction the camera looks at
        static Vector3 LookAtDirection;
        //this stores the Upwards direction of the camera
        static Vector3 UpDirection;
        // the plane to define the screen
        Plane screen = new Plane(position + UpDirection + LookAtDirection, 1, new Vector3(0));

    }
}
