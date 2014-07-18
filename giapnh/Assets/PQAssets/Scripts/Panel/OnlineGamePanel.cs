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

	int user_score;
	int op_score;
	public UILabel user_score_lable;
	public UILabel op_score_label;

	public GameObject[] buff_item;
	string used_buff_item = "";
	GameObject map;

	// Use this for initialization

	void OnEnable () {
		user_score = 0;
		op_score = 0;
		op_score_label.text = PlayerInfo.OpUsername + ": 0";
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
			//thay vi hien map thi load map tu prefab
			//maps[map_id-1].SetActive(true);
			map = Instantiate(maps[map_id-1], new Vector3(0, 0, 0.416687f), Quaternion.identity) as GameObject;
			//calculate number of item(gold, diamond, stone)
			string[] items_list = new string[3]{ "Gold", "Diamond", "Stone"};
			foreach(string item in items_list){
				GameObject[] items = GameObject.FindGameObjectsWithTag(item);
				item_count += items.Length;	
			}

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
			hook.transform.eulerAngles = rotation;
			hook.transform.position = initialPosition;
			hook_info.initialPosition = initialPosition;
			hook_info.rotateDirection = velocity/hook_info.hook_speed;
			hook.rigidbody.velocity = velocity;
			hook_info.state = Hook.HOOKING;

			timer.gameObject.SetActive(false);
			//TODO item used
			used_buff_item = cmd.getString(ArgCode.ARG_ITEM_USED, "");


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

		// finish game
		if (cmd.code == CmdCode.CMD_GAME_FINISH) {
			Debug.Log("nhan dc finish");
			int result = cmd.getInt(ArgCode.ARG_CODE,0);
			if(result == 0){
				PlayerInfo.Winner = "";

			}else{
				string winner = cmd.getString(ArgCode.ARG_PLAYER_USERNAME,"");
				PlayerInfo.Winner = winner;
			}
			controller.SendMessage("HidePanel" , ScreenManager.PN_ONLINE_ONGAME);
			controller.SendMessage("ShowPanel" , ScreenManager.PN_GAME_RESULT);
			//TODO reset game;
			GameObject map = GameObject.Find("Map"+PlayerInfo.MapID+"(Clone)");
			Destroy(map);
			item_count = 0;
			user_score = 0;

			message.ReceiveData = true;
			return;
		}

		//add score
		if (cmd.code == CmdCode.CMD_ADD_SCORE) {
			string username = cmd.getString(ArgCode.ARG_PLAYER_USERNAME,"");
			int score = cmd.getInt(ArgCode.ARG_SCORE, 0);
			if(username == PlayerInfo.Username){
				user_score += score;
				if(user_score<0) user_score = 0;
				user_score_lable.text = "You: "+ user_score;
			} else{
				op_score += score;
				if(op_score<0) op_score = 0;
				op_score_label.text = username + ": " + op_score;
			}
			Debug.Log(item_count);
			//check end game
			if (item_count == 0) {
				Debug.Log ("end game");
				Debug.Log(current_player);
				Debug.Log(PlayerInfo.Username);
			}
//			if(current_player==PlayerInfo.Username){
//				Debug.Log("chua vao day");
//				check_end_game();

//			}

			message.ReceiveData = true;
			return;
		}

		if (cmd.code == CmdCode.CMD_ITEM_APPEAR) {
			int item_type = cmd.getInt(ArgCode.ARG_MAP_OBJ_TYPE,0);
			int pos_x = cmd.getInt(ArgCode.ARG_POSITION_X,0);
			int pos_y = cmd.getInt(ArgCode.ARG_POSITION_Y,0);
			int time_life = cmd.getInt(ArgCode.ARG_ITEM_TIME_LIFE,0);
			GameObject buff = buff_item[0];
				if(item_type == 10){
				buff = buff_item[0];
			} else if (item_type == 11){
				buff = buff_item[1];
			}
			float x = (float)pos_x/10;
			float y = (float)pos_y/10;
			Vector3 pos = new Vector3(x, y, 0.4167f);
			GameObject obj = Instantiate(buff, pos, Quaternion.identity) as GameObject;
			obj.transform.parent = map.transform;
//			Destroy(obj, time_life);

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
	
	//check end game
	public void check_end_game(){
		StartCoroutine (wait ());
		if (item_count == 0) {
			//send end message
			Command cmd = new Command (CmdCode.CMD_GAME_FINISH);
			cmd.addInt (ArgCode.ARG_ROOM_ID, PlayerInfo.RoomId);
			ScreenManager.instance.Send (cmd);
		}
	}
	public IEnumerator wait(){
		yield return new WaitForSeconds(0.5f);
	}
}
