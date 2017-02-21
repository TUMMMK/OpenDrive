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
using System.Collections.Generic;
using System.Linq;

using GTA;
using GTA.Math;

using OpenDriveSimulator.Scripting;

namespace OpenDriveSimulator.UI.Pages
{
   internal class EditRouteMenu
   {
      // the updated list of barriers (deleted barriers not included)
      static List<Barrier> barriers { get; set; }
      // the updated list of barriers (including deleted barriers)
      static List<Barrier> tempBarriers { get; set; }
      static Barrier currentBarrier;

      static Vector3 spawnPosition;
      static Vector3 spawnRotation;

      static Vector3 previewPosition;
      static Vector3 previewDirection;

      static int barrierIndex = 0;

      #region EditRoute Elements
      static Label editRouteLabel;

      static Label routeNameLabel;
      static InputField routeNameInputField;

      static Label availableBarriersLabel;
      static ItemList availableBarriersList;

      static Button selectSpawnButton;
      static Button selectPreviewButton;

      static Button abortButton;
      static Button deleteButton;
      static Button applyButton;
      #endregion

      #region SelectSpwan Elements
      static Button applySpawnButton;
      static Button abortSpawnButton;
      #endregion

      #region SelectPreview Elements
      static Button applyPreviewButton;
      static Button abortPreviewButton;
      #endregion

      #region SelectPreview Elements
      static Label barrierNameLabel;
      static Button startRecordingButton;
      static Button stopRecordingButton;
      static Button abortBarrierButton;
      static Button deleteBarrierButton;
      static Button applyBarrierButton;
      #endregion


      static DrawMarkerScript MarkerScript;

      public static Page Create(Route route)
      {
         Page result = new Page();

         #region Init
         if (route == null)
            route = new Route()
            {
               Name = "<enter name>",
            };
         result.Daytime = Application.DefaultDaytime;
         result.Weather = Application.DefaultWeather;

         result.CameraPosition = route.PreviewCameraPosition;
         result.CameraDirection = route.PreviewCameraDirection;

         previewPosition = route.PreviewCameraPosition;
         previewDirection = route.PreviewCameraDirection;
         spawnPosition = route.SpawnPosition;
         spawnRotation = route.SpawnRotation;

         tempBarriers = new List<Barrier>();
         tempBarriers.AddRange(route.Barriers);

         barriers = new List<Barrier>();
         barriers.AddRange(tempBarriers);


         MarkerScript = Application.GetScript<DrawMarkerScript>();
         MarkerScript.Markers = barriers;
         MarkerScript.Start();

         barrierIndex = 0;
         #endregion

         #region EditRoute Elements
         editRouteLabel = new Label()
         {
            ScaledX = 950,
            ScaledY = 100,
            ScaledWidth = 1500,
            Name = "editRouteLabel",
            Content = "Edit Route",
            TextColor = Application.ColorText,
            Shadow = true,
            Outline = true,
            FontSize = 56,
         };
         result.AddElement(editRouteLabel);

         #region Name
         routeNameLabel = new Label()
         {
            Name = "routeNameLabel",
            Content = "Name:",
            TextColor = Application.ColorText,
            Shadow = true,
            Outline = true,
            FontSize = 34,
            ScaledWidth = 150,
            ScaledHeight = 75,
            ScaledX = 100,
            ScaledY = 250,
         };
         routeNameInputField = new InputField()
         {
            Name = "routeNameInputField",
            ScaledWidth = 700,
            ScaledHeight = 50,
            ScaledX = 200,
            ScaledY = 250,
            Content = route != null ? route.Name : "<enter name>",
         };
         result.AddElement(routeNameLabel);
         result.AddElement(routeNameInputField);
         #endregion

         #region Barriers
         availableBarriersLabel = new Label()
         {
            ScaledX = 550,
            ScaledY = 340,
            ScaledWidth = 1500,
            Name = "availableBarriersLabel",
            Content = "Barriers:",
            TextColor = Application.ColorText,
            Shadow = false,
            Outline = true,
            FontSize = 40,
         };
         availableBarriersList = new ItemList()
         {
            Name = "availableBarriersList",

            ScaledX = 200,
            ScaledY = 400,
            ScaledWidth = 700,
            ScaledHeight = 7 * ItemList.C_ItemHeight + 10,
         };
         tempBarriers.ForEach(x => availableBarriersList.AddItem("Barrier" + barrierIndex++));
         availableBarriersList.AddItem("<create new>");
         availableBarriersList.Selected += EditBarrier;

         result.AddElement(availableBarriersLabel);
         result.AddElement(availableBarriersList);
         #endregion

         #region Settings
         selectSpawnButton = new Button()
         {
            Name = "selectSpawnButton",
            Content = "Select spawnpoint",

            ScaledWidth = 700,
            ScaledHeight = 50,
            ScaledX = Tools.ScreenWidth - 900,
            ScaledY = 400
         };
         selectSpawnButton.Selected += SelectSpawn;


         selectPreviewButton = new Button()
         {
            Name = "selectPreviewButton",
            Content = "Select camera preview",

            ScaledWidth = 700,
            ScaledHeight = 50,
            ScaledX = Tools.ScreenWidth - 900,
            ScaledY = 500
         };
         selectPreviewButton.Selected += SelectPreview;

         result.AddElement(selectSpawnButton);
         result.AddElement(selectPreviewButton);
         #endregion

         #region Control buttons
         abortButton = new Button()
         {
            ScaledX = 50,
            ScaledY = 950,

            ScaledWidth = 200,
            ScaledHeight = 50,

            Name = "abortButton",
            Content = "Abort",
         };
         abortButton.Selected += (a, b) => Application.GUI.SetPage(ManageRoutesMenu.Create());

         deleteButton = new Button()
         {
            ScaledX = Tools.ScreenWidth / 2 - 175,
            ScaledY = 950,

            ScaledWidth = 350,
            ScaledHeight = 50,

            Name = "deleteButton",
            Content = "Delete route",

            TextColor = System.Drawing.Color.Red
         };
         deleteButton.Selected += (a, b) =>
         {
            route.Delete();
            Application.GUI.SetPage(ManageRoutesMenu.Create());
         };

         applyButton = new Button()
         {
            ScaledX = Tools.ScreenWidth - 250,
            ScaledY = 950,

            ScaledWidth = 200,
            ScaledHeight = 50,

            Name = "applyButton",
            Content = "OK",
         };
         applyButton.Selected += (a, b) =>
         {
            ApplyChanges(route);
            Application.GUI.SetPage(ManageRoutesMenu.Create());
         };


         result.AddElement(abortButton);
         result.AddElement(deleteButton);
         result.AddElement(applyButton);
         #endregion
         #endregion

         #region SelectSpwan Elements
         abortSpawnButton = new Button()
         {
            ScaledX = 50,
            ScaledY = 950,

            ScaledWidth = 200,
            ScaledHeight = 50,

            Name = "abortSpawnButton",
            Content = "Abort",
         };
         abortSpawnButton.Selected += AbortSpawnSelection;
         applySpawnButton = new Button()
         {
            ScaledX = Tools.ScreenWidth - 250,
            ScaledY = 950,

            ScaledWidth = 200,
            ScaledHeight = 50,

            Name = "applySpawnButton",
            Content = "Select",
         };
         applySpawnButton.Selected += ApplySpawnSelection;

         abortSpawnButton.Hide();
         applySpawnButton.Hide();
         result.AddElement(abortSpawnButton);
         result.AddElement(applySpawnButton);
         #endregion


         #region SelectPreview Elements
         abortPreviewButton = new Button()
         {
            ScaledX = 50,
            ScaledY = 950,

            ScaledWidth = 200,
            ScaledHeight = 50,

            Name = "abortPreviewButton",
            Content = "Abort",
         };
         abortPreviewButton.Selected += AbortPreviewSelection;

         applyPreviewButton = new Button()
         {
            ScaledX = Tools.ScreenWidth - 250,
            ScaledY = 950,

            ScaledWidth = 200,
            ScaledHeight = 50,

            Name = "applyPreviewButton",
            Content = "Select",
         };
         applyPreviewButton.Selected += ApplyPreviewSelection;

         abortPreviewButton.Hide();
         applyPreviewButton.Hide();
         result.AddElement(abortPreviewButton);
         result.AddElement(applyPreviewButton);
         #endregion

         #region EditBarrier Elements
         barrierNameLabel = new Label()
         {
            ScaledX = 950,
            ScaledY = 100,
            ScaledWidth = 1500,
            Name = "barrierNameLabel",
            TextColor = Application.ColorText,
            Shadow = true,
            Outline = true,
            FontSize = 56,
         };

         startRecordingButton = new Button()
         {
            ScaledX = 50,
            ScaledY = 850,

            ScaledWidth = 350,
            ScaledHeight = 50,

            Name = "startRecordingButton",
            Content = "Start recording",
         };
         startRecordingButton.Selected += StartRecording;

         stopRecordingButton = new Button()
         {
            ScaledX = 50,
            ScaledY = 850,

            ScaledWidth = 350,
            ScaledHeight = 50,

            Name = "stopRecordingButton",
            Content = "Stop recording",
         };
         stopRecordingButton.Selected += StopRecording;

         abortBarrierButton = new Button()
         {
            ScaledX = 50,
            ScaledY = 950,

            ScaledWidth = 200,
            ScaledHeight = 50,

            Name = "abortBarrierButton",
            Content = "Abort",
         };
         abortBarrierButton.Selected += AbortBarrierEditing;

         deleteBarrierButton = new Button()
         {
            ScaledX = Tools.ScreenWidth / 2 - 175,
            ScaledY = 950,

            ScaledWidth = 350,
            ScaledHeight = 50,

            Name = "deleteBarrierButton",
            Content = "Delete barrier",

            TextColor = System.Drawing.Color.Red
         };
         deleteBarrierButton.Selected += DeleteBarrier;

         applyBarrierButton = new Button()
         {
            ScaledX = Tools.ScreenWidth - 250,
            ScaledY = 950,

            ScaledWidth = 200,
            ScaledHeight = 50,

            Name = "applyBarrierButton",
            Content = "Save",
         };
         applyBarrierButton.Selected += SaveBarrier;

         barrierNameLabel.Hide();
         startRecordingButton.Hide();
         stopRecordingButton.Hide();
         abortBarrierButton.Hide();
         deleteBarrierButton.Hide();
         applyBarrierButton.Hide();

         result.AddElement(barrierNameLabel);
         result.AddElement(startRecordingButton);
         result.AddElement(stopRecordingButton);
         result.AddElement(abortBarrierButton);
         result.AddElement(deleteBarrierButton);
         result.AddElement(applyBarrierButton);
         #endregion

         return result;
      }

      static void updateTempBarriers()
      {
         barriers.Clear();

         var allBarriers = availableBarriersList.ListItems.Take(availableBarriersList.Items.Count - 1);

         Application.Console.WriteLine("[EB.updateTempBarriers]: allBarriers has " + allBarriers.Count() + " entries");
         Application.Console.WriteLine("[EB.updateTempBarriers]: allBarriers maxIndex is " + allBarriers.Last());
         Application.Console.WriteLine("[EB.updateTempBarriers]: tempBarriers has " + tempBarriers.Count + " entries");

         var indicesToSave = allBarriers.Select(x => Convert.ToInt32(x.Substring(7)));
         foreach (var index in indicesToSave)
         {
            Application.Console.WriteLine("[EB.updateTempBarriers]: Adding Barrier " + index);
            barriers.Add(tempBarriers[index]);
         }

         Application.Console.WriteLine("[EB.updateTempBarriers]: Update barrierlist in drawing script");
         MarkerScript.Markers = barriers;
      }

      #region Callback methods
      private static void AbortBarrierEditing(object sender, EventArgs e)
      {
         Application.Console.WriteLine("[EB.Buttons]: AbortBarrierEditing pressed");

         editRouteLabel.Show();
         routeNameLabel.Show();
         routeNameInputField.Show();
         availableBarriersLabel.Show();
         availableBarriersList.Show();
         selectSpawnButton.Show();
         selectPreviewButton.Show();
         abortButton.Show();
         deleteButton.Show();
         applyButton.Show();

         barrierNameLabel.Hide();
         startRecordingButton.Hide();
         stopRecordingButton.Hide();
         abortBarrierButton.Hide();
         deleteBarrierButton.Hide();
         applyBarrierButton.Hide();

         var Camera = Application.GetScript<CameraScript>();
         Camera.SetMode(CameraScript.CameraMode.Fixed);
         Application.ExecutionQueue.AddAction(2, () => Camera.ControlledPosition = (previewPosition != null) ? previewPosition : Application.DefaultCameraPosition);
      }

      private static void SaveBarrier(object sender, EventArgs e)
      {
         Application.Console.WriteLine("[EB.Buttons]: SaveBarrier pressed");

         editRouteLabel.Show();
         routeNameLabel.Show();
         routeNameInputField.Show();
         availableBarriersLabel.Show();
         availableBarriersList.Show();
         selectSpawnButton.Show();
         selectPreviewButton.Show();
         abortButton.Show();
         deleteButton.Show();
         applyButton.Show();

         barrierNameLabel.Hide();
         startRecordingButton.Hide();
         stopRecordingButton.Hide();
         abortBarrierButton.Hide();
         deleteBarrierButton.Hide();
         applyBarrierButton.Hide();

         if (currentBarrier == null)
            return;

         if (!availableBarriersList.Exists(barrierNameLabel.Content))
         {
            Application.Console.WriteLine("[EB.SaveBarrier]: Barrier \"" + barrierNameLabel.Content + "\" does not exist. Adding a new entry");
            availableBarriersList.RemoveItem("<create new>");
            availableBarriersList.AddItem(barrierNameLabel.Content);
            availableBarriersList.AddItem("<create new>");

            Application.Console.WriteLine("[EB.SaveBarrier]: Adding Barrier to tempBarriers");

            List<Vector2> newCoordinates = new List<Vector2>();
            newCoordinates.AddRange(currentBarrier.Coordinates);
            Barrier newBarrier = new Barrier() { Coordinates = newCoordinates };
            tempBarriers.Add(newBarrier);

            Application.Console.WriteLine("[EB.SaveBarrier]: tempBarriers has " + tempBarriers.Count + " entries");


            barrierIndex++;
         }
         else
         {
            Application.Console.WriteLine("[EB.SaveBarrier]: Barrier \"" + barrierNameLabel.Content + "\" exists. Modifying entry");
            int barrierNumber = Convert.ToInt32(barrierNameLabel.Content.Substring(7));

            List<Vector2> newCoordinates = new List<Vector2>();
            newCoordinates.AddRange(currentBarrier.Coordinates);
            tempBarriers[barrierNumber].Coordinates = newCoordinates;
         }

         var Camera = Application.GetScript<CameraScript>();
         Camera.SetMode(CameraScript.CameraMode.Fixed);
         Application.ExecutionQueue.AddAction(2, () => Camera.ControlledPosition = (previewPosition != null) ? previewPosition : Application.DefaultCameraPosition);

         updateTempBarriers();
      }

      private static void DeleteBarrier(object sender, EventArgs e)
      {
         Application.Console.WriteLine("[EB.Buttons]: DeleteBarrier pressed");

         editRouteLabel.Show();
         routeNameLabel.Show();
         routeNameInputField.Show();
         availableBarriersLabel.Show();
         availableBarriersList.Show();
         selectSpawnButton.Show();
         selectPreviewButton.Show();
         abortButton.Show();
         deleteButton.Show();
         applyButton.Show();

         barrierNameLabel.Hide();
         startRecordingButton.Hide();
         stopRecordingButton.Hide();
         abortBarrierButton.Hide();
         deleteBarrierButton.Hide();
         applyBarrierButton.Hide();

         if (availableBarriersList.Exists(barrierNameLabel.Content))
         {
            Application.Console.WriteLine("[EB.DeleteBarrier]: Deleting existing barrier \"" + barrierNameLabel.Content + "\".");
            availableBarriersList.RemoveItem(barrierNameLabel.Content);
         }
         else
            Application.Console.WriteLine("[EB.DeleteBarrier]: Barrier \"" + barrierNameLabel.Content + "\" does not exist. Nothing deleted");
         availableBarriersList.SelectItem(0);

         var Camera = Application.GetScript<CameraScript>();
         Camera.SetMode(CameraScript.CameraMode.Fixed);
         Application.ExecutionQueue.AddAction(2, () => Camera.ControlledPosition = (previewPosition != null) ? previewPosition : Application.DefaultCameraPosition);

         updateTempBarriers();
      }


      private static void EditBarrier(object sender, EventArgs e)
      {
         Application.Console.WriteLine("[EB.Buttons]: EditBarrier pressed");
         editRouteLabel.Hide();
         routeNameLabel.Hide();
         routeNameInputField.Hide();
         availableBarriersLabel.Hide();
         availableBarriersList.Hide();
         selectSpawnButton.Hide();
         selectPreviewButton.Hide();
         abortButton.Hide();
         deleteButton.Hide();
         applyButton.Hide();

         barrierNameLabel.Show();
         startRecordingButton.Show();
         stopRecordingButton.Hide();
         abortBarrierButton.Show();
         deleteBarrierButton.Show();
         applyBarrierButton.Show();

         Vector3 cameraPosition;

         string SelectedName = (sender as Button).Content;
         if (SelectedName == "<create new>")
         {
            Application.Console.WriteLine("[EB.EditBarrier]: <create new> pressed; creating new barrier with index " + barrierIndex);
            currentBarrier = new Barrier();
            barrierNameLabel.Content = "Barrier" + (barrierIndex);
            int indexInList = availableBarriersList.ListItems.IndexOf(SelectedName);
            if (indexInList == 0)
            {
               cameraPosition = spawnPosition;
            }

            else
            {
               int index = Convert.ToInt32(availableBarriersList.ListItems[indexInList - 1].Substring(7));
               cameraPosition = Tools.ConvertToVector3(tempBarriers[index].Midpoint);
            }
         }
         else
         {
            Application.Console.WriteLine("[EB.EditBarrier]: Editing existing barrier \"" + (sender as Button).Content + "\".");
            string entryName = (sender as Button).Content;
            int barrierNumber = Convert.ToInt32(entryName.Substring(7));
            currentBarrier = tempBarriers[barrierNumber];
            barrierNameLabel.Content = entryName;
            cameraPosition = Tools.ConvertToVector3(currentBarrier.Midpoint);
         }


         var Camera = Application.GetScript<CameraScript>();

         Camera.ControlledPosition = cameraPosition;
         Application.ExecutionQueue.AddAction(2, () => Camera.SetMode(CameraScript.CameraMode.Free));
         Application.Console.WriteLine("[EB.EditBarrier]: set camera to fixed mode");
      }

      private static void StartRecording(object sender, EventArgs e)
      {
         Application.Console.WriteLine("[EB.Buttons]: StartRecording pressed");
         startRecordingButton.Hide();
         stopRecordingButton.Show();

         Application.GUI.SetActiveElement(stopRecordingButton.Name);

         var recorder = Application.GetScript<RecordMarker>();
         recorder.Start();
         recorder.StartRecording();
      }


      private static void StopRecording(object sender, EventArgs e)
      {
         Application.Console.WriteLine("[EB.Buttons]: StopRecording pressed");
         startRecordingButton.Show();
         stopRecordingButton.Hide();

         var recorder = Application.GetScript<RecordMarker>();
         recorder.StopRecording();
         currentBarrier.Coordinates = recorder.GetCoordinates();
      }

      private static void ApplySpawnSelection(object sender, EventArgs e)
      {
         Application.Console.WriteLine("[EB.Buttons]: ApplySpawnSelection pressed");
         editRouteLabel.Show();
         routeNameLabel.Show();
         routeNameInputField.Show();
         availableBarriersLabel.Show();
         availableBarriersList.Show();
         selectSpawnButton.Show();
         selectPreviewButton.Show();
         abortButton.Show();
         deleteButton.Show();
         applyButton.Show();

         abortSpawnButton.Hide();
         applySpawnButton.Hide();

         spawnPosition = World.RenderingCamera.Position;
         spawnPosition.Z = World.GetGroundHeight(new Vector2(spawnPosition.X, spawnPosition.Y));
         spawnRotation = World.RenderingCamera.Rotation;

         var Camera = Application.GetScript<CameraScript>();
         Camera.SetMode(CameraScript.CameraMode.Fixed);
         Application.ExecutionQueue.AddAction(2, () => Camera.ControlledPosition = (previewPosition != null) ? previewPosition : Application.DefaultCameraPosition);
      }

      private static void AbortSpawnSelection(object sender, EventArgs e)
      {
         Application.Console.WriteLine("[EB.Buttons]: AbortSpawnSelection pressed");
         editRouteLabel.Show();
         routeNameLabel.Show();
         routeNameInputField.Show();
         availableBarriersLabel.Show();
         availableBarriersList.Show();
         selectSpawnButton.Show();
         selectPreviewButton.Show();
         abortButton.Show();
         deleteButton.Show();
         applyButton.Show();

         abortSpawnButton.Hide();
         applySpawnButton.Hide();

         var Camera = Application.GetScript<CameraScript>();
         Camera.SetMode(CameraScript.CameraMode.Fixed);
         Application.ExecutionQueue.AddAction(2, () => Camera.ControlledPosition = (previewPosition != null) ? previewPosition : Application.DefaultCameraPosition);
      }

      private static void ApplyPreviewSelection(object sender, EventArgs e)
      {
         Application.Console.WriteLine("[EB.Buttons]: ApplyPreviewSelection pressed");
         editRouteLabel.Show();
         routeNameLabel.Show();
         routeNameInputField.Show();
         availableBarriersLabel.Show();
         availableBarriersList.Show();
         selectSpawnButton.Show();
         selectPreviewButton.Show();
         abortButton.Show();
         deleteButton.Show();
         applyButton.Show();

         abortPreviewButton.Hide();
         applyPreviewButton.Hide();

         previewPosition = World.RenderingCamera.Position;
         previewDirection = World.RenderingCamera.Direction;

         var Camera = Application.GetScript<CameraScript>();

         Camera.SetMode(CameraScript.CameraMode.Fixed);
         Camera.ControlledPosition = previewPosition;
         Camera.ControlledDirection = previewDirection;
      }

      private static void AbortPreviewSelection(object sender, EventArgs e)
      {
         Application.Console.WriteLine("[EB.Buttons]: AbortPreviewSelection pressed");
         editRouteLabel.Show();
         routeNameLabel.Show();
         routeNameInputField.Show();
         availableBarriersLabel.Show();
         availableBarriersList.Show();
         selectSpawnButton.Show();
         selectPreviewButton.Show();
         abortButton.Show();
         deleteButton.Show();
         applyButton.Show();

         abortPreviewButton.Hide();
         applyPreviewButton.Hide();

         var Camera = Application.GetScript<CameraScript>();
         Camera.SetMode(CameraScript.CameraMode.Fixed);
         Application.ExecutionQueue.AddAction(2, () => Camera.ControlledPosition = (previewPosition != null) ? previewPosition : Application.DefaultCameraPosition);
      }

      private static void SelectPreview(object sender, EventArgs e)
      {
         Application.Console.WriteLine("[EB.Buttons]: SelectPreview pressed");
         editRouteLabel.Hide();
         routeNameLabel.Hide();
         routeNameInputField.Hide();
         availableBarriersLabel.Hide();
         availableBarriersList.Hide();
         selectSpawnButton.Hide();
         selectPreviewButton.Hide();
         abortButton.Hide();
         deleteButton.Hide();
         applyButton.Hide();

         abortPreviewButton.Show();
         applyPreviewButton.Show();

         var Camera = Application.GetScript<CameraScript>();
         Camera.SetMode(CameraScript.CameraMode.Free);
         Application.ExecutionQueue.AddAction(2, () => Camera.ControlledPosition = (previewPosition != null) ? previewPosition : Application.DefaultCameraPosition);
      }

      private static void SelectSpawn(object sender, EventArgs e)
      {
         Application.Console.WriteLine("[EB.Buttons]: SelectSpawn pressed");
         editRouteLabel.Hide();
         routeNameLabel.Hide();
         routeNameInputField.Hide();
         availableBarriersLabel.Hide();
         availableBarriersList.Hide();
         selectSpawnButton.Hide();
         selectPreviewButton.Hide();
         abortButton.Hide();
         deleteButton.Hide();
         applyButton.Hide();

         abortSpawnButton.Show();
         applySpawnButton.Show();

         var Camera = Application.GetScript<CameraScript>();
         Camera.SetMode(CameraScript.CameraMode.Free);
         Application.ExecutionQueue.AddAction(2, () => Camera.ControlledPosition = (previewPosition != null) ? previewPosition : Application.DefaultCameraPosition);
      }

      private static void ApplyChanges(Route oldRoute)
      {
         Application.Console.WriteLine("[EB.Buttons]: ApplyChanges pressed");
         Route route = new Route()
         {
            Name = routeNameInputField.Content,
            SpawnPosition = spawnPosition,
            SpawnRotation = spawnRotation,
            PreviewCameraPosition = previewPosition,
            PreviewCameraDirection = previewDirection,
         };
         updateTempBarriers();
         route.Barriers.AddRange(barriers);

         Application.Console.WriteLine("[EB.Apply]: Deleting old route");
         oldRoute.Delete();
         Application.Console.WriteLine("[EB.Apply]: Saving new route");
         route.Save();
      }
      #endregion
   }
}