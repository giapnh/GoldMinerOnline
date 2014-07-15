﻿using UnityEngine;
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
		
		
		if(cmd.code == CmdCode.CMD_GAME_MATCHING){
			int result = cmd.getInt(ArgCode.ARG_CODE, 0);
			if(result==1){
				//tim thay
				for(int i = 1; i < ScreenManager.instance.mScreens.Length; i++){
					ScreenManager.instance.mScreens[i].SetActive(false);
				}
				ScreenManager.instance.ShowPanel(ScreenManager.PN_WAITING_ROOM);
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
		ScreenManager.instance.OnApplicationQuit ();
		Application.Quit ();
	}

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
	}
}
