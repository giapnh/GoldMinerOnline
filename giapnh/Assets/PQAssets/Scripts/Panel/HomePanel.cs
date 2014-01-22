using UnityEngine;
using System.Collections;
using INet;
public class HomePanel : MonoBehaviour {

	public UILabel TxtLevel;
	public UILabel TxtProgress;

	public UILabel TxtCup;

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
			int pLevel = cmd.getInt(ArgCode.ARG_PLAYER_LEVEL, 1);
			int pLevelUpProgress = cmd.getInt(ArgCode.ARG_PLAYER_LEVEL_UP_POINT, 0);
			int PLevelUpRequire = cmd.getInt(ArgCode.ARG_PLAYER_LEVEL_UP_REQUIRE, 0);
			TxtLevel.text = ""+pLevel;
			TxtCup.text = ""+pCup;
			TxtProgress.text = "("+pLevelUpProgress+"/"+PLevelUpRequire+")";
			message.ReceiveData = true;
			return;
		}
		message.ReceiveData = false;
	}
}
