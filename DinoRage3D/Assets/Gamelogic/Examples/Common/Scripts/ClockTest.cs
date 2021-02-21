using UnityEngine.UI;

namespace Gamelogic.Examples
{
	public class ClockTest : GLMonoBehaviour, IClockListener
	{
		public Text clockText;
		public Text messageText;

		private Clock clock;

		public void Start()
		{
			clock = new Clock();
			clock.AddClockListener(this);

			Reset();
		}

		public void Update()
		{
			clock.Update();
		}

		public void Pause()
		{
			clock.Pause();
		}

		public void Unpause()
		{
			clock.Unpause();
		}

		public void Reset()
		{
			clock.Reset(5);
			clock.Unpause();
		}

		#region IClockListener methods
		public void OnSecondsChanged(int seconds)
		{
			clockText.text = clock.TimeInSeconds.ToString();
		}

		public void OnTimeOut()
		{
			messageText.gameObject.SetActive(true);
		}
		#endregion
	}
}