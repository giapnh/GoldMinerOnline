using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using INet;
using System.IO;
using System.Text;

namespace INet
{
	/// <summary>
	/// Command class.
	/// Each command instance hava struct: 
	/// CommandCode|NumArgument|Argument1(code|dataType|value)|Argument2(code|dataType|value).....
	/// </summary>
	public class Command
	{
		#region Fields and Constant
		public short code;
		public Dictionary<short , Argument> arguments = new Dictionary<short, Argument>();
		
		public static short CMD_INVALID = -1;
		#endregion
		
		#region Constructor
		public Command ()
		{
		}
		public Command(int code):this(){
			this.code = (short)code;
		}
		
		#endregion
		
		
		#region Methods and Functions
		//=============Read from stream===============//
		public Command read(StreamReader reader){
			string data = reader.ReadLine();
			return read(data);
		}
		
		public Command read(string data){
			string[] items = data.Split('|');
			//Command code
			this.code = short.Parse(items[0]);
			//Number of arguments
			int numArg = int.Parse(items[1]);
			if(numArg > (items.Length - 2)/3){
				this.code = CMD_INVALID;
				return this;
			}
			for(var i = 0; i < numArg; i++){
				int code = int.Parse(items[2 + 3 * i]);//code
				int type = int.Parse(items[2 + 3 * i + 1]);//type
				if(type == Argument.STRING){
					string val = items[2 + 3 * i + 2];
					this.addString(code , val);
				}else{
					this.addArgument(code , new Argument((byte)type , long.Parse(items[2 + 3 * i + 2])));
				}
			}
			return this;
		}
		
		public BinaryReader read(BinaryReader reader){
			int numArg = reader.ReadInt16();
			for(int i = 0 ; i < numArg ; i++){
				int argCode = reader.ReadInt16();
				int argType = reader.ReadByte();
				if(argType == Argument.BYTE){
					var val = reader.ReadByte();
					this.addByte(argCode, val);
				}else if(argType == Argument.SHORT){
					var val = reader.ReadInt16();
					this.addShort(argCode, val);
				}else if(argType == Argument.INT){
					var val = reader.ReadInt32();
					this.addInt(argCode, val);
				}else if(argType == Argument.LONG){
					var val = reader.ReadInt32();
					this.addLong(argCode, val);
				}else if(argType == Argument.STRING){
					int strLen = reader.ReadInt32();
					var strVal = Encoding.UTF8.GetString(reader.ReadBytes(strLen));
					this.addString(argCode, strVal);
				}else if(argType == Argument.RAW){
					int rawLen = reader.ReadInt32();
					var rawVal = reader.ReadBytes(rawLen);
					this.addRaw(argCode, rawVal);
				}
			}
			return reader;
		}
		
		//=============Add argument===================//
		public Command addArgument(int code , Argument argument){
			arguments.Add((short)code , argument);
			return this;
		}
		
		public Command addByte(int code , long val){
			arguments.Add((short)code, new Argument(Argument.BYTE, val));
			return this;
		}
		
		public Command addShort(int code , long val){
			return addArgument((short)code , new Argument(Argument.SHORT , val));
		}
		
		public Command addInt(int code , long val){
			return addArgument((short)code, new Argument(Argument.INT , val));
		}
		
		public Command addLong(int code , long val){
			return addArgument((short)code , new Argument(Argument.LONG , val));
		}
		
		public Command addString(int code , string val){
			return addArgument((short)code , new Argument(val));
		}
		
		public Command addRaw(int code, byte[] raw){
			return addArgument((short)code, new Argument(raw));
		}
		
		//==============Get Argument===================//
		public string getString(short code, string def) {
			Argument arg = arguments[code];
			if (arg != null)
				return arg.ToString();
			return def;
		}
		
		public int getInt(short code, long def) {
			Argument arg = arguments[code];
			if (arg != null)
				return (int) arg.numberValue;
			return (int) def;
		}

		public short getShort(short code, long def) {
			Argument arg = arguments[code];
			if (arg != null)
				return (short) arg.numberValue;
			return (short) def;
		}
	
		public long getLong(short code, long def) {
			Argument arg = arguments[code];
			if (arg != null)
				return arg.numberValue;
			return def;
		}
	
		public byte getByte(short code, long def) {
			Argument arg = arguments[code];
			if (arg != null)
				return (byte) arg.numberValue;
			return (byte) def;
		}
	
		public bool getBoolean(short code) {
			Argument arg = arguments[code];
			if (arg != null)
				return arg.numberValue != 0;
			return false;
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="INet.Command"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current <see cref="INet.Command"/>.
		/// </returns>
		public override string ToString ()
		{
			string str = "";
			//1. command code
			str += code;
			//2. number of argument
			str += "|";
			str += arguments.Count;
			
			var keys = arguments.GetEnumerator();
			while(keys.MoveNext()){
				//3. Argument
				short key = keys.Current.Key;
				str+= "|" + key;//code
				Argument arg = arguments[key];
				str+= "|" + arg.type;//type
				str += "|"+ arg.ToString();//value
			}
			return str;
		}
		
		public static string getCommandName(short _command) {
			//TODO
			return "Command";
		}
		//For debug
		public string GetLog() {
			string s = "************Command: " + getCommandName(code) + "[" + code
						+ "]\n";
			var keys = arguments.GetEnumerator();
			while(keys.MoveNext()){
				short key = keys.Current.Key;
				Argument arg = arguments[key];
				s += "    " + Argument.getArgumentAsString(key) + "[" + key + "]"
							+ arg.ToString() + "\n";
			}
			s+= "\n";
			return s;
		}
		/// <summary>
		/// Command lenght
		/// Command code
		///Number of argument
		///Argument iterator
		///	-- Argument code
		///	-- Argument data type
		/// [Op] Argument data length if the data type is string or raw
		///  -- Argument data
		/// </summary>
		/// <returns>
		/// The bytes.
		/// </returns>
		public byte[] getBytes(){
			// Size
			int len = 0;
			len += 2;//Command code
			len += 2;//Number of argument
			foreach(var item in arguments){
				len += 2; // Argument code
				len += 1; // Argument data type
				int argCode = item.Key;
				var arg = item.Value;
				if(arg.type == Argument.STRING){
					len += 4;
					len += Encoding.UTF8.GetBytes(arg.stringValue).Length;
				}else if(arg.type == Argument.RAW){
					len += 4;
					len += arg.byteValue.Length;
				}else{
					len += arg.type;
				}
			}
			MemoryStream stream = new MemoryStream(len);
			BinaryWriter writer = new BinaryWriter(stream);
			writer.Write((short)code);//cmd code
			// Length of the command
			writer.Write((short)arguments.Count);//num arg
			foreach(var item in arguments){
				var arg = item.Value;
				writer.Write((short)item.Key);
				writer.Write((byte)arg.type);
				arg.write(writer);
			}
			writer.Flush();
			return stream.GetBuffer();
		}
		#endregion
	}
}

