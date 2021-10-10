using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace MinimumBoundingBox
{
    public class MinimumBoundingBoxInfo : GH_AssemblyInfo
    {
        public override string Name { get { return "MinimumBoundingBox"; } }
        public override Bitmap Icon { get { return null; } }
        public override string Description { get { return "Compute the minimum bounding box from a set of points in a 2D space."; } }
        public override Guid Id { get { return new Guid("91fde6ca-2478-44f2-95c4-e43f31434b30"); } }
        public override string AuthorName { get { return "Ayoub Lharchi"; } }
        public override string AuthorContact { get { return "alha@kglakademi.dk"; } }
    }
}
