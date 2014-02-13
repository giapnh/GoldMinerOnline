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
	public static readonly int PN_MENU = 2;
	
	
	//Loading dialog
	public GameObject loading;
	// Use this for initialization
	void Start () {
		instance = this;
		mNetwork = new NetworkAPI(this);
		mScreens[0].SetActive(true);
	}

	void Update(){
//		if(reading){
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
		Debug.Log("On Error");
		// Cannot connect to server, please check your device network and try again!
		throw new System.NotImplementedException ();
	}
	
	void OnApplicationQuit(){
		mNetwork.Stop();
	}
}
