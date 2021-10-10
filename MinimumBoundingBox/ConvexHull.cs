using Rhino.Geometry;
using System.Collections.Generic;
using System.Linq;

namespace MinimumBoundingBox
{
    public class ConvexHull
    {
        /// <summary>
        /// Compute the convex hull of a set of points using the Monotone Chain algorithm (O(n log n) complexity)
        /// As described here: https://en.wikibooks.org/wiki/Algorithm_Implementation/Geometry/Convex_hull/Monotone_chain
        /// </summary>
        /// <param name="inputPoints">Points for convex hull solution</param>
        /// <returns>Points that constitute the polygon enclosing all the points</returns>
        public static List<Point3d> GetConvexHull(List<Point3d> inputPoints)
        {
            List<Point3d> outputPoints = new List<Point3d>();

            inputPoints.Sort((a, b) => a.X.CompareTo(b.X));

            if (inputPoints.Count <= 1)
                return inputPoints;

            List<Point3d> lower = new List<Point3d>();
            List<Point3d> upper = new List<Point3d>();

            foreach (Point3d p in inputPoints)
            {
                while (lower.Count >= 2 && CrossProduct(lower[lower.Count - 2], lower[lower.Count - 1], p) <= 0)
                    lower.RemoveAt(lower.Count - 1);
                lower.Add(p);
            }

            inputPoints.Reverse();

            foreach (Point3d p in inputPoints)
            {
                while (upper.Count >= 2 && CrossProduct(upper[upper.Count - 2], upper[upper.Count - 1], p) <= 0)
                    upper.RemoveAt(upper.Count - 1);
                upper.Add(p);
            }


            lower.RemoveAt(lower.Count - 1);
            upper.RemoveAt(upper.Count - 1);

            outputPoints = lower.Concat(upper).ToList();

            return outputPoints;
        }

        /// <summary>
        /// 2D Cross Product Utility
        /// </summary>
        /// <returns></returns>
        public static double CrossProduct(Point3d o, Point3d a, Point3d b)
        {
            return (a.X - o.X) * (b.Y - o.Y) - (a.Y - o.Y) * (b.X - o.X);
        }

    }
}
