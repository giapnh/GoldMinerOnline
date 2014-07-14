﻿using UnityEngine;
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
		Debug.Log ("goi exit");
		Destroy (this.gameObject);
	}

	public void set_message(string msg){
		message.text = msg;
	}

	public void set_username(string name){
		this.username = name;
	}

	public void Accept(){
		Command cmd = new Command (CmdCode.CMD_ACCEPT_FRIEND);
		cmd.addString (ArgCode.ARG_PLAYER_USERNAME, PlayerInfo.OpUsername);
		cmd.addInt (ArgCode.ARG_CODE, 1);
		ScreenManager.instance.Send (cmd);
		UILabel friend_button_label = GameObject.Find ("BtnAddFriend/Label").GetComponentInChildren<UILabel>();
		friend_button_label.text = "Remove Friend";
		PlayerInfo.FriendType = 1;
		Exit ();
	}

	public void Deny(){
		Command cmd = new Command (CmdCode.CMD_ACCEPT_FRIEND);
		cmd.addString (ArgCode.ARG_PLAYER_USERNAME, PlayerInfo.OpUsername);
		cmd.addInt (ArgCode.ARG_CODE, 0);
		ScreenManager.instance.Send (cmd);
		UILabel friend_button_label = GameObject.Find ("BtnAddFriend/Label").GetComponentInChildren<UILabel>();
		friend_button_label.text = "Add Friend";
		PlayerInfo.FriendType = 0;
		Exit ();
	}
}