﻿using UnityEngine;
using System.Collections;
using INet;
using IHelper;

public class Hook : MonoBehaviour {
	public GameObject onlineGameScreen;
	public static int IDLE = 0;
	public static int MOVING = 1;
	public static int ROTATING = 2;
	public static int CATCHING = 3;
	public static int HOOKING = 4;
	public static int WAITING = 5;
	public int state = WAITING;
	//item point
	public static int GOLD_POINT = 20;
	public static int BOMB_POINT = -10;
	
	public Texture[] textures;
	public Vector3 center_point;
	public int flag=-1;
	public float hook_speed;
	public Vector3 rotateDirection;
	public Vector3 initialPosition;
	Object caught_item;
	int caught_type = 0; //0: ko co gi, -1:bomb, 1: vat pham
	int item_id = 0; //0 khong co gi, 1:gold, 2: kim cuong
	//draw line
	public Color c1 = Color.black;
	public Color c2 = Color.red;
	// Use this for initialization
	void Start () {
//		state = IDLE; dat day thi luon bi goi -_-
		transform.localRotation.Set(transform.localRotation.x, transform.localRotation.y, 0, 0);
		//draw line
		LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.SetColors(c1, c2);
		lineRenderer.SetWidth(0.02f,0.02f);
		lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
	}
	
	// Update is called once per frame
	void Update () {
		//rotate
		OnlineGamePanel onlineGame_info = onlineGameScreen.gameObject.GetComponent<OnlineGamePanel> ();
		string current_player = onlineGame_info.current_player;
		float round_time = onlineGame_info.round_time;
		if(state == IDLE && current_player == PlayerInfo.Username && round_time <= 15){
			center_point = transform.parent.transform.position + new Vector3(0,-0.15f,0);
			//center_point = Vector3(0,0.1,0);
			if(transform.localRotation.z >= 0.5) flag=1;
			else if(transform.localRotation.z < -0.5) flag=-1;
			transform.RotateAround (center_point, Vector3.forward, 60 * Time.deltaTime * flag);
			
			//click
			//check current user
			//if(current_player == PlayerInfo.Username){
				if(Input.GetMouseButtonDown(0) || ( Input.touchCount >0 && Input.GetTouch(0).phase == TouchPhase.Began)){
					var mouse_pos = Input.mousePosition;
					if(mouse_pos.y<400 && transform.parent.GetComponent<Character>().state== Character.IDLE){
						initialPosition = transform.position;
						rotateDirection = transform.position - center_point;
						Vector3 velocity = rotateDirection*hook_speed;
						state = HOOKING;

						//send hook velocity to server
						Command cmd = new Command(CmdCode.CMD_PLAYER_DROP);
						cmd.addInt(ArgCode.ARG_ROOM_ID, PlayerInfo.RoomId);
						int angle_x = (int) (velocity.x*100);
						int angle_y = (int) (velocity.y*100);
						string rotation = transform.eulerAngles.x + "," + transform.eulerAngles.y + "," + transform.eulerAngles.z;
						cmd.addString(ArgCode.ARG_DROP_ROTATION, rotation);
						cmd.addInt(ArgCode.ARG_DROP_ANGLE_X, angle_x);
						cmd.addInt(ArgCode.ARG_DROP_ANGLE_Y, angle_y);
						ScreenManager.instance.Send(cmd);
						
					}	
				}
			//}
		}//
		//catching
		if(state == CATCHING){
			if(transform.position.y > initialPosition.y){
				if(caught_item){ 
					Destroy(caught_item);
					onlineGame_info.item_count --;
				}
				returnIDLE();
			}
		}
		//hooking
		if(state == HOOKING){
			if(transform.position.y <= -0.9 || transform.position.x < -1.6 || transform.position.x > 1.6) GoBack();
			if(transform.position.y > initialPosition.y) returnIDLE();
		}
		//draw line
		if (state == HOOKING || state == CATCHING) {					
			LineRenderer lineRenderer = GetComponent<LineRenderer>();
			lineRenderer.SetPosition(0, initialPosition);
			Vector3 pos = transform.position;
			lineRenderer.SetPosition(1, pos);
		}
	}

	void OnTriggerEnter(Collider col) {
		//catch gold
		if(state == HOOKING){
			if(col.gameObject.tag == "Gold") {
				renderer.material.mainTexture = textures[1];
				col.transform.position = transform.position + rotateDirection * 1.8f;
				GoBack();
				col.rigidbody.velocity = rigidbody.velocity;
				caught_item = col.gameObject;
				state = CATCHING;
				caught_type = 1;
				item_id = 1;
			}
			//catch bomb
			if(col.gameObject.tag == "Bomb") {
				GoBack();
				state = CATCHING;
				col.GetComponent<Bomb>().state = Bomb.HOOKED;
				caught_type = -1;

				OnlineGamePanel onlineGame_info = onlineGameScreen.gameObject.GetComponent<OnlineGamePanel> ();
				string current_user = onlineGame_info.current_player;
				if (current_user == PlayerInfo.Username) {
					Command cmd = new Command (CmdCode.CMD_PLAYER_DROP_RESULT);
					cmd.addInt (ArgCode.ARG_ROOM_ID, PlayerInfo.RoomId);
					cmd.addInt (ArgCode.ARG_CODE, caught_type);
					ScreenManager.instance.Send (cmd);
				}
			}
			//catch pig
			if(col.gameObject.tag == "Pig") {
				renderer.material.mainTexture = textures[1];
				GoBack();
				col.rigidbody.velocity = rigidbody.velocity;
				caught_item = col.gameObject;
				state = CATCHING;
				col.GetComponent<Pig>().state = Pig.HOOKED;
			}
		}
	}

	void GoBack(){
		rigidbody.velocity =  -rotateDirection * hook_speed;
	}
	void returnIDLE(){
		state = WAITING;
		renderer.material.mainTexture = textures[0];
		rigidbody.velocity = new Vector3(0,0,0);
		//transform.position = initialPosition;
		transform.localPosition = new Vector3 (0,-0.6f,-5f);
		transform.eulerAngles = new Vector3 (0,180,0);
		flag = -1;

		//send result if is current user
		OnlineGamePanel onlineGame_info = onlineGameScreen.gameObject.GetComponent<OnlineGamePanel> ();
		string current_user = onlineGame_info.current_player;
		if (current_user == PlayerInfo.Username) {
				Command cmd = new Command (CmdCode.CMD_PLAYER_DROP_RESULT);
				cmd.addInt (ArgCode.ARG_ROOM_ID, PlayerInfo.RoomId);
				cmd.addInt (ArgCode.ARG_CODE, caught_type);
				cmd.addInt (ArgCode.ARG_MAP_OBJ_TYPE, item_id);
				ScreenManager.instance.Send (cmd);
		}

		//check end game
		if (onlineGame_info.item_count == 0) {
			Debug.Log ("end game");
			//send end message
			Command cmd = new Command (CmdCode.CMD_GAME_FINISH);
			cmd.addInt (ArgCode.ARG_ROOM_ID, PlayerInfo.RoomId);
			ScreenManager.instance.Send (cmd);
		}
	}
}
