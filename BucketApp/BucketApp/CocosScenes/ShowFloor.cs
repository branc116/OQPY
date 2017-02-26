using static BucketApp.CocosScenes.Helper.Colors;
using static BucketApp.CocosScenes.Helper.Geometry;
using CocosSharp;
using System;
using System.Collections.Generic;

namespace BucketApp.CocosScenes
{
    class ShowFloor : BaseScene
    {
        List<CCPoint> PolygonPoints;
        CCDrawNode HelperNode = new CCDrawNode();
        CCDrawNode FloorNode;
        public ShowFloor(CCGameView view) : base(view)
        {
            FloorNode = new CCDrawNode();
            PolygonPoints = new List<CCPoint>() { new CCPoint(0, 0),
                                                  new CCPoint(50, 0),
                                                  new CCPoint(50, 100),
                                                  new CCPoint(0, 100)};
            ReDrawFloor(PolygonPoints.ToArray());

            FloorNode.AddEventListener(new CCEventListenerTouchOneByOne()
            {
                IsSwallowTouches = false,
                IsEnabled = true,
                OnTouchMoved = TransformPolygon,
                OnTouchBegan = ModifyPolygon,
                OnTouchEnded = EndModifyPolygon

            });
            RootNode.AddChild(FloorNode);
            RootNode.AddChild(HelperNode);
        }

        private void TransformPolygon(CCTouch touch, CCEvent sender)
        {
            if (sender.CurrentTarget is CCDrawNode)
            {
                var Transformd = RootNode.AdditionalTransform.Inverse.Transform(touch.Delta);
                if (LastCollision != null && LastCollision.CollisionType != Helper.CollisionType.None)
                {
                    for (int i = 0; i < LastCollision.DetectedCollision.Count; i++)
                    {
                        LastCollision.DetectedCollision[i] += touch.Delta / RootNode.AdditionalTransform.ScaleX;
                    }
                    switch (LastCollision.CollisionType)
                    {
                        case Helper.CollisionType.Face:
                            HelperNodePolygone(LastCollision.DetectedCollision.ToArray());
                            break;
                        case Helper.CollisionType.Edge:
                            HelperNodeEdge(LastCollision.DetectedCollision[0], LastCollision.DetectedCollision[1]);
                            break;
                        case Helper.CollisionType.Polygone:
                            HelperNodePolygone(LastCollision.DetectedCollision.ToArray());
                            break;
                        case Helper.CollisionType.Vertex:
                            HelperNodeVertex(LastCollision.DetectedCollision[0]);
                            break;

                    }
                }

            }
        }
        private void EndModifyPolygon(CCTouch touch, CCEvent sender)
        {
            
            HelperNode.Clear();
            if (LastCollision != null && LastCollision.CollisionType != Helper.CollisionType.None)
            {
                LastCollision.DetectedCollision = RoundToGrid(50, LastCollision.DetectedCollision);
                bool same = true;
                for (int i = 0; i < LastCollision.Indexes.Count; i++)
                {
                    if (PolygonPoints[LastCollision.Indexes[i]] != LastCollision.DetectedCollision[i])
                        same = false;
                    PolygonPoints[LastCollision.Indexes[i]] = LastCollision.DetectedCollision[i];
                }
                if (same)
                {
                    if (LastCollision.CollisionType == Helper.CollisionType.Edge)
                        PolygonPoints.Insert(LastCollision.Indexes[0] + 1, RoundToGrid(50, CCPoint.Midpoint(LastCollision.DetectedCollision[0], LastCollision.DetectedCollision[1]))[0]);
                    else if (LastCollision.CollisionType == Helper.CollisionType.Vertex)
                        PolygonPoints.RemoveAt(LastCollision.Indexes[0]);
                }
                ReDrawFloor(PolygonPoints.ToArray());
            }
            LastCollision = null;
        }

        private void ReDrawFloor(CCPoint[] polygonPoints)
        {
            FloorNode.Clear();
            int i = 0;
            while(i < polygonPoints.Length)
            {
                FloorNode.DrawLine(polygonPoints[i], polygonPoints[(i + 1) % polygonPoints.Length], 3, CCColor4B.Black);
                FloorNode.DrawSolidCircle(polygonPoints[i], 5, CCColor4B.Black);
                FloorNode.DrawSolidCircle(polygonPoints[(i + 1) % polygonPoints.Length], 5, CCColor4B.Black);
                i++;
            }
            //FloorNode.DrawPolygon(PolygonPoints, 4, CCColor3B4B(DarkBackgroundColor), 2, CCColor4B.Black);
        }
        
        private bool ModifyPolygon(CCTouch touch, CCEvent sender)
        {
            if (sender.CurrentTarget is CCDrawNode)
            {
                var Transformd = RootNode.AdditionalTransform.Inverse.Transform(touch.Location);
                LastCollision = Helper.Geometry.DetectCollision(PolygonPoints, Transformd);
                switch (LastCollision.CollisionType)
                {
                    case Helper.CollisionType.Vertex:
                        HelperNodeVertex(LastCollision.DetectedCollision[0]);
                        break;
                    case Helper.CollisionType.Edge:
                        HelperNodeEdge(LastCollision.DetectedCollision[0], LastCollision.DetectedCollision[1]);
                        break;
                    case Helper.CollisionType.Face:
                        HelperNodePolygone(LastCollision.DetectedCollision.ToArray());
                        break;
                    case Helper.CollisionType.Polygone:
                        HelperNodePolygone(LastCollision.DetectedCollision.ToArray());
                        break;
                }
                System.Diagnostics.Debug.WriteLineIf(true, Transformd, "LocationDebug");
                return true;
            }
            return true;
            throw new NotImplementedException();
        }
        private void HelperNodeVertex(CCPoint point)
        {
            HelperNode.Clear();
            HelperNode.DrawSolidCircle(point, 10, CCColor3B4B(PrimaryDark));
        }
        private void HelperNodeEdge(CCPoint to, CCPoint from)
        {
            HelperNode.Clear();
            HelperNode.DrawLine(to, from, 5f, CCColor3B4B(PrimaryDark));
        }
        private void HelperNodePolygone(CCPoint[] arr)
        {
            HelperNode.Clear();
            HelperNode.DrawPolygon(arr, arr.Length, CCColor3B4B(PrimaryDark), 0f, CCColor3B4B(PrimaryDark));
        }
    }
}
