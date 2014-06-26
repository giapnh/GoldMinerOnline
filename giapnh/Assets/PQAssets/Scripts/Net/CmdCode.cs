using System;

namespace INet
{
	public class CmdCode
	{
		//========================Command code===========================//
		public static readonly int CMD_INVALID = -1;
		public static readonly int CMD_REGISTER = 1;
		public static readonly int CMD_LOGIN = 2;
		public static readonly int CMD_PLAYER_INFO = 3;
		public static readonly int CMD_OPPONENT_INFO = 30;
		
    	public static readonly int  CMD_LIST_FRIEND = 10;
    	public static readonly int CMD_ADD_FRIEND = 11;
    	public static readonly int CMD_ACCEPT_FRIEND = 12;
    	public static readonly int CMD_REMOVE_FRIEND = 13;
    	public static readonly int CMD_PLAYER_CHAT = 20;

    	public static readonly int CMD_GAME_MATCHING = 100;
		public static readonly int CMD_GAME_MATCHING_CANCEL = 101;
		public static readonly int CMD_ROOM_INFO = 102;
		public static readonly int CMD_ROOM_EXIT = 105;
		public static readonly int CMD_GAME_READY = 110;
		public static readonly int CMD_GAME_START = 111;
		public static readonly int CMD_MAP_INFO = 120;
		public static readonly int CMD_PLAYER_MOVE = 130;
		public static readonly int CMD_PLAYER_DROP = 131;
		public static readonly int CMD_PLAYER_DROP_RESULT = 132;
		public static readonly int CMD_PLAYER_TURN = 141;
		public static readonly int CMD_ADD_SCORE = 150;
		
	}
}

