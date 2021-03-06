using UnityEngine;
using System.Collections;
using INet;
using IHelper;

public class WaitingRoomPanel : MonoBehaviour {
	public UILabel TxtReady;
	//self
	public UILabel TxtMoveSpeed;
	public UILabel TxtDropSpeed;
	public UILabel TxtDragSpeed;
	public UILabel TxtUsername;
	public UILabel TxtCup;
	public UILabel TxtLevel;
	public UILabel TxtProgress;
	public UISprite Avatar;
	public GameObject ReadySelf;
	//opponent
	public UILabel TxtOpMoveSpeed;
	public UILabel TxtOpDropSpeed;
	public UILabel TxtOpDragSpeed;
	public UILabel TxtOpUsername;
	public UILabel TxtOpLevel;
	public UILabel TxtOpCup;
	public UISprite OpAvatar;
	public GameObject ReadyOp;

	public UIButton BtnAddFriend;
	public GameObject SpeechBoxPrefab;
	public UILabel txtMessage;
	public GameObject controller;
	private float hide_speechBox_time;
	private float hide_speechBoxOP_time;
	private int ready_state = 0;
	private int room_id;
	UILabel button_label;
	// Use this for initialization
	void Start(){
		button_label = BtnAddFriend.GetComponentInChildren<UILabel>();
	}
	void OnEnable () {
		TxtCup.text = PlayerPrefs.GetInt ("player_cup").ToString();
		TxtLevel.text = PlayerPrefs.GetInt ("player_level").ToString ();
		int pLevelUpProgress = PlayerPrefs.GetInt("player_levelup_point");
		int pLevelUpRequire = PlayerPrefs.GetInt("player_levelup_require");
		TxtProgress.text = pLevelUpProgress.ToString () + "/" + pLevelUpRequire.ToString ();
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void OnCommand(SendMessageContext message){
		//If process, return true
		if(message.InputData == null)
			return;
		Command cmd = (Command)message.InputData;
		if(cmd.code == CmdCode.CMD_ROOM_INFO){
			room_id = cmd.getInt(ArgCode.ARG_ROOM_ID, 0);
			Debug.Log ("roomid "+room_id);
			PlayerInfo.RoomId = room_id;
			int cup_win = cmd.getInt(ArgCode.ARG_CUP_WIN, 0);
			int cup_lost = cmd.getInt(ArgCode.ARG_CUP_LOST, 0);
			string first_player = cmd.getString(ArgCode.ARG_PLAYER_USERNAME,"");

			/**/
			//set avatar for user choi truoc mau den
			if(first_player== PlayerInfo.Username){
//				player.renderer.material = player_mats[0];
//				waiter.renderer.material = player_mats[1];
				Avatar.spriteName = "character7";
				OpAvatar.spriteName = "character6";
			} else{
				Avatar.spriteName = "character6";
				OpAvatar.spriteName = "character7";
			}

			message.ReceiveData = true;
			return;
		}
		
		if(cmd.code == CmdCode.CMD_OPPONENT_INFO){
			PlayerInfo.OpUsername = cmd.getString(ArgCode.ARG_PLAYER_USERNAME , "");
			PlayerInfo.OpLevel = cmd.getInt(ArgCode.ARG_PLAYER_LEVEL , 0);
			PlayerInfo.OpCup = cmd.getInt(ArgCode.ARG_PLAYER_CUP , 0);
			PlayerInfo.OpMoveSpeed = cmd.getInt(ArgCode.ARG_PLAYER_SPEED_MOVE , 0);
			PlayerInfo.OpDropSpeed = cmd.getInt(ArgCode.ARG_PLAYER_SPEED_DROP , 0);
			PlayerInfo.OpDragSpeed = cmd.getInt(ArgCode.ARG_PLAYER_SPEED_DRAG , 0);
			//TODO update opponent info
			TxtOpMoveSpeed.text = "" + PlayerInfo.OpMoveSpeed;
			TxtOpDropSpeed.text = "" + PlayerInfo.OpDropSpeed;
			TxtOpDragSpeed.text = "" + PlayerInfo.OpDragSpeed;
			TxtOpUsername.text = "" + PlayerInfo.OpUsername;
			//self info
			TxtMoveSpeed.text = "" + PlayerInfo.MoveSpeed;
			TxtDropSpeed.text = "" + PlayerInfo.DropSpeed;
			TxtDragSpeed.text = "" + PlayerInfo.DragSpeed;
			TxtUsername.text = "" + PlayerInfo.Username;
			//them nut add friend neu chua la ban, accept neu da dc request
			PlayerInfo.FriendType = cmd.getInt(ArgCode.ARG_FRIEND_TYPE, 0);
			switch(PlayerInfo.FriendType){
			case 0:
				//chua la ban
				button_label.text = "Add Friend";
				break;
			case 1:
				//da la ban
				button_label.text = "Unfriend";
				break;
			case 2:
				//da gui loi moi ket ban
				button_label.text = "Cancel Request";
				break;
			case 3:
				//dang cho accept
				button_label.text = "Accept Friend";
				break;
			}
			message.ReceiveData = true;
			return;
		}
		//recv chat message
		if(cmd.code == CmdCode.CMD_PLAYER_CHAT){
			string chat_text = cmd.getString(ArgCode.ARG_MESSAGE , "");
			ReceiveChat(chat_text);
			message.ReceiveData = true;
			return;
		}
		//opponent ready
		if (cmd.code == CmdCode.CMD_GAME_READY) {
			message.ReceiveData = true;
			ReadyOp.gameObject.SetActive (true);
			return;
		}
		//all ready, game start
		if (cmd.code == CmdCode.CMD_GAME_START) {
			Debug.Log("start");
			controller.SendMessage("ShowPanel" , ScreenManager.PN_ONLINE_ONGAME);
			controller.SendMessage("HidePanel" , ScreenManager.PN_WAITING_ROOM);

			//reset for next game
			ready_state = 0;
			ReadySelf.gameObject.SetActive (false);
			ReadyOp.gameObject.SetActive (false);
			TxtReady.gameObject.transform.parent.gameObject.SetActive(true);

			message.ReceiveData = true;
			return;
		}

		if(cmd.code == CmdCode.CMD_ROOM_EXIT){
			//reset
			ready_state = 0;
			ReadySelf.gameObject.SetActive (false);
			ReadyOp.gameObject.SetActive (false);
			TxtReady.gameObject.transform.parent.gameObject.SetActive(true);
			
			controller.SendMessage("ShowPanel" , ScreenManager.PN_HOME);
			controller.SendMessage("HidePanel" , ScreenManager.PN_WAITING_ROOM);

			message.ReceiveData = true;
			return;
		}
		message.ReceiveData = false;
	}
	
	void SendChat(){
		//clone
		GameObject clone = Instantiate(SpeechBoxPrefab, new Vector3(-0.2053595F, 0.7534268F, 0), Quaternion.identity) as GameObject;
		clone.GetComponentInChildren<UILabel>().text = "" + txtMessage.text;
		//send
		Command cmd = new Command(CmdCode.CMD_PLAYER_CHAT);
		cmd.addInt(ArgCode.ARG_ROOM_ID, room_id);
		cmd.addString(ArgCode.ARG_PLAYER_USERNAME, PlayerInfo.OpUsername);
		cmd.addString(ArgCode.ARG_MESSAGE , txtMessage.text);
		ScreenManager.instance.Send (cmd);
		//reset
		txtMessage.text = "enter message";
		Destroy (clone, 5);
	}
	
	void ReceiveChat(string chat_text){
		GameObject clone = Instantiate(SpeechBoxPrefab, new Vector3(1.059338F, 0.7533067F, 0), Quaternion.identity) as GameObject;
		clone.GetComponentInChildren<UILabel>().text = "" + chat_text;
		Destroy (clone, 5);
	}
	
	void Ready(){
		Command cmd = new Command(CmdCode.CMD_GAME_READY);
		cmd.addInt(ArgCode.ARG_ROOM_ID, room_id);
		if(ready_state == 0){
			cmd.addInt(ArgCode.ARG_CODE, 1);
			TxtReady.gameObject.transform.parent.gameObject.SetActive(false);
			ready_state = 1;
		}
		//hien thi dau tich trang thai san sang
		ReadySelf.gameObject.SetActive (true);
		ScreenManager.instance.Send (cmd);
		
	}

	void OpReady(){
	
	}
	
	void OutRoom(){
		Command cmd = new Command(CmdCode.CMD_ROOM_EXIT);
		Debug.Log ("Outroom "+ room_id);
		cmd.addInt(ArgCode.ARG_ROOM_ID, room_id);
		ScreenManager.instance.Send (cmd);

//		ready_state = 0;
//		ReadySelf.gameObject.SetActive (false);
//		ReadyOp.gameObject.SetActive (false);
//		TxtReady.gameObject.transform.parent.gameObject.SetActive(true);
//		controller.SendMessage("HidePanel" , ScreenManager.PN_WAITING_ROOM);
//		controller.SendMessage("ShowPanel" , ScreenManager.PN_HOME);
	}
	void AddFriend(){
		Command cmd;
		switch(PlayerInfo.FriendType){
		case 0:
			//gui add friend
			cmd = new Command (CmdCode.CMD_ADD_FRIEND);
			cmd.addString (ArgCode.ARG_PLAYER_USERNAME, PlayerInfo.OpUsername);
			ScreenManager.instance.Send (cmd);
			break;
		case 1:
			//gui unfriend
			cmd = new Command(CmdCode.CMD_REMOVE_FRIEND);
			cmd.addString (ArgCode.ARG_PLAYER_USERNAME, PlayerInfo.OpUsername);
			ScreenManager.instance.Send (cmd);
			break;
		case 2:
			//huy ket ban
			cmd = new Command(CmdCode.CMD_CANCEL_REQUEST);
			cmd.addString (ArgCode.ARG_PLAYER_USERNAME, PlayerInfo.OpUsername);
			ScreenManager.instance.Send (cmd);
			break;
		case 3:
			//chap nhan ket ban
			cmd = new Command (CmdCode.CMD_ACCEPT_FRIEND);
			cmd.addString (ArgCode.ARG_PLAYER_USERNAME, PlayerInfo.OpUsername);
			cmd.addInt (ArgCode.ARG_CODE, 1);
			ScreenManager.instance.Send (cmd);
			break;
		}
	}
}
