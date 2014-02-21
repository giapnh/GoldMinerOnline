using UnityEngine;
using System.Collections;
using INet;
public class CompainPanel : MonoBehaviour {
	public GameObject controller;
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
		
		message.ReceiveData = false;
	}
	
	void LoadLevel1(){
		controller.SendMessage("HidePanel" , ScreenManager.PN_COMPAIN_MAP);
		controller.SendMessage("ShowPanel" , ScreenManager.PN_COMPAIN_ONGAME);
	}
	
	void LoadLevel2(){
		controller.SendMessage("HidePanel" , ScreenManager.PN_COMPAIN_MAP);
		controller.SendMessage("ShowPanel" , ScreenManager.PN_COMPAIN_ONGAME);
	}
	
	void LoadLevel3(){
		controller.SendMessage("HidePanel" , ScreenManager.PN_COMPAIN_MAP);
		controller.SendMessage("ShowPanel" , ScreenManager.PN_COMPAIN_ONGAME);
	}
	
	void LoadLevel4(){
		controller.SendMessage("HidePanel" , ScreenManager.PN_COMPAIN_MAP);
		controller.SendMessage("ShowPanel" , ScreenManager.PN_COMPAIN_ONGAME);
	}
	
	void LoadLevel5(){
		controller.SendMessage("HidePanel" , ScreenManager.PN_COMPAIN_MAP);
		controller.SendMessage("ShowPanel" , ScreenManager.PN_COMPAIN_ONGAME);
	}
	
	void LoadLevel6(){
		controller.SendMessage("HidePanel" , ScreenManager.PN_COMPAIN_MAP);
		controller.SendMessage("ShowPanel" , ScreenManager.PN_COMPAIN_ONGAME);
	}
	
	void LoadLevel7(){
		controller.SendMessage("HidePanel" , ScreenManager.PN_COMPAIN_MAP);
		controller.SendMessage("ShowPanel" , ScreenManager.PN_COMPAIN_ONGAME);
	}
	
	void LoadLevel8(){
		controller.SendMessage("HidePanel" , ScreenManager.PN_COMPAIN_MAP);
		controller.SendMessage("ShowPanel" , ScreenManager.PN_COMPAIN_ONGAME);
	}
	
	void LoadLevel9(){
		controller.SendMessage("HidePanel" , ScreenManager.PN_COMPAIN_MAP);
		controller.SendMessage("ShowPanel" , ScreenManager.PN_COMPAIN_ONGAME);
	}
	
	void LoadLevel10(){
		controller.SendMessage("HidePanel" , ScreenManager.PN_COMPAIN_MAP);
		controller.SendMessage("ShowPanel" , ScreenManager.PN_COMPAIN_ONGAME);
	}
	
	void LoadLevel11(){
		controller.SendMessage("HidePanel" , ScreenManager.PN_COMPAIN_MAP);
		controller.SendMessage("ShowPanel" , ScreenManager.PN_COMPAIN_ONGAME);
	}
}
