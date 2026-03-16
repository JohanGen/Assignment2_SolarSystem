# Quick Start Guide - Adding Windows Forms GUI

## You Have Everything Ready! Just Follow These Steps:

### Step 1: Create Windows Forms Project (2 minutes)
1. In Visual Studio, right-click your **solution** (not the project)
2. **Add** → **New Project**
3. Choose **Windows Forms App** (.NET)
4. Name: `Assignment2_SolarSystem.WinFormsApp`
5. Framework: **.NET 10.0**
6. Click **Create**

### Step 2: Add Reference to Your Console Project (30 seconds)
1. In the new WinFormsApp project, expand **Dependencies**
2. Right-click **Dependencies** → **Add Project Reference**
3. ✅ Check `Assignment2_SolarSystem` (your console app project)
4. Click **OK**

This allows the GUI to use all your SpaceSim classes!

### Step 3: Delete Default Form (10 seconds)
1. In WinFormsApp project, right-click `Form1.cs`
2. **Delete** (we don't need it)

### Step 4: Add MainForm.cs (1 minute)
1. Right-click WinFormsApp project → **Add** → **Existing Item**
2. Navigate to where you saved `MainForm.cs`
3. Select it and click **Add**

**OR manually create it:**
1. Right-click WinFormsApp → **Add** → **Class**
2. Name: `MainForm.cs`
3. Copy all code from the `MainForm.cs` file I created
4. Paste into the new file

### Step 5: Add SolarSystemView.cs (1 minute)
Same as Step 4, but for `SolarSystemView.cs`:
1. Right-click WinFormsApp → **Add** → **Existing Item** (or **Add** → **Class**)
2. Add the `SolarSystemView.cs` file

### Step 6: Update Program.cs (30 seconds)
1. Open `Program.cs` in the WinFormsApp project (auto-generated)
2. Replace ALL content with code from `WinFormsApp_Program.cs`

### Step 7: Set Startup Project (10 seconds)
1. Right-click `Assignment2_SolarSystem.WinFormsApp` project
2. **Set as Startup Project**

### Step 8: Build and Run! (10 seconds)
Press **F5** or click **▶ Start**

## What You Should See:
- Black window with solar system visualization
- Planets orbiting the sun
- Controls at bottom:
  - Speed slider
  - Pause/Resume button
  - Show Labels checkbox
  - Show Orbits checkbox

## How to Use:
- **Mouse Wheel**: Zoom in/out
- **Click a planet**: Zoom to that planet and see its moons
- **ESC key**: Return to solar system view
- **Speed Slider**: Adjust from 0.1x to 100x speed

## Troubleshooting:

### If you get "namespace not found" errors:
Make sure the using statement at the top of MainForm.cs and SolarSystemView.cs is:
```csharp
using SpaceSim;
```

### If planets don't appear:
The scaling might need adjustment. Check that:
1. SimulationController is created
2. DoTick event is subscribed
3. Timer is started

### If you want to keep BOTH projects:
**Console App:**
- Right-click `Assignment2_SolarSystem` → Set as Startup Project
- Run it

**GUI App:**
- Right-click `Assignment2_SolarSystem.WinFormsApp` → Set as Startup Project  
- Run it

You can switch between them anytime!

## Assignment Requirements Met:
✅ Task 5: Windows Forms application created
✅ Task 6: 2D visualization with zoom, labels, orbits toggle
✅ Task 7: DoTick event system with central controller
✅ Speed adjustment (slider)
✅ Animation based on orbital periods
✅ No objects using their own timers (all subscribe to DoTick)

## File Structure After Setup:
```
Assignment2_SolarSystem/
├── Assignment2_SolarSystem/           (Console App)
│   └── Program.cs                     (All your classes + console app)
│
└── Assignment2_SolarSystem.WinFormsApp/  (Windows Forms)
    ├── Program.cs                     (Entry point)
    ├── MainForm.cs                    (GUI with controls)
    └── SolarSystemView.cs             (Rendering + animation)
```

## Important Notes:
1. **Both projects share the same classes** via project reference
2. **DoTick event is in SpaceSim namespace** (in your console app)
3. **GUI subscribes to DoTick** (Task 7 requirement)
4. **No individual timers** - only SimulationController uses one internally

Good luck! 🚀
