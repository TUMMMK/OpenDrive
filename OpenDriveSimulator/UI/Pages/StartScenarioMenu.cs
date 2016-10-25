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

namespace OpenDriveSimulator.UI.Pages
{
   static class StartScenarioMenu
   {
      public static Page Create(Scenario scenario)
      {
         Page result = new Page();


         result.AddElement(new Label()
         {
            Name = "PartIDLabel",
            Content = "Participant ID:",
            TextColor = Application.ColorText,
            Shadow = true,
            Outline = true,
            FontSize = 34,
            ScaledWidth = 350,
            ScaledHeight = 75,
            ScaledX = 380,
            ScaledY = 450,
         });
         var partInputField = new InputField()
         {
            Name = "NameInput",
            ScaledWidth = 700,
            ScaledHeight = 50,
            ScaledX = (Tools.ScreenWidth - 700) / 2,
            ScaledY = 450,
            Content = "",
         };
         result.AddElement(partInputField);


         var abortBut = new Button()
         {
            ScaledX = 50,
            ScaledY = 950,

            ScaledWidth = 200,
            ScaledHeight = 50,

            Name = "abortBut",
            Content = "Abort",
         };
         abortBut.Selected += (a, b) => Application.GUI.SetPage(SelectScenariosMenu.Create());
         result.AddElement(abortBut);

         var startButton = new Button()
         {
            ScaledX = Tools.ScreenWidth - 250,
            ScaledY = 950,

            ScaledWidth = 200,
            ScaledHeight = 50,

            Name = "EditScenarioApplyBut",
            Content = "OK",
         };
         startButton.Selected += (a, b) =>
         {
            if (String.IsNullOrWhiteSpace(partInputField.Content))
               return;
            scenario.Start(partInputField.Content);
         };
         result.AddElement(startButton);

         return result;
      }
   }
}