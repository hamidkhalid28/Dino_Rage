using UnityEngine;
using System.Collections;
using Gamelogic;


public class GameManager : Singleton<GameManager>
{
	public delegate void StateChangeEvent();

	public event StateChangeEvent OnMainMenuState;
	public event StateChangeEvent OnGamePlayState;
	public event StateChangeEvent OnGameOverState;
	public event StateChangeEvent OnSettingState;
	public event StateChangeEvent OnBirdSelectionState;
	public event StateChangeEvent onPurchaseConfirmationState;
	public event StateChangeEvent OnOutOfCoinsState;
	public event StateChangeEvent OnSwtchSceneState;
	public event StateChangeEvent OnPauseState;
	public event StateChangeEvent OnShareState;
	public event StateChangeEvent OnPlayerSelectionState;
	public event StateChangeEvent OnRateUsState;
	public event StateChangeEvent OnAdNotAvaiableState;
	public event StateChangeEvent OnInternetNotConnectedState;
	public event StateChangeEvent OnVideoAdConfirmationState;
	public event StateChangeEvent OnLockedPlayerState;
	public event StateChangeEvent OnNoneState;

	public GameState gameState;
	public SoundController soundState;
	public SoundManager soundManager;
	public AdsManager adsManager;
	public MenuManager menuManager;

	public LocalNotificationController notificationController;

	private static  bool created = false;

	[HideInInspector]
	public int numberOfTimesPlayed;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape)) 
			Application.Quit(); 
	}

	void Start()
	{
		if(!created)
		{
			DontDestroyOnLoad(this.gameObject);
			created = true;
			instance = this;
			Initialize();

		}
		else
		{
			Destroy(gameObject);


		}

	}

	// Initialize GameState.
	void Initialize()
	{

		if(!Prefs.isSound)
		{
			soundManager.muted = true;
		}
		else
		{
			soundManager.muted = false;
		}

		notificationController.sendNotification();

		setGameState(GameState.States.MainMenu);
	}

	public void setGameState(GameState.States state)
	{

		gameState.setGameState(state);

		if(state == GameState.States.None)
		{
			OnNoneState();
		}
		else if(state == GameState.States.GameOver)
		{
			OnGameOverState();
		}
		else if(state == GameState.States.GamePlay)
		{
			OnGamePlayState();
		}
		else if(state == GameState.States.SwtichScene)
		{
			OnSwtchSceneState();
		}
		else if(state == GameState.States.PurchaseConfirmation)
		{
			onPurchaseConfirmationState();
		}
		else if(state == GameState.States.BirdSelection)
		{
			OnBirdSelectionState();
		}
		else if(state == GameState.States.OutOfCoins)
		{
			OnOutOfCoinsState();
		}
		else if(state == GameState.States.MainMenu)
		{
			OnMainMenuState();
		}
		else if(state == GameState.States.Settings)
		{
			OnSettingState();
		}
		else if(state == GameState.States.Pause)
		{
			OnPauseState();
		}
		else if(state == GameState.States.Share)
		{
			OnShareState();
		}
		else if(state == GameState.States.PlayerSelection)
		{
			OnPlayerSelectionState();
		}
		else if(state == GameState.States.RateUs)
		{
			OnRateUsState();
		}
		else if(state == GameState.States.AdNotAvaiableState)
		{
			OnAdNotAvaiableState();
		}
		else if(state == GameState.States.InternetNotConnected)
		{
			OnInternetNotConnectedState();
		}
		else if(state == GameState.States.VideoAdConfirmation)
		{
			OnVideoAdConfirmationState();
		}
		else if(state == GameState.States.LockedPlayer)
		{
			OnLockedPlayerState();
		}
	}

	public GameState.States getGameState()
	{
		return gameState.getGameState();
		
	}
}
