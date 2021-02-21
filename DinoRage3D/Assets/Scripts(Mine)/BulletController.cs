using UnityEngine;
using System.Collections;
using Lean;
using YounGenTech.HealthScript;

public class BulletController : MonoBehaviour 
{
	public float speed;
	public float damage;


	void Start () 
	{
		Invoke("DeSpawn",3);
	}

	void Update()
	{
		transform.Translate(Vector3.forward * speed * Time.deltaTime);
	}

	void OnCollisionEnter(Collision other)
	{
		if(other.transform.CompareTag(Tags.player))
		{
			other.gameObject.GetComponent<Health>().Damage(new HealthEvent(gameObject,damage));

			GameObject.FindObjectOfType<EffectsController>().playBulletHitEffect(other.contacts[0].point);
			DeSpawn();

		}
	}

	void DeSpawn()
	{
		LeanPool.Despawn(gameObject);
	}

}
