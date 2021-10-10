# Minimum Bounding Box (MMB)
Minimum Bounding Box is a single component Grasshopper plugin that can be used to compute the minimum bounding box of set of points in a 2-dimensional space.

It is a re-implementation of the LLTS algorithms originally written by [Florian Bruggisser](<https://github.com/cansik/LongLiveTheSquare>). The advantages of this one is that it uses native Rhino3D/Grasshopper geometric types and does not require external libraries as it has its own implementation of the convex hull algorithm.

It was built against Rhino3D 7.10, using .NET Framework 4.8. If you get any errors about SDK version compatibility, ensure your Rhino is updated to the latest version.

## What is a minimum bounding box?

A minimum bounding box is a rectangle that contains all the points and has the particularity of having the smallest area of all the possible containing rectangles:

<img src="https://raw.githubusercontent.com/ALharchi/MinimumBoundingBox/master/MinimumBoundingBoxImage.png" width="300" height="300">

## Installation:
Just copy the gha file to your Grasshopper Libraries folder. Make sure to unlock it if necessary (Right click > Properties).
  
## Usage:
The component is available under Curve>Util.
  
## Credit:
MMB is developed by [Ayoub Lharchi](<https://www.lharchi.com>) at the [Center for IT and Architecture](<https://kadk.dk/en/CITA>).
