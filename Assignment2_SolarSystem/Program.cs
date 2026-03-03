using System;
using System.Collections.Generic;
using Spacesim;

class Astronomy
{
    public static void Main()
    {
        List<SpaceObject> solarSystem = new List<SpaceObject>
        {
            new Star("Sun"),
            new Planet("Mercury"),
            new Planet("Venus"),
            new Planet("Terra"),
            new Moon("The Moon")
        };

        foreach (SpaceObject obj in solarSystem)
        {
            obj.Draw();
        }

        Console.ReadLine();
    }
}

namespace Spacesim
{
    public class SpaceObject
    {
        public String Name { get; protected set; }

        public SpaceObject(String name)
        {
            Name = name;
        }
        public virtual void Draw()
        {
            Console.WriteLine(Name);
        }
    }
    public class Star : SpaceObject
    {
        public Star(String name) : base(name) { }
        public override void Draw()
        {
            Console.Write("Star  : ");
            base.Draw();
        }
    }
    public class Planet : SpaceObject
    {
        public Planet(String name) : base(name) { }
        public override void Draw()
        {
            Console.Write("Planet:  ");
            base.Draw();
        }
    }
    public class Moon : Planet
    {
        public Moon(String name) : base(name) { }

        public override void Draw()
        {
            Console.Write("Moon  : ");
            base.Draw();
        }
    }
}