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
		Destroy(this.gameObject.GetComponent<UIPanel>());
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void set_cup(int cup){
		this.cup = cup;
		cup_lbl.text = this.cup.ToString();
		}
	void set_name(string name){
		this.name = name;
		name_lbl.text = this.name;

	}
	void set_level(int level){
		this.level = level;
		level_lbl.text = this.level.ToString();
	}
	void set_online_status(int online_status){
		this.online_status = online_status;

		if(this.online_status == 0)
			online_status_sprite.spriteName = "offline";
		else
			online_status_sprite.spriteName = "online";
		}

	void Invite(){
		Command cmd = new Command (CmdCode.CMD_INVITE_GAME);
		cmd.addString (ArgCode.ARG_PLAYER_USERNAME, this.name);
		ScreenManager.instance.Send (cmd);
	}
}
