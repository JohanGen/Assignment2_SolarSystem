using System;
using System.Drawing;
using System.Windows.Forms;
using SpaceSim;

namespace Assignment2_SolarSystem.WinFormsApp
{
    public partial class MainForm : Form
    {
        private SolarSystemView solarSystemView;
        private TrackBar speedTrackBar;
        private Label speedLabel;
        private CheckBox showLabelsCheckBox;
        private CheckBox showOrbitsCheckBox;
        private Button pauseButton;
        private Label infoLabel;

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Solar System Simulation - DAT154 Assignment 2";
            this.Size = new Size(1400, 900);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.Black;

            // Solar system view (main canvas)
            solarSystemView = new SolarSystemView
            {
                Location = new Point(0, 0),
                Size = new Size(1400, 800),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                BackColor = Color.Black
            };
            this.Controls.Add(solarSystemView);

            // Control panel at bottom
            Panel controlPanel = new Panel
            {
                Location = new Point(0, 800),
                Size = new Size(1400, 100),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                BackColor = Color.FromArgb(30, 30, 30)
            };
            this.Controls.Add(controlPanel);

            // Speed control
            Label speedTitleLabel = new Label
            {
                Text = "Simulation Speed:",
                Location = new Point(20, 15),
                Size = new Size(120, 20),
                ForeColor = Color.White
            };
            controlPanel.Controls.Add(speedTitleLabel);

            speedTrackBar = new TrackBar
            {
                Location = new Point(20, 40),
                Size = new Size(200, 45),
                Minimum = -10,  // 0.1x speed
                Maximum = 20,   // 100x speed
                Value = 0,      // 1x speed
                TickFrequency = 5
            };
            speedTrackBar.ValueChanged += SpeedTrackBar_ValueChanged;
            controlPanel.Controls.Add(speedTrackBar);

            speedLabel = new Label
            {
                Text = "1.0x",
                Location = new Point(230, 45),
                Size = new Size(60, 20),
                ForeColor = Color.LightGreen
            };
            controlPanel.Controls.Add(speedLabel);

            // Pause button
            pauseButton = new Button
            {
                Text = "Pause",
                Location = new Point(310, 40),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(60, 60, 60),
                ForeColor = Color.White
            };
            pauseButton.Click += PauseButton_Click;
            controlPanel.Controls.Add(pauseButton);

            // Show labels checkbox
            showLabelsCheckBox = new CheckBox
            {
                Text = "Show Labels",
                Location = new Point(420, 45),
                Size = new Size(120, 20),
                Checked = true,
                ForeColor = Color.White
            };
            showLabelsCheckBox.CheckedChanged += (s, e) => solarSystemView.ShowLabels = showLabelsCheckBox.Checked;
            controlPanel.Controls.Add(showLabelsCheckBox);

            // Show orbits checkbox
            showOrbitsCheckBox = new CheckBox
            {
                Text = "Show Orbits",
                Location = new Point(560, 45),
                Size = new Size(120, 20),
                Checked = true,
                ForeColor = Color.White
            };
            showOrbitsCheckBox.CheckedChanged += (s, e) => solarSystemView.ShowOrbits = showOrbitsCheckBox.Checked;
            controlPanel.Controls.Add(showOrbitsCheckBox);

            // Info label
            infoLabel = new Label
            {
                Text = "Click on a planet to zoom | Mouse wheel to zoom | ESC to return to solar system view",
                Location = new Point(720, 15),
                Size = new Size(650, 60),
                ForeColor = Color.LightBlue,
                Font = new Font("Arial", 10, FontStyle.Italic)
            };
            controlPanel.Controls.Add(infoLabel);

            // Handle key press for ESC
            this.KeyPreview = true;
            this.KeyDown += MainForm_KeyDown;
        }

        private void SpeedTrackBar_ValueChanged(object sender, EventArgs e)
        {
            // Convert trackbar value to speed multiplier
            // -10 to 20 maps to 0.1x to 100x
            double speed;
            if (speedTrackBar.Value <= 0)
            {
                // Logarithmic scale for slow speeds
                speed = Math.Pow(10, speedTrackBar.Value / 10.0);
            }
            else
            {
                // Linear scale for fast speeds
                speed = 1 + (speedTrackBar.Value * 4.95);
            }

            solarSystemView.SetSimulationSpeed(speed);
            speedLabel.Text = $"{speed:F1}x";
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            if (solarSystemView.IsPaused)
            {
                solarSystemView.Resume();
                pauseButton.Text = "Pause";
            }
            else
            {
                solarSystemView.Pause();
                pauseButton.Text = "Resume";
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                solarSystemView.ReturnToSolarSystemView();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            solarSystemView.Dispose();
            base.OnFormClosing(e);
        }
    }
}
