using GTA;
using GTA.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDriveSimulator.Marker
{
   abstract class PropertyBasedMarker : MarkerBase
   {
      Prop m_property = null;
      const int c_modelHash = -205311355;

      public new Vector3 Position 
      {
         get
         {
            if (m_property != null && m_property.Exists())
               return m_property.Position;
            else
               return base.Position;
         }
         set
         {
            base.Position = value;
            if (m_property != null && m_property.Exists() && !SetToGround)
               m_property.Position = value;
         }
      }

      protected abstract int ModelHash { get; }

      protected PropertyBasedMarker()
      {
      }
      public override void Delete()
      {
         if (m_property == null)
            return;

         if (m_property.Exists())
         {
            m_property.Delete();
            m_property = null;
         }
      }
      public override void Draw()
      {
         var Rotation = new Vector3(0, 0, (float)(Math.Atan(Direction.Y / Direction.X) / Math.PI * 180));

         if (m_property == null)
         {
            m_property = World.CreateProp(new Model(ModelHash), Position, Rotation, false, SetToGround);
            if (m_property.Exists())
            {
               m_property.IsPositionFrozen = true;
            }
            m_updateObject = false;
         }
         else if (m_updateObject)
         {
            m_property.Delete();
            m_property = World.CreateProp(new Model(ModelHash), Position, Rotation, false, SetToGround);
            if (m_property.Exists())
            {
               m_property.IsPositionFrozen = true;
            }
            m_updateObject = false;
         }
      }
   }
}
