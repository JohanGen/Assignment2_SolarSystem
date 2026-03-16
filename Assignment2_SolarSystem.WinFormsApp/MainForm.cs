using System;
using System.Drawing;
using System.Windows.Forms;
using SpaceSim;

namespace Assignment2_SolarSystem.WinFormsApp
{
    public partial class MainForm : Form
    {
        private SolarSystemView solarSystemView;
        private SimulationController simulationController;
        private System.Windows.Forms.Timer animationTimer;
        private TrackBar speedTrackBar;
        private Label speedLabel;
        private Button pauseButton;
        private CheckBox showLabelsCheckBox;
        private CheckBox showOrbitsCheckBox;
        private bool isPaused = false;

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Form setup
            this.Text = "Solar System Simulation - DAT154 Assignment 2";
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Create simulation controller
            simulationController = new SimulationController();
            simulationController.Speed = 1.0; // Start at 1x speed

            // Create solar system
            SolarSystem solarSystem = CreateSolarSystem();

            // Create solar system view (main drawing area)
            solarSystemView = new SolarSystemView(solarSystem, simulationController);
            solarSystemView.Dock = DockStyle.Fill;

            // Create control panel
            Panel controlPanel = new Panel();
            controlPanel.Dock = DockStyle.Top;
            controlPanel.Height = 80;
            controlPanel.BackColor = Color.FromArgb(240, 240, 240);

            // Speed label
            Label speedTitleLabel = new Label();
            speedTitleLabel.Text = "Simulation Speed:";
            speedTitleLabel.Location = new Point(10, 10);
            speedTitleLabel.AutoSize = true;
            controlPanel.Controls.Add(speedTitleLabel);

            // Speed track bar
            speedTrackBar = new TrackBar();
            speedTrackBar.Location = new Point(10, 30);
            speedTrackBar.Width = 300;
            speedTrackBar.Minimum = -10; // 0.1x speed (10^-1)
            speedTrackBar.Maximum = 50;   // 100000x speed (10^5)
            speedTrackBar.Value = 0;      // 1x speed (10^0)
            speedTrackBar.TickFrequency = 5;
            speedTrackBar.ValueChanged += SpeedTrackBar_ValueChanged;
            controlPanel.Controls.Add(speedTrackBar);

            // Speed display label
            speedLabel = new Label();
            speedLabel.Location = new Point(320, 35);
            speedLabel.Width = 100;
            speedLabel.Text = "1.0x";
            controlPanel.Controls.Add(speedLabel);

            // Pause/Resume button
            pauseButton = new Button();
            pauseButton.Location = new Point(430, 30);
            pauseButton.Width = 100;
            pauseButton.Height = 30;
            pauseButton.Text = "Pause";
            pauseButton.Click += PauseButton_Click;
            controlPanel.Controls.Add(pauseButton);

            // Show Labels checkbox
            showLabelsCheckBox = new CheckBox();
            showLabelsCheckBox.Location = new Point(550, 30);
            showLabelsCheckBox.Width = 120;
            showLabelsCheckBox.Text = "Show Labels";
            showLabelsCheckBox.Checked = true;
            showLabelsCheckBox.CheckedChanged += (s, e) => 
            {
                solarSystemView.ShowLabels = showLabelsCheckBox.Checked;
                solarSystemView.Invalidate();
            };
            controlPanel.Controls.Add(showLabelsCheckBox);

            // Show Orbits checkbox
            showOrbitsCheckBox = new CheckBox();
            showOrbitsCheckBox.Location = new Point(680, 30);
            showOrbitsCheckBox.Width = 120;
            showOrbitsCheckBox.Text = "Show Orbits";
            showOrbitsCheckBox.Checked = true;
            showOrbitsCheckBox.CheckedChanged += (s, e) => 
            {
                solarSystemView.ShowOrbits = showOrbitsCheckBox.Checked;
                solarSystemView.Invalidate();
            };
            controlPanel.Controls.Add(showOrbitsCheckBox);

            // Instructions label
            Label instructionsLabel = new Label();
            instructionsLabel.Location = new Point(810, 15);
            instructionsLabel.AutoSize = true;
            instructionsLabel.Text = "Mouse wheel: Zoom | Click planet: Focus | ESC: Reset view";
            instructionsLabel.Font = new Font(instructionsLabel.Font.FontFamily, 8);
            controlPanel.Controls.Add(instructionsLabel);

            // Time display
            Label timeLabel = new Label();
            timeLabel.Location = new Point(810, 40);
            timeLabel.Width = 300;
            timeLabel.Text = "Time: 0 days";
            controlPanel.Controls.Add(timeLabel);

            // Add controls to form
            this.Controls.Add(solarSystemView);
            this.Controls.Add(controlPanel);

            // Animation timer (60 FPS)
            animationTimer = new System.Windows.Forms.Timer();
            animationTimer.Interval = 16; // ~60 FPS
            animationTimer.Tick += (s, e) =>
            {
                if (!isPaused)
                {
                    simulationController.Tick(0.016); // 16ms in seconds
                    timeLabel.Text = $"Time: {simulationController.CurrentTime:F2} days";
                }
            };
            animationTimer.Start();
        }

        private void SpeedTrackBar_ValueChanged(object sender, EventArgs e)
        {
            // Convert slider value to speed (exponential scale)
            // Value -10 to 50 maps to 0.1x to 100000x
            double speed = Math.Pow(10, speedTrackBar.Value / 10.0);
            simulationController.Speed = speed;
            speedLabel.Text = $"{speed:F1}x";
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            isPaused = !isPaused;
            pauseButton.Text = isPaused ? "Resume" : "Pause";
        }

        // Same CreateSolarSystem method as in console app
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
            Planet uranus = new Planet("Uranus", 1500000000, 30685, 25362, -0.72, "light-blue");
            Planet neptune = new Planet("Neptune", 2147483647, 60190, 24764, 0.67, "light-blue");

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
}
