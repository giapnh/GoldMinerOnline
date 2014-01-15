using UnityEngine;
using System.Collections;
using INet;
using IHelper;
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
			//TODO
			message.ReceiveData = true;
		}else if(cmd.code == CmdCode.CMD_REGISTER){
			//TODO
			message.ReceiveData = true;
		}
		message.ReceiveData = false;
	}
	
	/// <summary>
	/// Send message login
	/// </summary>
	void OnLoginClick(){
		controller.SendMessage("ShowLoading");
		var username = txtUsername.text;
		var password = txtPasswrd.text;
		
		if(!InputFilter.CheckEmail(username)){
			Debug.Log("Invalid username");
			//Show notify
			return;
		}else if(!InputFilter.CheckEmail(password)){
			Debug.Log("Invalid password");
			//Show notify
			return;
		}else{
			Command cmd = new Command(CmdCode.CMD_LOGIN);
			cmd.addString(ArgCode.ARG_LOGIN_USERNAME, txtUsername.text);
			cmd.addString(ArgCode.ARG_LOGIN_PASSWRD , txtPasswrd.text);
			ScreenManager.instance.Send (cmd);
		}
	}
}
