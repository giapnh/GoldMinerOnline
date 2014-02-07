using UnityEngine;
using System.Collections;
using INet;
using IHelper;
public class RegisterPanel : MonoBehaviour {
	public UILabel txtUsername;
	public UILabel txtPasswrd;
	public UILabel txtRepasswrd;
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
		if(cmd.code == CmdCode.CMD_REGISTER){
			controller.SendMessage("ShowPanel", ScreenManager.PN_LOGIN);
			controller.SendMessage("HidePanel", ScreenManager.PN_REGISTER);
			controller.SendMessage("HideLoading");
			//TODO
			message.ReceiveData = true;
			return;
		}
		message.ReceiveData = false;
	}
	
	/// <summary>
	/// Send message login
	/// </summary>
	void OnRegisterSubmit(){
		controller.SendMessage("ShowLoading");
		var username = txtUsername.text;
		var password = txtPasswrd.text;
		var repasswrd = txtRepasswrd.text;
		
		if(!InputFilter.CheckEmail(username)){
			Debug.Log("Invalid username");
			controller.SendMessage("HideLoading");
			//Show notify
			return;
		}else if(!InputFilter.CheckEmail(password)){
			Debug.Log("Invalid password");
			controller.SendMessage("HideLoading");
			//Show notify
			return;
		}else if(!password.Equals(repasswrd)){
			Debug.Log("Password invalid");
			
		}else{
			Command cmd = new Command(CmdCode.CMD_REGISTER);
			cmd.addString(ArgCode.ARG_PLAYER_USERNAME, txtUsername.text);
			cmd.addString(ArgCode.ARG_PLAYER_PASSWRD , txtPasswrd.text);
			ScreenManager.instance.Send (cmd);
		}
	}
	
	void OnRegisterCancel(){
		controller.SendMessage("ShowPanel" , ScreenManager.PN_LOGIN);
		controller.SendMessage("HidePanel" , ScreenManager.PN_REGISTER);
	}
}
