using System;
using System.Collections.Generic;
using SpaceSim;

class Astronomy
{
    public static void Main()
    {
        List<SpaceObject> solarSystem = new List<SpaceObject>
        {
            //Planet, orbital_radius (km), orbital_period (days), object_radius (km), rotational_period (days), object_color
            //------------------ Stjerna ------------------
            new Star("Sun", 0, 0, 696340, 27, "yellow"),

            //------------------ Planetene ------------------
            new Planet("Mercury", 57910, 88, 2440, 58.6, "gray"),
            new Planet("Venus", 108200, 225, 6052, -243, "yellowish-white"),
            new Planet("Terra", 149600, 365, 6371, 1, "blue"),
            new Planet("Earth", 149600, 365, 6371, 1.03, "blue"),
            new Planet("Mars", 227940, 687, 3390, 0.41, "orange"),
            new Planet("Jupiter", 778330, 4333, 71490000, 0.45, "redish-orangey-white"),
            new Planet("Saturn", 1429400, 10760, 58232, -0.72, "yellowey-orange"),
            new Planet("Uranus", 2870990, 30685, 25362, 0.67, "light-blue"),
            new Planet("Neptune", 4504300, 60190, 24764, -6.39, "light-blue"),

            //Dvergplanetene
            new DwarfPlanet("Pluto", 5913520, 90550, 1151, 6, "brown"),

            //------------------ Månene ------------------
            //Jorda
            new Moon("Earth", "The Moon", 384, 27, 1737, 0, "gray"),

            //Mars
            new Moon("Mars", "Phobos", 9, 0.32, 11, 0, "gray"),
            new Moon("Mars", "Deimos", 23, 1.26, 6, 0, "gray"),

            //Jupiter
            new Moon("Jupiter", "Metis", 128, 0.29, 20, 0, "gray"),
            new Moon("Jupiter", "Adrastea", 129, 0.30, 10, 0, "gray"),
            new Moon("Jupiter", "Amalthea", 181, 0.50, 94, 0, "gray"),
            new Moon("Jupiter", "Thebe", 222, 0.67, 50, 0, "gray"),
            new Moon("Jupiter", "Io", 422, 1.77, 1821, 0, "yellow-red-orange"),
            new Moon("Jupiter", "Europa", 671, 3.55, 1565, 0, "gray-red"),
            new Moon("Jupiter", "Ganymede", 1070, 7.15, 2634, 0, "red-orange"),
            new Moon("Jupiter", "Callisto", 1883, 16.69, 2403, 0, "orange"),
            new Moon("Jupiter", "Leda", 11094, 238.72, 8, 0, "gray"),
            new Moon("Jupiter", "Himalia", 11480, 250.57, 93, 0, "gray"),
            new Moon("Jupiter", "Lysithea", 11720, 259.22, 18, 0, "gray"),
            new Moon("Jupiter", "Elara", 11737, 259.65, 38, 0, "gray"),
            new Moon("Jupiter", "Ananke", 21200, -631.00, 15, 0, "gray"),
            new Moon("Jupiter", "Carme", 22600, -692.00, 20, 0, "gray"),
            new Moon("Jupiter", "Pasiphae", 23500, -735.00, 25, 0, "gray"),
            new Moon("Jupiter", "Sinope", 23700, -758.00, 18, 0, "gray"),

            //Saturn
            new Moon("Saturn", "Pan", 134, 0.58, 10, 0, "gray"),
            new Moon("Saturn", "Atlas", 138, 0.60, 15, 0, "gray"),
            new Moon("Saturn", "Prometheus", 139, 0.61, 46, 0, "gray"),
            new Moon("Saturn", "Pandora", 142, 0.63, 42, 0, "gray"),
            new Moon("Saturn", "Epimetheus", 151, 0.69, 57, 0, "gray"),
            new Moon("Saturn", "Janus", 151, 0.69, 89, 0, "gray"),
            new Moon("Saturn", "Mimas", 186, 0.94, 199, 0, "gray"),
            new Moon("Saturn", "Enceladus", 238, 1.37, 249, 0, "gray"),
            new Moon("Saturn", "Tethys", 295, 1.89, 530, 0, "gray"),
            new Moon("Saturn", "Telesto", 295, 1.89, 15, 0, "gray"),
            new Moon("Saturn", "Calypso", 295, 1.89, 13, 0, "gray"),
            new Moon("Saturn", "Dione", 377, 2.74, 560, 0, "gray"),
            new Moon("Saturn", "Helene", 377, 2.74, 16, 0, "gray"),
            new Moon("Saturn", "Rhea", 527, 4.52, 764, 0, "gray"),
            new Moon("Saturn", "Titan", 1222, 15.95, 2575, 0, "orange-mixed"),
            new Moon("Saturn", "Hyperion", 1481, 21.28, 143, 0, "gray"),
            new Moon("Saturn", "Iapetus", 3561, 79.33, 718, 0, "gray-brown"),
            new Moon("Saturn", "Phoebe", 12952, -550.48, 110, 0, "gray"),

            //Uranus
            new Moon("Uranus", "Cordelia", 50, 0.34, 13, 0, "gray"),
            new Moon("Uranus", "Ophelia", 54, 0.38, 16, 0, "gray"),
            new Moon("Uranus", "Bianca", 59, 0.43, 22, 0, "gray"),
            new Moon("Uranus", "Cressida", 62, 0.46, 33, 0, "gray"),
            new Moon("Uranus", "Desdemona", 63, 0.47, 29, 0, "gray"),
            new Moon("Uranus", "Juliet", 64, 0.49, 42, 0, "gray"),
            new Moon("Uranus", "Portia", 66, 0.51, 55, 0, "gray"),
            new Moon("Uranus", "Rosalind", 70, 0.56, 27, 0, "gray"),
            new Moon("Uranus", "Beinda", 75, 0.62, 34, 0, "gray"),
            new Moon("Uranus", "Puck", 86, 0.76, 77, 0, "gray"),
            new Moon("Uranus", "Miranda", 130, 1.41, 236, 0, "gray"),
            new Moon("Uranus", "Ariel", 191, 2.52, 581, 0, "brown"),
            new Moon("Uranus", "Umbriel", 266, 4.14, 585, 0, "gray"),
            new Moon("Uranus", "Titania", 436, 8.71, 789, 0, "gray"),
            new Moon("Uranus", "Oberon", 583, 13.46, 761, 0, "yellow-orange"),
            new Moon("Uranus", "Caliban", 7169, -580.00, 40, 0, "gray"),
            new Moon("Uranus", "Stephano", 7948, -674.00, 15, 0, "gray"),
            new Moon("Uranus", "Sycorax", 12213, -1289.00, 80, 0, "gray"),
            new Moon("Uranus", "Prospero", 16568, -2019.00, 20, 0, "gray"),
            new Moon("Uranus", "Setebos", 17681, -2239.00, 20, 0, "gray"),

            //Neptune
            new Moon("Neptune", "Naiad", 48, 0.29, 29, 0, "gray"),
            new Moon("Neptune", "Thalassa", 50, 0.31, 40, 0, "gray"),
            new Moon("Neptune", "Despina", 53, 0.33, 74, 0, "gray"),
            new Moon("Neptune", "Galatea", 62, 0.43, 79, 0, "gray"),
            new Moon("Neptune", "Larissa", 74, 0.55, 96, 0, "gray"),
            new Moon("Neptune", "Proteus", 118, 1.12, 209, 0, "gray"),
            new Moon("Neptune", "Triton", 355, -5.88, 1353, 0, "gray-blue"),
            new Moon("Neptune", "Nereid", 5513, 360.13, 170, 0, "gray"),

            //Pluto
            new Moon("Pluto", "Charon", 20, 6.39, 603, 0, "brown-black"),
            new Moon("Pluto", "Nix", 49, 24.86, 23, 0, "gray"),
            new Moon("Pluto", "Hydra", 65, 38.21, 30, 0, "gray"),

            //------------------ Kometene ------------------
            new Comet("Halley's Comet", 2667, 27375, 11, 52, "dirty white"),

            //------------------ Asteroidene ------------------
            new Asteroid("Ceres", 414, 1682, 473, 9, "gray"),

            //------------------ Asteroidebelta ------------------
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
        public double Orbital_period { get; protected set; }
        public int Object_radius { get; protected set; }
        public double Rotational_period { get; protected set; }
        public String Object_color { get; protected set; }

        public SpaceObject(String name, int orbital_radius, double orbital_period, int object_radius, double rotational_period, String object_color)
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
        public Star(String name, int orbital_radius, double orbital_period, int object_radius, double rotational_period, String object_color) 
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
        public Planet(String name, int orbital_radius, double orbital_period, int object_radius, double rotational_period, String object_color) 
            : base(name, orbital_radius, orbital_period, object_radius, rotational_period, object_color)
        {
        }
        public override void Draw()
        {
            Console.Write("Planet       :  " + Name + "  Orbital radius: " + Orbital_radius + " / " + "Orbital period: " + Orbital_period + " / " + "Object radius: " + Object_radius + "km / " + "Rotational period: " + Rotational_period + " / " + "Object color: " + Object_color);
            Console.WriteLine();
        }
    }

    public class DwarfPlanet : SpaceObject
    {
        public DwarfPlanet(String name, int orbital_radius, double orbital_period, int object_radius, double rotational_period, String object_color)
            : base(name, orbital_radius, orbital_period, object_radius, rotational_period, object_color)
        {
        }
        public override void Draw()
        {
            Console.Write("Dwarf Planet       :  " + Name + "  Orbital radius: " + Orbital_radius + " / " + "Orbital period: " + Orbital_period + " / " + "Object radius: " + Object_radius + "km / " + "Rotational period: " + Rotational_period + " / " + "Object color: " + Object_color);
            Console.WriteLine();
        }
    }

    public class Moon : Planet
    {
        public String PlanetName { get; protected set; }

        public Moon(String planetName, String name, int orbital_radius, double orbital_period, int object_radius, double rotational_period, String object_color) 
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
        public Comet(String name, int orbital_radius, double orbital_period, int object_radius, double rotational_period, String object_color) 
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
        public Asteroid_belt(String name, int orbital_radius, double orbital_period, int object_radius, double rotational_period, String object_color) 
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
        public Asteroid(String name, int orbital_radius, double orbital_period, int object_radius, double rotational_period, String object_color) 
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