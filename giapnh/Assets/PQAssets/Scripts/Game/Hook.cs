﻿using UnityEngine;
using System.Collections;
using INet;
using IHelper;
using System.Collections.Generic;

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
	GameObject caught_item;
	int caught_type = 0; //0: ko co gi, -1:bomb, 1: vat pham
	int item_id = 0; //0 khong co gi, 1:gold, 8: kim cuong
	//draw line
	public Color c1 = Color.black;
	public Color c2 = Color.red;
	// Use this for initialization
	string current_player;
	OnlineGamePanel onlineGame_info;
	LineRenderer lineRenderer;
//	int[] remaining_buff_item;
//	int[] used_buff_item;
//	List<int> remaining_buff_item;
	public string used_buff_item = "";
	public GameObject ItemX2;
	string tmp_item;
	
	string[] items_list = new string[4]{ "Gold", "Diamond", "Stone", "Buff"};

	void OnEnable(){
//		remaining_buff_item [11] = 0;
	}
	void Start () {
//		state = IDLE; dat day thi luon bi goi -_-
				transform.localRotation.Set (transform.localRotation.x, transform.localRotation.y, 0, 0);
				//draw line
				lineRenderer = gameObject.AddComponent<LineRenderer> ();
				lineRenderer.SetColors (c1, c2);
				lineRenderer.SetWidth (0.02f, 0.02f);
				lineRenderer.material = new Material (Shader.Find ("Particles/Additive"));
		}
	
	// Update is called once per frame
	void Update () {
		//rotate
		onlineGame_info = onlineGameScreen.gameObject.GetComponent<OnlineGamePanel> ();
		current_player = onlineGame_info.current_player;
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
				if(mouse_pos.y < 370 && transform.parent.GetComponent<Character>().state== Character.IDLE){
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
					//TODO fix if use item
//					used_buff_item.ForEach();
					cmd.addString(ArgCode.ARG_ITEM_USED, used_buff_item);
					ScreenManager.instance.Send(cmd);
					tmp_item = used_buff_item;
					used_buff_item = "";

				}	
			}
			//}
		}

		//draw line
		if (state == HOOKING || state == CATCHING) {					
			lineRenderer = GetComponent<LineRenderer>();
			lineRenderer.SetVertexCount(2);
			lineRenderer.SetPosition(0, initialPosition);
			Vector3 pos = transform.position;
			lineRenderer.SetPosition(1, pos);
		}

		//catching
		if(state == CATCHING){
			if(transform.position.y > initialPosition.y){
				if(caught_item){
					if(caught_item.tag!="Buff"){
						onlineGame_info.item_count --;
						Debug.Log("con lai "+ onlineGame_info.item_count + " item");
					} else{
						Item item_info = caught_item.GetComponent<Item>();
						item_id = item_info.item_id;
						if(item_id == 10){
							if(used_buff_item == "")
								used_buff_item = item_id.ToString();
							else
								used_buff_item = used_buff_item + ";" + item_id.ToString();
						} else if(item_id == 11){
							//TODO them vao list remaining item
							Debug.Log("bat dc x2");
							if(current_player==PlayerInfo.Username)
								ItemX2.SendMessage("Increase", item_id);
							//chuyen remaining item ve dang list -key
						}
					}
					Destroy(caught_item);
					Debug.Log ("so item "+ onlineGame_info.item_count);
				}
				Debug.Log("return vi y > initial y khi dang catching");
				returnIDLE();
			}
		}

		//hooking
		if(state == HOOKING){
			if(transform.position.y <= -0.9 || transform.position.x < -1.6 || transform.position.x > 1.6){
				caught_type = 0;
				item_id = 0;
				Debug.Log("goback vi gap vien");
				GoBack();
			}
			if(transform.position.y > initialPosition.y){
				returnIDLE();
				Debug.Log("return vi y > initial y khi dang hooking");
			}
		}
	}

	void OnTriggerEnter(Collider col) {
		if(state == HOOKING){
			//catch item
			if(System.Array.IndexOf(items_list,col.gameObject.tag)!=-1){
				renderer.material.mainTexture = textures[1];
				col.transform.position = transform.position + rotateDirection * 1.8f;
				col.rigidbody.velocity = rigidbody.velocity;
				if(col.gameObject.tag=="Buff"){
					caught_type = 2;
					col.gameObject.GetComponent<Item>().state = 0;
				}
				else 
					caught_type = 1;

				Item item_info = col.GetComponent<Item>();
				item_id = item_info.item_id;
				
				GoBack();
				rigidbody.velocity = new Vector3(rigidbody.velocity.x*item_info.speed_rate, rigidbody.velocity.y*item_info.speed_rate,0);
				col.rigidbody.velocity = rigidbody.velocity;

				caught_item = col.gameObject;
				state = CATCHING;
			}

			//catch bomb
			if(col.gameObject.tag == "Bomb") {
				state = CATCHING;
				GoBack();
				col.GetComponent<Bomb>().state = Bomb.HOOKED;
				caught_type = 1;
				item_id = -1;
			}
		}
	}



	void GoBack(){
		Debug.Log ("da goi go back");
		rigidbody.velocity =  -rotateDirection * hook_speed;
	}
	void returnIDLE(){
		Debug.Log ("da return idle");
		state = WAITING;
		renderer.material.mainTexture = textures[0];
		rigidbody.velocity = new Vector3(0,0,0);
		//transform.position = initialPosition;
		transform.localPosition = new Vector3 (0,-0.6f,-5f);
		transform.eulerAngles = new Vector3 (0,180,0);
		flag = -1;
		lineRenderer.SetVertexCount (0);

		//send result if is current user
		//OnlineGamePanel onlineGame_info = onlineGameScreen.gameObject.GetComponent<OnlineGamePanel> ();
		//string current_user = onlineGame_info.current_player;
		if (current_player == PlayerInfo.Username) {
			Command cmd = new Command (CmdCode.CMD_PLAYER_DROP_RESULT);
			cmd.addInt (ArgCode.ARG_ROOM_ID, PlayerInfo.RoomId);
			cmd.addInt (ArgCode.ARG_CODE, caught_type);
			cmd.addInt (ArgCode.ARG_MAP_OBJ_TYPE, item_id);
			//TODO them item used
			cmd.addString(ArgCode.ARG_ITEM_USED, tmp_item);
			ScreenManager.instance.Send (cmd);
//			used_buff_item = ""; loi o dayyyyyyyy
			onlineGameScreen.GetComponent<OnlineGamePanel>().check_end_game();
		}	
	}
}
