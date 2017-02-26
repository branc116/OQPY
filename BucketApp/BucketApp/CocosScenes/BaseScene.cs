using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using CocosSharp;
using System.Threading.Tasks;

namespace BucketApp.CocosScenes
{

    class BaseScene : CCScene
    {
        protected CCLayerColor BaseLayer;
        protected CCNode RootNode;
        protected Helper.CollisionObject LastCollision;
        public bool GridActive;
        public BaseScene(CCGameView view) : base(view)
        {
            int _n = 0;
            BaseLayer = new CCLayerColor(Helper.Colors.CCColor3B4B(Helper.Colors.LightBackgroundColor));
            RootNode = new CCNode(BaseLayer.ContentSize);
            var Zoomy = new CCEventListenerTouchAllAtOnce()
            {
                IsEnabled = true,
                OnTouchesBegan = (touch, nes) =>
                {
                    _n += touch.Count;
                },
                OnTouchesEnded = (touch, nes) =>
                {
                    _n -= touch.Count;
                },
                OnTouchesCancelled = (touch, nes) =>
                {
                    _n -= touch.Count;
                },
                OnTouchesMoved = (touch, nes) =>
                {
                    
                    try {
                        if (touch.Count == 1 && _n == 1 && (LastCollision == null || LastCollision.CollisionType == Helper.CollisionType.None))
                        {
                            float dx = touch[0].LocationOnScreen.X - touch[0].PreviousLocationOnScreen.X, dy = touch[0].LocationOnScreen.Y - touch[0].PreviousLocationOnScreen.Y;
                            var sc = RootNode.AdditionalTransform.ScaleX;
                            RootNode.AdditionalTransform = CCAffineTransform.Translate(RootNode.AdditionalTransform, -(dx / sc / 1000), -(dy / sc / 1000));
                        }

                        if (touch.Count >= 2 && _n >= 2)
                        {
                            CCPoint mid;
                            float x = 0, y = 0;
                            foreach (var t in touch)
                            {
                                x += t.PreviousLocationOnScreen.X;
                                y += t.PreviousLocationOnScreen.Y;
                                //dx = Math.Min(t.LocationOnScreen.X - t.PreviousLocationOnScreen.X, dx);
                                //dy = Math.Min(t.LocationOnScreen.Y - t.PreviousLocationOnScreen.Y, dy);
                            }
                            mid = new CCPoint(x / touch.Count, y / touch.Count);

                            float scale = 1;
                            foreach (var t in touch)
                            {
                                scale *= CCPoint.Distance(mid, t.LocationOnScreen) / CCPoint.Distance(mid, t.PreviousLocationOnScreen);
                            }
                            if (scale != float.NaN && scale < 100 && scale > -100 && scale != float.NegativeInfinity && scale != float.PositiveInfinity)
                                RootNode.AdditionalTransform = CCAffineTransform.ScaleCopy(RootNode.AdditionalTransform, scale, scale);
                        }
                    }catch(Exception ex)
                    {

                    }
                }
            };
            RootNode.Tag = 666;
            RootNode.AddChild(new CocosDrawObjects.GridObject());
            RootNode.AdditionalTransform = CCAffineTransform.ScaleCopy(RootNode.AdditionalTransform, 0.002f, 0.002f);
            RootNode.AdditionalTransform = CCAffineTransform.Translate(RootNode.AdditionalTransform, -200, -200);
            BaseLayer.AddChild(RootNode);
            this.AddLayer(BaseLayer);
            BaseLayer.AddEventListener(Zoomy,10);
        }

        private void Resoluton(float obj)
        {
        }
    }
}
