using System;
using System.Collections.Generic;
using INet;
namespace INet
{
	public class Command
	{
		#region Fields and Constant
		public int code;
		public Dictionary<int , Argument> arguments = new Dictionary<int, Argument>();
		//========================Command code===========================//
		public static int CMD_REGISTER = 1;
		public static int CMD_LOGIN = 2;
		#endregion
		
		#region Constructor
		public Command ()
		{
		}
		public Command(int code):this(){
			this.code = code;
		}
		#endregion
		
		
		#region Methods and Functions
		public Command addArgument(int code , Argument argument){
			arguments.Add(code , argument);
			return this;
		}
		
		public Command addShort(int code , long val){
			return addArgument(code , new Argument(Argument.SHORT , val));
		}
		
		public Command addInt(int code , long val){
			return addArgument(code, new Argument(Argument.INT , val));
		}
		
		public Command addLong(int code , long val){
			return addArgument(code , new Argument(Argument.LONG , val));
		}
		#endregion
		
	
	}
}

