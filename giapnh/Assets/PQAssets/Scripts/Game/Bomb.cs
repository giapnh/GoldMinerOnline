using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {
	public int state;
	public static int IDLE = 0;
	public static int HOOKED = 1;

	// Use this for initialization
	void Start () {
		state=IDLE;
	
	}
	
	// Update is called once per frame
	void Update () {
		if(state==HOOKED){
			Destroy(gameObject);
			//TODO destroy other object in area
			//TODO create explosion effect 
		}
	}
}
