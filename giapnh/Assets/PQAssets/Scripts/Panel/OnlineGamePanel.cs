using UnityEngine;
using System.Collections;
using INet;
using IHelper;

public class OnlineGamePanel : MonoBehaviour {
	public GameObject controller;
	public GameObject[] maps;
	public GameObject user;
	public GameObject op_user;
	public GameObject user_hook;
	public GameObject op_hook;
	public string current_player;
	public GameObject[] arrows;
	GameObject player, waiter;
	public Material[] player_mats;
	public float round_time = 0;
	public UILabel timer;
	public int item_count=0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		round_time += Time.deltaTime;
		int display_time = 15 - Mathf.FloorToInt (round_time);
		timer.text = display_time.ToString() + " s";
		if (round_time > 15) {
			timer.gameObject.SetActive(false);

			//send timeout
			if(current_player== PlayerInfo.Username){
				Command cmd = new Command (CmdCode.CMD_PLAYER_TURN_TIME_OUT);
				cmd.addInt (ArgCode.ARG_ROOM_ID, PlayerInfo.RoomId);
				ScreenManager.instance.Send (cmd);
			}
			round_time = 0;
		}
	
	}

	void OnCommand(SendMessageContext message){
		//If process, return true
		if(message.InputData == null)
			return;
		Command cmd = (Command)message.InputData;
		if(cmd.code == CmdCode.CMD_MAP_INFO){
			int map_id = cmd.getInt(ArgCode.ARG_MAP_ID, 0);
			PlayerInfo.MapID = map_id;
			maps[map_id-1].SetActive(true);
			//cal number of item(gold, diamond, stone)
			GameObject[] golds = GameObject.FindGameObjectsWithTag("Gold");
			item_count = golds.Length;

			current_player = cmd.getString(ArgCode.ARG_PLAYER_USERNAME,"");
			user.GetComponentInChildren<UILabel>().text = PlayerInfo.Username;
			op_user.GetComponentInChildren<UILabel>().text = PlayerInfo.OpUsername;
			if(current_player== PlayerInfo.Username){
				player = user;
				waiter = op_user;
			} else{
				player = op_user;
				waiter = user;
			}
			player.transform.position = arrows[0].transform.position;
			waiter.transform.position = arrows[3].transform.position;
			player.renderer.material = player_mats[0];
			waiter.renderer.material = player_mats[1];
			Hook player_hook = player.GetComponentInChildren<Hook>();
			player_hook.state = Hook.IDLE;
			Hook waiter_hook = waiter.GetComponentInChildren<Hook>();
			waiter_hook.state = Hook.WAITING;
			round_time = 0;
			timer.gameObject.SetActive(true);

			message.ReceiveData = true;
			return;
		}

		//Move
		if (cmd.code == CmdCode.CMD_PLAYER_MOVE) {
			string username = cmd.getString(ArgCode.ARG_PLAYER_USERNAME,"");
//			int from_pos = cmd.getInt(ArgCode.ARG_MOVE_FROM,0);
			int to_pos = cmd.getInt(ArgCode.ARG_MOVE_TO,0);
			Character ch;
			if(username == PlayerInfo.Username){
				ch = user.GetComponent<Character>();
			} else{
				ch = op_user.GetComponent<Character>();
			}
			ch.state = Character.MOVING;
			ch.target.x = arrows[to_pos-1].gameObject.transform.position.x;

			message.ReceiveData = true;
			return;
		}

		//Hook
		if (cmd.code == CmdCode.CMD_PLAYER_DROP) {
			string username = cmd.getString(ArgCode.ARG_PLAYER_USERNAME,"");
			GameObject hook;
			if(username == PlayerInfo.Username){
				player = user;
				hook = user_hook;
			} else{
				player = op_user;
				hook = op_hook;
			}

			//hook.gameObject.SetActive(true);
			Hook hook_info = hook.gameObject.GetComponent<Hook>();

			float angel_x = (float)cmd.getInt(ArgCode.ARG_DROP_ANGLE_X,0);
			float angel_y = (float)cmd.getInt(ArgCode.ARG_DROP_ANGLE_Y,0);
			string rotation_str = cmd.getString(ArgCode.ARG_DROP_ROTATION,"");
			string[] tab = rotation_str.Split(',');
			Vector3 rotation = new Vector3(float.Parse(tab[0]),float.Parse(tab[1]),float.Parse(tab[2]));
			Vector3 velocity = new Vector3(angel_x/100, angel_y/100, 0);
			
			Vector3 center_point = player.transform.position + new Vector3(0,-0.15f,0);
			Vector3 initialPosition = center_point + velocity/hook_info.hook_speed;

			//set
			hook.gameObject.SetActive(true);
			hook_info.state = Hook.HOOKING;
			hook.transform.eulerAngles = rotation;
			hook.transform.position = initialPosition;
			hook_info.initialPosition = initialPosition;
			hook_info.rotateDirection = velocity/hook_info.hook_speed;
			hook.rigidbody.velocity = velocity;

			timer.gameObject.SetActive(false);

			message.ReceiveData = true;
			return;
		}
		// change turn
		if (cmd.code == CmdCode.CMD_PLAYER_TURN) {
			current_player = cmd.getString(ArgCode.ARG_PLAYER_USERNAME,"");
			if(current_player== PlayerInfo.Username){
				player = user;
				waiter = op_user;
			} else{
				player = op_user;
				waiter = user;
			}
			Hook player_hook_info = player.GetComponentInChildren<Hook>();
			player_hook_info.state = Hook.IDLE;
			Hook waiter_hook_info = waiter.GetComponentInChildren<Hook>();
			waiter_hook_info.state = Hook.WAITING;
			round_time = 0;
			timer.gameObject.SetActive(true);

			message.ReceiveData = true;
			return;
		}
		// add score
		if (cmd.code == CmdCode.CMD_ADD_SCORE) {
			string username = cmd.getString(ArgCode.ARG_PLAYER_USERNAME,"");
			int score = cmd.getInt(ArgCode.ARG_SCORE,0);
			Debug.Log(username + " them " + score);
			
			message.ReceiveData = true;
			return;
		}

		// finish game
		if (cmd.code == CmdCode.CMD_GAME_FINISH) {
			int result = cmd.getInt(ArgCode.ARG_CODE,0);
			if(result == 0){
				Debug.Log ("draw");
			}else{
				string winner = cmd.getString(ArgCode.ARG_PLAYER_USERNAME,"");
				Debug.Log(winner + " win");
			}
			controller.SendMessage("HidePanel" , ScreenManager.PN_ONLINE_ONGAME);
			controller.SendMessage("ShowPanel" , ScreenManager.PN_GAME_RESULT);
			
			message.ReceiveData = true;
			return;
		}

		message.ReceiveData = false;
	}

	void Move(int to_pos){
		Command cmd = new Command(CmdCode.CMD_PLAYER_MOVE);
		cmd.addInt(ArgCode.ARG_ROOM_ID, PlayerInfo.RoomId);
//		cmd.addInt(ArgCode.ARG_MOVE_FROM, from_pos);
		cmd.addInt(ArgCode.ARG_MOVE_TO, to_pos);
		ScreenManager.instance.Send (cmd);
	}
}
