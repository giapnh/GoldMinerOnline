using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {
	public GameObject onlineGameScreen;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnClick(){
		GameObject character =  GameObject.Find("Character");
		Hook hook_info = character.gameObject.GetComponentInChildren<Hook>();
		//check current user' turn
		OnlineGamePanel onlineGame_info = onlineGameScreen.gameObject.GetComponent<OnlineGamePanel> ();
		string current_user = onlineGame_info.current_player;
        if(hook_info.state == Hook.IDLE && current_user == PlayerInfo.Username){
			int to_pos = int.Parse(this.gameObject.name.Substring(this.gameObject.name.Length - 1));
			onlineGameScreen.SendMessage("Move", to_pos);
		}
	}
}
