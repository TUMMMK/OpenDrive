///////////////////////////////////////////////////////////////////////////////////
///                                                                             ///
/// This file is part of the OpenDriveSimulator project                         ///
/// (www.github.com/TUMMMK/OpenDrive)                                           ///
///                                                                             ///
/// Copyright (c) 2016 Simon Schenk (simon.schenk@tum.de)                       ///
///                                                                             ///
///////////////////////////////////////////////////////////////////////////////////
/// The MIT License                                                             ///
///                                                                             ///
/// Permission is hereby granted, free of charge, to any person obtaining a     ///
/// copy of this software and associated documentation files (the "Software"),  ///
/// to deal in the Software without restriction, including without limitation   ///
/// the rights to use, copy, modify, merge, publish, distribute, sublicense,    ///
/// and/or sell copies of the Software, and to permit persons to whom the       ///
/// Software is furnished to do so, subject to the following conditions:        ///
///                                                                             ///
/// The above copyright notice and this permission notice shall be included     ///
/// in all copies or substantial portions of the Software.                      ///
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS     ///
/// OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, ///
/// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL     ///
/// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER  ///
/// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING     ///
/// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER         ///
/// DEALINGS IN THE SOFTWARE.                                                   ///
///////////////////////////////////////////////////////////////////////////////////

using System;
using System.Windows.Forms;

using GTA;

using OpenDriveSimulator.Scripting;

namespace OpenDriveSimulator
{
   class SimulationManager : Script
   {
      CameraScript.CameraMode m_lastCameraMode;
      private bool m_showGUIAfterExitingFreemode;

      int m_initTickCount = 5;

      public SimulationManager()
      {
         Tick += InitSlide;
      }
      void InitSlide(object sender, EventArgs e)
      {
         switch(m_initTickCount--)
         {
            case 0:
               Tick -= InitSlide;

               Application.Start();
               Application.GUI.SetHelpText("F1: Start/Stop recording\nF2: Cancel recording\nF4: Enter freemode\nF9: Set ego car to spawnpoint\nF11: Toggle console\nF12: Toggle help window\n\nControls:\nTab: Select next element\nUp/Down: Navigate in list\nLeft/Right: Change option\nEnter: Select");

               //TODO: This camera initialization only works, if the script is started using the 'load game' option. It does not work when reloading the scripts using the 'Ins' key.
               //To fix the camera after pressing 'Ins', wait until the GUI is showing, then press F4, F8, and F4, one after another and wait a moment between the key presses.
               Application.ExecutionQueue.AddAction(5, () =>
               {
                  Application.GetScript<CameraScript>().SetMode(CameraScript.CameraMode.Free);
               });
               Application.ExecutionQueue.AddAction(50, () =>
               {
                  CameraScript.PressF8Key();
               });
               Application.ExecutionQueue.AddAction(100, () =>
               {
                  Application.GetScript<CameraScript>().SetMode(CameraScript.CameraMode.Fixed);
                  m_lastCameraMode = CameraScript.CameraMode.Fixed;

                  Application.GUI.Show();
                  KeyDown += SafeKeyDownHandler;
               });
 
               break;
            default:
               break;
         }
      }
      void SafeKeyDownHandler(object sender, KeyEventArgs e)
      {
         try
         {
            KeyDownHandler(sender, e);
         }
         catch (Exception ex)
         {
            Application.Console.WriteLine(ex.GetType().ToString());
            Application.Console.WriteLine(ex.Message);
            Application.Console.WriteLine(ex.StackTrace);

            foreach (var key in ex.Data.Keys)
            {
               Application.Console.WriteLine(key + ": " + ex.Data[key]);
            }
         }
      }
      void KeyDownHandler(object sender, KeyEventArgs e)
      {
         if (Application.GUI.IsRunning)
         {
            Application.GUI.KeyPressed(e.KeyCode, e.Shift);
         }

         switch (e.KeyCode)
         {
            case Keys.F1:
            case Keys.F2:
            case Keys.F3:
               break;
            case Keys.F4:
               ToggleFreeMode();
               break;
            case Keys.F5:
               //Application.Console.WriteLine("[KeyHandler.F5]: camera: (" + World.RenderingCamera.Position.X + " | " + World.RenderingCamera.Position.Y + " | " + World.RenderingCamera.Position.Z +
               //      ") (" + World.RenderingCamera.Direction.X + " | " + World.RenderingCamera.Direction.Y + " | " + World.RenderingCamera.Direction.Z + ")");
               break;

            case Keys.F6:
               break;
            case Keys.F7:
               break;

            case Keys.F9:
               if (Scenario.CurrentRoute != null && Game.PlayerPed.CurrentVehicle != null)
               {
                  Game.PlayerPed.CurrentVehicle.Position = Scenario.CurrentRoute.SpawnPosition;
                  Game.PlayerPed.CurrentVehicle.Rotation = Scenario.CurrentRoute.SpawnRotation;
                  Game.PlayerPed.CurrentVehicle.Speed = 0;
               }
               break;
            case Keys.F10:
               break;
            case Keys.F11:
               Application.Console.IsVisible = !Application.Console.IsVisible;
               break;
            case Keys.F12:
               Application.GUI.ToggleHelp();
               break;
         }
         e.Handled = true;
      }

      void ToggleFreeMode()
      {
         var Camera = Application.GetScript<CameraScript>();

         if (Camera.CurrentMode != CameraScript.CameraMode.Free)
         {
            Application.Console.WriteLine("[KeyHandler.F4]: Entering freemode");
            m_lastCameraMode = Camera.CurrentMode;
            Camera.SetMode(CameraScript.CameraMode.Free);
            m_showGUIAfterExitingFreemode = Application.GUI.IsVisible;
            if (m_showGUIAfterExitingFreemode)
               Application.Console.WriteLine("[KeyHandler.F4]: GUI will be shown after exiting freemode");

            Application.GUI.Hide();
         }
         else
         {
            Application.Console.WriteLine("[KeyHandler.F4]: Exiting freemode");
            Camera.SetMode(m_lastCameraMode);
            if (m_showGUIAfterExitingFreemode)
            {
               Application.Console.WriteLine("[KeyHandler.F4]: Showing GUI");
               Application.GUI.Show();
            }
         }
      }
   }
}