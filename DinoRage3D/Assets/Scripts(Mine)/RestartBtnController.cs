using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class RestartBtnController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void onRestartBtnClick()
	{
		SceneManager.LoadScene(Constants.GAMEPLAY_SCENE);
	}
}
