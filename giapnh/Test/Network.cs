using System.Collections;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System;
using System.Threading;
using MiscUtil.Conversion;
using MiscUtil.IO;

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
	public static NetworkAPI instance; 
	public Socket client;
	NetworkStream stream;
	/// <summary>
	/// The reader.
	/// Responsibility: Read all receive data from server
	/// </summary>
	BinaryReader reader;
	Thread tReader;
	#endregion
	
	#region Constructors
	public NetworkAPI(){
		Start();
		Command cmd = new Command(CmdCode.CMD_LOGIN);
		cmd.addString(ArgCode.ARG_LOGIN_USERNAME,"giapnh");
		cmd.addInt(ArgCode.ARG_CODE, 10);
		cmd.addString(ArgCode.ARG_LOGIN_PASSWRD, "kachimasu");
		Send(cmd);
	}
	public static NetworkAPI GetInstance(){
		if(instance==null){
			instance = new NetworkAPI();
		}
		return instance;
	}
	#endregion
	
	#region Methods and Functions
	/// <summary>
	/// Connect to server.
	/// </summary>
	public void Start () {
		client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );
		client.Connect(Configs.HOST, Configs.PORT);
		client.ReceiveBufferSize = 1024 * 1024;
		client.NoDelay = true;
		 // 1. connect
		stream = new NetworkStream(client);
		reader = new BinaryReader(stream);
		//create reader thread and writer thread
		tReader = new Thread(new ThreadStart(this.Read));
		tReader.Start();
	}	
	/// <summary>
	/// Read data from clients sent to
	/// </summary>
	public void Read(){
		while(true){
			if(stream.DataAvailable && stream.CanRead){
				int code = reader.ReadInt16();
				Command cmd = new Command(code);
				cmd.read(reader);
				Console.WriteLine("Received: "+cmd.GetLog());
			}
		}
	}
	
	public void Send(Command cmd){
		client.Send(cmd.getBytes());
		Console.Write("Sent: " + cmd.GetLog());
	}
	/// <summary>
	/// Stop read and write.
	/// </summary>
	public void Stop(){
		tReader.Interrupt();
		client.Close();
	}
	#endregion
	
	public static void Main(){
		new NetworkAPI();
	}
}
