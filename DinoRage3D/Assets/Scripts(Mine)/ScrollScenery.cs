using UnityEngine;
using System.Collections;

public class ScrollScenery : MonoBehaviour 
{
	private Transform cameraTransform;
	private float spriteWidth;	// Use this for initialization

	void Start () 
	{
		//1
		cameraTransform = Camera.main.transform;
		//2
		SpriteRenderer spriteRenderer = GetComponent<Renderer>() as SpriteRenderer;
		spriteWidth = spriteRenderer.sprite.bounds.size.x;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!SharedVariables.isPlayerDead && SharedVariables.isGamePlay)
		{
		transform.position = new Vector3(transform.position.x - (4 * Time.smoothDeltaTime),transform.position.y,transform.position.z);

			if( (transform.position.x + spriteWidth) < cameraTransform.position.x) {
				Vector3 newPos = transform.position;
				newPos.x += 2.0f * spriteWidth; 
				transform.position = newPos;
			}
		}
	}
}
