using System;
using System.Collections.Generic;
using System.Linq;
using SpaceSim;

class Astronomy
{
    public static void Main()
    {
        Console.WriteLine("=== Solar System Simulation ===\n");

        // Create and populate the solar system
        SolarSystem solarSystem = CreateSolarSystem();

        // Main interaction loop (Task 4)
        while (true)
        {
            Console.WriteLine("\n--- New Query ---");

            // Get time from user
            Console.Write("Enter time (days since time 0): ");
            string timeInput = Console.ReadLine();
            if (!double.TryParse(timeInput, out double days))
            {
                Console.WriteLine("Invalid time. Using 0.");
                days = 0;
            }

            // Get planet name from user
            Console.Write("Enter planet name (or press Enter for Sun/all planets): ");
            string planetName = Console.ReadLine()?.Trim();

            Console.WriteLine();

            if (string.IsNullOrWhiteSpace(planetName))
            {
                // Show sun and all planets
                Console.WriteLine(solarSystem.Sun.GetDetailsString(days));
                Console.WriteLine("\n--- Planets ---");
                foreach (var planet in solarSystem.Planets)
                {
                    Console.WriteLine(planet.GetDetailsString(days));
                }
                if (solarSystem.DwarfPlanets.Count > 0)
                {
                    Console.WriteLine("\n--- Dwarf Planets ---");
                    foreach (var dwarf in solarSystem.DwarfPlanets)
                    {
                        Console.WriteLine(dwarf.GetDetailsString(days));
                    }
                }
            }
            else
            {
                // Show specific planet
                var planet = solarSystem.GetPlanet(planetName);
                if (planet == null)
                {
                    Console.WriteLine($"Planet '{planetName}' not found.");
                }
                else
                {
                    Console.WriteLine(planet.GetDetailsString(days));

                    var moons = solarSystem.GetMoonsForPlanet(planetName);
                    if (moons.Count > 0)
                    {
                        Console.WriteLine($"\n--- Moons of {planetName} ---");
                        foreach (var moon in moons)
                        {
                            Console.WriteLine(moon.GetDetailsString(days));
                        }
                    }
                }
            }

            Console.WriteLine("\nPress any key to continue, or ESC to exit...");
            if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                break;
        }
    }

    static SolarSystem CreateSolarSystem()
    {
        SolarSystem system = new SolarSystem();

                    // Sun
                    Star sun = new Star("Sun", 0, 0, 696340, 27, "yellow");
                    system.SetSun(sun);

                    // Planets
                    Planet mercury = new Planet("Mercury", 57910000, 88, 2440, 58.6, "gray");
                    Planet venus = new Planet("Venus", 108200000, 225, 6052, -243, "yellowish-white");
                    Planet earth = new Planet("Earth", 149600000, 365, 6371, 1.03, "blue");
                    Planet mars = new Planet("Mars", 227940000, 687, 3390, 1.03, "orange");
                    Planet jupiter = new Planet("Jupiter", 778330000, 4333, 71492, 0.41, "redish-orangey-white");
                    Planet saturn = new Planet("Saturn", 1429400000, 10760, 58232, 0.45, "yellowey-orange");
                    Planet uranus = new Planet("Uranus", 1500000000, 30685, 25362, -0.72, "light-blue");  // Reduced to fit in int
                    Planet neptune = new Planet("Neptune", 2147483647, 60190, 24764, 0.67, "light-blue"); // Max int value

                    system.AddPlanet(mercury);
                    system.AddPlanet(venus);
                    system.AddPlanet(earth);
                    system.AddPlanet(mars);
                    system.AddPlanet(jupiter);
                    system.AddPlanet(saturn);
                    system.AddPlanet(uranus);
                    system.AddPlanet(neptune);

                    // Dwarf Planets
                    DwarfPlanet pluto = new DwarfPlanet("Pluto", (int)(5913520000L & 0x7FFFFFFF), 90550, 1151, 6.39, "brown");
                    system.AddDwarfPlanet(pluto);

                    // Moons - Earth
                    system.AddMoon(new Moon("Earth", "The Moon", 384400, 27.3, 1737, 27.3, "gray"), earth);

                    // Moons - Mars
                    system.AddMoon(new Moon("Mars", "Phobos", 9376, 0.32, 11, 0.32, "gray"), mars);
                    system.AddMoon(new Moon("Mars", "Deimos", 23460, 1.26, 6, 1.26, "gray"), mars);

                    // Moons - Jupiter (major ones)
                    system.AddMoon(new Moon("Jupiter", "Io", 421800, 1.77, 1821, 1.77, "yellow-red-orange"), jupiter);
                    system.AddMoon(new Moon("Jupiter", "Europa", 671100, 3.55, 1565, 3.55, "gray-red"), jupiter);
                    system.AddMoon(new Moon("Jupiter", "Ganymede", 1070400, 7.15, 2634, 7.15, "red-orange"), jupiter);
                    system.AddMoon(new Moon("Jupiter", "Callisto", 1882700, 16.69, 2403, 16.69, "orange"), jupiter);

                    // Moons - Saturn (major ones)
                    system.AddMoon(new Moon("Saturn", "Mimas", 185540, 0.94, 198, 0.94, "gray"), saturn);
                    system.AddMoon(new Moon("Saturn", "Enceladus", 238040, 1.37, 252, 1.37, "gray"), saturn);
                    system.AddMoon(new Moon("Saturn", "Tethys", 294670, 1.89, 533, 1.89, "gray"), saturn);
                    system.AddMoon(new Moon("Saturn", "Dione", 377420, 2.74, 562, 2.74, "gray"), saturn);
                    system.AddMoon(new Moon("Saturn", "Rhea", 527070, 4.52, 764, 4.52, "gray"), saturn);
                    system.AddMoon(new Moon("Saturn", "Titan", 1221870, 15.95, 2575, 15.95, "orange-mixed"), saturn);

                    // Moons - Uranus (major ones)
                    system.AddMoon(new Moon("Uranus", "Miranda", 129900, 1.41, 236, 1.41, "gray"), uranus);
                    system.AddMoon(new Moon("Uranus", "Ariel", 190900, 2.52, 579, 2.52, "brown"), uranus);
                    system.AddMoon(new Moon("Uranus", "Umbriel", 266000, 4.14, 585, 4.14, "gray"), uranus);
                    system.AddMoon(new Moon("Uranus", "Titania", 436300, 8.71, 789, 8.71, "gray"), uranus);
                    system.AddMoon(new Moon("Uranus", "Oberon", 583500, 13.46, 761, 13.46, "yellow-orange"), uranus);

                    // Moons - Neptune (major ones)
                    system.AddMoon(new Moon("Neptune", "Triton", 354800, 5.88, 1353, 5.88, "gray-blue"), neptune);
                    system.AddMoon(new Moon("Neptune", "Nereid", 5513400, 360.13, 170, 360.13, "gray"), neptune);

                    // Moons - Pluto
                    system.AddMoon(new Moon("Pluto", "Charon", 19640, 6.39, 606, 6.39, "brown-black"), pluto);

                    // Comets
                    system.AddComet(new Comet("Halley's Comet", 2000000000, 27375, 11, 52, "dirty white"));

                    // Asteroids
                    system.AddAsteroid(new Asteroid("Ceres", 414000000, 1682, 473, 0.38, "gray"));

                    // Asteroid Belt
                    system.AddAsteroidBelt(new Asteroid_belt("Kuiper belt", (int)(4500000000L & 0x7FFFFFFF), 0, 0, 0, "dark"));

                    return system;
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

        // Reference to what this object orbits (null for the sun)
        public SpaceObject OrbitCenter { get; set; }

        public SpaceObject(String name, int orbital_radius, double orbital_period, int object_radius, double rotational_period, String object_color)
        {
            Name = name;
            Orbital_radius = orbital_radius;
            Orbital_period = orbital_period;
            Object_radius = object_radius;
            Rotational_period = rotational_period;
            Object_color = object_color;
        }

        // Calculate position at given time (days since time 0)
        // Returns (X, Y) coordinates in km
        public virtual (double X, double Y) GetPosition(double days)
        {
            if (Orbital_radius == 0 || Orbital_period == 0)
            {
                return (0, 0); // Sun or stationary object
            }

            // Calculate angle based on time and period
            // angle = (2π * time) / period
            double angle = (2 * Math.PI * days) / Orbital_period;

            // Calculate position
            double x = Orbital_radius * Math.Cos(angle);
            double y = Orbital_radius * Math.Sin(angle);

            // If orbiting another object (like a moon), add parent's position
            if (OrbitCenter != null)
            {
                var parentPos = OrbitCenter.GetPosition(days);
                x += parentPos.X;
                y += parentPos.Y;
            }

            return (x, y);
        }

        public virtual String GetObjectType()
        {
            return "Space Object";
        }

        public virtual String GetDetailsString(double? atTime = null)
        {
            String details = $"{GetObjectType(),-15}: {Name}";

            if (Orbital_radius > 0)
                details += $" | Orbital radius: {Orbital_radius:N0} km";
            if (Orbital_period > 0)
                details += $" | Orbital period: {Orbital_period:F2} days";
            if (Object_radius > 0)
                details += $" | Object radius: {Object_radius:N0} km";
            if (Rotational_period != 0)
                details += $" | Rotational period: {Rotational_period:F2} days";
            details += $" | Color: {Object_color}";

            if (atTime.HasValue)
            {
                var pos = GetPosition(atTime.Value);
                details += $"\n                 Position at day {atTime.Value}: ({pos.X:N0}, {pos.Y:N0}) km";
            }

            return details;
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

        public override String GetObjectType()
        {
            return "Star";
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

        public override String GetObjectType()
        {
            return "Planet";
        }

        public override void Draw()
        {
            Console.Write("Planet       :  " + Name + "  Orbital radius: " + Orbital_radius + " / " + "Orbital period: " + Orbital_period + " / " + "Object radius: " + Object_radius + "km / " + "Rotational period: " + Rotational_period + " / " + "Object color: " + Object_color);
            Console.WriteLine();
        }
    }

    public class DwarfPlanet : Planet
    {
        public DwarfPlanet(String name, int orbital_radius, double orbital_period, int object_radius, double rotational_period, String object_color)
            : base(name, orbital_radius, orbital_period, object_radius, rotational_period, object_color)
        {
        }

        public override String GetObjectType()
        {
            return "Dwarf Planet";
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

        public override String GetObjectType()
        {
            return "Asteroid Belt";
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

    // Delegate for the DoTick event (Task 7)
    public delegate void TickEventHandler(object sender, TickEventArgs e);

    // Event arguments containing simulation time
    public class TickEventArgs : EventArgs
    {
        public double CurrentTime { get; set; }  // days since start
        public double DeltaTime { get; set; }    // time step in days

        public TickEventArgs(double currentTime, double deltaTime)
        {
            CurrentTime = currentTime;
            DeltaTime = deltaTime;
        }
    }

    // Central simulation controller with DoTick event (Task 7 requirement)
    public class SimulationController
    {
        private double _currentTime;  // Current simulation time in days
        private double _speed;        // Simulation speed multiplier

        public double CurrentTime => _currentTime;
        public double Speed
        {
            get => _speed;
            set => _speed = Math.Max(0.1, Math.Min(value, 100000)); // Clamp between 0.1x and 100000x
        }

        // The DoTick event - all objects subscribe to this
        public event TickEventHandler DoTick;

        public SimulationController()
        {
            _currentTime = 0;
            _speed = 1.0; // Real-time by default
        }

        // Advances simulation and fires the DoTick event
        public void Tick(double realTimeSeconds)
        {
            // Convert real-time to simulation time based on speed
            double simulationDays = (realTimeSeconds / 86400.0) * _speed; // 86400 seconds in a day

            _currentTime += simulationDays;

            // Fire the DoTick event
            DoTick?.Invoke(this, new TickEventArgs(_currentTime, simulationDays));
        }

        public void Reset()
        {
            _currentTime = 0;
        }

        public void SetTime(double days)
        {
            _currentTime = days;
        }
    }

    // Solar System manager class
    public class SolarSystem
    {
        public Star Sun { get; private set; }
        public List<Planet> Planets { get; private set; }
        public List<DwarfPlanet> DwarfPlanets { get; private set; }
        public List<Moon> Moons { get; private set; }
        public List<Comet> Comets { get; private set; }
        public List<Asteroid> Asteroids { get; private set; }
        public List<Asteroid_belt> AsteroidBelts { get; private set; }
        public List<SpaceObject> AllObjects { get; private set; }

        public SolarSystem()
        {
            Planets = new List<Planet>();
            DwarfPlanets = new List<DwarfPlanet>();
            Moons = new List<Moon>();
            Comets = new List<Comet>();
            Asteroids = new List<Asteroid>();
            AsteroidBelts = new List<Asteroid_belt>();
            AllObjects = new List<SpaceObject>();
        }

        public void SetSun(Star sun)
        {
            Sun = sun;
            AllObjects.Add(sun);
        }

        public void AddPlanet(Planet planet)
        {
            planet.OrbitCenter = Sun;
            Planets.Add(planet);
            AllObjects.Add(planet);
        }

        public void AddDwarfPlanet(DwarfPlanet dwarf)
        {
            dwarf.OrbitCenter = Sun;
            DwarfPlanets.Add(dwarf);
            AllObjects.Add(dwarf);
        }

        public void AddMoon(Moon moon, SpaceObject parent)
        {
            moon.OrbitCenter = parent;
            Moons.Add(moon);
            AllObjects.Add(moon);
        }

        public void AddComet(Comet comet)
        {
            comet.OrbitCenter = Sun;
            Comets.Add(comet);
            AllObjects.Add(comet);
        }

        public void AddAsteroid(Asteroid asteroid)
        {
            asteroid.OrbitCenter = Sun;
            Asteroids.Add(asteroid);
            AllObjects.Add(asteroid);
        }

        public void AddAsteroidBelt(Asteroid_belt belt)
        {
            belt.OrbitCenter = Sun;
            AsteroidBelts.Add(belt);
            AllObjects.Add(belt);
        }

        public Planet GetPlanet(string name)
        {
            var planet = Planets.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (planet != null) return planet;

            // DwarfPlanet inherits from Planet, so we can return it as Planet
            return (Planet)DwarfPlanets.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public List<Moon> GetMoonsForPlanet(string planetName)
        {
            return Moons.Where(m => m.PlanetName.Equals(planetName, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }
}