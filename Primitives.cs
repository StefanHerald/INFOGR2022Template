using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


internal class Primitives
{
    //a vector 3 for the color. All float values must be between 0 and 1.
    internal Vector3 RGB;
    public Primitives(Vector3 RGB)
    {
        this.RGB = RGB;
    }

    //this returns the normal, if given a distance. Every primitve must have this. 
    //The regular expression simply return itself * -1, which is almost never correct
    internal virtual Vector3 ReturnNormal(Vector3 distance)
    {
        distance.Normalize();
        return -distance;
    }
}

internal class Plane : Primitives
{
    //defines the normal and distance by which the plane is defined
    internal Vector3 normal;
    internal float distance;
    //intitialize
    public Plane(Vector3 normal, float distance, Vector3 RGB) : base(RGB)
    {

        this.normal = normal;
        this.normal.Normalize();
        this.distance = distance;
    }

    // a plane can only return two normals, one for each side.

    internal override Vector3 ReturnNormal(Vector3 dist)
    {
        float inproduct = dist.X * normal.X + dist.Y * normal.Y + dist.Z * normal.Z;
        double angle = Math.Acos(inproduct / (dist.Length * normal.Length));
        if(angle <= 90)
        {
            return normal;
        }
        return -normal;
    }
}

internal class Sphere : Primitives
{
    //a position and a radius to define the circle
    internal Vector3 Position;
    internal float Radius;
    //intitialize
    public Sphere(Vector3 Position, float Radius, Vector3 RGB) : base(RGB)
    {
        this.Position = Position;
        this.Radius = Radius;
    }

    //returns a normal on the surface 
    internal override Vector3 ReturnNormal(Vector3 distance)
    {
        Vector3 toReturn = new Vector3(Position.X - distance.X, Position.Y - distance.Y, Position.Z - distance.Z);
        toReturn.Normalize();
        return toReturn; 
    }
}
        

