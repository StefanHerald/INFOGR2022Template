using OpenTK;
using System;

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
        internal float FOV;
        // the plane to define the screen
        internal Plane screen;

        public Camera(Vector3 position, Vector3 lookAtDirection, Vector3 upDirection,  float alpha = 120)
        {
            this.position = position;
            this.lookAtDirection = lookAtDirection;
            this.upDirection = upDirection;
            Vector3 rightDirection = Vector3.Cross(upDirection, lookAtDirection);
            screen = new Plane(position + lookAtDirection, position + lookAtDirection * FOV, new Vector3(0));
            FOV = rightDirection.Length / ((float)(Math.Tan(alpha / 2)) * lookAtDirection.Length);
        }
    }
}
