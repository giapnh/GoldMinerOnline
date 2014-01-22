using UnityEngine;
using System.Collections;

public class Hook : MonoBehaviour {
	public static readonly int VISIBLE = -1;
	public static readonly int IDLE = 0;
	public static readonly int DROP = 1;
	public static readonly int DRAG = 2;
	
	public int State;

	// Hook indexs
	public int Vel_Drop = -10;
	public int Vel_Drag = 10;



	// Use this for initialization
	void Start () {
		State = IDLE;
	}
	
	// Update is called once per frame
	void Update () {
		if(State == DROP|| State == DRAG){
			Vector3 position = gameObject.transform.position;
			gameObject.transform.position = new Vector3(position.x + Vel_Drop * Time.deltaTime, position.y + Vel_Drop * Time.deltaTime, position.z);
			Debug.Log(gameObject.transform.position.y);
		}
		//TODO
		if(Input.GetMouseButtonDown(0)){
			Drop();
		}
	}

	public void Drop(){
		if(State == Hook.DROP){
			return;
		}
		State = DROP;
		Debug.Log("Drop");
	}
}
