using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
	public GameObject player;

	private Vector3 offset;

	bool zoomin = false;

	Vector3 original_offset;

	Vector3 newOffset;

	bool setout = true;

	// Use this for initialization
	void Start () 
	{
		offset = transform.position - player.transform.position;
		original_offset = offset;
		newOffset = original_offset - new Vector3 (original_offset.x - 5f, original_offset.y - 5f, original_offset.z - 5f);
	}
	
	// Update is called once per frame
	void LateUpdate () 
	{
		if(zoomin && !setout)
		{
			offset = Vector3.Lerp (offset, newOffset, Time.deltaTime);
		}
		else if(setout && !zoomin)
		{
			offset = Vector3.Lerp (offset, original_offset, Time.deltaTime);
		}

		transform.position = player.transform.position + offset;
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag(Tags.Boundary))
		{
			setout = false;
			zoomin = true;
			Invoke ("setSetOut", 2);
		}

	}


	void OnTriggerExit(Collider other)
	{
		if(other.CompareTag(Tags.Boundary))
		{
			zoomin = false;
		}
	}

	void setSetOut()
	{
		setout = true;
	}

	bool CompareVectors(Vector3 a ,Vector3 b, float angleError) 
	{
		//if they aren't the same length, don't bother checking the rest.
		if(!Mathf.Approximately(a.magnitude, b.magnitude))
			return false;
		float cosAngleError = Mathf.Cos(angleError * Mathf.Deg2Rad);
		//A value between -1 and 1 corresponding to the angle.
		var cosAngle = Vector3.Dot(a.normalized, b.normalized);
		//The dot product of normalized Vectors is equal to the cosine of the angle between them.
		//So the closer they are, the closer the value will be to 1.  Opposite Vectors will be -1
		//and orthogonal Vectors will be 0.

		if(cosAngle >= cosAngleError) {
			//If angle is greater, that means that the angle between the two vectors is less than the error allowed.
			return true;
		}
		else 
			return false;
	}
}
