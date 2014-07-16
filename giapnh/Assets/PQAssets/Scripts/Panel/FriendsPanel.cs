using UnityEngine;
using System.Collections;
using INet;
using IHelper;
using System.Collections.Generic;

public class FriendsPanel : MonoBehaviour {
	public GameObject controller;
	public GameObject FriendPrefab;
	public GameObject friends_grid;
	int friends_num;
	// Use this for initialization

	void OnEnable(){
		Refresh_List ();
	}
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
		if (cmd.code == CmdCode.CMD_LIST_FRIEND) {
			friends_num = cmd.getInt(ArgCode.ARG_COUNT,0);		
		}
		if (cmd.code == CmdCode.CMD_FRIEND_INFO) {
			string name = cmd.getString(ArgCode.ARG_PLAYER_USERNAME,"");
			int level = cmd.getInt(ArgCode.ARG_PLAYER_LEVEL,0);
			int cup = cmd.getInt(ArgCode.ARG_PLAYER_CUP,0);
			int online_status = cmd.getInt(ArgCode.ARG_ONLINE,0);
			object friend_info = new object[level,cup,online_status];

			//create clone in grid then reposition
			GameObject friend = Instantiate (FriendPrefab) as GameObject;
			friend.SendMessage("set_level",level);
			friend.SendMessage("set_cup",cup);
			friend.SendMessage("set_online_status",online_status);
			friend.SendMessage("set_name", name);
			friend.transform.parent = friends_grid.transform;
			friends_num --;
			if(friends_num == 0){
				//reposition
				friends_grid.GetComponent<UIGrid>().Reposition();
			}

			message.ReceiveData = true;
			return;
		}

		message.ReceiveData = false;
	}

	void Refresh_List(){
		//xoa list cu
//		List<GameObject> children = new List<GameObject>();
		foreach (Transform child in friends_grid.transform)
						Destroy (child.gameObject);
//		foreach (Transform child in friends_grid.transform) children.Add(child.gameObject);
//		children.ForEach (child => Destroy (child));
		//get friend
		Command cmd = new Command(CmdCode.CMD_LIST_FRIEND);
		cmd.addInt (ArgCode.ARG_LIMIT,100);
		cmd.addInt (ArgCode.ARG_OFFSET,0);
		ScreenManager.instance.Send (cmd);
	}
}
