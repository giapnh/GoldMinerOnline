using UnityEngine;
using System.Collections;
using INet;

public class ScreenManager : MonoBehaviour,NetworkListener {
	public static ScreenManager instance;
	public NetworkAPI mNetwork;
	//Panel
	public GameObject[] mScreens;
//	public GameObject mLoginScreen;
//	public GameObject mRegisterScreen;
	public bool reading = false;
	//Panel ID:
	public static readonly int PN_LOGIN = 0;
	public static readonly int PN_REGISTER = 1;
	public static readonly int PN_HOME = 2;
	public static readonly int PN_COMPAIN_MAP = 3;
	public static readonly int PN_COMPAIN_ONGAME = 4;
	public static readonly int PN_ONLINE_ONGAME = 5;
	public static readonly int PN_SEARCH_OPPONENT = 6;
	public static readonly int PN_WAITING_ROOM = 7;
	public static readonly int PN_GAME_RESULT = 8;
	public static readonly int PN_OFFLINE_GAME_RESULT = 9;
	
	
	//Loading dialog
	public GameObject loading;
	// Use this for initialization
	void Start () {
		instance = this;
		mNetwork = new NetworkAPI(this);
		mScreens[0].SetActive(true);
		for(int i = 1; i < mScreens.Length; i++){
			mScreens[i].SetActive(false);
		}
	}

	void Update(){
//		if(reading){
		if(mNetwork == null)
			return;
		if(mNetwork.Connected){

			if(mNetwork.queueMessage.Count == 0)
				return;
			Command cmd = mNetwork.queueMessage.Dequeue() as Command;
			SendMessageContext command = new SendMessageContext();
			command.InputData = cmd;
			
			foreach(GameObject screen in mScreens){
				if(screen.activeSelf)
					screen.SendMessage("OnCommand", command);
			}
			if(command.ReceiveData == false){
				// If no have panel process this command
				// Debug.Log("Screen manager have to process this command");
				// If screen manager can't process this command, enqueue
				mNetwork.queueMessage.Enqueue(command.InputData);
			}
			mNetwork.queueMessage.Clear();
//			Debug.Log("Remain command = " + mNetwork.queueMessage.Count);
//			foreach(Command cmd2 in mNetwork.queueMessage){
//				Debug.Log(cmd2.code);
//			}
		}
//			reading = false;
//		}
	}
	
	public void ShowPanel(int panelId){
		((GameObject)mScreens[panelId]).SetActive(true);
	}
	
	public void HidePanel(int panelId){
		((GameObject)mScreens[panelId]).SetActive(false);
	}
	
	public void ShowLoading(){
		loading.SetActive(true);
	}
	
	public void HideLoading(){
		loading.SetActive(false);
	}
	
	public void Send(Command cmd){
		mNetwork.Send(cmd);
	}
	
	public void receiveCmd (Command cmd)
	{
		Debug.Log("Screen received command");
		reading = true;
		throw new System.NotImplementedException ();
	}
	
	public void SendMessage(GameObject receiver, object message){
		receiver.SendMessageUpwards("OnCommand", message);
	}
	
	public void onConnected ()
	{
		Debug.Log("On Connected");
		throw new System.NotImplementedException ();
	}
	
	public void onConnectFailure ()
	{
		Debug.Log("On connect failure");
		//Invalid username or password
		throw new System.NotImplementedException ();
	}
	
	public void onError ()
	{
		Debug.Log("Cannot connect to server, please check your device network and try again!");
	}
	
	void OnApplicationQuit(){
		
		if(mNetwork!=null)
			mNetwork.Stop();
	}
}
