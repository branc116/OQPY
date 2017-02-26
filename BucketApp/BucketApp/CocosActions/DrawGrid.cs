using System;
using System.Collections.Generic;
using System.Text;
using CocosSharp;
using BucketApp.CocosScenes;
using System.Threading.Tasks;

namespace BucketApp.CocosActions
{
    class DrawGrid : CCAction
    {
        private CCAffineTransform _oldTransform;
        public DrawGrid(float Duration) : base()
        {
            
        }
        protected override CCActionState StartAction(CCNode Target)
        {
            try
            {
                var target = Target.Scene;
                if (_oldTransform != null && _oldTransform != target.AdditionalTransform)
                {
                    _oldTransform = target.AdditionalTransform;
                    CCV3F_C4B[] Lines = new CCV3F_C4B[40];
                    for (int i = 0; i < 10; i++)
                    {
                        Lines[i * 2] = new CCV3F_C4B(new CCPoint(target.BoundingBox.MinX + (target.BoundingBox.Size.Width / 10) * i, target.BoundingBox.MinY - 10), Helper.Colors.CCColor3B4B(Helper.Colors.LightBackgroundColor));
                        Lines[i * 2 + 1] = new CCV3F_C4B(new CCPoint(target.BoundingBox.MinX + (target.BoundingBox.Size.Width / 10) * i, target.BoundingBox.MaxX + 10), Helper.Colors.CCColor3B4B(Helper.Colors.LightBackgroundColor));
                        Lines[i * 2 + 20] = new CCV3F_C4B(new CCPoint(target.BoundingBox.MinX - 10, target.BoundingBox.MinY + (target.BoundingBox.Size.Height / 10) * i), Helper.Colors.CCColor3B4B(Helper.Colors.LightBackgroundColor));
                        Lines[i * 2 + 20] = new CCV3F_C4B(new CCPoint(target.BoundingBox.MaxX + 10, target.BoundingBox.MinY + (target.BoundingBox.Size.Height / 10) * i), Helper.Colors.CCColor3B4B(Helper.Colors.LightBackgroundColor));
                    }
                    if (Target is CCDrawNode dn)
                        dn.DrawLineList(Lines);

                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
            return base.StartAction(Target);
        }
    }
}
