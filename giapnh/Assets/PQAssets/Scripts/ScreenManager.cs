using UnityEngine;
using System.Collections;
using INet;
using IHelper;
public class ScreenManager : MonoBehaviour,NetworkListener {
	public static ScreenManager instance;
	public NetworkAPI mNetwork;
	//Panel
	public GameObject[] mScreens;
	public GameObject NotiPopup,FriendPopup, ConfirmPopup;
//	public GameObject mLoginScreen;
//	public GameObject mRegisterScreen;
	public bool reading = false;
	//Panel ID:
	public static readonly int PN_LOGIN = 0;
	public static readonly int PN_REGISTER = 1;
	public static readonly int PN_HOME = 2;
	public static readonly int PN_CAMPAIN_MAP = 3;
	public static readonly int PN_CAMPAIN_ONGAME = 4;
	public static readonly int PN_ONLINE_ONGAME = 5;
	public static readonly int PN_SEARCH_OPPONENT = 6;
	public static readonly int PN_WAITING_ROOM = 7;
	public static readonly int PN_GAME_RESULT = 8;
	public static readonly int PN_OFFLINE_GAME_RESULT = 9;
	//public static readonly int PN_FRIENDS_LIST = 10;

	//Loading dialog
	public GameObject loading;
	public UILabel friend_button_label;
	// Use this for initialization
	void Start () {
		instance = this;
		mNetwork = new NetworkAPI(this);
		mScreens[0].SetActive(true);
		for(int i = 1; i < mScreens.Length; i++){
			mScreens[i].SetActive(false);
		}
	}

	void OnCommand(SendMessageContext message){
		//If process, return true
		if(message.InputData == null)
			return;
		Command cmd = (Command)message.InputData;


		//add friend
		if(cmd.code == CmdCode.CMD_ADD_FRIEND){
			int arg_code = cmd.getInt(ArgCode.ARG_CODE, 0);
			string msg = cmd.getString(ArgCode.ARG_MESSAGE, "");
			if(arg_code == 0 ){
				//gui ket ban khong thanh cong
				showNoti(msg);
			}
			else if(arg_code ==  1){
				//gui ket ban thanh cong
				showNoti(msg);
				friend_button_label.text = "Cancel";
				PlayerInfo.FriendType = 2;
			} else{
				//nhan loi moi ket ban
				string username = cmd.getString(ArgCode.ARG_PLAYER_USERNAME, "");
				GameObject noti = Instantiate (FriendPopup) as GameObject;
				noti.SendMessage ("set_message", msg);
				noti.SendMessage("set_username", username);
			}

			message.ReceiveData = true;
			return;
		}

		if(cmd.code == CmdCode.CMD_CANCEL_REQUEST){
			int arg_code = cmd.getInt(ArgCode.ARG_CODE, 0);
			string msg = cmd.getString(ArgCode.ARG_MESSAGE, "");
			if(arg_code == 1){
				friend_button_label.text = "Add Friend";
				PlayerInfo.FriendType = 0;
				//doi lai button tu cancel ve add friend
			}
			showNoti(msg);

			message.ReceiveData = true;
			return;
		}

		if(cmd.code == CmdCode.CMD_REMOVE_FRIEND){
			int arg_code = cmd.getInt(ArgCode.ARG_CODE, 0);
			string msg = cmd.getString(ArgCode.ARG_MESSAGE, "");
			if(arg_code == 1){
				friend_button_label.text = "Add Friend";
				PlayerInfo.FriendType = 0;
				//doi lai button tu remove ve accept friend
			}else if(arg_code == 2){
				//nhan dc huy ket ban
				friend_button_label.text = "Add Friend";
				PlayerInfo.FriendType = 0;
			}
			showNoti(msg);

			message.ReceiveData = true;
			return;
		}

		if(cmd.code == CmdCode.CMD_ACCEPT_FRIEND){
			int arg_code = cmd.getInt(ArgCode.ARG_CODE, 0);
			string msg = cmd.getString(ArgCode.ARG_MESSAGE, "");

			showNoti(msg);

			if(arg_code==0){
				friend_button_label.text = "Add Friend";
				PlayerInfo.FriendType = 0;
			} else{
				friend_button_label.text = "Unfriend";
				PlayerInfo.FriendType = 1;
			}


			message.ReceiveData = true;
			return;
		}

		if (cmd.code == CmdCode.CMD_INVITE_GAME) {
			string username = cmd.getString(ArgCode.ARG_PLAYER_USERNAME,"");
			int arg_code = cmd.getInt(ArgCode.ARG_CODE, 0);
			string msg = cmd.getString(ArgCode.ARG_MESSAGE,"");

			if(arg_code == 0){
				// gui thanh cong hay that bai
				showNoti(msg);
			} else if (arg_code == 2){
				// nhan dc loi moi choi
				// show confirm msg
				GameObject popup = Instantiate (ConfirmPopup) as GameObject;
				popup.SendMessage("set_command_type", CmdCode.CMD_ACCEPT_INVITE_GAME);
				popup.SendMessage("set_username", username);
				popup.SendMessage ("set_message", msg);
			}

			message.ReceiveData = true;
			return;
		}

		if (cmd.code == CmdCode.CMD_DISCONNECT) {
			string msg = cmd.getString(ArgCode.ARG_MESSAGE,"");
			GameObject noti = Instantiate (NotiPopup) as GameObject;
			noti.SendMessage ("set_message", msg);
			noti.SendMessage("set_type",CmdCode.CMD_DISCONNECT);

			message.ReceiveData = true;
			return;
		}
		
		if(cmd.code == CmdCode.CMD_GAME_MATCHING){
			int result = cmd.getInt(ArgCode.ARG_CODE, 0);
			if(result==1){
				//tim thay
				for(int i = 1; i < ScreenManager.instance.mScreens.Length; i++){
					ScreenManager.instance.mScreens[i].SetActive(false);
				}
				ScreenManager.instance.ShowPanel(ScreenManager.PN_WAITING_ROOM);
			}
			message.ReceiveData = true;
			return;
		}


		message.ReceiveData = false;
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
			command.ReceiveData = true;
//			OnCommand(command);
				foreach(GameObject screen in mScreens){
					if(screen.activeSelf){
						screen.SendMessage("OnCommand", command);
						break;
					}
				}

			if(command.ReceiveData == false){
				OnCommand(command);
			}
			if(command.ReceiveData == false){
				// If no have panel process this command
				// Debug.Log("Screen manager have to process this command");
				// If screen manager can't process this command, enqueue
				Command com = command.InputData as Command;

//				if(!(com.code == CmdCode.CMD_GAME_MATCHING || com.code == CmdCode.CMD_ROOM_EXIT)){
//					Debug.Log("Enqueue: " + com.code);

					mNetwork.queueMessage.Enqueue(command.InputData);
//				}
			}
//			Debug.Log("Queue size : " + mNetwork.queueMessage.Count);
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
	
	public void OnApplicationQuit(){
		
		if(mNetwork!=null)
			mNetwork.Stop();
	}

	public void showNoti(string msg){
		GameObject noti = Instantiate (NotiPopup) as GameObject;
		noti.SendMessage ("set_message", msg);
		Destroy (noti, 2);
	}

}
