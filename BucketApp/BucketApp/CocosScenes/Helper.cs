using System;
using System.Collections.Generic;
using System.Text;
using CocosSharp;
using System.Linq;

namespace BucketApp.CocosScenes
{
    public class Helper
    {
        public static class Colors
        {
            public static CCColor3B Primary => new CCColor3B(33, 150, 243);
            public static CCColor3B PrimaryDark => new CCColor3B(25, 118, 210);
            public static CCColor3B Accent => new CCColor3B(150, 209, 255);
            public static CCColor3B LightBackgroundColor => new CCColor3B(250, 250, 250);
            public static CCColor3B DarkBackgroundColor => new CCColor3B(192, 192, 192);
            public static CCColor3B MediumGrayTextColor => new CCColor3B(77, 77, 77);
            public static CCColor3B LightTextColor => new CCColor3B(153, 153, 153);
            public static CCColor4B CCColor3B4B(CCColor3B color)
            {
                return new CCColor4B(color.R, color.G, color.B);
            }
            public static Xamarin.Forms.Color CCColor3BXC(CCColor3B color)
            {
                return new Xamarin.Forms.Color(color.R, color.G, color.B);
            }
        }
        public static class Geometry
        {
            private const int _margine = 20;
            public static CollisionObject OnVertex(IEnumerable<CCPoint> Polygone, CCPoint PointTest)
            {
                int i = 0;
                CollisionObject ou = new CollisionObject(Polygone, PointTest);
                foreach(var point in Polygone)
                {
                    if (CCPoint.Distance(point, PointTest) < _margine)
                    {
                        ou.DetectedCollision = new List<CCPoint>() { point };
                        ou.Indexes = new List<int>() { i };
                        ou.CollisionType = CollisionType.Vertex; 
                        return ou;
                    }
                    i++;
                }
                ou.CollisionType = CollisionType.None;
                return ou;
            }
            public static CollisionObject OnEdge(IEnumerable<CCPoint> Polygone, CCPoint PointTest)
            {
                var list = Polygone.ToList();
                var ou = new CollisionObject(Polygone, PointTest);
                for (int i = 0; i < list.Count; i++)
                {
                    var a = new Box2D.Collision.Shapes.b2PolygonShape();
                    CCPoint one = list[i], two = list[(i + 1) % list.Count];
                    var dist = LineToPointDistance2D(new double[2] { (double)one.X, (double)one.Y }, new double[2] { (double)two.X, (double)two.Y }, new double[2] { (double)PointTest.X, (double)PointTest.Y }, true);    
                    if (dist < _margine)
                    {
                        ou.CollisionType = CollisionType.Edge;
                        ou.DetectedCollision = new List<CCPoint>() { one, two };
                        ou.Indexes = new List<int>() { i, (i + 1)%list.Count };
                        return ou;
                    }

                }
                ou.CollisionType = CollisionType.None;
                return ou;
            }
            public static string TestingCollision()
            {
                var a = new List<CCPoint>() { new CCPoint(0,0),
                                              new CCPoint(50,0),
                                              new CCPoint(50,100),
                                              new CCPoint(0,100)};
                string s = string.Empty;
                for(int x = 0; x < 150; x++)
                {
                    for (int y = 0; y < 150; y++)
                    {
                        var d =DetectCollision(a, new CCPoint(x, y));
                        switch (d.CollisionType)
                        {
                            case CollisionType.Edge:
                                System.Diagnostics.Debug.Write("E");
                                s += "E";
                                break;
                            case CollisionType.Vertex:
                                System.Diagnostics.Debug.Write("V");
                                s += "V";
                                break;
                            case CollisionType.Face:
                                System.Diagnostics.Debug.Write("F");
                                s += "F";
                                break;
                            case CollisionType.Polygone:
                                System.Diagnostics.Debug.Write("P");
                                s += "P";
                                break;
                            case CollisionType.None:
                                System.Diagnostics.Debug.Write("N");
                                s += "N";
                                break;
                        }
                    }
                    System.Diagnostics.Debug.WriteLine("L");
                    s += "\n";
                }
                return s;
            }
            public static CollisionObject OnFace(IEnumerable<CCPoint> Polygone, CCPoint PointTest)
            {
                var list = Polygone.ToList();
                var ou = new CollisionObject(Polygone, PointTest);
                var rand = new Random((int)(DateTime.Now.Ticks%Int32.MaxValue));
                var randPoint = new CCPoint((float)(rand.Next(int.MinValue, int.MaxValue)), (float)(rand.Next(int.MinValue, int.MaxValue)));
                for (int i = 0; i < list.Count; i++)
                {
                    CCPoint one = list[i], two = list[(i + 1) % list.Count], three = list[(i + 2) % list.Count], four = list[(i + 3) % list.Count];
                    
                    bool inside = LineIntersect(one, two, PointTest, randPoint) ^
                                  LineIntersect(two, three, PointTest, randPoint) ^
                                  LineIntersect(three, four, PointTest, randPoint) ^
                                  LineIntersect(four, one, PointTest, randPoint);
                    if (inside)
                    {
                        ou.CollisionType = CollisionType.Face;
                        ou.DetectedCollision = new List<CCPoint>() { one, two, three, four };
                        ou.Indexes = new List<int>() { i, (i + 1) % list.Count, (i + 2)% list.Count, (i + 3) %list.Count };
                        return ou;
                    }
                }
                ou.CollisionType = CollisionType.None;
                return ou;
            }
            public static CollisionObject OnPolygone(IEnumerable<CCPoint> Polygone, CCPoint PointTest)
            {
                int intes = 0;
                var list = Polygone.ToList();
                var ou = new CollisionObject(Polygone, PointTest);
                var rand = new Random(DateTime.Now.Millisecond);
                var randPoint = new CCPoint((float)(rand.Next(int.MinValue, int.MaxValue)), (float)(rand.Next(int.MinValue, int.MaxValue)));
                for (int i = 0; i < list.Count; i++)
                {
                    intes += LineIntersect(list[i], list[(i + 1) % list.Count], PointTest, randPoint) ? 1 : 0;
                }
                if (intes % 2 == 1)
                {
                    ou.CollisionType = CollisionType.Polygone;
                    ou.DetectedCollision = list;
                    var ind = new List<int>(list.Count);
                    for (int i = 0; i < list.Count; i++)
                    {
                        ind.Add(i);
                    }
                    ou.Indexes = ind;
                    return ou;
                }
                ou.CollisionType = CollisionType.None;
                return ou;
            }
            public static CollisionObject DetectCollision(IEnumerable<CCPoint> Polygome, CCPoint PointTest)
            {
                CollisionObject colis = OnVertex(Polygome, PointTest);
                if (colis.CollisionType != CollisionType.None)
                {
                    return colis;
                }
                colis = OnEdge(Polygome, PointTest);
                if (colis.CollisionType != CollisionType.None)
                {
                    return colis;
                }
                colis = OnFace(Polygome, PointTest);
                if (colis.CollisionType != CollisionType.None)
                {
                    return colis;
                }
                return OnPolygone(Polygome, PointTest);
            }
            public static bool LineIntersect(CCPoint LineAa, CCPoint LineAb, CCPoint LineBa, CCPoint LineBb)
            {
                float t = 0f, s = 0f;
                bool c = CCPoint.LineIntersect(LineAa, LineAb, LineBa, LineBb, ref s, ref t);
                //bool c = (CCPoint.CrossProduct(LineAa - LineBa, LineBb - LineBa) < 0 != CCPoint.CrossProduct(LineBb - LineAb, LineAb - LineAb) < 0);
                return c && s >= t && s <=1 && s>= 0 && t>=0;
            }
            //Compute the dot product AB . AC
            private static double DotProduct(double[] pointA, double[] pointB, double[] pointC)
            {
                double[] AB = new double[2];
                double[] BC = new double[2];
                AB[0] = pointB[0] - pointA[0];
                AB[1] = pointB[1] - pointA[1];
                BC[0] = pointC[0] - pointB[0];
                BC[1] = pointC[1] - pointB[1];
                double dot = AB[0] * BC[0] + AB[1] * BC[1];

                return dot;
            }       
            public static List<CCPoint> RoundToGrid(int GridGrain, params CCPoint[] array)
            {
                return RoundToGrid(GridGrain, array.ToList());
            }
            public static List<CCPoint> RoundToGrid(int GridGrain, List<CCPoint> array )
            {
                for (int i = 0;i<array.Count;i++)
                {
                    var point = array[i];
                    array[i] = new CCPoint((float)(Math.Round((point.X / GridGrain)) * GridGrain), (float)(Math.Round((point.Y / GridGrain)) * GridGrain));
                }
                return array;
            }
            public static short CCW(CCPoint a, CCPoint b, CCPoint c)
            {
                float t = CCPoint.CrossProduct(b - a, c - a);
                if (t < 0)
                    return -1;
                else if (t > 0)
                    return 1;
                return 0;
            }
            //Compute the cross product AB x AC
            public static double CrossProduct(double[] pointA, double[] pointB, double[] pointC)
            {
                double[] AB = new double[2];
                double[] AC = new double[2];
                AB[0] = pointB[0] - pointA[0];
                AB[1] = pointB[1] - pointA[1];
                AC[0] = pointC[0] - pointA[0];
                AC[1] = pointC[1] - pointA[1];
                double cross = AB[0] * AC[1] - AB[1] * AC[0];

                return cross;
            }

            //Compute the distance from A to B
            public static double Distance(double[] pointA, double[] pointB)
            {
                double d1 = pointA[0] - pointB[0];
                double d2 = pointA[1] - pointB[1];

                return Math.Sqrt(d1 * d1 + d2 * d2);
            }

            //Compute the distance from AB to C
            //if isSegment is true, AB is a segment, not a line.
            public static double LineToPointDistance2D(double[] pointA, double[] pointB, double[] pointC,
                bool isSegment)
            {
                double dist = CrossProduct(pointA, pointB, pointC) / Distance(pointA, pointB);
                if (isSegment)
                {
                    double dot1 = DotProduct(pointA, pointB, pointC);
                    if (dot1 > 0)
                        return Distance(pointB, pointC);

                    double dot2 = DotProduct(pointB, pointA, pointC);
                    if (dot2 > 0)
                        return Distance(pointA, pointC);
                }
                return Math.Abs(dist);
            }

        }
        public class GeometryObjects
        {
            public class Line
            {

            }
        }
        public class CollisionObject
        {
            public IEnumerable<CCPoint> OriginalPolygone { get; set; }
            public List<CCPoint> DetectedCollision { get; set; }
            public List<int> Indexes { get; set; }
            public CCPoint TestPoint { get; set; }
            public CollisionType CollisionType { get; set; }
            public CollisionObject(IEnumerable<CCPoint> OriginalPolygone, CCPoint TestPoint)
            {
                this.OriginalPolygone = OriginalPolygone;
                this.TestPoint = TestPoint;
            }
        }
        public enum CollisionType
        {
            None,
            Vertex,
            Edge,
            Face,
            Polygone

        }
    }
}
