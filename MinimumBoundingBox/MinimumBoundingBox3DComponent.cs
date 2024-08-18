using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace MinimumBoundingBox
{
    public class MinimumBoundingBox3DComponent : GH_Component
    {

        public MinimumBoundingBox3DComponent() : base("Minimum Bounding Box 3D", "MinBB3D", "Compute the minimum bounding box of a set of geometries in 3D.", "Mesh", "Triangulation") { }
        protected override System.Drawing.Bitmap Icon { get { return Properties.Resources.iconMinBB3D; } }
        public override Guid ComponentGuid { get { return new Guid("314230da-27fa-493c-87ad-06fd8134e1d4"); } }
        public override GH_Exposure Exposure { get { return GH_Exposure.quinary; } }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGeometryParameter("Content", "C", "Geometry to contain (Supported types are: Brep, Mesh and Points).", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddBoxParameter("Box", "B", "Minimum bounding box in XYZ coordinates", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            List<dynamic> inputGeometries = new List<dynamic>();
            DA.GetDataList(0, inputGeometries);

            List<Point3d> pointsToContain = new List<Point3d>();

            bool warn = false;
            foreach (dynamic geo in inputGeometries)
            {
                if (geo == null)
                    continue;

                if (geo.Value is Brep)
                {
                    MeshingParameters para = new MeshingParameters();
                    Mesh inputM = new Mesh();
                    Mesh[] inputMs = Mesh.CreateFromBrep(geo.Value, MeshingParameters.Default);
                    
                    foreach (Mesh m in inputMs)
                    {
                        inputM.Append(m);
                    }
                    inputM.Compact();

                    foreach (Point3d p in inputM.Vertices)
                        pointsToContain.Add(p);
                }
                else if (geo.Value is Mesh)
                {
                    Mesh m = (Mesh)geo.Value;
                    foreach (Point3d p in m.Vertices)
                        pointsToContain.Add(p);
                }
                else if (geo.Value is Point3d)
                {
                    pointsToContain.Add(geo.Value);
                }
                else
                {
                    warn = true; // some non-supported geometry was passed in
                }
            }
            if (warn)
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, "One or more non-supported geometries detected - It will be ignored.");

            if (pointsToContain.Count < 3)
                return;

            double tolerance = 1e-6;
            try
            {
                tolerance = Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance;
            }
            catch { }

            if (Point3d.ArePointsCoplanar(pointsToContain, tolerance))
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Input geometries are co-planar points. You may want to consider using the 2D component!");
                return;
            }

            Mesh convexHullMesh = ConvexHull3D.GetConvexHull3D(pointsToContain);

            Box minimumBoundingBox = MinimumBoundingBox3D.GetMinimumBoundingBox3D(convexHullMesh);


            DA.SetData(0, minimumBoundingBox);
        }

    }
}