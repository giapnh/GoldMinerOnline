using UnityEngine;
using System.Collections;
using INet;
using IHelper;
public class LoginPanel : MonoBehaviour {
	public UILabel txtUsername;
	public UILabel txtPasswrd;
	public UICheckbox cbxRemember;
	public GameObject controller;     
	// Use this for initialization
	void Start () {
		if(PlayerPrefs.GetInt("remember") == 1){
			var username = PlayerPrefs.GetString("login_username");
			var password = PlayerPrefs.GetString("login_password");
			txtUsername.text = username;
			txtPasswrd.text = password;
		}
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
			int result = cmd.getInt(ArgCode.ARG_CODE,0);
			if(result == 0){
				// Login failure
				//TODO
			}else{
				// Save login info to PlayerPrefs
				if(PlayerPrefs.GetInt("remember") == 1){
					PlayerPrefs.SetString("login_username", txtUsername.text);
					PlayerPrefs.SetString("login_password", txtPasswrd.text);
				}
				// Login successful
				controller.SendMessage("HidePanel" , ScreenManager.PN_LOGIN);
				controller.SendMessage("ShowPanel" , ScreenManager.PN_HOME);
				controller.SendMessage("HideLoading");
			}
			message.ReceiveData = true;
			return;
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
		// Check if uer remember username and password
		if(cbxRemember.isChecked){
			PlayerPrefs.SetInt ("remember", 1);
		}else{
			PlayerPrefs.SetInt ("remember", 0);
		}
		// Check valid username and password
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
		}else{
			Command cmd = new Command(CmdCode.CMD_LOGIN);
			cmd.addString(ArgCode.ARG_PLAYER_USERNAME, txtUsername.text);
			cmd.addString(ArgCode.ARG_PLAYER_PASSWRD , txtPasswrd.text);
#if UNITY_STANDALONE_WIN
			cmd.addInt(ArgCode.ARG_OS, Fields.OS_WINDOW);
#elif UNITY_IPHONE
			cmd.addInt(ArgCode.ARG_OS, Fields.OS_IOS);
#elif UNITY_ANDROID
			cmd.addInt(ArgCode.ARG_OS, Fields.OS_ANDROID);
#endif
			ScreenManager.instance.Send (cmd);
		}
	}
	/// <summary>
	/// Login remember infor or not
	/// </summary>
	void OnRemember(){
		if(cbxRemember.isChecked){
			PlayerPrefs.SetInt("remember", 1);
		}else{
			PlayerPrefs.SetInt("remember", 0);
		}
	}
	
	void OnRegisterClick(){
		controller.SendMessage("ShowPanel" , ScreenManager.PN_REGISTER);
		controller.SendMessage("HidePanel" , ScreenManager.PN_LOGIN);
	}
}
