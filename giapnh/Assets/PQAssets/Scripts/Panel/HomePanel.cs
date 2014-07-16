using UnityEngine;
using System.Collections;
using INet;
public class HomePanel : MonoBehaviour {

	public UILabel TxtLevel;
	public UILabel TxtProgress;
	public UILabel TxtCup;
	
	public UILabel TxtMoveSpeed;
	public UILabel TxtDropSpeed;
	public UILabel TxtDragSpeed;
	public GameObject controller;

	//Friend
	
	public GameObject FriendPrefab;
	public GameObject friends_grid;
	int friends_num;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnEnable(){
		TxtCup.text = PlayerPrefs.GetInt ("player_cup").ToString();
		TxtLevel.text = PlayerPrefs.GetInt ("player_level").ToString ();
		int pLevelUpProgress = PlayerPrefs.GetInt("player_levelup_point");
		int pLevelUpRequire = PlayerPrefs.GetInt("player_levelup_require");
		TxtProgress.text = pLevelUpProgress.ToString () + "/" + pLevelUpRequire.ToString ();
	}

	void OnCommand(SendMessageContext message){
		//If process, return true
		if(message.InputData == null)
			return;
		Command cmd = (Command)message.InputData;
		if(cmd.code == CmdCode.CMD_PLAYER_INFO){
			int pCup = cmd.getInt(ArgCode.ARG_PLAYER_CUP, 1000);
			PlayerPrefs.SetInt("player_cup", pCup);
			int pLevel = cmd.getInt(ArgCode.ARG_PLAYER_LEVEL, 1);
			PlayerPrefs.SetInt("player_level", pLevel);
			int pLevelUpProgress = cmd.getInt(ArgCode.ARG_PLAYER_LEVEL_UP_POINT, 0);
			PlayerPrefs.SetInt("player_levelup_point", pLevelUpProgress);
			int pLevelUpRequire = cmd.getInt(ArgCode.ARG_PLAYER_LEVEL_UP_REQUIRE, 0);
			PlayerPrefs.SetInt("player_levelup_require", pLevelUpRequire);
			TxtLevel.text = ""+pLevel;
			TxtCup.text = ""+pCup;
			TxtProgress.text = "("+pLevelUpProgress+"/"+pLevelUpRequire+")";
			
			
			PlayerInfo.MoveSpeed = cmd.getInt(ArgCode.ARG_PLAYER_SPEED_MOVE , 0);
			PlayerInfo.DropSpeed = cmd.getInt(ArgCode.ARG_PLAYER_SPEED_DROP , 0);
			PlayerInfo.DragSpeed = cmd.getInt(ArgCode.ARG_PLAYER_SPEED_DRAG , 0);
			//TODO update hook info
			TxtMoveSpeed.text = "" + PlayerInfo.MoveSpeed;
			TxtDropSpeed.text = "" + PlayerInfo.DropSpeed;
			TxtDragSpeed.text = "" + PlayerInfo.DragSpeed;

			
			message.ReceiveData = true;
			return;
		}

		if (cmd.code == CmdCode.CMD_LIST_FRIEND) {
			friends_num = cmd.getInt(ArgCode.ARG_COUNT,0);		
		}
		if (cmd.code == CmdCode.CMD_FRIEND_INFO) {
			string name = cmd.getString(ArgCode.ARG_PLAYER_USERNAME,"");
			int level = cmd.getInt(ArgCode.ARG_PLAYER_LEVEL,0);
			int cup = cmd.getInt(ArgCode.ARG_PLAYER_CUP,0);
			int online_status = cmd.getInt(ArgCode.ARG_ONLINE,0);
			object friend_info = new object[level,cup,online_status];
			
			//create clone in grid then reposition
			GameObject friend = Instantiate (FriendPrefab) as GameObject;
			friend.SendMessage("set_level",level);
			friend.SendMessage("set_cup",cup);
			friend.SendMessage("set_online_status",online_status);
			friend.SendMessage("set_name", name);
			friend.transform.parent = friends_grid.transform;
			friends_num --;
			if(friends_num == 0){
				//reposition
				friends_grid.GetComponent<UIGrid>().Reposition();
			}
			
			message.ReceiveData = true;
			return;
		}

		message.ReceiveData = false;
	}
	
	void OnCompain(){
		controller.SendMessage("HidePanel" , ScreenManager.PN_HOME);
		controller.SendMessage("ShowPanel" , ScreenManager.PN_CAMPAIN_MAP);
	}
	
	void JoinOnline(){
		controller.SendMessage("HidePanel" , ScreenManager.PN_HOME);
		controller.SendMessage("ShowPanel" , ScreenManager.PN_SEARCH_OPPONENT);
		Command cmd = new Command(CmdCode.CMD_GAME_MATCHING);
		ScreenManager.instance.Send (cmd);
	}

	void OnQuit(){
		Command cmd = new Command(CmdCode.CMD_LOGOUT);
		ScreenManager.instance.Send (cmd);
		ScreenManager.instance.OnApplicationQuit ();
		Application.Quit ();
	}
	/*
	void InviteFriend(){
		if (ScreenManager.instance.mScreens [ScreenManager.PN_FRIENDS_LIST].activeSelf) {
			controller.SendMessage("HidePanel" , ScreenManager.PN_FRIENDS_LIST);
		} else{
			Command cmd = new Command(CmdCode.CMD_LIST_FRIEND);
			cmd.addInt (ArgCode.ARG_LIMIT,100);
			cmd.addInt (ArgCode.ARG_OFFSET,0);
			ScreenManager.instance.Send (cmd);
			controller.SendMessage("ShowPanel" , ScreenManager.PN_FRIENDS_LIST);
		}
	}*/
}
