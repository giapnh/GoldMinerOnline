using System;

namespace IHelper
{
	public class InputFilter
	{
		static string IGNORE_EMAIL = "`~#$%^&*()_+=-[]{}\\|;:\"/,";
		
		public static bool CheckEmail(string str){
			return CheckStringFormat(str, IGNORE_EMAIL);
		}
		
		public static bool CheckStringFormat(string str, string ignore){
			for(int i = 0; i < ignore.Length; i++){
				foreach(char it in str){
					if(it == ignore[i]){
						return false;
					}
				}
			}
			return true;
		}
	}
}

