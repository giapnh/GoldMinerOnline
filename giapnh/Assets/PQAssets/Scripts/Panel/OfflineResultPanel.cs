using UnityEngine;
using System.Collections;

public class OfflineResultPanel : MonoBehaviour {
	public GameObject controller;
	public UILabel score_label;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Back(){
		controller.SendMessage("HidePanel" , ScreenManager.PN_OFFLINE_GAME_RESULT);
		controller.SendMessage("ShowPanel" , ScreenManager.PN_COMPAIN_MAP);
	}
	public void Score(int score){
		score_label.text = "Score: " + score;
		Debug.Log (score);
	}
}
