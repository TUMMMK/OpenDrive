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

using GTA;
using GTA.Math;

namespace OpenDriveSimulator.Scripting
{
   class CameraScript : SimScriptBase
   {
      public enum CameraMode
      {
         Fixed,
         Vehicle,
         Free,
         InTransition
      }
      public CameraMode CurrentMode { get; private set; } = CameraMode.Free;

      public Vector3 ControlledPosition { get; set; } = new Vector3();
      public Vector3 ControlledDirection { get; set; } = new Vector3();
      public float ControlledFOV { get; set; } = 90.0f;

      public Vehicle CarToFollow { get; set; }

      Camera m_controlledCamera = World.CreateCamera( new Vector3(), new Vector3(), 90.0f );
      Camera m_freeCamera = null;

      public void InitCamera()
      {
         Application.Console.WriteLine("[Camera.Init]: FreeCam ID is " + m_freeCamera?.NativeValue);
         Application.Console.WriteLine("[Camera.Init]: ControlledCam ID is " + m_controlledCamera?.NativeValue);

         Application.Console.WriteLine("[Camera.Init]: Camera ID is " + World.RenderingCamera.NativeValue);
         Application.Console.WriteLine("[Camera.Init]: Pressing F8 Key"); 
         PressF8Key();
         Application.Console.WriteLine("[Camera.Init]: Camera ID is " + World.RenderingCamera.NativeValue);

         Application.ExecutionQueue.AddAction(4, () =>
         {
            Application.Console.WriteLine("[Camera.Init delayed]: Camera ID is " + World.RenderingCamera.NativeValue);
            m_freeCamera = World.RenderingCamera;
            Application.Console.WriteLine("[Camera.Init delayed]: FreeCam ID is " + m_freeCamera.NativeValue);
            Application.Console.WriteLine("[Camera.Init delayed]: ControlledCam ID is " + m_controlledCamera.NativeValue);
         });
         Application.ExecutionQueue.AddAction(6, () =>
         {
            SetMode(CameraMode.Fixed);
            m_controlledCamera.Position = ControlledPosition;
            m_controlledCamera.Direction = ControlledDirection;
            m_controlledCamera.FieldOfView = ControlledFOV;
         });
         Application.ExecutionQueue.AddAction(8, () =>
         {
            Application.Console.WriteLine("[Camera.Init delayed]: Camera ID in fixed mode is " + World.RenderingCamera.NativeValue);
            Application.Console.WriteLine("[Camera.Init delayed]: FreeCam ID is " + m_freeCamera.NativeValue);
            Application.Console.WriteLine("[Camera.Init delayed]: ControlledCam ID is " + m_controlledCamera.NativeValue);
         });

      }
      public void SetMode(CameraMode newMode)
      {
         if (CurrentMode == CameraMode.Fixed && newMode == CameraMode.Free)
            FromFixedToFree();
         else if (CurrentMode == CameraMode.Fixed && newMode == CameraMode.Vehicle)
            FromFixedToVehicle();
         else if (CurrentMode == CameraMode.Vehicle && newMode == CameraMode.Fixed)
            FromVehicleToFixed();
         else if (CurrentMode == CameraMode.Vehicle && newMode == CameraMode.Free)
            FromVehicleToFree();
         else if (CurrentMode == CameraMode.Free && newMode == CameraMode.Fixed)
            FromFreeToFixed();
         else if (CurrentMode == CameraMode.Free && newMode == CameraMode.Vehicle)
            FromFreeToVehicle();
      }

      protected override void OnAbort(object sender, EventArgs e)
      {
         World.RenderingCamera = m_freeCamera;
      }
      protected override void OnTick(object sender, EventArgs e)
      {
         switch (CurrentMode)
         {
            case CameraMode.Fixed:
               Game.PlayerPed.HasGravity = false;
               Game.PlayerPed.LodDistance = int.MaxValue;

               Game.PlayerPed.Position = ControlledPosition - 5 * ControlledDirection;
               Game.PlayerPed.Rotation = ControlledDirection;

               m_controlledCamera.Position = ControlledPosition;
               m_controlledCamera.Direction = ControlledDirection;
               m_controlledCamera.FieldOfView = ControlledFOV;
               World.RenderingCamera = m_controlledCamera;
               break;

            case CameraMode.Vehicle:
               if (Entity.Exists(CarToFollow))
               {
                  Game.PlayerPed.HasGravity = true;
                  Game.PlayerPed.LodDistance = int.MaxValue;

                  Vector3 vehicle_offset = getVehicleOffset(CarToFollow);

                  //if ((uint)CarToFollow.Model.Hash == (uint)VehicleHash.Adder)
                  //   vehicle_offset = vehicle_offset * 0.65f;

                  m_controlledCamera.Position = CarToFollow.Position + CarToFollow.ForwardVector + vehicle_offset;
                  m_controlledCamera.Rotation = CarToFollow.Rotation;
                  m_controlledCamera.FieldOfView = 100.0f;
               }
               World.RenderingCamera = m_controlledCamera;
               break;

            case CameraMode.Free:
            default:
               break;
         }

      }

      Vector3 getVehicleOffset(Vehicle car)
      {

         Vector3 rot = car.Rotation;
         Vector3 pos = car.Position;

         float rotX = 2 * (float)Math.PI / 360 * rot.X;
         float rotY = 2 * (float)Math.PI / 360 * rot.Y;
         float rotZ = 2 * (float)Math.PI / 360 * (rot.Z + 90.0f);
         float sinX = (float)Math.Sin(rotX);
         float sinY = (float)Math.Sin(rotY);
         float sinZ = (float)Math.Sin(rotZ);
         float cosX = (float)Math.Cos(rotX);
         float cosY = (float)Math.Cos(rotY);
         float cosZ = (float)Math.Cos(rotZ);

         return new Vector3(-sinX * cosY * cosZ + cosX * sinY * sinZ, -sinX * cosY * sinZ - cosX * sinY * cosZ, cosX * cosY);
      }

      void FromFreeToVehicle()
      {
         Application.Console.WriteLine("[Camera]: Going from Free to Vehicle");
         m_freeCamera = World.RenderingCamera;

         CurrentMode = CameraMode.InTransition;

         Application.ExecutionQueue.AddAction(2, () =>
         {
            Application.Console.WriteLine("[Camera Delayed]: Pressing F8");
            PressF8Key();
         });
         Application.ExecutionQueue.AddAction(4, () =>
         {
            Application.Console.WriteLine("[Camera Delayed]: Setting World camera to controlled camera");
            World.RenderingCamera = m_controlledCamera;
            Application.Console.WriteLine("[Camera Delayed]: Camera ID is " + World.RenderingCamera.NativeValue);
            CurrentMode = CameraMode.Vehicle;
         });
      }
      void FromFreeToFixed()
      {
         Application.Console.WriteLine("[Camera]: Going from Free to Fixed");
         m_freeCamera = World.RenderingCamera;

         CurrentMode = CameraMode.InTransition;
         Application.ExecutionQueue.AddAction(2, () =>
         {
            Application.Console.WriteLine("[Camera Delayed]: Pressing F8");
            PressF8Key();
         });
         Application.ExecutionQueue.AddAction(4, () =>
         {
            Application.Console.WriteLine("[Camera Delayed]: Setting World camera to controlled camera");
            World.RenderingCamera = m_controlledCamera;
            Application.Console.WriteLine("[Camera Delayed]: Camera ID is " + World.RenderingCamera.NativeValue);
            CurrentMode = CameraMode.Fixed;
         });
      }
      void FromVehicleToFree()
      {
         Application.Console.WriteLine("[Camera]: Going from Vehicle to Free");
         World.RenderingCamera = m_freeCamera;
         Application.Console.WriteLine("[Camera Delayed]: Camera ID is " + World.RenderingCamera.NativeValue);

         CurrentMode = CameraMode.InTransition;
         Application.ExecutionQueue.AddAction(2, () =>
         {
            Application.Console.WriteLine("[Camera Delayed]: Pressing F8");
            PressF8Key();
            CurrentMode = CameraMode.Free;
         });
      }
      void FromFixedToFree()
      {
         Application.Console.WriteLine("[Camera]: Going from Fixed to Free");
         World.RenderingCamera = m_freeCamera;
         Application.Console.WriteLine("[Camera Delayed]: Camera ID is " + World.RenderingCamera.NativeValue);

         CurrentMode = CameraMode.InTransition;
         Application.ExecutionQueue.AddAction(2, () =>
         {
            Application.Console.WriteLine("[Camera Delayed]: Pressing F8");
            PressF8Key();
            CurrentMode = CameraMode.Free;
         });
      }
      void FromVehicleToFixed()
      {
         Application.Console.WriteLine("[Camera]: Going from Vehicle to Fixed");

         CurrentMode = CameraMode.Fixed;
      }
      void FromFixedToVehicle()
      {
         Application.Console.WriteLine("[Camera]: Going from Fixed to Vehicle");
         CurrentMode = CameraMode.Vehicle;
      }

      //[DllImport("user32.dll")]
      //static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);
      public static void PressF8Key()
      {
         System.Windows.Forms.SendKeys.SendWait("{F8}");
         return;

         //const UInt32 WM_KEYDOWN = 0x0100;
         //const int VK_F8 = 0x77;

         //Application.Console.WriteLine("[CameraScript.PressF8Key]: current process has the name: " + Process.GetCurrentProcess().ProcessName);
         //PostMessage(Process.GetCurrentProcess().MainWindowHandle, WM_KEYDOWN, VK_F8, 0);
      }
   }
}
