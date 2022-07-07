using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OETSystem.CoreOET
{
	public class CoreMessage
	{
		public string Message
		{
			get;set;
		}
		public int Wrong
		{
			get;set;
		}
		public int Corrected
		{
			get;set;
		}
		public int Mark
		{
			get;set;
		}
		public CoreMessage ( string message,int wrong,int correct )
		{
			Message=message;
			Wrong=wrong;
			Corrected=correct;
		}
	}
	public class DateConverter
	{
		public static DateTime Convert ( string ddMMyyyy )
		{
			string[] s = ddMMyyyy.Split ( '/' );
			if(s.Length>2)
			{
				return new DateTime ( int.Parse ( s[2] ),int.Parse ( s[1] ),int.Parse ( s[0] ) );
			}
			return DateTime.Now;
		}
	}
}