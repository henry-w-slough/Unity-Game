SHAPES3D: HOLLOW SHAPES v1.0.0

Upgrading to URP/HDRP: From the Unity toolbar, go to Window->Rendering->Render Pipeline Converter. Select Built-in to URP/HDRP in the dropdown menu. Tick the Material Upgrade Option, and at the bottom right corner, click the "Initialize and Convert" button. This will fix the pink material issue.

Make sure to NOT rename any of the folders/files and make sure the Shapes3D folder is in "Assets/Plugins" folder. 

Complete documentation: https://drive.google.com/uc?export=download&id=15Q4hEvZdhxZ4c84DTldkewjYo1WDGl2J

Creating a basic shape:
a. Right click the hierarchy -> Shapes3D -> Any shape
b. OR in the unity toolbar, GameObject -> Shapes3D -> Any shape

S3D POLYGON:
Changing shape details:
1. Select any S3D Shape in the hierarchy.
2. Scroll to S3D Polygon component in the inspector:

Vertex: Controls the number of vertices the bottom and top faces will have. e.g. 3 for a triangle, 4 for a square, 5 for pentagon and more for a circle.

Scale: Adjusts the scale of the shape in the respective axes, respective of the pivot point.

Pivot Point: Ranges from -0.5 to 0.5 for each axis. Controls where the positioning, scaling and rotation will take place.

Taper: Ranges from 0 to infinity. Only applicable to x and z axes. If below than 0, shape will taper inward. Greater than 0 will taper outward. Set taperX = 0 and taperZ = 0 at one side to get a cone shape.

Hollow: Enable to make the shape hollow.

Additional settings for hollow:

	1. Thickness: Adjusts the thickness of the walls of the object. Set above 0 to make inner walls, and below 0 to make outer walls.
	2. Side Range: Opens up the sides of the shape. Set equal to vertex/2 to perfectly cut the object in half.
	3. Cap Thickness: Create a top/bottom cap to enclose the top/bottom part of the object.
	4. Full Top/Bottom Cap: Appear only when top/bottom cap > 0. Enable to get a full uncut top/bottom cap.

Rotate by pi radians: Rotate the object by the (input value)*PI radians. For a triangle, set to 1/6 to make it perfectly straight. For a square, set to 1/4.

S3D SPHERE:

Resolution: Determines how many vertices and triangles are created for the shape. The higher the number, the smoother the sphere.

Scale: Adjusts the scale of the shape in the respective axes, respective of the pivot point.

Pivot Point: Ranges from -0.5 to 0.5 for each axis. Controls where the positioning, scaling and rotation will take place.

Radius: Controls the radius of the sphere.

Height: Cuts of the shape based on the height. Put 1 for a full sphere, 0.5 for a semi-sphere.

Hollow: Enable to make the shape hollow.

Additional settings for hollow:

	1. Thickness: Adjusts the thickness of the walls of the object. Set above 0 to make inner walls, and below 0 to make outer walls.

Creating and Modifying Shapes from Script

It is possible to create and adjust any shapes during runtime from script. Refer the gameObject "RUNTIME SHAPE EXAMPLE" in the demo scene and play the scene out. Refer the script "S3D Demo" of the gameObject. For more information, refer the documentation.

////USEFUL TIPS////
1. Put shapes next to each other, select them in the hierarchy, right click -> create empty parent to combine them under a single parent.
2. Set pivot point Y to -0.5 for easier placement on the floor.
3. Rotation is in respect to the pivot point. So set the pivot point on where you would like the shape to rotate from.
4. All shapes come with a mesh collider. Disable the collider if you don't need it.
5. The Transform component in the inspector works as usual. You can use it to move, rotate, and scale as usual.

####################################################################
ADDITIONAL NOTES
####################################################################
1. Do not remove any native folders in the Shapes3D folder, otherwise you will most likely get errors.
2. The demo scene is completely made using S3D Shapes. Do check it out.
3. We appreciate if you could leave a kind review in the Unity Asset Store. It helps us out a lot.
4. Before leaving any bad reviews, we would appreciate it if you could email us at official.adstudios@gmail.com first for any problems or complaints. We will try to sort it out.
5. Email us at official.adstudios@gmail.com for any recommendations.

THANK YOU FOR USING HOLLOW SHAPES 3D.


