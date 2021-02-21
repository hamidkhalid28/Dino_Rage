using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gamelogic.Examples
{
	public class ResponseCurveTest:GLMonoBehaviour
	{
		public GameObject pathPrefab;
		public List<GameObject> nodes;

		public void Start()
		{
			var curve = MakeCurve();

			MakePath(curve);
		}

		private void MakePath(ResponseCurveVector3 curve)
		{
			for (float t = 0; t <= 1; t += 0.02f)
			{
				var newPosition = curve[t];

				var obj = Instantiate(pathPrefab);
				obj.transform.position = newPosition;
			}
		}

		private ResponseCurveVector3 MakeCurve()
		{
			int nodeCount = nodes.Count;
			var inputs = new List<float>();

			for (int i = 0; i < nodes.Count; i++)
			{
				inputs.Add(i/(float) (nodeCount - 1));
			}

			var outputs = nodes.Select(node => node.transform.position);
			var curve = new ResponseCurveVector3(inputs, outputs);
			return curve;
		}
	}
}