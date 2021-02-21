using UnityEngine;
using System.Collections;
using Gamelogic;
using UnityEngine.SceneManagement;

public class GameState : Singleton<GameState> 
{

	public enum States
	{
		MainMenu,
		GamePlay,
		GameOver,
		Settings,
		BirdSelection,
		PurchaseConfirmation,
		OutOfCoins,
		SwtichScene,
		Pause,
		Share,
		PlayerSelection,
		RateUs,
		AdNotAvaiableState,
		InternetNotConnected,
		VideoAdConfirmation,
		LockedPlayer,
		None
	}

	public StateMachine<States> stateMachine;
	private States previousGameState;

	AsyncOperation loading_operation = null;

	
	// Use this for initialization
	void Awake () 
	{
		stateMachine = new StateMachine<States>();

		stateMachine.AddState(States.MainMenu,null,null,null);
		stateMachine.AddState(States.GamePlay,null,null,null);
		stateMachine.AddState(States.GameOver,null,null,null);
		stateMachine.AddState(States.Settings,null,null,null);
		stateMachine.AddState(States.BirdSelection,null,null,null);
		stateMachine.AddState(States.PurchaseConfirmation,null,null,null);
		stateMachine.AddState(States.OutOfCoins,null,null,null);
		stateMachine.AddState(States.SwtichScene,null,null,null);
		stateMachine.AddState(States.Pause,null,null,null);
		stateMachine.AddState(States.Share,null,null,null);
		stateMachine.AddState(States.PlayerSelection,null,null,null);
		stateMachine.AddState(States.RateUs,null,null,null);
		stateMachine.AddState(States.AdNotAvaiableState,null,null,null);
		stateMachine.AddState(States.InternetNotConnected,null,null,null);
		stateMachine.AddState(States.VideoAdConfirmation,null,null,null);
		stateMachine.AddState(States.LockedPlayer,null,null,null);
		stateMachine.AddState(States.None,null,null,null);

	}


#region OBSERVER_BEHAVIOR

	void OnEnable()
	{
		GameManager.Instance.OnMainMenuState += onMainMenuState;
		GameManager.Instance.OnGamePlayState += onGamePlayState;
		GameManager.Instance.OnSwtchSceneState += onSwitchSceneState;
		GameManager.Instance.OnPauseState += OnPauseState;
		GameManager.Instance.OnGameOverState += OnGameOverState;

		
	}
	
	void OnDisable()
	{
		GameManager.Instance.OnMainMenuState -= onMainMenuState;
		GameManager.Instance.OnGamePlayState -= onGamePlayState;
		GameManager.Instance.OnSwtchSceneState -= onSwitchSceneState;
		GameManager.Instance.OnPauseState -= OnPauseState;
		GameManager.Instance.OnGameOverState -= OnGameOverState;
	}
	void onMainMenuState()
	{
//			if(Application.loadedLevel.Equals(1))
//				Application.LoadLevel(0);

		SharedVariables.isGamePlay = false;
		Time.timeScale = 1;


	}

	void onSwitchSceneState()
	{
		Time.timeScale = 1;
		StartCoroutine(loadLevel());
	}

	void onGamePlayState()
	{
		SharedVariables.isGamePlay = true;
		Time.timeScale = 1;

	}

	void OnPauseState()
	{
		SharedVariables.isGamePlay = false;
	}

	void OnGameOverState()
	{
		SharedVariables.isGamePlay = false;
		GameManager.instance.numberOfTimesPlayed++;
	}

#endregion


	public void setPreviousGameState()
	{
		previousGameState = stateMachine.CurrentState;
	}

	public States getPreviousGameState()
	{
		return previousGameState;
	}

	public void setGameState(States state)
	{
		stateMachine.CurrentState = state;
	}

	public States getGameState()
	{
		return stateMachine.CurrentState;
	}

	public void allowSceneActivation()
	{
		loading_operation.allowSceneActivation = true;
	}

	IEnumerator loadLevel()
	{		
		if(SceneManager.GetActiveScene().name.Equals(Constants.GAMEPLAY_SCENE))
			loading_operation = SceneManager.LoadSceneAsync(Constants.MAINMENU_SCENE);
		else
		{
			loading_operation = SceneManager.LoadSceneAsync(Constants.GAMEPLAY_SCENE);
			loading_operation.allowSceneActivation = false;
		}

		while (!loading_operation.isDone)
		{
			yield return(0);
		}

		if(SceneManager.GetActiveScene().name.Equals(Constants.MAINMENU_SCENE))
			GameManager.Instance.setGameState(States.MainMenu);

	}
	
}
