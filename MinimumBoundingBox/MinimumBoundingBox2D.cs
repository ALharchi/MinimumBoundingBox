using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace MinimumBoundingBox
{

    /// <summary>
    /// This is a reimplementation of the Long Live The Square (LLTS) algorithms by Florian Bruggisser.
    /// Available at: https://github.com/cansik/LongLiveTheSquare
    /// The goal of this reimplemetation is to support native Rhino/Grasshopper geometric types and run without dependecies and includes its own convexhull computation.
    /// </summary>
    public class MinBoundingBox2D
    {

        /// <summary>
        /// Compute the minimum bounding box in 2D
        /// </summary>
        /// <param name="inputPoints">Points to contain.</param>
        /// <param name="inputPlane">Bounding Box orientation Plane</param>
        /// <returns></returns>
        public static Rectangle3d GetMinimumBoundingBox(List<Point3d> inputPoints, Plane inputPlane)
        {
            // Check if we're getting more than 3 points
            if (inputPoints.Count < 3)
                return new Rectangle3d();

            // We transform the geometry to the XY for the purpose of calculation
            Transform toXY = Transform.PlaneToPlane(inputPlane, Plane.WorldXY);
            Transform toInputPlane;
            if (!toXY.TryGetInverse(out toInputPlane))
                throw new Exception("Something went wrong with the projection to XY plane!");


            // Converting input points to a 2D Vector and applying the transformation
            List<Vector2d> inputVectors = new List<Vector2d>();
            foreach (Point3d pt in inputPoints)
            {
                pt.Transform(toXY);
                inputVectors.Add(new Vector2d(pt.X, pt.Y));
            }

            Rectangle3d minimumRectangle = new Rectangle3d();
            double minimumAngle = 0;

            // For each edge of the convex hull
            for (int i = 0; i < inputVectors.Count; i++)
            {
                int nextIndex = i + 1;
                Vector2d current = inputVectors[i];
                Vector2d next = inputVectors[nextIndex % inputVectors.Count];


                Point3d start = new Point3d(current.X, current.Y, 0);
                Point3d end = new Point3d(next.X, next.Y, 0);
                Line segment = new Line(start, end);

                // getting limits
                double top = double.MinValue;
                double bottom = double.MaxValue;
                double left = double.MaxValue;
                double right = double.MinValue;

                // Angle to X Axis
                double angle = AngleToXAxis(segment);

                // Rotate every point and get min and max values for each direction
                foreach (Vector2d v in inputVectors)
                {
                    Vector2d rotatedVec = RotateToXAxis(v, angle);

                    top = Math.Max(top, rotatedVec.Y);
                    bottom = Math.Min(bottom, rotatedVec.Y);

                    left = Math.Min(left, rotatedVec.X);
                    right = Math.Max(right, rotatedVec.X);
                }

                // Create axis aligned bounding box
                Rectangle3d rec = new Rectangle3d(Plane.WorldXY, new Point3d(left, bottom, 0), new Point3d(right, top, 0));

                if (minimumRectangle.IsValid == false || minimumRectangle.Area > rec.Area)
                {
                    minimumRectangle = rec;
                    minimumAngle = angle;
                }
            }

            // Rotate the rectangle to fit the points
            Transform finalRotation = Transform.Rotation(-minimumAngle, Point3d.Origin);
            minimumRectangle.Transform(finalRotation);

            // Transform the rectangle to it's initial orientation
            minimumRectangle.Transform(toInputPlane);

            return minimumRectangle;
        }

        /// <summary>
        /// Calculate the angle to the X Axis.
        /// </summary>
        /// <param name="s">The segment to get the angle from.</param>
        /// <returns>The angle to the X Axis.</returns>
        static double AngleToXAxis(Line s)
        {
            Vector3d delta = s.From - s.To;
            return -Math.Atan(delta.Y / delta.X);
        }

        /// <summary>
        /// Rotate a vector bu an angle to the X Axis
        /// </summary>
        /// <param name="v">Vector to rotate</param>
        /// <param name="angle">Angle (in radian)</param>
        /// <returns>Rotated Vector</returns>
        static Vector2d RotateToXAxis(Vector2d v, double angle)
        {
            var newX = v.X * Math.Cos(angle) - v.Y * Math.Sin(angle);
            var newY = v.X * Math.Sin(angle) + v.Y * Math.Cos(angle);

            return new Vector2d(newX, newY);
        }


    }
}
