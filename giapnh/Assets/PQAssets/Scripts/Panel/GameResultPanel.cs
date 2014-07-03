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
	int current_exp;
	int level_require;
	int bonus_cup;
	int bonus_exp;
	// Use this for initialization
	void Start () {
		//level info
		TxtCup.text = PlayerPrefs.GetInt ("player_cup").ToString();
		TxtLevel.text = PlayerPrefs.GetInt ("player_level").ToString ();
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
			TxtProgress.text = current_exp.ToString() + "/" + level_require.ToString();
			bonus_exp -= step;
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

			message.ReceiveData = true;
			return;
		}

		message.ReceiveData = false;
	}
}
