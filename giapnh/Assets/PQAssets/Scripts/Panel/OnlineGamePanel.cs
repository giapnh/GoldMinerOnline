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

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCommand(SendMessageContext message){
		//If process, return true
		if(message.InputData == null)
			return;
		Command cmd = (Command)message.InputData;
		if(cmd.code == CmdCode.CMD_MAP_INFO){
			int map_id = cmd.getInt(ArgCode.ARG_MAP_ID, 0);
			maps[map_id-1].SetActive(true);
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
			Hook hook_info = player.GetComponentInChildren<Hook>();
			hook_info.state = Hook.IDLE;

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
			//initial positon and velo


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
