using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace MinimumBoundingBox
{
    public class MinimumBoundingBoxComponent : GH_Component
    {
        public MinimumBoundingBoxComponent() : base("Minimum Bounding Box", "MinBB", "Compute the minimum bounding box of a set of points", "Curve", "Util") { }
        protected override System.Drawing.Bitmap Icon { get { return Properties.Resources.icon; } }
        public override Guid ComponentGuid { get { return new Guid("03f1bc0e-3399-4df7-ac85-83f35af8cf6f"); } }
        public override GH_Exposure Exposure { get { return GH_Exposure.tertiary; } }


        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Points", "P", "Points to contain. (Need a convex cull input!)", GH_ParamAccess.list);
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

            if (inputPoints.Count < 3)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "You need to input 3 points at least!");
                return;
            }

            Rectangle3d outputRectangle = MinBoundingBox.GetMinimumBoundingBox(inputPoints, inputPlane);

            DA.SetData(0, outputRectangle);

        }

    }
}