using UnityEngine;
using System.Collections;

public class MainMenuCameraController : MonoBehaviour 
{
	private float jumpIter = 9.5f;
	public Transform center_point;

	public void shakeCamera()
	{
		float height = Mathf.PerlinNoise(jumpIter, 0f)*10f;

		float shakeAmt = height*1f; // the degrees to shake the camera
		float shakePeriodTime = 0.32f; // The period of each shake
		float dropOffTime = 1f; // How long it takes the shaking to settle down to nothing

		LTDescr shakeTween = LeanTween.rotateAroundLocal( gameObject, Vector3.right, shakeAmt, shakePeriodTime)
			.setEase( LeanTweenType.easeShake ) // this is a special ease that is good for shaking
			.setLoopClamp()
			.setRepeat(-1);

		// Slow the camera shake down to zero
		LeanTween.value(gameObject, shakeAmt, 0f, dropOffTime).setOnUpdate( 
			(float val)=>{
				shakeTween.setTo(Vector3.right*val);
			}
		).setEase(LeanTweenType.easeOutQuad);
	}


	void Update()
	{
		transform.RotateAround(center_point.position, Vector3.up, 20 * Time.deltaTime);

	}
}
