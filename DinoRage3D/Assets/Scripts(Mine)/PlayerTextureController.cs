using UnityEngine;
using System.Collections;

public class PlayerTextureController : MonoBehaviour 
{
	SkinnedMeshRenderer texture_renderer;
	public Material[] textures;


	// Use this for initialization
	void Awake () 
	{
		texture_renderer = GetComponent<SkinnedMeshRenderer>();

	}

	public void changeTexture()
	{
		texture_renderer.sharedMaterial = textures[Prefs.currentPlayer];
	}
}
