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

using System.Windows.Forms;

namespace OpenDriveSimulator
{
   static class KeyMapping // German Keyboard layout!!!
   {
      public static char GetChar(Keys KeyCode, bool Shift)
      {
         switch (KeyCode)
         {
            case Keys.A:
               if (Shift)
                  return 'A';
               return 'a';
            case Keys.B:
               if (Shift)
                  return 'B';
               return 'b';
            case Keys.C:
               if (Shift)
                  return 'C';
               return 'c';
            case Keys.D:
               if (Shift)
                  return 'D';
               return 'd';
            case Keys.E:
               if (Shift)
                  return 'E';
               return 'e';
            case Keys.F:
               if (Shift)
                  return 'F';
               return 'f';
            case Keys.G:
               if (Shift)
                  return 'G';
               return 'g';
            case Keys.H:
               if (Shift)
                  return 'H';
               return 'h';
            case Keys.I:
               if (Shift)
                  return 'I';
               return 'i';
            case Keys.J:
               if (Shift)
                  return 'J';
               return 'j';
            case Keys.K:
               if (Shift)
                  return 'K';
               return 'k';
            case Keys.L:
               if (Shift)
                  return 'L';
               return 'l';
            case Keys.M:
               if (Shift)
                  return 'M';
               return 'm';
            case Keys.N:
               if (Shift)
                  return 'N';
               return 'n';
            case Keys.O:
               if (Shift)
                  return 'O';
               return 'o';
            case Keys.P:
               if (Shift)
                  return 'P';
               return 'p';
            case Keys.Q:
               if (Shift)
                  return 'Q';
               return 'q';
            case Keys.R:
               if (Shift)
                  return 'R';
               return 'r';
            case Keys.S:
               if (Shift)
                  return 'S';
               return 's';
            case Keys.T:
               if (Shift)
                  return 'T';
               return 't';
            case Keys.U:
               if (Shift)
                  return 'U';
               return 'u';
            case Keys.V:
               if (Shift)
                  return 'V';
               return 'v';
            case Keys.W:
               if (Shift)
                  return 'W';
               return 'w';
            case Keys.X:
               if (Shift)
                  return 'X';
               return 'x';
            case Keys.Y:
               if (Shift)
                  return 'Y';
               return 'y';
            case Keys.Z:
               if (Shift)
                  return 'Z';
               return 'z';
            case Keys.D0:
               if (Shift)
                  return '=';
               return '0';
            case Keys.D1:
               if (Shift)
                  return '!';
               return '1';
            case Keys.D2:
               return '2';
            case Keys.D3:
               if (Shift)
                  return '§';
               return '3';
            case Keys.D4:
               if (Shift)
                  return '$';
               return '4';
            case Keys.D5:
               if (Shift)
                  return '%';
               return '5';
            case Keys.D6:
               if (Shift)
                  return '&';
               return '6';
            case Keys.D7:
               return '7';
            case Keys.D8:
               if (Shift)
                  return '(';
               return '8';
            case Keys.D9:
               if (Shift)
                  return ')';
               return '9';
            case Keys.Add:
               return '+';
            case Keys.Divide:
               return '/';
            case Keys.OemCloseBrackets:
               return ')';
            case Keys.Oemcomma:
               if (Shift)
                  return ';';
               return ',';
            case Keys.OemMinus:
               if (Shift)
                  return '_';
               return '-';
            case Keys.OemOpenBrackets:
               return '(';
            case Keys.OemPeriod:
               return '.';
            case Keys.Oemplus:
               return '+';
            case Keys.OemSemicolon:
               return ';';
            case Keys.Space:
               return ' ';
            case Keys.Subtract:
               return '-';

            default:
               return '\0';
         }
      }
   }
}
