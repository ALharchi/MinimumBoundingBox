using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;
using Rhino.Geometry.Collections;


namespace MinimumBoundingBox
{
    class MinimumBoundingBox3D
    {

        /// <summary>
        /// Compute the minimum bounding box of an input Mesh
        /// </summary>
        /// <param name="inputMesh">Mesh to contain</param>
        /// <returns>Minimum Bounding Box</returns>
        public static Box GetMinimumBoundingBox3D(Mesh inputMesh) 
        {

            // Note: The inputMesh is already a convex hull

            MeshFaceList faces = inputMesh.Faces;

            List<Plane> planes = new List<Plane>();

            // Get all the possible planes
            foreach (MeshFace face in faces)
            {
                List<Point3d> pts = new List<Point3d>();
                pts.Add(inputMesh.Vertices[face.A]);
                pts.Add(inputMesh.Vertices[face.B]);
                pts.Add(inputMesh.Vertices[face.C]);
                Plane tempPlane = new Plane();
                if (Plane.FitPlaneToPoints(pts, out tempPlane) == PlaneFitResult.Success)
                    planes.Add(tempPlane);
            }

            List<Box> orientedBoxes = new List<Box>();

            foreach (Plane pln in planes)
            {
                Box bb = new Box();
                inputMesh.GetBoundingBox(pln, out bb);
                orientedBoxes.Add(bb);
            }

            // Sort the bounding boxes by volume
            List<Box> SortedBoudningBoxes = orientedBoxes.OrderBy(o => o.Volume).ToList();

            // Return the smallest one
            return SortedBoudningBoxes[0];
        }
    }
}
