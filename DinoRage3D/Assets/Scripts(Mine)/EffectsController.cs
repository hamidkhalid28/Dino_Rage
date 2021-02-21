using UnityEngine;
using System.Collections;
using Lean;

public class EffectsController : MonoBehaviour 
{
	public ParticleSystem prey_hit_effect;
	public ParticleSystem leaves_falling_effect;
	public ParticleSystem bullet_spark_effect;
	public ParticleSystem bullet_hit_effect;


	void Start()
	{
		prey_hit_effect = LeanPool.Spawn(prey_hit_effect,Vector3.zero,Quaternion.identity) as ParticleSystem;
		leaves_falling_effect = LeanPool.Spawn(leaves_falling_effect,Vector3.zero,Quaternion.identity) as ParticleSystem;
		bullet_spark_effect = LeanPool.Spawn(bullet_spark_effect,Vector3.zero,Quaternion.identity) as ParticleSystem;

	}

	public void playPreyHitEffect(Transform reference)
	{
		prey_hit_effect.transform.position = reference.transform.position;

		prey_hit_effect.Play();
	}

	public void playLeavesFallingEffect(Transform reference)
	{
		leaves_falling_effect.transform.position = reference.transform.position;

		leaves_falling_effect.Play();
	}

	public void playBulletSparkEffect(Transform reference)
	{
		bullet_spark_effect.transform.position = reference.transform.position;

		bullet_spark_effect.Play();
	}

	public void playBulletHitEffect(Vector3 reference)
	{
		GameObject effect = LeanPool.Spawn(bullet_hit_effect.gameObject,reference,Quaternion.identity);

		LeanPool.Despawn(effect,0.5f);
	}
}
