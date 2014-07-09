using UnityEngine;
using System.Collections;
using INet;
using IHelper;

public class Popup : MonoBehaviour {
	public UILabel message;
	string username = "";
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Exit(){
		Destroy (this.gameObject);
	}

	public void set_message(string msg){
		message.text = msg;
	}

	public void set_username(string name){
		this.username = name;
	}

	public void Accept(string username){
		Command cmd = new Command (CmdCode.CMD_ACCEPT_FRIEND);
		cmd.addString (ArgCode.ARG_PLAYER_USERNAME, username);
		cmd.addInt (ArgCode.ARG_CODE, 1);
		ScreenManager.instance.Send (cmd);
	}

	public void Deny(string username){
		Command cmd = new Command (CmdCode.CMD_ACCEPT_FRIEND);
		cmd.addString (ArgCode.ARG_PLAYER_USERNAME, username);
		cmd.addInt (ArgCode.ARG_CODE, 0);
		ScreenManager.instance.Send (cmd);
	}
}
