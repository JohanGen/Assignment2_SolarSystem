using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SpaceSim;

namespace Assignment2_SolarSystem.WinFormsApp
{
    public class SolarSystemView : Control
    {
        private SolarSystem solarSystem;
        private SimulationController simulationController;
        private SpaceObject focusedObject = null;
        private double zoomLevel = 1.0;
        private const double MinZoom = 0.1;
        private const double MaxZoom = 50.0;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ShowLabels { get; set; } = true;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ShowOrbits { get; set; } = true;

        // Scaling factors for drawing.
        // Real solar system proportions are impossible to display accurately, so we use
        // a square-root distance scale: this spreads inner planets out while keeping
        // outer planets on screen. Sizes are entirely separate from distances.
        private const double SqrtDistScale = 0.012;   // sqrt(km) -> pixels  (Neptune lands ~556px out)
        private const double SunSizePixels = 30;       // Fixed Sun size in pixels (not to scale)
        private const double PlanetSizeScale = 0.08;   // sqrt(km) -> pixels for planet sizes
        private const double MaxPlanetSizeRatio = 0.25; // Planets are at most 25% of the Sun's drawn size (7.5px)

        public SolarSystemView(SolarSystem solarSystem, SimulationController simulationController)
        {
            this.solarSystem = solarSystem;
            this.simulationController = simulationController;

            // Subscribe to the DoTick event (Task 7 requirement)
            simulationController.DoTick += OnSimulationTick;

            // Enable double buffering for smooth animation
            this.DoubleBuffered = true;

            // Enable keyboard input
            this.SetStyle(ControlStyles.Selectable, true);
            this.TabStop = true;

            // Mouse event handlers
            this.MouseWheel += SolarSystemView_MouseWheel;
            this.MouseClick += SolarSystemView_MouseClick;
            this.KeyDown += SolarSystemView_KeyDown;
        }

        private void OnSimulationTick(object sender, TickEventArgs e)
        {
            // Redraw the view when simulation updates
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Fill background with black (space)
            g.Clear(Color.Black);

            if (focusedObject == null)
            {
                DrawSolarSystemView(g);
            }
            else
            {
                DrawPlanetView(g, focusedObject);
            }
        }

        // Converts a real-space position (km) to screen coordinates using sqrt distance scaling.
        // This keeps the correct angle while spacing inner planets out more than outer ones.
        private (int x, int y) ToScreen(double posX, double posY, int centerX, int centerY)
        {
            double dist = Math.Sqrt(posX * posX + posY * posY);
            if (dist < 1) return (centerX, centerY);
            double screenDist = Math.Sqrt(dist) * SqrtDistScale * zoomLevel;
            return (
                centerX + (int)(posX / dist * screenDist),
                centerY + (int)(posY / dist * screenDist)
            );
        }

        private int PlanetDrawSize(double objectRadiusKm)
        {
            int sunPixels = (int)(SunSizePixels * zoomLevel);
            int maxSize = (int)(sunPixels * MaxPlanetSizeRatio);
            int rawSize = (int)(Math.Sqrt(objectRadiusKm) * PlanetSizeScale * zoomLevel);
            return Math.Min(maxSize, Math.Max(4, rawSize));
        }

        private void DrawSolarSystemView(Graphics g)
        {
            int centerX = Width / 2;
            int centerY = Height / 2;

            // Draw orbit rings first (behind everything else)
            if (ShowOrbits)
            {
                using (Pen orbitPen = new Pen(Color.FromArgb(60, 100, 100, 120), 1))
                {
                    foreach (var planet in solarSystem.Planets)
                    {
                        int orbitRadius = (int)(Math.Sqrt(planet.Orbital_radius) * SqrtDistScale * zoomLevel);
                        if (orbitRadius > 0)
                            g.DrawEllipse(orbitPen, centerX - orbitRadius, centerY - orbitRadius,
                                         orbitRadius * 2, orbitRadius * 2);
                    }
                }
            }

            // Draw Sun (fixed size, not to real scale)
            int sunSize = (int)(SunSizePixels * zoomLevel);
            using (Brush sunBrush = new SolidBrush(GetColorFromName(solarSystem.Sun.Object_color)))
                g.FillEllipse(sunBrush, centerX - sunSize / 2, centerY - sunSize / 2, sunSize, sunSize);
            if (ShowLabels)
                g.DrawString("Sun", this.Font, Brushes.White, centerX + sunSize / 2 + 5, centerY);

            // Draw planets
            foreach (var planet in solarSystem.Planets)
            {
                var pos = planet.GetPosition(simulationController.CurrentTime);
                var (x, y) = ToScreen(pos.X, pos.Y, centerX, centerY);
                int size = PlanetDrawSize(planet.Object_radius);

                using (Brush brush = new SolidBrush(GetColorFromName(planet.Object_color)))
                    g.FillEllipse(brush, x - size / 2, y - size / 2, size, size);

                if (ShowLabels)
                    g.DrawString(planet.Name, this.Font, Brushes.White, x + size / 2 + 3, y);
            }

            // Draw dwarf planets (slightly smaller than regular planets)
            foreach (var dwarf in solarSystem.DwarfPlanets)
            {
                var pos = dwarf.GetPosition(simulationController.CurrentTime);
                var (x, y) = ToScreen(pos.X, pos.Y, centerX, centerY);
                int size = Math.Max(3, PlanetDrawSize(dwarf.Object_radius) - 2);

                using (Brush brush = new SolidBrush(GetColorFromName(dwarf.Object_color)))
                    g.FillEllipse(brush, x - size / 2, y - size / 2, size, size);

                if (ShowLabels)
                    g.DrawString(dwarf.Name, this.Font, Brushes.Gray, x + size / 2 + 3, y);
            }
        }

        private void DrawPlanetView(Graphics g, SpaceObject planet)
        {
            int centerX = Width / 2;
            int centerY = Height / 2;

            // Sqrt scale for moon distances: sqrt(km) -> pixels
            // Moon (384,400 km) -> ~149px, Callisto (1,882,700 km) -> ~330px
            const double MoonSqrtScale = 0.24;
            const double MoonSizeScale = 0.15;

            // Draw the focused planet at the screen center, sized by object radius
            int planetSize = (int)(Math.Max(30, Math.Sqrt(planet.Object_radius) * 0.2) * zoomLevel);
            using (Brush brush = new SolidBrush(GetColorFromName(planet.Object_color)))
                g.FillEllipse(brush, centerX - planetSize / 2, centerY - planetSize / 2, planetSize, planetSize);
            if (ShowLabels)
                g.DrawString(planet.Name, this.Font, Brushes.White, centerX + planetSize / 2 + 5, centerY);

            // Draw moons orbiting this planet
            var moons = solarSystem.GetMoonsForPlanet(planet.Name);
            foreach (var moon in moons)
            {
                // Draw orbit ring using same sqrt scale as the moon position
                int orbitRadius = (int)(Math.Sqrt(moon.Orbital_radius) * MoonSqrtScale * zoomLevel);
                if (ShowOrbits && orbitRadius > 0)
                {
                    using (Pen orbitPen = new Pen(Color.FromArgb(60, 150, 150, 150), 1))
                        g.DrawEllipse(orbitPen, centerX - orbitRadius, centerY - orbitRadius,
                                     orbitRadius * 2, orbitRadius * 2);
                }

                // Moon position relative to the planet (in km)
                var moonPos = moon.GetPosition(simulationController.CurrentTime);
                var planetPos = planet.GetPosition(simulationController.CurrentTime);
                double relX = moonPos.X - planetPos.X;
                double relY = moonPos.Y - planetPos.Y;

                // Apply sqrt scaling: keep direction, compress distance
                double actualDist = Math.Sqrt(relX * relX + relY * relY);
                if (actualDist < 1) continue;
                double screenDist = Math.Sqrt(actualDist) * MoonSqrtScale * zoomLevel;

                int moonX = centerX + (int)(relX / actualDist * screenDist);
                int moonY = centerY + (int)(relY / actualDist * screenDist);
                int moonSize = Math.Max(3, (int)(Math.Sqrt(moon.Object_radius) * MoonSizeScale * zoomLevel));

                using (Brush moonBrush = new SolidBrush(GetColorFromName(moon.Object_color)))
                    g.FillEllipse(moonBrush, moonX - moonSize / 2, moonY - moonSize / 2, moonSize, moonSize);

                if (ShowLabels)
                    g.DrawString(moon.Name, this.Font, Brushes.LightGray, moonX + moonSize / 2 + 3, moonY);
            }

            if (moons.Count == 0)
                g.DrawString("No moons", this.Font, Brushes.Gray, centerX + planetSize / 2 + 5, centerY + 18);

            g.DrawString("Press ESC to return | Mouse wheel to zoom", this.Font, Brushes.Yellow, 10, 10);
        }

        private void SolarSystemView_MouseWheel(object sender, MouseEventArgs e)
        {
            // Zoom in/out with mouse wheel
            if (e.Delta > 0)
                zoomLevel = Math.Min(MaxZoom, zoomLevel * 1.2);
            else
                zoomLevel = Math.Max(MinZoom, zoomLevel / 1.2);

            this.Invalidate();
        }

        private void SolarSystemView_MouseClick(object sender, MouseEventArgs e)
        {
            // Give focus to this control so it can receive keyboard events
            this.Focus();

            if (focusedObject != null)
                return; // Already focused on something

            int centerX = Width / 2;
            int centerY = Height / 2;

            // Check if clicked on a planet
            foreach (var planet in solarSystem.Planets)
            {
                var pos = planet.GetPosition(simulationController.CurrentTime);
                var (x, y) = ToScreen(pos.X, pos.Y, centerX, centerY);

                // Use same size calculation as drawing
                int size = PlanetDrawSize(planet.Object_radius);

                double distance = Math.Sqrt((e.X - x) * (e.X - x) + (e.Y - y) * (e.Y - y));
                if (distance <= size / 2 + 5)
                {
                    focusedObject = planet;
                    zoomLevel = 1.0;
                    this.Invalidate();
                    return;
                }
            }
        }

        private void SolarSystemView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape && focusedObject != null)
            {
                focusedObject = null;
                zoomLevel = 1.0;
                this.Invalidate();
            }
        }

        private Color GetColorFromName(string colorName)
        {
            switch (colorName.ToLower())
            {
                case "yellow": return Color.Yellow;
                case "gray": return Color.Gray;
                case "yellowish-white": return Color.LightYellow;
                case "blue": return Color.DodgerBlue;
                case "orange": return Color.Orange;
                case "redish-orangey-white": return Color.LightSalmon;
                case "yellowey-orange": return Color.Gold;
                case "light-blue": return Color.LightBlue;
                case "brown": return Color.SaddleBrown;
                case "yellow-red-orange": return Color.OrangeRed;
                case "gray-red": return Color.RosyBrown;
                case "red-orange": return Color.Coral;
                case "orange-mixed": return Color.DarkOrange;
                case "yellow-orange": return Color.Goldenrod;
                case "gray-blue": return Color.LightSteelBlue;
                case "brown-black": return Color.DarkSlateGray;
                case "dirty white": return Color.WhiteSmoke;
                case "dark": return Color.DarkGray;
                default: return Color.White;
            }
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            // Ensure the control gets focus when clicked
            this.Focus();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Unsubscribe from events to prevent memory leaks
                if (simulationController != null)
                {
                    simulationController.DoTick -= OnSimulationTick;
                }
            }
            base.Dispose(disposing);
        }

        protected override bool IsInputKey(Keys keyData)
        {
            // Allow arrow keys and ESC to be captured
            if (keyData == Keys.Escape)
                return true;
            return base.IsInputKey(keyData);
        }
    }
}
