using UnityEngine;
using System.Collections;

public class ResizeSpriteToScreen : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		resizeSprite();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void resizeSprite() 
	{
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		if (sr == null) return;
		
		transform.localScale = new Vector3(1,1,1);
		
		float width = sr.sprite.bounds.size.x;
		float height = sr.sprite.bounds.size.y;
		
		float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
		float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

		float localScale_x = transform.localScale.x;
		float localScale_y = transform.localScale.y;
		
		localScale_x = worldScreenWidth / width;
		localScale_y = worldScreenHeight / height;

		transform.localScale = new Vector3(localScale_x,localScale_y,transform.localScale.z);
	}
}
