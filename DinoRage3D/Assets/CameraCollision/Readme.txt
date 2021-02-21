/*
README

Camera Collision Script
Lylek Games

Hope you enjoy this asset! ;)

CAMERA COLLISION PREFAB:
To use the Camera Collision Script, simply drag and drop the FocusPoint prefab from the Prefabs folder onto your player. Reset the
transform position to (0, 0, 0) so the FocusPoint is centered on the player. Then adjust the y axis so the pivot point
lies above your players head. You should be good to go!

You may position the Camera anywhere, such as to the sides of your character, however, note that the script will only detect
obstacles between the camera, and the focusPoint, so the focusPoint should remain centered on your character.

Update 1.3
- Camera is no longer dependent on fixed positions, or using it's own scroll/zoom mechanics.
- You may place the camera anywhere, including to the sides of your character.
- You may use other Camera Control scripts which include angling, zooming, and other mechanics of the camera,
so long as any positioning is done to the CamSpot variable (as gameObject) of the Camera Collision Script, and not the Camera itself.

SCRIPT VARIABLE COMPONENTS:
Focus Point		 	: used as the focal rotation point, and raycast point | must be centered on the player(x and z)
Detection Radius 	: radius detection | used to prevent the camera from peeking through nearby walls
Cushion Offset		: a 'cushioned' space between the camera and obstacles
Zoom Intensity	 	: the distance the camera will zoom in and out per scroll
Max Zoom Distance	: used to limit distance you can zoom out, away from your character (maxZoomIn will be limited based on the starting position of the camera)
Cam Follow			: this is an empty gameObject, used to monitor our camera | if left null, the script will auto-genterate an empty gameObject, and assign it as a child of our FocusPoint. this object will not be visible in the inspector
Cam Spot			: camera's destination | used for positioning the camera when an obstacle blocks its view, and zooming the camera in and out | if left null, the script will auto-genterate an empty gameObject, and assign it as a child of our Camera's parent. this object will not be visible in the inspector
Masked Tags			: if you have an object you do not wish for the camera to detect, give the object a tag, and add the name of that tag to this list, in the inspector | the object will be ignored
Masked Layers		: this is an additional mask, however uses Layers instead of Tags | remove any layer from this list, in the inspector, and every object on that layer will be ignored

For any suggestions, questions, or comments please contact:
support@lylekgames.com

Hope you enjoy, and please feel free to leave a rating or review!
Thank you. =)

*******************************************************************************************

Website
http://www.lylekgames.com/

Email
support@lylekgames.com
*/
