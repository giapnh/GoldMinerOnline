using UnityEngine;
using System.Collections;
using INet;

public class ScreenManager : MonoBehaviour,NetworkListener {
	public static ScreenManager instance;
	
	public NetworkAPI mNetwork;
	
	//Panel
	public ArrayList mScreens;
	public GameObject mLoginScreen;
	
	// Use this for initialization
	void Start () {
		instance = this;
		mScreens = new ArrayList();
		mNetwork = new NetworkAPI(this);
		//Add all screen to list
		mScreens.Add(mLoginScreen);
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
		throw new System.NotImplementedException ();
	}
	
	public void onError ()
	{
		throw new System.NotImplementedException ();
	}
	
	void OnApplicationQuit(){
		mNetwork.Stop();
	}
}
