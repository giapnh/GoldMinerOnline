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
	
	//Panel ID:
	public static int PN_LOGIN = 0;
	public static int PN_REGISTER = 1;
	
	
	//Loading dialog
	public GameObject loading;
	// Use this for initialization
	void Start () {
		instance = this;
		mNetwork = new NetworkAPI(this);
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
		Debug.Log("Recv: " + cmd.GetLog());
		foreach(GameObject screen in mScreens){
			
		}
		throw new System.NotImplementedException ();
	}
	
	public void onConnected ()
	{
		throw new System.NotImplementedException ();
	}
	
	public void onConnectFailure ()
	{
		//Invalid username or password
		throw new System.NotImplementedException ();
	}
	
	public void onError ()
	{
		// Cannot connect to server, please check your device network and try again!
		throw new System.NotImplementedException ();
	}
	
	void OnApplicationQuit(){
		mNetwork.Stop();
	}
}
