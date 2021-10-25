# Minimum Bounding Box (MMB)
Minimum Bounding Box is a two-components Grasshopper plugin that can be used to compute the minimum bounding box of set of geometries in a 2-dimensions and 3-dimensions spaces.

+ The 2D version is a re-implementation of the LLTS algorithms originally written by [Florian Bruggisser](<https://github.com/cansik/LongLiveTheSquare>). The advantages of this one is that it uses native Rhino3D/Grasshopper geometric types and does not require external libraries as it has its own implementation of the convex hull algorithm.
+ The 3D version is my own approach, but that relies on [MIConvexHull implementation](https://github.com/DesignEngrLab/MIConvexHull) to generate the convex hull mesh.

It was built against Rhino3D 7.10, using .NET Framework 4.8. If you get any errors about SDK version compatibility, ensure your Rhino is updated to the latest version.

## What is a minimum bounding box?

A minimum bounding box is a **rectangle (2D)** or a **box (3D)** that contains all the geometries and has the particularity of having the smallest area/volume of all the possible containing rectangles/boxes:

<img src="https://raw.githubusercontent.com/ALharchi/MinimumBoundingBox/master/MinimumBoundingBoxImage.png" width="300" height="300">

## Installation:
Get the zip file from the release section on the right, or if you prefer from [Food4Rhino Website](https://www.food4rhino.com/en/app/minimum-bounding-box?lang=en "foo").
Copy the MinimumBoundingBox folder to your Grasshopper Libraries folder. Make sure to unlock the DLL and GHA files if necessary (Right click > Properties).
  
## Usage:
2D version is available under **Curve>Util**.
3D version is available under **Mesh>Triangulation**.
  
## Compiling
If you want to compile the source yourself, you will need an additional DLL as a reference: `Petzold.Media3d`.  You can get from [here](http://www.charlespetzold.com/3D/ "here").
  
## Credit:
MMB is developed by [Ayoub Lharchi](<https://www.lharchi.com>) at the [Center for IT and Architecture](<https://royaldanishacademy.com/CITA>).
