using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using SpaceSim;

namespace Assignment2_SolarSystem.WinFormsApp
{
    /// <summary>
    /// Custom control for rendering the solar system with zoom and animation.
    /// Subscribes to SimulationController.DoTick event (Task 7 requirement).
    /// </summary>
    public class SolarSystemView : Control
    {
        private SolarSystem solarSystem;
        private SimulationController simulationController;
        private System.Windows.Forms.Timer renderTimer;
        
        private bool showLabels = true;
        private bool showOrbits = true;
        private bool isPaused = false;
        
        private SpaceObject selectedPlanet = null;
        private float zoom = 1.0f;
        private DateTime lastFrameTime;

        // Scaling factors to make everything visible
        private const double DistanceScale = 1e-7;  // Scale down distances
        private const double SizeScale = 1e-3;      // Scale up sizes slightly
        private const double MinVisibleRadius = 3; // Minimum pixel size for visibility

        public bool ShowLabels
        {
            get => showLabels;
            set { showLabels = value; Invalidate(); }
        }

        public bool ShowOrbits
        {
            get => showOrbits;
            set { showOrbits = value; Invalidate(); }
        }

        public bool IsPaused => isPaused;

        public SolarSystemView()
        {
            this.DoubleBuffered = true;
            this.BackColor = Color.Black;

            // Create solar system
            solarSystem = CreateSolarSystem();

            // Create simulation controller (Task 7)
            simulationController = new SimulationController();
            simulationController.Speed = 5.0; // Start at 5x speed

            // Subscribe to DoTick event (Task 7 requirement - objects don't use their own timers)
            simulationController.DoTick += OnSimulationTick;

            // Render timer (60 FPS)
            renderTimer = new System.Windows.Forms.Timer();
            renderTimer.Interval = 16; // ~60 FPS
            renderTimer.Tick += RenderTimer_Tick;
            renderTimer.Start();

            lastFrameTime = DateTime.Now;

            // Mouse events for zoom and selection
            this.MouseWheel += SolarSystemView_MouseWheel;
            this.MouseClick += SolarSystemView_MouseClick;
        }

        private void OnSimulationTick(object sender, TickEventArgs e)
        {
            // This method is called by the DoTick event
            // All position updates are driven by this event, not individual timers
            Invalidate(); // Request redraw
        }

        private void RenderTimer_Tick(object sender, EventArgs e)
        {
            if (isPaused) return;

            // Calculate delta time
            DateTime now = DateTime.Now;
            double deltaSeconds = (now - lastFrameTime).TotalSeconds;
            lastFrameTime = now;

            // Advance simulation (fires DoTick event)
            simulationController.Tick(deltaSeconds);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            // Center point
            PointF center = new PointF(Width / 2, Height / 2);

            if (selectedPlanet == null)
            {
                // Solar system overview
                DrawSolarSystemView(g, center);
            }
            else
            {
                // Planet zoom view
                DrawPlanetView(g, center);
            }
        }

        private void DrawSolarSystemView(Graphics g, PointF center)
        {
            double currentTime = simulationController.CurrentTime;

            // Draw sun
            DrawSpaceObject(g, center, solarSystem.Sun, currentTime, Color.Yellow, 15);

            // Draw orbits first (behind planets)
            if (showOrbits)
            {
                foreach (var planet in solarSystem.Planets)
                {
                    DrawOrbit(g, center, planet.Orbital_radius);
                }
                foreach (var dwarf in solarSystem.DwarfPlanets)
                {
                    DrawOrbit(g, center, dwarf.Orbital_radius);
                }
            }

            // Draw planets
            foreach (var planet in solarSystem.Planets)
            {
                Color color = GetColorFromName(planet.Object_color);
                DrawSpaceObject(g, center, planet, currentTime, color, 8);
            }

            // Draw dwarf planets
            foreach (var dwarf in solarSystem.DwarfPlanets)
            {
                Color color = GetColorFromName(dwarf.Object_color);
                DrawSpaceObject(g, center, dwarf, currentTime, color, 5);
            }

            // Draw info
            DrawInfo(g, $"Solar System View | Time: {currentTime:F2} days | Speed: {simulationController.Speed:F1}x");
        }

        private void DrawPlanetView(Graphics g, PointF center)
        {
            double currentTime = simulationController.CurrentTime;

            // Draw selected planet at center
            Color planetColor = GetColorFromName(selectedPlanet.Object_color);
            DrawCenteredObject(g, center, selectedPlanet, planetColor, 20);

            // Draw its moons
            var moons = solarSystem.Moons.Where(m => m.OrbitCenter == selectedPlanet).ToList();
            
            if (showOrbits)
            {
                foreach (var moon in moons)
                {
                    // Scale orbit radius for visibility
                    double scaledRadius = moon.Orbital_radius * 0.01; // Adjust scale for moon view
                    DrawOrbit(g, center, (int)scaledRadius);
                }
            }

            foreach (var moon in moons)
            {
                // Calculate moon position relative to planet
                var moonPos = moon.GetPosition(currentTime);
                var planetPos = selectedPlanet.GetPosition(currentTime);
                
                double relX = (moonPos.X - planetPos.X) * 0.01; // Scale for visibility
                double relY = (moonPos.Y - planetPos.Y) * 0.01;

                float screenX = center.X + (float)(relX * zoom);
                float screenY = center.Y + (float)(relY * zoom);

                Color moonColor = GetColorFromName(moon.Object_color);
                DrawObjectAt(g, new PointF(screenX, screenY), moon, moonColor, 6);
            }

            // Draw info
            DrawInfo(g, $"{selectedPlanet.Name} View | Moons: {moons.Count} | Time: {currentTime:F2} days | Press ESC to return");
        }

        private void DrawSpaceObject(Graphics g, PointF center, SpaceObject obj, double time, Color color, float size)
        {
            var pos = obj.GetPosition(time);
            
            // Convert to screen coordinates
            float screenX = center.X + (float)(pos.X * DistanceScale * zoom);
            float screenY = center.Y + (float)(pos.Y * DistanceScale * zoom);

            DrawObjectAt(g, new PointF(screenX, screenY), obj, color, size);
        }

        private void DrawObjectAt(Graphics g, PointF screenPos, SpaceObject obj, Color color, float size)
        {
            // Draw the object
            using (SolidBrush brush = new SolidBrush(color))
            {
                g.FillEllipse(brush, screenPos.X - size / 2, screenPos.Y - size / 2, size, size);
            }

            // Draw label
            if (showLabels)
            {
                using (SolidBrush textBrush = new SolidBrush(Color.White))
                {
                    g.DrawString(obj.Name, new Font("Arial", 8), textBrush, screenPos.X + size, screenPos.Y);
                }
            }
        }

        private void DrawCenteredObject(Graphics g, PointF center, SpaceObject obj, Color color, float size)
        {
            using (SolidBrush brush = new SolidBrush(color))
            {
                g.FillEllipse(brush, center.X - size / 2, center.Y - size / 2, size, size);
            }

            if (showLabels)
            {
                using (SolidBrush textBrush = new SolidBrush(Color.White))
                {
                    g.DrawString(obj.Name, new Font("Arial", 10, FontStyle.Bold), textBrush, center.X + size, center.Y);
                }
            }
        }

        private void DrawOrbit(Graphics g, PointF center, int radius)
        {
            float screenRadius = (float)(radius * DistanceScale * zoom);
            
            if (screenRadius < 5) return; // Don't draw tiny orbits

            using (Pen pen = new Pen(Color.FromArgb(60, 100, 100, 100), 1))
            {
                pen.DashStyle = DashStyle.Dot;
                g.DrawEllipse(pen, center.X - screenRadius, center.Y - screenRadius, 
                             screenRadius * 2, screenRadius * 2);
            }
        }

        private void DrawInfo(Graphics g, string text)
        {
            using (SolidBrush brush = new SolidBrush(Color.LightGray))
            {
                g.DrawString(text, new Font("Arial", 10), brush, 10, 10);
            }
        }

        private Color GetColorFromName(string colorName)
        {
            var colorMap = new Dictionary<string, Color>(StringComparer.OrdinalIgnoreCase)
            {
                {"yellow", Color.Yellow},
                {"gray", Color.Gray},
                {"yellowish-white", Color.LightYellow},
                {"blue", Color.DodeSkyBlue},
                {"orange", Color.Orange},
                {"redish-orangey-white", Color.OrangeRed},
                {"yellowey-orange", Color.Gold},
                {"light-blue", Color.LightSkyBlue},
                {"brown", Color.SaddleBrown},
                {"yellow-red-orange", Color.OrangeRed},
                {"gray-red", Color.RosyBrown},
                {"red-orange", Color.OrangeRed},
                {"orange-mixed", Color.Orange},
                {"gray-brown", Color.Gray},
                {"yellow-orange", Color.Gold},
                {"gray-blue", Color.LightSteelBlue},
                {"brown-black", Color.SaddleBrown},
                {"dirty white", Color.WhiteSmoke},
                {"dark", Color.DarkSlateGray}
            };

            return colorMap.TryGetValue(colorName, out Color color) ? color : Color.White;
        }

        private void SolarSystemView_MouseWheel(object sender, MouseEventArgs e)
        {
            // Zoom in/out with mouse wheel
            zoom *= e.Delta > 0 ? 1.2f : 0.8f;
            zoom = Math.Max(0.1f, Math.Min(zoom, 10f));
            Invalidate();
        }

        private void SolarSystemView_MouseClick(object sender, MouseEventArgs e)
        {
            if (selectedPlanet != null) return; // Already in planet view

            // Check if user clicked on a planet
            PointF center = new PointF(Width / 2, Height / 2);
            double currentTime = simulationController.CurrentTime;

            foreach (var planet in solarSystem.Planets)
            {
                var pos = planet.GetPosition(currentTime);
                float screenX = center.X + (float)(pos.X * DistanceScale * zoom);
                float screenY = center.Y + (float)(pos.Y * DistanceScale * zoom);

                float distance = (float)Math.Sqrt(Math.Pow(e.X - screenX, 2) + Math.Pow(e.Y - screenY, 2));
                if (distance < 15) // Click tolerance
                {
                    selectedPlanet = planet;
                    zoom = 1.0f;
                    Invalidate();
                    return;
                }
            }
        }

        public void ReturnToSolarSystemView()
        {
            selectedPlanet = null;
            zoom = 1.0f;
            Invalidate();
        }

        public void SetSimulationSpeed(double speed)
        {
            simulationController.Speed = speed;
        }

        public void Pause()
        {
            isPaused = true;
        }

        public void Resume()
        {
            isPaused = false;
            lastFrameTime = DateTime.Now; // Reset time to avoid jump
        }

        private SolarSystem CreateSolarSystem()
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
            Planet uranus = new Planet("Uranus", 2870990000, 30685, 25362, -0.72, "light-blue");
            Planet neptune = new Planet("Neptune", 4504300000, 60190, 24764, 0.67, "light-blue");

            system.AddPlanet(mercury);
            system.AddPlanet(venus);
            system.AddPlanet(earth);
            system.AddPlanet(mars);
            system.AddPlanet(jupiter);
            system.AddPlanet(saturn);
            system.AddPlanet(uranus);
            system.AddPlanet(neptune);

            // Dwarf Planets
            DwarfPlanet pluto = new DwarfPlanet("Pluto", 5913520000, 90550, 1151, 6.39, "brown");
            system.AddDwarfPlanet(pluto);

            // Moons - Earth
            system.AddMoon(new Moon("Earth", "Moon", 384400, 27.3, 1737, 27.3, "gray"), earth);

            // Moons - Mars
            system.AddMoon(new Moon("Mars", "Phobos", 9376, 0.32, 11, 0.32, "gray"), mars);
            system.AddMoon(new Moon("Mars", "Deimos", 23460, 1.26, 6, 1.26, "gray"), mars);

            // Moons - Jupiter
            system.AddMoon(new Moon("Jupiter", "Io", 421800, 1.77, 1821, 1.77, "yellow-red-orange"), jupiter);
            system.AddMoon(new Moon("Jupiter", "Europa", 671100, 3.55, 1565, 3.55, "gray-red"), jupiter);
            system.AddMoon(new Moon("Jupiter", "Ganymede", 1070400, 7.15, 2634, 7.15, "red-orange"), jupiter);
            system.AddMoon(new Moon("Jupiter", "Callisto", 1882700, 16.69, 2403, 16.69, "orange"), jupiter);

            // Moons - Saturn
            system.AddMoon(new Moon("Saturn", "Titan", 1221870, 15.95, 2575, 15.95, "orange-mixed"), saturn);
            system.AddMoon(new Moon("Saturn", "Rhea", 527070, 4.52, 764, 4.52, "gray"), saturn);

            return system;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                renderTimer?.Stop();
                renderTimer?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
