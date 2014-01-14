using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System;
using System.Threading;
using INet;

/// <summary>
/// @author GiapNH
/// Network API.
/// </summary>
public class NetworkAPI{
	#region Fields and Constants
	/// <summary>
	/// The instance of networkApi
	/// Take all functions.
	/// </summary>
	public TcpClient client;
	/// <summary>
	/// The reader.
	/// Responsibility: Read all receive data from server
	/// </summary>
	StreamReader reader;
	StreamWriter writer;
	Thread tReader;
	bool reading = true;
	NetworkListener handler;
	#endregion
	
	#region Constructors
	public NetworkAPI(NetworkListener mListener){
		handler = mListener;
		Start();
	}
	
	#endregion
	
	#region Methods and Functions
	/// <summary>
	/// Connect to server.
	/// </summary>
	public void Start () {
		client = new TcpClient();
		 // 1. connect
        client.Connect(Configs.HOST,Configs.PORT);
		client.ReceiveBufferSize = 1024 * 1024;
		client.NoDelay = true;
		
        Stream stream = client.GetStream();
		reader = new StreamReader(stream);
		writer = new StreamWriter(stream);
		//create reader thread and writer thread
		tReader = new Thread(new ThreadStart(this.Read));
		tReader.Start();
	}	
	/// <summary>
	/// Read data from server
	/// </summary>
	public void Read(){
		
		while(reading){
		try{
			Command cmd = new Command();
			cmd.read(reader);
			ReceiveCommand(cmd);
			}catch(Exception e){
				if(reading){
					OnError();
				}
			}
		}
	}
	
	public void ReceiveCommand(Command cmd){
		if(reading){
			handler.receiveCmd(cmd);
		}
	}
	
	public void Send(Command cmd){
		Debug.Log("Sent to server");
		writer.WriteLine(cmd.ToString());
		writer.Flush();
	}
	
	public void OnError(){
		Stop();
		handler.onError();
	}
	/// <summary>
	/// Stop read and write.
	/// </summary>
	public void Stop(){
		reader.Close();
		writer.Close();
		reading = false;
		client.GetStream().Close();
		client.Close();
	}
	#endregion
}
