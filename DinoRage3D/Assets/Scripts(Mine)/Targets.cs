using UnityEngine;
using System.Collections;

public class Targets : MonoBehaviour 
{
	[HideInInspector]
	public Transform[] targets;

	// Use this for initialization
	void Awake () 
	{
		targets = new Transform[transform.childCount];

		for(int i = 0;i < targets.Length;i++)
		{
			targets[i] = transform.GetChild(i);
		}
	}
}
