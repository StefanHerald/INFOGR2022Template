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
        internal Vector3 position;
        //this vector stores the direction the camera looks at
        internal Vector3 lookAtDirection;
        //the up direction of the camera for the plane
        internal Vector3 upDirection;
        // the Field Of View of the camera
        internal float FOV = 1;
        // the plane to define the screen
        internal Plane screen;

        public Camera(Vector3 position, Vector3 lookaAtDirection, Vector3 upDirection,  float FOV = 1)
        {
            this.position = position;
            this.FOV = FOV;
            this.lookAtDirection = lookaAtDirection;
            this.upDirection = upDirection;
            screen = new Plane(position + lookAtDirection, FOV, new Vector3(0));
        }
    }
}
