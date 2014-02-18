using UnityEngine;
using System.Collections;
using INet;
public class HomePanel : MonoBehaviour {

	public UILabel TxtLevel;
	public UILabel TxtProgress;

	public UILabel TxtCup;
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
			return;
		}
		message.ReceiveData = false;
	}
	
	void OnCompain(){
		controller.SendMessage("HidePanel" , ScreenManager.PN_HOME);
		controller.SendMessage("ShowPanel" , ScreenManager.PN_COMPAIN_MAP);
	}
	
	void OnOnline(){
		controller.SendMessage("HidePanel" , ScreenManager.PN_HOME);
		controller.SendMessage("ShowPanel" , ScreenManager.PN_COMPAIN_ONGAME);
	}
}
