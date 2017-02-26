using System;
using System.Collections.Generic;
using System.Text;
using CocosSharp;
using BucketApp.CocosScenes;

namespace BucketApp.CocosDrawObjects
{
    class GridObject : CCDrawNode
    {
        private CCAffineTransform _oldTransform;
        private CCNode RootNode;
        public int NumberOfLines { get; set; } = 15;
        public GridObject()
        {
            base.AddAction(new CocosActions.DrawGrid(16));
            base.Schedule((a) =>
                {
                    StartAction(this);
                }, 0.016f);
        }
        /// <summary>
        /// Draw grid on root object whit tag id=666 on layout of the Target
        /// </summary>
        /// <param name="Target"></param>
        private void StartAction(CCNode Target)
        {
            try
            {
                var target = Target.Scene;
                if (RootNode != null && _oldTransform != RootNode.AdditionalTransform)
                {
                    CCRect rec = RootNode.AdditionalTransform.Inverse.Transform(Target.Layer.VisibleBoundsWorldspace);
                    float devieBy = (float)Math.Pow(10, (int)Math.Log10((double)rec.Size.Width));

                    rec.Origin.X = (((int)(rec.Origin.X / devieBy)) * devieBy - devieBy * 2);
                    rec.Origin.Y = (((int)(rec.Origin.Y / devieBy)) * devieBy - devieBy * 2);
                    _oldTransform = target.AdditionalTransform;
                    CCV3F_C4B[] Lines = new CCV3F_C4B[NumberOfLines * 4 + 1];
                    for (int i = 0; i < NumberOfLines; i++)
                    {
                        float x = rec.MinX + devieBy * i, y = rec.MinY + devieBy * i;
                        Lines[i * 2] = new CCV3F_C4B(new CCPoint(x,
                                                                 rec.MinY - 3 * devieBy),
                                                    (int)x == 0 ?
                                                                Helper.Colors.CCColor3B4B(Helper.Colors.Accent) :
                                                                Helper.Colors.CCColor3B4B(Helper.Colors.LightTextColor));
                        Lines[i * 2 + 1] = new CCV3F_C4B(new CCPoint(x, 
                                                                     rec.MaxY + 3 * devieBy),
                                                        (int)x == 0 ?
                                                                    Helper.Colors.CCColor3B4B(Helper.Colors.Accent) :
                                                                    Helper.Colors.CCColor3B4B(Helper.Colors.LightTextColor));
                        Lines[i * 2 + NumberOfLines * 2] = new CCV3F_C4B(new CCPoint(rec.MinX - 3 * devieBy,
                                                                                     y),
                                                                        (int)y == 0 ?
                                                                                    Helper.Colors.CCColor3B4B(Helper.Colors.Accent) :
                                                                                    Helper.Colors.CCColor3B4B(Helper.Colors.LightTextColor));
                        Lines[i * 2 + NumberOfLines * 2 + 1] = new CCV3F_C4B(new CCPoint(rec.MaxX + 3 * devieBy, 
                                                                                         y),
                                                                            (int)y == 0 ?
                                                                                        Helper.Colors.CCColor3B4B(Helper.Colors.Accent) :
                                                                                        Helper.Colors.CCColor3B4B(Helper.Colors.LightTextColor));
                    }
                    if (Target is CCDrawNode dn)
                    {
                        dn.Clear();
                        dn.DrawLineList(Lines);
                    }

                }
                else if (RootNode == null)
                {
                    RootNode = Target.Layer.GetChildByTag(666);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }
    }
}
