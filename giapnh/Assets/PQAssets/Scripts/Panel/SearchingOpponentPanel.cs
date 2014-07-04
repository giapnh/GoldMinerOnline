using UnityEngine;
using System.Collections;
using INet;
using IHelper;

public class SearchingOpponentPanel : MonoBehaviour {
	public GameObject controller;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//TODO after 10s auto back to home screen
	}
	
	void OnCommand(SendMessageContext message){
		//If process, return true
		if(message.InputData == null)
			return;
		Command cmd = (Command)message.InputData;
		if(cmd.code == CmdCode.CMD_GAME_MATCHING){
			int result = cmd.getInt(ArgCode.ARG_CODE, 0);
			if(result==1){
				//tim thay
				controller.SendMessage("HidePanel" , ScreenManager.PN_SEARCH_OPPONENT);
				controller.SendMessage("ShowPanel" , ScreenManager.PN_WAITING_ROOM);
			}
			message.ReceiveData = true;
			return;
		}

		message.ReceiveData = false;
	}
	
	void ReturnHome(){
		controller.SendMessage("HidePanel" , ScreenManager.PN_SEARCH_OPPONENT);
		controller.SendMessage("ShowPanel" , ScreenManager.PN_HOME);
		Command cmd = new Command(CmdCode.CMD_GAME_MATCHING_CANCEL);
		ScreenManager.instance.Send (cmd);
		
	}
}
