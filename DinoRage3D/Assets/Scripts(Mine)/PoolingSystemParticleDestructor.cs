using UnityEngine;
using System.Collections;
using Lean;

public class PoolingSystemParticleDestructor : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		Invoke("destroyParticle",0.2f);
	}

	void destroyParticle()
	{
		LeanPool.Despawn(gameObject,0);
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
