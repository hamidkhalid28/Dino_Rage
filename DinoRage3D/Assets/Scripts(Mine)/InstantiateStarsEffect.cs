using UnityEngine;
using System.Collections;

public class InstantiateStarsEffect : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	
	public void instantiateStarEffect()
	{
		GameObject effect = (GameObject) Resources.Load(Effects.StarEffect);
		Vector3 pos = new Vector3(transform.position.x,transform.position.y,0);
		GameObject star_effect = (GameObject) Instantiate(effect,pos,effect.transform.rotation);
		star_effect.transform.parent = transform;
		Destroy(star_effect,10.0f);

	}
}
