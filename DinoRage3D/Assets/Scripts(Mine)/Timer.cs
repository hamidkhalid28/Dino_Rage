using UnityEngine;
using System.Collections;
using Gamelogic;
using UnityEngine.UI;

public class Timer : MonoBehaviour, IClockListener
{
	private Clock clock;

	public Text clockText;

	// Use this for initialization
	public void Start()
	{
		clock = new Clock();
		clock.AddClockListener(this);
		
		Reset();
	}
	
	// Update is called once per frame
	void Update () 
	{
		clock.Update();
	}

	public void Reset()
	{
		clock.Reset(60);
		clock.Unpause();
	}

	#region IClockListener methods
	public void OnSecondsChanged(int seconds)
	{
		clockText.text = clock.TimeInSeconds.ToString();
	}
	
	public void OnTimeOut()
	{
	}

	#endregion
}
