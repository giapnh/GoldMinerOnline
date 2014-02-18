using System;
using System.IO;
using System.Net;
using System.Collections;
using System.Text;
using MiscUtil.IO;
using MiscUtil.Conversion;


namespace Test
{
	class MainClass
	{
		EndianBinaryWriter writer;
		EndianBinaryReader reader;
		public MainClass(){
			
			Stream stream = new MemoryStream();
			
			writer = new EndianBinaryWriter(new LittleEndianBitConverter() , stream , Encoding.UTF8);
			reader = new EndianBinaryReader(new LittleEndianBitConverter() , stream , Encoding.UTF8);
			writer.Write(100);
			writer.Write(200);
			writer.Flush();
			stream.Seek(0,SeekOrigin.Begin);
			int val = reader.ReadInt32();
			int val2 = reader.ReadInt32();
			Console.WriteLine(val);
			Console.WriteLine(val2);
		}
		
//		public static void Main (string[] args)
//		{
//			new MainClass();
//		}
		
	}
}
