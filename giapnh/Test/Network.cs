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
	public TcpClient client;
	NetworkStream stream;
	/// <summary>
	/// The reader.
	/// Responsibility: Read all receive data from server
	/// </summary>
	BinaryReader reader;
	BinaryWriter writer;
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
		client = new TcpClient();
		client.ReceiveBufferSize = 1024 * 1024;
		 // 1. connect
        client.Connect(Configs.HOST,Configs.PORT);
        stream = client.GetStream();
		reader = new BinaryReader(stream);
		writer = new BinaryWriter(stream);
		//create reader thread and writer thread
		tReader = new Thread(new ThreadStart(this.Read));
		tReader.Start();
	}	
	
	public void Read(){
		while(true){
			if(stream.DataAvailable && stream.CanRead){
			int code = reader.ReadInt32();
			Command cmd = new Command(code);
			cmd.read(reader);
			}
		}
	}
	
	public void Send(Command cmd){
		writer.Write(cmd.getBytes());
		writer.Flush();
		Console.Write(cmd.GetLog());
	}
	/// <summary>
	/// Stop read and write.
	/// </summary>
	public void Stop(){
		tReader.Interrupt();
		client.GetStream().Close();
		client.Close();
	}
	#endregion
	
	public static void Main(){
		new NetworkAPI();
	}
}
