using UnityEngine;
using System.Collections;
//THIS SCRIPT IS USED TO PREVENT THE CAMERA FROM GOING THROUGH WALLS
//ATTACH THIS SCRIPT TO A CAMERA

public class CameraCollision : MonoBehaviour {
	public bool canScroll = true; //whether or not you can zoom in and out
	public Transform focusPoint; //used as the focal rotation point, and raycast point | must be centered on the player(x and z)
	public float detectionRadius = 0.3F; //radius detection | used to prevent the camera from peering through when standing up against a wall
	public float zoomIntensiity = 1.0f; //the distance the camera will zoom per scroll
	public float cushionOffset = 3.0F; //A 'cushioned' space between the camera and obstacles
	public int maxZoomOut = 5; //used to limit distance you can zoom out, away from your character
	private int maxZoomIn; //used to limit distance you can zoom in, towards your character
	private RaycastHit hit; //used to detect objects in front of camera
	public GameObject camFollow; //monitors camera's position
	public GameObject camSpot; //camera's destination | used for zooming camera in and out
	public string[] maskedTags; //if you have an object you do not wish for the camera to detect, give the object a tag, and add the name of that tag to this list, in the inspector | the object will be ignored
	public LayerMask maskedLayers; //this is an additional mask, however uses Layers instead of Tags | remove any layer from this list, in the inspector, and every object on that layer will be ignored

	//Use this for initialization
	void Start () 
	{

		focusPoint = transform.parent.transform;

		//if camSpot does not exist
		//create camSpot
		if (camSpot == null) {
			camSpot = new GameObject ();
			camSpot.transform.name = "CameraSpot";
			camSpot.transform.parent = focusPoint;
			camSpot.transform.position = transform.position;
		}
		
		//if camFollow does not exist
		//create camFollow
		if (camFollow == null) {
			camFollow = new GameObject ();
			camFollow.transform.name = "CameraFollow";
			camFollow.transform.parent = focusPoint;
			camFollow.transform.position = focusPoint.position;
			//make sure the camFollow is looking at the camera
			camFollow.transform.LookAt (transform);
		}
		
		//Set maxZoomIn distance to not exceed starting distance from camera to focusPoint
		maxZoomIn = (int)(Vector3.Distance(transform.position, focusPoint.transform.position) / zoomIntensiity);
	}
	
	// Update is called once per frame
	void Update () {
		//If player mouse-scrolls foward
		if(Input.GetAxis("Mouse ScrollWheel") > 0) {
			if(canScroll == true) {
				//can only zoom in four intervals from camSpot's starting pos
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
				//can only zoom out four intervals from camSpot's starting pos
				if(maxZoomOut > 0) {
					//zoom camSpot out
					camSpot.transform.position = camSpot.transform.position - zoomIntensiity * -camFollow.transform.forward;
					maxZoomOut -= 1; maxZoomIn += 1;
				}
			}
		}
		
		//distance between camFollow and camSpot
		float distFromCamSpot = Vector3.Distance(camFollow.transform.position, camSpot.transform.position);
		//distance between camFollow and camera
		float distFromCamera = Vector3.Distance(camFollow.transform.position, transform.position);
		
		//ShereCast from camFollow to camSpot
		if(Physics.SphereCast(camFollow.transform.position, detectionRadius, camFollow.transform.forward, out hit, distFromCamSpot + cushionOffset, maskedLayers)) {
			//get distance betwen camFollow and hitPoint of raycast
			var distFromHit = Vector3.Distance(camFollow.transform.position, hit.point);
			//if camera is behind an object, immediately put it in front
			if(distFromHit < distFromCamera) {
				bool maskedHit = false;
				//check to see if what we hit was tagged
				foreach(string myTag in maskedTags) {
					if(hit.transform.tag == myTag) {
						maskedHit = true;
					}
				}
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
				bool maskedHit = false;
				//check to see if what we hit was tagged
				foreach(string myTag in maskedTags) {
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
}
