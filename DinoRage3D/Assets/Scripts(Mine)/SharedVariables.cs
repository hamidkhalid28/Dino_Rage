using UnityEngine;
using System.Collections;

public class SharedVariables : MonoBehaviour 
{
	public static float speed;			//Moving Speed of player;
	public static float upwardForce;		//Force lifting the player;
	public static float rocketSpeed;			//Rocket Speed of player;
	public static float mass;			// Stores initial value of mass of player;
	public static float gravityScale;	// Stores initial value of gravityScale of player;	
	public static float linearDrag;		// Stores initial value of Linear Drag of player;	
	public static float angularDrag;	// Stores initial value of Angular Drag of player;	
	public static bool isGamePlay = false;		//Indicates if game is playing or not. True on destruction of tutorial.
	public static bool isPlayerDead;			//Indicates if player is dead or not.


}
