using UnityEngine;
using System.Collections;
using INet;
using IHelper;

public class Friend : MonoBehaviour {
	string name;
	int cup;
	int level;
	int online_status;
	public UILabel name_lbl;
	public UILabel cup_lbl;
	public UILabel level_lbl;
	public UISprite online_status_sprite;



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void set_info(string name, object[] friend_info){
		this.cup = (int)friend_info[1];
		this.level = (int)friend_info[2];
		this.online_status = (int)friend_info[3];
		//display
		cup_lbl.text = this.cup.ToString();
		level_lbl.text = this.level.ToString();
		if(this.online_status == 0)
			online_status_sprite.spriteName = "offline";
		else
			online_status_sprite.spriteName = "online";
	}

	void set_name(string name){
		this.name = name;
		name_lbl.text = this.name;

	}

	void invite(){
		Command cmd = new Command (CmdCode.CMD_INVITE_GAME);
		cmd.addString (ArgCode.ARG_PLAYER_USERNAME, this.name);
		ScreenManager.instance.Send (cmd);
		Debug.Log ("send request to " + this.name);
	}
}
