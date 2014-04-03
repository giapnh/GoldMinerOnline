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
			message.ReceiveData = true;
			
			
			PlayerInfo.MoveSpeed = cmd.getInt(ArgCode.ARG_PLAYER_SPEED_MOVE , 0);
			PlayerInfo.DropSpeed = cmd.getInt(ArgCode.ARG_PLAYER_SPEED_DROP , 0);
			PlayerInfo.DragSpeed = cmd.getInt(ArgCode.ARG_PLAYER_SPEED_DRAG , 0);
			//TODO update hook info
			TxtMoveSpeed.text = "" + PlayerInfo.MoveSpeed;
			TxtDropSpeed.text = "" + PlayerInfo.DropSpeed;
			TxtDragSpeed.text = "" + PlayerInfo.DragSpeed;
			return;
		}
	}
	
	void OnCompain(){
		controller.SendMessage("HidePanel" , ScreenManager.PN_HOME);
		controller.SendMessage("ShowPanel" , ScreenManager.PN_COMPAIN_MAP);
	}
	
	void JoinOnline(){
		controller.SendMessage("HidePanel" , ScreenManager.PN_HOME);
		controller.SendMessage("ShowPanel" , ScreenManager.PN_SEARCH_OPPONENT);
		Command cmd = new Command(CmdCode.CMD_GAME_MATCHING);
		ScreenManager.instance.Send (cmd);
	}
}
