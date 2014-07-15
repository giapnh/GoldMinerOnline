using UnityEngine;
using System.Collections;
using INet;
using IHelper;

public class ConfirmPopup : MonoBehaviour {
	public UILabel message;
	int command_type;
	string username;
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
	public void set_command_type(int type){
		this.command_type = type;
	}
	public void set_username(string username){
		this.username = username;
	}
	
	public void Accept(){
		Command cmd = new Command (command_type);
		cmd.addString (ArgCode.ARG_PLAYER_USERNAME, username);
		cmd.addInt (ArgCode.ARG_CODE, 1);
		ScreenManager.instance.Send (cmd);
		Exit ();
	}
	
	public void Deny(){
		Command cmd = new Command (command_type);
		cmd.addString (ArgCode.ARG_PLAYER_USERNAME, username);
		cmd.addInt (ArgCode.ARG_CODE, 0);
		ScreenManager.instance.Send (cmd);
		Exit ();
	}
}
