using System;
using System.Collections.Generic;
using SpaceSim;

class Astronomy
{
    public static void Main()
    {
        List<SpaceObject> solarSystem = new List<SpaceObject>
        {
            new Star("Sun", 0, 0, 696340, 27, "yellow"),
            new Planet("Mercury", 58, 88, 2440, 59, "gray"),
            new Planet("Venus", 108, 225, 6052, 243, "yellowish-white"),
            new Planet("Terra", 150, 365, 6371, 1, "blue"),
            new Planet("Earth", 150, 365, 6371, 1, "blue"),
            new Moon("Earth", "The Moon", 384, 27, 1737, 27, "gray"),
            new Comet("Halley's Comet", 2667, 27375, 11, 52, "dirty white"),
            new Asteroid("Ceres", 414, 1682, 473, 9, "gray"),
            new Asteroid_belt("Kuiper belt", 4500, 0, 0, 0, "dark")
        };

        foreach (SpaceObject obj in solarSystem)
        {
            obj.Draw();
        }

        Console.ReadLine();
    }
}

namespace SpaceSim
{
    public class SpaceObject
    {
        public String Name { get; protected set; }
        public int Orbital_radius { get; protected set; }
        public int Orbital_period { get; protected set; }
        public int Object_radius { get; protected set; }
        public int Rotational_period { get; protected set; }
        public String Object_color { get; protected set; }

        public SpaceObject(String name, int orbital_radius, int orbital_period, int object_radius, int rotational_period, String object_color)
        {
            Name = name;
            Orbital_radius = orbital_radius;
            Orbital_period = orbital_period;
            Object_radius = object_radius;
            Rotational_period = rotational_period;
            Object_color = object_color;
        }
        public virtual void Draw()
        {
            Console.WriteLine(Name);
        }
    }
    public class Star : SpaceObject
    {
        public Star(String name, int orbital_radius, int orbital_period, int object_radius, int rotational_period, String object_color) 
            : base(name, orbital_radius, orbital_period, object_radius, rotational_period, object_color)
        {
        }
        public override void Draw()
        {
            Console.Write("Star         :  " + Name + "  Orbital radius: " + Orbital_radius + "/" + "Orbital period: " + Orbital_period + "/" + "Object radius: " + Object_radius + "km" + "/" + "Rotational period: " + Rotational_period + "/" +  "Object color: " + Object_color);
            Console.WriteLine();
        }
    }
    public class Planet : SpaceObject
    {
        public Planet(String name, int orbital_radius, int orbital_period, int object_radius, int rotational_period, String object_color) 
            : base(name, orbital_radius, orbital_period, object_radius, rotational_period, object_color)
        {
        }
        public override void Draw()
        {
            Console.Write("Planet       :  " + Name + "  Orbital radius: " + Orbital_radius + " / " + "Orbital period: " + Orbital_period + " / " + "Object radius: " + Object_radius + "km / " + "Rotational period: " + Rotational_period + " / " + "Object color: " + Object_color);
            Console.WriteLine();
        }
    }
    public class Moon : Planet
    {
        public String PlanetName { get; protected set; }

        public Moon(String planetName, String name, int orbital_radius, int orbital_period, int object_radius, int rotational_period, String object_color) 
            : base(name, orbital_radius, orbital_period, object_radius, rotational_period, object_color)
        {
            PlanetName = planetName;
        }

        public override void Draw()
        {
            Console.Write("Moon         :  Planet(" + PlanetName + ")  " + Name + "  Orbital radius: " + Orbital_radius + " / " + "Orbital period: " + Orbital_period + " / " + "Object radius: " + Object_radius + "km / " + "Rotational period: " + Rotational_period + " / " + "Object color: " + Object_color);
            Console.WriteLine();
        }
    }
    public class Comet : SpaceObject
    {
        public Comet(String name, int orbital_radius, int orbital_period, int object_radius, int rotational_period, String object_color) 
            : base(name, orbital_radius, orbital_period, object_radius, rotational_period, object_color)
        {
        }
        public override void Draw()
        {
            Console.Write("Comet        :  " + Name + "  Orbital radius: " + Orbital_radius + " / " + "Orbital period: " + Orbital_period + " / " + "Object radius: " + Object_radius + "km / " + "Rotational period: " + Rotational_period + " / " + "Object color: " + Object_color);
            Console.WriteLine();
        }
    }
    public class Asteroid_belt : SpaceObject
    {
        public Asteroid_belt(String name, int orbital_radius, int orbital_period, int object_radius, int rotational_period, String object_color) 
            : base(name, orbital_radius, orbital_period, object_radius, rotational_period, object_color)
        {
        }
        public override void Draw()
        {
            Console.Write("Asteroid Belt:  " + Name + "  Orbital radius: " + Orbital_radius + " / " + "Orbital period: " + Orbital_period + " / " + "Object radius: " + Object_radius + "km / " + "Rotational period: " + Rotational_period + " / " + "Object color: " + Object_color);
            Console.WriteLine();
        }
    }
    public class Asteroid : Asteroid_belt
    {
        public Asteroid(String name, int orbital_radius, int orbital_period, int object_radius, int rotational_period, String object_color) 
            : base(name, orbital_radius, orbital_period, object_radius, rotational_period, object_color)
        {
        }
        public override void Draw()
        {
            Console.Write("Asteroid     :  " + Name + "  Orbital radius: " + Orbital_radius + " / " + "Orbital period: " + Orbital_period + " / " + "Object radius: " + Object_radius + "km / " + "Rotational period: " + Rotational_period + " / " + "Object color: " + Object_color);
            Console.WriteLine();
        }
    }
}