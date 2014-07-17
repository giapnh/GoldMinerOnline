using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {
	public GameObject onlineGameScreen;
	public GameObject character;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnClick(){
//		GameObject character =  GameObject.Find("Character");
		Hook hook_info = character.gameObject.GetComponentInChildren<Hook>();
		Character character_info = character.gameObject.GetComponent<Character> ();
		//check current user' turn
		OnlineGamePanel onlineGame_info = onlineGameScreen.gameObject.GetComponent<OnlineGamePanel> ();
		string current_user = onlineGame_info.current_player;
		float round_time = onlineGame_info.round_time;

		if (character_info.state != Character.MOVING && hook_info.state == Hook.IDLE && current_user == PlayerInfo.Username && round_time <= 15) {
				int to_pos = int.Parse (this.gameObject.name.Substring (this.gameObject.name.Length - 1));
				onlineGameScreen.SendMessage ("Move", to_pos);
		}
	}
}
