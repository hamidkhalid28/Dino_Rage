using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class setBirdSpritesToDisplay : MonoBehaviour 
{
	public Button[] birds;

	// Use this for initialization
	void Start () 
	{
		birds[0] =  transform.Find("Bird1").GetComponent<Button>();
		birds[1] =  transform.Find("Bird2").GetComponent<Button>();
		birds[2] =  transform.Find("Bird3").GetComponent<Button>();
		birds[3] =  transform.Find("Bird4").GetComponent<Button>();

		//GameObject.FindGameObjectWithTag(Tags.BirdSelectionManager).GetComponent<BirdSelectionManager>().setBirdSprites(birds);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
