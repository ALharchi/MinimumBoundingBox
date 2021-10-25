using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;
using MIConvexHull;
using Petzold.Media3D;
using System.Windows.Media.Media3D;
using System.Windows.Media;



namespace MinimumBoundingBox
{
    public class ConvexHull3D
    {

        /// <summary>
        /// Compute the convex hull of a set of points in 3D.
        /// </summary>
        /// <param name="inputPoints">Points for convex hull solution</param>
        /// <returns>Mesh that constitue the convex hull containing all the input points.</returns>
        public static Mesh GetConvexHull3D(List<Point3d> inputPoints)
        {
            Mesh outputMesh = new Mesh();

            List<Vertex> vertices;
            List<Vertex> convexHullVertices;
            List<Face> convexHullFaces;


            vertices = new List<Vertex>();
            //var r = new Random();


            foreach (Point3d pt in inputPoints)
            {
                var vi = new Vertex(pt.X, pt.Y, pt.Z);
                vertices.Add(vi);
            }

            var convexHull = ConvexHull.Create<Vertex, Face>(vertices);
            convexHullVertices = convexHull.Result.Points.ToList();
            convexHullFaces = convexHull.Result.Faces.ToList();


            // We need this to find the point index
            List<string> indexPoints = new List<string>();

            foreach (Vertex v in convexHullVertices)
            {
                outputMesh.Vertices.Add(v.Center.X, v.Center.Y, v.Center.Z);
                indexPoints.Add(new Point3d(v.Center.X, v.Center.Y, v.Center.Z).ToString());
            }

            foreach (Face f in convexHullFaces)
            {
                // Let's find the fin the index of the vertices
                int vIndex0 = indexPoints.IndexOf(f.Vertices[0].Center.X + "," + f.Vertices[0].Center.Y + "," + f.Vertices[0].Center.Z);
                int vIndex1 = indexPoints.IndexOf(f.Vertices[1].Center.X + "," + f.Vertices[1].Center.Y + "," + f.Vertices[1].Center.Z);
                int vIndex2 = indexPoints.IndexOf(f.Vertices[2].Center.X + "," + f.Vertices[2].Center.Y + "," + f.Vertices[2].Center.Z);
                outputMesh.Faces.AddFace(vIndex0, vIndex1, vIndex2);
            }


            outputMesh.Normals.ComputeNormals();
            outputMesh.Compact();

            return outputMesh;
        }


    }


    public class Vertex : ModelVisual3D, IVertex
    {
        static readonly Material material = new DiffuseMaterial(Brushes.Black);
        static readonly SphereMesh mesh = new SphereMesh { Slices = 6, Stacks = 4, Radius = 0.5 };

        static readonly Material hullMaterial = new DiffuseMaterial(Brushes.Yellow);
        static readonly SphereMesh hullMesh = new SphereMesh { Slices = 6, Stacks = 4, Radius = 1.0 };

        public Vertex(double x, double y, double z, bool isHull = false)
        {
            Content = new GeometryModel3D
            {
                Geometry = isHull ? hullMesh.Geometry : mesh.Geometry,
                Material = isHull ? hullMaterial : material,
                Transform = new TranslateTransform3D(x, y, z)
            };
            Position = new double[] { x, y, z };
        }

        public Vertex AsHullVertex()
        {
            return new Vertex(Position[0], Position[1], Position[2], true);
        }

        public Point3D Center { get { return new Point3D(Position[0], Position[1], Position[2]); } }

        public double[] Position
        {
            get;
            set;
        }
    }

    public class Face : ConvexFace<Vertex, Face>
    { }

}
