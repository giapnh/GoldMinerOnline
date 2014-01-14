using UnityEngine;
using System.Collections;
using INet;
public class LoginPanel : MonoBehaviour {
	public UILabel txtUsername;
	public UILabel txtPasswrd;
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
		if(cmd.code == CmdCode.CMD_LOGIN){
			message.ReceiveData = true;
		}else if(cmd.code == CmdCode.CMD_REGISTER){
			message.ReceiveData = true;
		}
		
		message.ReceiveData = false;
	}
	
	/// <summary>
	/// Send message login
	/// </summary>
	void OnLoginClick(){
		var username = txtUsername.text;
		var password = txtPasswrd.text;
		
		Command cmd = new Command(CmdCode.CMD_LOGIN);
		cmd.addString(ArgCode.ARG_LOGIN_USERNAME, txtUsername.text);
		cmd.addString(ArgCode.ARG_LOGIN_PASSWRD , txtPasswrd.text);
		ScreenManager.instance.Send (cmd);
	}
}
