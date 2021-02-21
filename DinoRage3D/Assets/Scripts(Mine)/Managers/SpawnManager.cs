using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Lean;
using Gamelogic;

public class SpawnManager : Singleton<SpawnManager>
{
	public GameObject Human_white;
	public GameObject Human_black;
	public GameObject Soldier;

	public GameObject Health_Powerup;


	Transform[] spawnPositions;
	Transform[] powerupSpawnPositions;


	int difficulty = 0;

	private List<Constants.PREY_TYPES> spawner_list;

	void Start()
	{
		spawner_list = new List<Constants.PREY_TYPES>();
	}

	public void EnQueue(Constants.PREY_TYPES temp)
	{
		spawner_list.Add(temp);

		Invoke("DeQueue",3);
	}

	public void DeQueue()
	{
		difficulty += 5;

		int num =  Random.Range(0,100);

		if(num > difficulty)
		{
			int temp =  Random.Range(0,1);

			if(temp == 0)
			{
				if(Human_white)
					LeanPool.Spawn(Human_white,spawnPositions[Random.Range(0,spawnPositions.Length)].position,Human_white.transform.rotation);

			}
			else
			{
				if(Human_black)
					LeanPool.Spawn(Human_black,spawnPositions[Random.Range(0,spawnPositions.Length)].position,Human_black.transform.rotation);
			}
		}
		else
		{
			if(Soldier)
				LeanPool.Spawn(Soldier,spawnPositions[Random.Range(0,spawnPositions.Length)].transform.position,Soldier.transform.rotation);

		}

//		if(spawner_list[0].prey_type.Equals(Constants.PREY_TYPES.Human))
//		{
//			int num =  Random.Range(0,1);
//			if(num == 0)
//			{
//				Instantiate(Human_white,spawner_list[0].prey_Gameobj.transform.position,Human_white.transform.rotation);
//
//			}
//			else
//			{
//				Instantiate(Human_black,spawner_list[0].prey_Gameobj.transform.position,Human_black.transform.rotation);
//			}
//
//		}
//		else
//		{
//			Instantiate(Soldier,spawner_list[0].prey_Gameobj.transform.position,Soldier.transform.rotation);
//		}

		spawner_list.RemoveAt(0);
	}

	public void spawnEnemies()
	{
		getSpawnPositions ();

		for(int i = 0;i < spawnPositions.Length;i++)
		{
			int num =  Random.Range(0,1);
			if(num == 0)
			{
				LeanPool.Spawn(Human_white,spawnPositions[i].position,Human_white.transform.rotation);

			}
			else
			{
				LeanPool.Spawn(Human_black,spawnPositions[i].position,Human_black.transform.rotation);
			}
		}
	}
		

	void getSpawnPositions()
	{
		GameObject spawnPositionParent = GameObject.FindGameObjectWithTag(Tags.SpawnPositions);
		spawnPositions = new Transform[spawnPositionParent.transform.childCount];

		for(int i = 0;i < spawnPositions.Length;i++)
		{
			spawnPositions [i] = spawnPositionParent.transform.GetChild (i);
		}
	}

	#region POWER_UPS

	public void InitPowerups()
	{
		getPowerUpSpawnPositions ();

		InvokeRepeating ("spawnPowerUps", 4, Random.Range (15, 20));
	}

	void spawnPowerUps()
	{
		if(GameManager.Instance.getGameState().Equals(GameState.States.GamePlay))
		LeanPool.Spawn(Health_Powerup,powerupSpawnPositions[Random.Range(0,powerupSpawnPositions.Length)].position,Quaternion.identity);
	}

	void getPowerUpSpawnPositions()
	{
		GameObject spawnPositionParent = GameObject.FindGameObjectWithTag(Tags.PowerUpSpawnPositions);
		powerupSpawnPositions = new Transform[spawnPositionParent.transform.childCount];

		for(int i = 0;i < powerupSpawnPositions.Length;i++)
		{
			powerupSpawnPositions [i] = spawnPositionParent.transform.GetChild (i);
		}
	}

	#endregion
}
