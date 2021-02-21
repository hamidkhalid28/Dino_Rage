#pragma strict
//THIS SCRIPT IS USED TO PREVENT THE CAMERA FROM GOING THROUGH WALLS
//ATTACH THIS SCRIPT TO A CAMERA

var canScroll = true; //whether or not you can zoom in and out
var focusPoint : Transform; //used as the focal rotation point, and raycast point | must be centered on the player(x and z)
var detectionRadius : float = 0.3; //radius detection | used to prevent the camera from peering through when standing up against a wall
var zoomIntensiity : float = 1.0f; //the distance the camera will zoom per scroll
var cushionOffset : float = 3.0f; //A 'cushioned' space between the camera and obstacles
var maxZoomOut : int = 5; //used to limit distance you can zoom out, away from your character
private var maxZoomIn : int; //used to limit distance you can zoom in, towards your character | calculated on start
private var hit : RaycastHit; //used to detect objects in front of camera
var camFollow : GameObject; //monitors camera's position
var camSpot : GameObject; //camera's destination | used for positioning the camera when an obstacle blocks its view, and zooming the camera in and out
var maskedTags : String[]; //if you have an object you do not wish for the camera to detect, give the object a tag, and add the name of that tag to this list, in the inspector | the object will be ignored
var maskedLayers : LayerMask; //this is an additional mask, however uses Layers instead of Tags | remove any layer from this list, in the inspector, and every object on that layer will be ignored

function Start () {
	//if camSpot does not exist
	//create camSpot
	if(camSpot == null) {
		camSpot = new GameObject();
		camSpot.transform.name = "CameraSpot";
		camSpot.transform.parent = focusPoint;
		camSpot.transform.position = transform.position;
		camSpot.hideFlags = HideFlags.HideInHierarchy;
	}
	
	//if camFollow does not exist
	//create camFollow
	if(camFollow == null) {
		camFollow = new GameObject();
		camFollow.transform.name = "CameraFollow";
		camFollow.transform.parent = focusPoint;
		camFollow.transform.position = focusPoint.position;
		//make sure the camFollow is looking at the camera
		camFollow.transform.LookAt(camSpot.transform);
		camFollow.hideFlags = HideFlags.HideInHierarchy;
	}
	
	//Set maxZoomIn distance to not exceed starting distance from camera to focusPoint
	maxZoomIn = Vector3.Distance(transform.position, focusPoint.transform.position) / zoomIntensiity;
}
function Update () {
	//camFollow will always look at our camera
	camFollow.transform.LookAt(camSpot.transform);
	
	//If player mouse-scrolls foward
	if(Input.GetAxis("Mouse ScrollWheel") > 0) {
		if(canScroll == true) {
			if(maxZoomIn > 0) {
				//zoom camSpot in
				camSpot.transform.position = camSpot.transform.position + zoomIntensiity * -camFollow.transform.forward;
				maxZoomOut += 1; maxZoomIn -= 1;
			}
		}
	}
	//If player mouse-scrolls backward
	if(Input.GetAxis("Mouse ScrollWheel") < 0) {
		if(canScroll == true) {
			if(maxZoomOut > 0) {
				//zoom camSpot out
				camSpot.transform.position = camSpot.transform.position - zoomIntensiity * -camFollow.transform.forward;
				maxZoomOut -= 1; maxZoomIn += 1;
			}
		}
	}
	
	//distance between camFollow and camSpot
	var distFromCamSpot : float = Vector3.Distance(camFollow.transform.position, camSpot.transform.position);
	//distance between camFollow and camera
	var distFromCamera : float = Vector3.Distance(camFollow.transform.position, transform.position);
	
	//ShereCast from camFollow to camSpot
	if(Physics.SphereCast(camFollow.transform.position, detectionRadius, camFollow.transform.forward, hit, distFromCamSpot + cushionOffset, maskedLayers)) {
		//get distance betwen camFollow and hitPoint of raycast
		var distFromHit = Vector3.Distance(camFollow.transform.position, hit.point);
		//if camera is behind an object, immediately put it in front
		if(distFromHit < distFromCamera) {
			var maskedHit = false;
			//check to see if what we hit was tagged
			for(var myTag : String in maskedTags) {
				if(hit.transform.tag == myTag) {
					maskedHit = true;
				}
			}
			//if obstacle is not masked
			if(maskedHit == false) {
				//if camera is behind an object, immediately place it in front
				if(distFromCamera > cushionOffset) {
					transform.position = hit.point + cushionOffset * -camFollow.transform.forward;
				}
				else {
					//if camera is behind an object, but the player is up against a wall, place the camera on top of our FocusPoint
					//if your focusPoint is inside your character, you may end up seeing inside your character's mesh
					//you may disable your character mesh here, or move the focus point above your character's head
					transform.position = camFollow.transform.position;
				}
			}
		}
		else {
			//check to see if what we hit was tagged
			maskedHit = false;
			for(var myTag : String in maskedTags) {
				if(hit.transform.tag == myTag) {
					maskedHit = true;
				}
			}
			//if obstacle is not masked
			if(maskedHit == false) {
				//if the camera is already in front of the obstacle, yet is moving closer toward it (like walking backwards), gradually slide the camera forward
				if(distFromHit > cushionOffset) {
					transform.position = Vector3.MoveTowards(transform.position, hit.point + cushionOffset * -camFollow.transform.forward, 5 * Time.deltaTime);
				}
				else {
					//if the player is up against a wall, place the camera on our FocusPoint
					transform.position = camFollow.transform.position;
				}
			}
		}
	}
	else {
		//ease camera back to camSpot
		transform.position = Vector3.MoveTowards(transform.position, camSpot.transform.position, 5 * Time.deltaTime);
	}
}