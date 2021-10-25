using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace MinimumBoundingBox
{
    public class MinimumBoundingBox2DComponent : GH_Component
    {
        public MinimumBoundingBox2DComponent() : base("Minimum Bounding Box 2D", "MinBB2D", "Compute the minimum bounding box of a set of points in 2D.", "Curve", "Util") { }
        protected override System.Drawing.Bitmap Icon { get { return Properties.Resources.icon; } }
        public override Guid ComponentGuid { get { return new Guid("03f1bc0e-3399-4df7-ac85-83f35af8cf6f"); } }
        public override GH_Exposure Exposure { get { return GH_Exposure.tertiary; } }


        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Points", "P", "Points to contain.", GH_ParamAccess.list);
            pManager.AddPlaneParameter("Plane", "Plane", "Bounding box orientation plane.", GH_ParamAccess.item, Plane.WorldXY);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddRectangleParameter("Box", "B", "Minimum bounding box oriented to the input plane.", GH_ParamAccess.item);
        }


        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Point3d> inputPoints = new List<Point3d>();
            Plane inputPlane = new Plane();

            DA.GetDataList(0, inputPoints);
            DA.GetData(1, ref inputPlane);

            // Check if there is at least 3 points
            if (inputPoints.Count < 3)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "You need to input 3 points at least!");
                return;
            }

            // Check if the points are coplanar
            if (!Point3d.ArePointsCoplanar(inputPoints, Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance))
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "The input points needs to be coplanar!");
                return;
            }

            List<Point3d> convexHullPoints = ConvexHull2D.GetConvexHull(inputPoints, inputPlane);
            Rectangle3d outputRectangle = MinBoundingBox2D.GetMinimumBoundingBox(convexHullPoints, inputPlane);

            DA.SetData(0, outputRectangle);
        }

    }
}