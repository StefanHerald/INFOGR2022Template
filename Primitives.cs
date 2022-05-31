using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


internal class Primitives
{
    //the material
    internal enum materials { diffuse, glossy, mirror};
    internal double reflectiveIndex;
    internal materials material;
    internal bool getTexture;
    //a vector 3 for the color. All float values must be between 0 and 1.
    internal Vector3 RGB;
    /// <summary>
    /// initialize
    /// </summary>
    /// <param name="RGB"></param>
    /// <param name="material"></param>
    /// <param name="reflectiveIndex"></param>
    public Primitives(Vector3 RGB, materials material = materials.diffuse, double reflectiveIndex = 1)
    {
        this.RGB = RGB;
        this.material = material;
        this.reflectiveIndex = reflectiveIndex;
    }

    /// <summary>
    /// this returns the normal, if given a distance. Every primitive must have this. 
    /// </summary>
    /// <param name="distance"></param>
    /// <returns></returns>
    internal virtual Vector3 ReturnNormal(Vector3 distance)
    {
        return new Vector3(0);
    }
    /// <summary>
    /// returns the color at a specific point
    /// is either something specific, or just the RGB colour
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    internal virtual Vector3 GetColour(Vector3 point)
    {
        return RGB;
    }
}

internal class Plane : Primitives
{
    //defines the normal and distance by which the plane is defined
    internal Vector3 normal;
    internal Vector3 distance;
    /// <summary>
    /// intitialize
    /// </summary>
    /// <param name="normal"></param>
    /// <param name="distance"></param>
    /// <param name="RGB"></param>
    /// <param name="material"></param>
    public Plane(Vector3 normal, Vector3 distance, Vector3 RGB, materials material = materials.diffuse) : base(RGB, material)
    {

        this.normal = normal;
        this.normal.Normalize();
        this.distance = distance;
        this.material = material;
    }

    /// <summary>
    /// a plane can only return two normals, one for each side.
    /// </summary>
    /// <param name="dist"></param>
    /// <returns></returns>
    internal override Vector3 ReturnNormal(Vector3 dist)
    {
        float inproduct = dist.X * normal.X + dist.Y * normal.Y + dist.Z * normal.Z;
        double angle = Math.Acos(inproduct / (dist.Length * normal.Length));
        if(Math.Abs(angle) > 90)
        {
            return normal;
        }
        return -normal;
    }
    /// <summary>
    /// get the colour or the texture
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    internal override Vector3 GetColour(Vector3 point)
    {
        if (getTexture)
        {
            return new Vector3(point.X/10f,point.Y/5f, point.Z/2f);
        }
        return base.GetColour(point);
    }
}

internal class Sphere : Primitives
{
    //a position and a radius to define the circle
    internal Vector3 Position;
    internal float Radius;
    /// <summary>
    /// intitialize
    /// </summary>
    /// <param name="Position"></param>
    /// <param name="Radius"></param>
    /// <param name="RGB"></param>
    /// <param name="material"></param>
    public Sphere(Vector3 Position, float Radius, Vector3 RGB, materials material = materials.diffuse) : base(RGB, material)
    {
        this.Position = Position;
        this.Radius = Radius;
        this.material = material;
    }
    /// <summary>
    /// returns a normal on the surface 
    /// </summary>
    internal override Vector3 ReturnNormal(Vector3 distance)
    {
        Vector3 toReturn = new Vector3(Position.X - distance.X, Position.Y - distance.Y, Position.Z - distance.Z);
        toReturn.Normalize();
        return toReturn; 
    }
}
        

