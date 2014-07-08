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
	public UILabel TxtCup;
	public UILabel TxtLevel;
	public UILabel TxtProgress;
	public GameObject Progress;
	int current_exp;
	int current_level;
	int current_cup;
	int level_require;
	int bonus_cup;
	int bonus_exp;
	int next_level_require;
	public GameObject SpeechBoxPrefab;
	public UILabel txtMessage;
	// Use this for initialization
	void OnEnable () {
		//level info
		current_cup = PlayerPrefs.GetInt ("player_cup");
		TxtCup.text = current_cup.ToString();
		current_level = PlayerPrefs.GetInt ("player_level");
		TxtLevel.text = current_level.ToString ();
		current_exp = PlayerPrefs.GetInt("player_levelup_point");
		level_require = PlayerPrefs.GetInt("player_levelup_require");
		TxtProgress.text = current_exp.ToString() + "/" + level_require.ToString();

		//result
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
		//TODO change value of exp and cup
		if (bonus_exp > 0) {
			int step = 1;
			current_exp += step;
			float progress_percent = (float)current_exp / level_require;
			Progress.transform.localScale = new Vector3(progress_percent, Progress.transform.localScale.y, Progress.transform.localScale.z);
			TxtProgress.text = current_exp.ToString() + "/" + level_require.ToString();
			bonus_exp -= step;
			if(current_exp>=level_require){
				current_exp = current_exp - level_require;
				current_level +=1;
				PlayerPrefs.SetInt("player_levelup_point", bonus_exp);
				PlayerPrefs.SetInt("player_level", current_level);
				TxtLevel.text = current_level.ToString ();
				level_require = next_level_require;
			}
		}
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
			bonus_cup = cmd.getInt(ArgCode.ARG_PLAYER_BONUS_CUP,0);
			bonus_exp = cmd.getInt(ArgCode.ARG_PLAYER_BONUS_LEVELUP_POINT,0);
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
			//set pref			
			PlayerPrefs.SetInt("player_cup", current_cup + bonus_cup);
			PlayerPrefs.SetInt("player_levelup_point", current_exp + bonus_exp);

			message.ReceiveData = true;
			return;
		}
		if (cmd.code == CmdCode.CMD_PLAYER_INFO) {
			int require = cmd.getInt(ArgCode.ARG_PLAYER_LEVEL_UP_REQUIRE, 0);
			if(require!=level_require){
				next_level_require = require;
				PlayerPrefs.SetInt("player_levelup_require", require);
			}
			
			message.ReceiveData = true;
			return;
		}
		//recv chat message
		if(cmd.code == CmdCode.CMD_PLAYER_CHAT){
			string chat_text = cmd.getString(ArgCode.ARG_MESSAGE , "");
			ReceiveChat(chat_text);
			message.ReceiveData = true;
			return;
		}

		message.ReceiveData = false;
	}

	void Back(){
		controller.SendMessage("HidePanel" , ScreenManager.PN_GAME_RESULT);
		controller.SendMessage("ShowPanel" , ScreenManager.PN_HOME);
		PlayerInfo.MapID = 0;
		PlayerInfo.RoomId = 0;
		PlayerInfo.Winner = "";
		PlayerInfo.OpUsername = "";
	}

	
	
	void SendChat(){
		//clone
		GameObject clone = Instantiate(SpeechBoxPrefab, new Vector3(-0.2053595F, 0.7534268F, 0), Quaternion.identity) as GameObject;
		clone.GetComponentInChildren<UILabel>().text = "" + txtMessage.text;
		//send
		Command cmd = new Command(CmdCode.CMD_PLAYER_CHAT);
		cmd.addInt(ArgCode.ARG_ROOM_ID, PlayerInfo.RoomId);
		cmd.addString(ArgCode.ARG_PLAYER_USERNAME, PlayerInfo.OpUsername);
		cmd.addString(ArgCode.ARG_MESSAGE , txtMessage.text);
		ScreenManager.instance.Send (cmd);
		//reset
		txtMessage.text = "enter message";
		Destroy (clone, 5);
	}
	
	void ReceiveChat(string chat_text){
		GameObject clone = Instantiate(SpeechBoxPrefab, new Vector3(1.059338F, 0.7533067F, 0), Quaternion.identity) as GameObject;
		clone.GetComponentInChildren<UILabel>().text = "" + chat_text;
		Destroy (clone, 5);
	}
}
