﻿using UnityEngine;
using System.Collections;

public class BuffItem : MonoBehaviour {
	public int item_id;
	public int remainingNums;
	public GameObject onlineGameScreen;
	public GameObject hook;
	public UILabel lbl_remaining;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnClick(){
		OnlineGamePanel onlineGame_info = onlineGameScreen.gameObject.GetComponent<OnlineGamePanel> ();
		string current_player = onlineGame_info.current_player;
		Hook hook_info = hook.GetComponent<Hook> ();
		if (remainingNums > 0 && current_player==PlayerInfo.Username && hook_info.state == Hook.IDLE) {
			remainingNums--;
			lbl_remaining.text = remainingNums.ToString();
			Debug.Log(hook_info.used_buff_item);
//			string used_buff_item = hook_info.used_buff_item;
			if(hook_info.used_buff_item == "")
				hook_info.used_buff_item = item_id.ToString();
			else
				hook_info.used_buff_item = hook_info.used_buff_item + ";" + item_id.ToString();
			Debug.Log(hook_info.used_buff_item);
		}
	}

	void Increase(int item_id){
		remainingNums++;
		lbl_remaining.text = remainingNums.ToString();
	}
}
