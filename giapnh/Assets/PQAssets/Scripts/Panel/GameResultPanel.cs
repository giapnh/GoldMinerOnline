using UnityEngine;
using System.Collections;
using INet;
using IHelper;

public class GameResultPanel : MonoBehaviour {
	public GameObject controller;
	public GameObject self_info;
	public GameObject op_info;
	GameObject user_info;
	public UILabel result_lable;
	// Use this for initialization
	void Start () {
		if (PlayerInfo.Winner == "") {
			GameObject.Find("Opponent/Win").gameObject.SetActive(false);
			GameObject.Find("Self/Win").gameObject.SetActive(false);
			result_lable.text = "DRAW";
		} else if (PlayerInfo.Winner == PlayerInfo.Username) {			
			GameObject.Find("Opponent/Win").gameObject.SetActive(false);
			result_lable.text = PlayerInfo.Winner + " Won";
		} else {
			GameObject.Find("Self/Win").gameObject.SetActive(false);
			result_lable.text = PlayerInfo.Winner + " Won";
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	// On Command
	void OnCommand(SendMessageContext message){
		//If process, return true
		if(message.InputData == null)
			return;
		Command cmd = (Command)message.InputData;


		if (cmd.code == CmdCode.CMD_PLAYER_GAME_RESULT) {
			string username = cmd.getString(ArgCode.ARG_PLAYER_USERNAME,"");
			int score = cmd.getInt(ArgCode.ARG_SCORE,0);
			int bonus_cup = cmd.getInt(ArgCode.ARG_PLAYER_BONUS_CUP,0);
			int bonus_exp = cmd.getInt(ArgCode.ARG_PLAYER_BONUS_LEVELUP_POINT,0);

			if(username == PlayerInfo.Username){
				user_info = self_info;
			}
			else{
				user_info = op_info;
			}
			foreach(Transform child in user_info.transform){
				switch(child.gameObject.name){
				case "Cup":
					child.gameObject.GetComponent<UILabel>().text = bonus_cup.ToString();
					break;
				case "Exp":
					child.gameObject.GetComponent<UILabel>().text = bonus_exp.ToString();
					break;
				case "Score":
					child.gameObject.GetComponent<UILabel>().text = score.ToString();
					break;
				case "Name":
					child.gameObject.GetComponent<UILabel>().text = username;
					break;
				}

			}

			message.ReceiveData = true;
			return;
		}

		message.ReceiveData = false;
	}
}
