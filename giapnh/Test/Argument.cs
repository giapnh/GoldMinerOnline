using System;
using System.Text;
using System.IO;


namespace INet
{
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
		
		public Argument(byte type) {
			this.type = type;
		}

		public Argument(byte type, long val):this(type) {
			this.numberValue = val;
		}
	
		public Argument(string val):this(STRING) {
			this.stringValue = val;
		}
	
		public Argument(byte[] val):this(RAW) {
			this.byteValue = val;
		}
		#endregion
		public override string ToString ()
		{
			String s = "";
			if(type == SHORT){
				s += "short: " + (short) numberValue;
			}else if(type == INT){
				s += "int: " + (int) numberValue;
			}else if(type == STRING){
				s += "String: " + stringValue;
			}else if(type == RAW){
				s += "Raw: " + byteValue.Length;
			}else if(type == BYTE){
				s += "byte: " + (byte) numberValue;
			}else if(type == LONG){
				s += "long: " + numberValue;
			}
			return s;
		}
		
		public String toStringS() {
			if (stringValue != null)
				return stringValue;
			if (byteValue != null){
				String s="";
				try {
					s = Encoding.UTF8.GetString(byteValue);
				} catch (Exception e) {
					e.ToString();
				}
				return s;
			}
			else
				return string.Format("%d",numberValue);
			}
		}
}

