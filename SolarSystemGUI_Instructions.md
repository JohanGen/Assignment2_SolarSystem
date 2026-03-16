# Windows Forms GUI Application Instructions

## Creating the Windows Forms Project

1. **Add a new Windows Forms project to your solution:**
   - Right-click on your solution in Solution Explorer
   - Add → New Project
   - Choose "Windows Forms App"
   - Name it: `Assignment2_SolarSystem.WinFormsApp`
   - Framework: .NET 10.0
   - Click Create

2. **Add reference to your existing project:**
   - Right-click on the WinFormsApp project → Add → Project Reference
   - Check the box for `Assignment2_SolarSystem` (your console app with the classes)
   - Click OK

3. **Replace Form1.cs with the MainForm.cs code provided below**

4. **Add SolarSystemView.cs as a new class to the project**

5. **Build and run!**

## Controls:
- **Mouse Wheel**: Zoom in/out
- **Click on a planet**: Zoom to that planet and see its moons
- **ESC key**: Return to solar system view
- **Speed Slider**: Adjust simulation speed (0.1x to 100x)
- **Show Labels**: Toggle planet/moon names
- **Show Orbits**: Toggle orbit circles
- **Pause/Resume**: Pause the animation

## Features Implemented:
✅ Task 5: Windows Forms GUI
✅ Task 6: 2D visualization with zoom, labels, orbits toggle
✅ Task 7: DoTick event system with SimulationController
✅ Adjustable simulation speed
✅ Proper scaling for viewability
✅ All major planets and their moons
