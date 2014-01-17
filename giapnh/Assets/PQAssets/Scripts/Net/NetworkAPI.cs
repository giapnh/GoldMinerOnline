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
	public bool Connected = false;
	public NetworkStream stream;
	/// <summary>
	/// The reader.
	/// Responsibility: Read all receive data from server
	/// </summary>
	BinaryReader reader;
	BinaryWriter writer;
	Thread tReader;
	bool reading = true;
	NetworkListener handler;

	public Queue queueMessage = new Queue();
	#endregion
	
	#region Constructors
	public NetworkAPI(NetworkListener mListener){
		handler = mListener;
		Connect();
	}
	#endregion
	
	#region Methods and Functions
	/// <summary>
	/// Connect to server.
	/// </summary>
	public void Connect () {
		if(!Connected){
			client = new TcpClient();
			 // 1. connect
			try{
			    client.Connect(Configs.HOST,Configs.PORT);
				client.ReceiveBufferSize = 1024 * 1024;
				client.NoDelay = true;
				stream = client.GetStream();
				reader = new BinaryReader(stream);
				writer = new BinaryWriter(stream);
				//create reader thread and writer thread
				tReader = new Thread(new ThreadStart(this.Read));
				tReader.Start();
				Connected = true;
			}catch(SocketException ex){
				Connected = false;
				handler.onError();
			}
		}
        
	}	
	/// <summary>
	/// Read data from server
	/// </summary>
	public void Read(){
		while(reading){
		try{
			if(stream.DataAvailable && stream.CanRead){
				int code = reader.ReadInt16();
				Command cmd = new Command(code);
				cmd.read(reader);
				ReceiveCommand(cmd);
			}
			}catch(Exception e){
				if(reading){
					OnError();
				}
			}
			
		}
	}
	/// <summary>
	/// Receives the command.
	/// </summary>
	/// <param name='cmd'>
	/// The command you received from server
	/// </param>
	public void ReceiveCommand(Command cmd){
		if(reading){
			Debug.Log("Received:  " + cmd.GetLog());
			queueMessage.Enqueue(cmd);
			handler.receiveCmd(cmd);
		}
	}
	
	public void Send(Command cmd){
		Debug.Log("Sent: " + cmd.GetLog());
		writer.Write(cmd.getBytes());
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
		client.Close();
	}
	#endregion
}
