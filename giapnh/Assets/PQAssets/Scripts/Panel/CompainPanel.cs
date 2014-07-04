using UnityEngine;
using System.Collections;
using INet;
public class CompainPanel : MonoBehaviour {
	public GameObject controller;
	public GameObject dmHook;
	public GameObject[] maps;
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

	//TODO code ngu lam lai
	void LoadLevel1(){
		Instantiate(maps[0], new Vector3(0, 0, 0.416687f), Quaternion.identity);
		controller.SendMessage("HidePanel" , ScreenManager.PN_COMPAIN_MAP);
		controller.SendMessage("ShowPanel" , ScreenManager.PN_COMPAIN_ONGAME);
		DmHook dmHook_info = dmHook.GetComponent<DmHook> ();
		dmHook_info.map_id = 1;
	}
	
	void LoadLevel2(){
		Instantiate(maps[1], new Vector3(0, 0, 0.416687f), Quaternion.identity);
		controller.SendMessage("HidePanel" , ScreenManager.PN_COMPAIN_MAP);
		controller.SendMessage("ShowPanel" , ScreenManager.PN_COMPAIN_ONGAME);
		DmHook dmHook_info = dmHook.GetComponent<DmHook> ();
		dmHook_info.map_id = 1;
	}
	
	void LoadLevel3(){
		Instantiate(maps[2], new Vector3(0, 0, 0.416687f), Quaternion.identity);
		controller.SendMessage("HidePanel" , ScreenManager.PN_COMPAIN_MAP);
		controller.SendMessage("ShowPanel" , ScreenManager.PN_COMPAIN_ONGAME);
		DmHook dmHook_info = dmHook.GetComponent<DmHook> ();
		dmHook_info.map_id = 1;
	}
	
	void LoadLevel4(){
		Instantiate(maps[3], new Vector3(0, 0, 0.416687f), Quaternion.identity);
		controller.SendMessage("HidePanel" , ScreenManager.PN_COMPAIN_MAP);
		controller.SendMessage("ShowPanel" , ScreenManager.PN_COMPAIN_ONGAME);
		DmHook dmHook_info = dmHook.GetComponent<DmHook> ();
		dmHook_info.map_id = 1;
	}
	
	void LoadLevel5(){
		Instantiate(maps[4], new Vector3(0, 0, 0.416687f), Quaternion.identity);
		controller.SendMessage("HidePanel" , ScreenManager.PN_COMPAIN_MAP);
		controller.SendMessage("ShowPanel" , ScreenManager.PN_COMPAIN_ONGAME);
		DmHook dmHook_info = dmHook.GetComponent<DmHook> ();
		dmHook_info.map_id = 1;
	}
	
	void LoadLevel6(){
		Instantiate(maps[0], new Vector3(0, 0, 0.416687f), Quaternion.identity);
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

	void Back(){
		controller.SendMessage("HidePanel" , ScreenManager.PN_COMPAIN_MAP);
		controller.SendMessage("ShowPanel" , ScreenManager.PN_HOME);
	}
}
