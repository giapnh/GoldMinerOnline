﻿using UnityEngine;
using System.Collections;
using INet;
using IHelper;

public class OnlineGamePanel : MonoBehaviour {
	public GameObject controller;
	public GameObject[] maps;
	public GameObject user;
	public GameObject op_user;
	public string current_player;
	public GameObject[] arrows;

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
		if(cmd.code == CmdCode.CMD_MAP_INFO){
			int map_id = cmd.getInt(ArgCode.ARG_MAP_ID, 0);
			maps[map_id-1].SetActive(true);
			current_player = cmd.getString(ArgCode.ARG_PLAYER_USERNAME,"");
			if(current_player== PlayerInfo.Username){
				user.transform.position = arrows[0].transform.position;
				op_user.transform.position = arrows[3].transform.position;
			} else{
				user.transform.position = arrows[3].transform.position;
				op_user.transform.position = arrows[0].transform.position;
			}

			message.ReceiveData = true;
			return;
		}

		//Opponent move
		if (cmd.code == CmdCode.CMD_PLAYER_MOVE) {
			string username = cmd.getString(ArgCode.ARG_PLAYER_USERNAME,"");
//			int from_pos = cmd.getInt(ArgCode.ARG_MOVE_FROM,0);
			int to_pos = cmd.getInt(ArgCode.ARG_MOVE_TO,0);
			Character ch;
			if(username == PlayerInfo.Username){
				ch = user.GetComponent<Character>();
			} else{
				ch = op_user.GetComponent<Character>();
			}
			ch.state = Character.MOVING;
			ch.target.x = arrows[to_pos-1].gameObject.transform.position.x;

			message.ReceiveData = true;
			return;
		}
		message.ReceiveData = false;
	}

	void Move(int to_pos){
		Command cmd = new Command(CmdCode.CMD_PLAYER_MOVE);
		cmd.addInt(ArgCode.ARG_ROOM_ID, PlayerInfo.RoomId);
//		cmd.addInt(ArgCode.ARG_MOVE_FROM, from_pos);
		cmd.addInt(ArgCode.ARG_MOVE_TO, to_pos);
		ScreenManager.instance.Send (cmd);
	}
}