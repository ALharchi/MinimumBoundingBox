using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MinimumBoundingBox
{
    public class ConvexHull2D
    {

        /// <summary>
        /// Compute the convex hull of a set of points using the Monotone Chain algorithm (O(n log n) complexity)
        /// As described here: https://en.wikibooks.org/wiki/Algorithm_Implementation/Geometry/Convex_hull/Monotone_chain
        /// </summary>
        /// <param name="inputPoints">Points for convex hull solution</param>
        /// <param name="inputPlane">Working plane for the convex hull algorithm</param>
        /// <returns>Points that constitute the polygon enclosing all the points</returns>
        public static List<Point3d> GetConvexHull(List<Point3d> inputPoints, Plane inputPlane, double tolerance)
        {
            // Remove any duplicated points - with the document absolute tolerance
            inputPoints = RemoveDuplicatePoints(inputPoints, tolerance);

            Transform toXY = Transform.PlaneToPlane(inputPlane, Plane.WorldXY);
            Transform toInputPlane = Transform.PlaneToPlane(Plane.WorldXY, inputPlane);

            List<Point3d> workingPoints = new List<Point3d>();

            // Orient to XY
            foreach (Point3d pt in inputPoints)
            {
                Point3d newPt = new Point3d(pt);
                newPt.Transform(toXY);
                workingPoints.Add(newPt);
            }

            // Sort points
            workingPoints.Sort((a, b) => a.X.CompareTo(b.X));

            if (workingPoints.Count <= 1)
                return inputPoints;

            List<Point3d> lower = new List<Point3d>();
            List<Point3d> upper = new List<Point3d>();

            foreach (Point3d p in workingPoints)
            {
                while (lower.Count >= 2 && CrossProduct(lower[lower.Count - 2], lower[lower.Count - 1], p) <= 0)
                    lower.RemoveAt(lower.Count - 1);
                lower.Add(p);
            }

            workingPoints.Reverse();

            foreach (Point3d p in workingPoints)
            {
                while (upper.Count >= 2 && CrossProduct(upper[upper.Count - 2], upper[upper.Count - 1], p) <= 0)
                    upper.RemoveAt(upper.Count - 1);
                upper.Add(p);
            }


            lower.RemoveAt(lower.Count - 1);
            upper.RemoveAt(upper.Count - 1);

            List<Point3d> combinedPoints = lower.Concat(upper).ToList();

            List<Point3d> orientedPts = new List<Point3d>();

            foreach (Point3d pt in combinedPoints)
            {
                Point3d orientedPt = new Point3d(pt);
                orientedPt.Transform(toInputPlane);
                orientedPts.Add(orientedPt);
            }

            return orientedPts;
        }

        /// <summary>
        /// 2D Cross Product Utility
        /// </summary>
        /// <returns></returns>
        public static double CrossProduct(Point3d o, Point3d a, Point3d b)
        {
            return (a.X - o.X) * (b.Y - o.Y) - (a.Y - o.Y) * (b.X - o.X);
        }

        /// <summary>
        /// Remove all duplicate points in a list of points
        /// </summary>
        /// <param name="inputPoints">Points to clean</param>
        /// <param name="tolerance">tolerance (in all axes)</param>
        /// <returns>Unique points</returns>
        public static List<Point3d> RemoveDuplicatePoints(List<Point3d> inputPoints, double tolerance)
        {
            List<Point3d> outputPoints = new List<Point3d>();

            foreach (Point3d pt in inputPoints)
            {
                bool addMe = true;

                foreach (Point3d exPt in outputPoints)
                {
                    if ((pt - exPt).Length < tolerance)
                        addMe = false;
                }
                if (addMe)
                    outputPoints.Add(pt);
            }

            return outputPoints;
        }

    }
}
