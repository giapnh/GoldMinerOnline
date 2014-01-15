using System;
using System.Text;
using System.IO;

namespace INet
{
	/// <summary>
	/// @Author: Steve Giap
	/// Argument class
	/// Each argument instance have struct:
	/// 	ArgumentCode|ArgType|ArgValue
	/// 		(short)	  (byte)   (type-value)
	/// </summary>
	public class Argument
	{
		
		#region Fields and Constants
		public static byte LONG = 8;
		public static byte INT = 4;
		public static byte SHORT = 2;
		public static byte BYTE = 1;
		// Raw data
		public static byte STRING = 10;
		public static byte RAW = 11;
		public byte type;
		public long numberValue;
		public string stringValue;
		public byte[] byteValue;
		
		#endregion
		
		#region Constructors
		public Argument ()
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="INet.Argument"/> class.
		/// </summary>
		/// <param name='type'>
		/// Type of argument: byte, short, int , boolean, string,...
		/// </param>
		public Argument (byte type)
		{
			this.type = type;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="INet.Argument"/> class.
		/// </summary>
		/// <param name='type'>
		/// Type.
		/// </param>
		/// <param name='val'>
		/// Argument has number value
		/// </param>
		public Argument (byte type, long val):this(type)
		{
			this.numberValue = val;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="INet.Argument"/> class.
		/// </summary>
		/// <param name='val'>
		/// Argument has string value
		/// </param>
		public Argument (string val):this(STRING)
		{
			this.stringValue = val;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="INet.Argument"/> class.
		/// </summary>
		/// <param name='val'>
		/// Raw data value: bytes
		/// </param>
		public Argument (byte[] val):this(RAW)
		{
			this.byteValue = val;
		}
		#endregion
		/// <summary>
		/// Gets the log.
		/// For debuging, get log of message
		/// </summary>
		/// <returns>
		/// </returns>
		public  string GetLog ()
		{
			String s = "";
			if (type == SHORT) {
				s += "short: " + (short)numberValue;
			} else if (type == INT) {
				s += "int: " + (int)numberValue;
			} else if (type == STRING) {
				s += "String: " + stringValue;
			} else if (type == RAW) {
				s += "Raw: " + byteValue.Length;
			} else if (type == BYTE) {
				s += "byte: " + (byte)numberValue;
			} else if (type == LONG) {
				s += "long: " + numberValue;
			}
			return s;
		}
		/// <summary>
		/// Use to get content of argument in string mode
		/// Returns a <see cref="System.String"/> that represents the current <see cref="INet.Argument"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current <see cref="INet.Argument"/>.
		/// </returns>
		public override string ToString ()
		{
			if (stringValue != null)
				return stringValue;
			if (byteValue != null) {
				String s = "";
				try {
					s = Encoding.UTF8.GetString (byteValue);
				} catch (Exception e) {
					e.ToString ();
				}
				return s;
			} else
				return String.Format ("{0}", numberValue);
		}
		/// <summary>
		/// Gets the argument as string.
		/// Comming soon!
		/// </summary>
		/// <returns>
		/// The argument as string.
		/// </returns>
		/// <param name='code'>
		
		/// </param>
		public static string getArgumentAsString (int code)
		{
			return "ArgName";
		}
		/// <summary>
		/// Write the specified writer.
		/// </summary>
		/// <param name='writer'>
		/// BinaryWriter
		/// </param>
		public BinaryWriter write(BinaryWriter writer){
			if(type == BYTE){
				writer.Write((byte)numberValue);
			}else if(type == INT){
				writer.Write((int)numberValue);
			}else if(type == SHORT){
				writer.Write ((short)numberValue);
			}else if(type == LONG){
				writer.Write((long)numberValue);
			}else if(type == STRING){
				writer.Write((int)Encoding.UTF8.GetBytes(stringValue).Length);
				writer.Write (Encoding.UTF8.GetBytes(stringValue));
			}else if(type == RAW){
				writer.Write((int)byteValue.Length);
				writer.Write(byteValue);
			}
			return writer;
		}
	}
}

