// See httpsusing System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;

namespace ProxyScraper
{
	class Program
	{
		public static void Main(string[] args)
		{
			Program.title();
			if (!Directory.Exists("Resource"))
			{
				Directory.CreateDirectory("Resource");
				File.Create("Resource\\socks4.txt");
				File.Create("Resource\\socks5.txt");
				File.Create("Resource\\http.txt");
			}
			string variable = Console.ReadLine();
			
			if (variable == "1")
			{
				bool httpResource = File.Exists("Resource\\http.txt");
				if (httpResource)
				{
					Program.SourceList = File.ReadLines("Resource\\http.txt").ToList<string>();
					if (Program.SourceList.Count > 0)
                    {
						Program.writer = new StreamWriter("http-scraped.txt");
						Program.StartScraper();
						Logger.Printf("Для закрытия программы нажми ENTER", Logger.Type.INFO);
						Logger.Printf("Подпишись на https://t.me/End_Soft !!");
						Console.ReadKey();
					}
                    else
                    {
						Logger.Printf("Извините. но ресурсы для парса не были найдены!!!", Logger.Type.ERROR);
						Console.ReadKey();
						Environment.Exit(0);
					}
					
				}
				else
				{
					File.Create("Resource\\http.txt");
					Logger.Printf("Ты не загрузил ресурсы для парса http прокси!", Logger.Type.ERROR);
					Console.ReadKey();
					Environment.Exit(0);
				}

			}
			else if (variable == "2")
			{
				bool socks4Resource = File.Exists("Resource\\socks4.txt");
				if (socks4Resource)
				{
					Program.SourceList = File.ReadLines("Resource\\socks4.txt").ToList<string>();
					if (Program.SourceList.Count > 0)
                    {
						Program.writer = new StreamWriter("socks4-scraped.txt");
						Program.StartScraper();
						Logger.Printf("Для закрытия программы нажми ENTER", Logger.Type.INFO);
						Logger.Printf("Подпишись на https://t.me/End_Soft !!");
						Console.ReadKey();
					}
                    else
                    {
						Logger.Printf("Извините. но ресурсы для парса не были найдены!!!", Logger.Type.ERROR);
						Console.ReadKey();
						Environment.Exit(0);
					}

				}
				else
				{
					File.Create("Resource\\socks4.txt");
					Logger.Printf("Ты не загрузил ресурсы для парса socks4 прокси!", Logger.Type.ERROR);
					Console.ReadKey();
					Environment.Exit(0);
				}

			}
			else if (variable == "3")
			{
				bool socks5Resource = File.Exists("Resource\\socks5.txt");
				if (socks5Resource)
				{
					Program.SourceList = File.ReadLines("Resource\\socks5.txt").ToList<string>();
					if (Program.SourceList.Count > 0)
                    {
						Program.writer = new StreamWriter("socks5-scraped.txt");
						Program.StartScraper();
						Logger.Printf("Для закрытия программы нажми ENTER", Logger.Type.INFO);
						Logger.Printf("Подпишись на https://t.me/End_Soft !!");
						Console.ReadKey();
					}

				}
				else
				{
					File.Create("Resource\\socks5.txt");
					Logger.Printf("Ты не загрузил ресурсы для парса socks5 прокси!", Logger.Type.ERROR);
					Console.ReadKey();
					Environment.Exit(0);
				}
			}
			else
			{
				Logger.Printf("Ты кусок говна, ебучего гавна!", Logger.Type.ERROR);
				Console.ReadKey();
				Environment.Exit(0);
			}
		}

		private static void StartScraper()
		{
			using (List<string>.Enumerator enumerator = Program.SourceList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string Url = enumerator.Current;
					Thread thread = new Thread(delegate ()
					{
						Program.Scrape(Url);
					});
					Program.ThreadList.Add(thread);
					thread.IsBackground = true;
					thread.Start();
				}
			}
			foreach (Thread thread2 in Program.ThreadList)
			{
				thread2.Join();
			}
		}

		private static void Scrape(string Url)
		{
			try
			{
				string input = Parser.MakeRequest(Url);
				MatchCollection matchCollection = Regex.Matches(input, "\\b(\\d{1,3}\\.){3}\\d{1,3}\\:\\d{1,8}\\b");				
				object obj = Program.printLock;
				lock (obj)
				{
					Logger.Printf(string.Format("Получено {0} прокси с {1} \n", matchCollection.Count, Url), Logger.Type.SUCCESS);
				}
				foreach (object obj2 in matchCollection)
				{
					Match match = (Match)obj2;
					Program.Save(match.Groups[0].Value);
					Program.Parsed++;
				}
			}
			catch
			{
			}
			finally
			{
				Program.Completed++;
				Program.UpdateTitle();
			}
		}

		private static void Save(string dataToSave)
		{
			Program.Lock.EnterWriteLock();
			try
			{
				Program.writer.WriteLine(dataToSave);
			}
			finally
			{
				Program.Lock.ExitWriteLock();
			}
		}

		private static void UpdateTitle()
		{
			Console.Title = string.Format("Proxy Scraper by VoXDoX | Удачно = {0} / {1}  |  Всего = {2} | https://t.me/End_Soft ", Program.Completed, Program.SourceList.Count, Program.Parsed);
		}

		public static void title()
		{
			Console.Title = "Proxy Scraper by VoXDoX | https://t.me/End_Soft";

			Console.WriteLine("╔══╗───────────╔╗─╔╗     ───────────────╔══╗───────");
			Console.WriteLine("║╔╗║───────────║║─║║     ───────────────║╔╗║───────");
			Console.WriteLine("║╚╝║╔═╗╔══╗╔╗╔╗║╚═╝║     ╔══╗╔══╗╔═╗╔══╗║╚╝║╔══╗╔═╗");
			Console.WriteLine("║╔═╝║╔╝║╔╗║╚╬╬╝╚═╗╔╝     ║══╣║╔═╝║╔╝║╔╗║║╔═╝║║═╣║╔╝");
			Console.WriteLine("║║──║║─║╚╝║╔╬╬╗╔═╝║─     ─══║║╚═╗║║─║╔╗║║║──║║═╣║║─");
			Console.WriteLine("╚╝──╚╝─╚══╝╚╝╚╝╚══╝─     ╚══╝╚══╝╚╝─╚╝╚╝╚╝──╚══╝╚╝─");
			Console.WriteLine("Owner github: https://github.com/VoXDoX");
			Console.WriteLine("Owner channel: https://t.me/End_Soft");
			Console.WriteLine("");
			Console.WriteLine("Выберите режим работы:");
			Console.WriteLine("[1] Парс http прокся");
			Console.WriteLine("[2] Парс socks4 прокся");
			Console.WriteLine("[3] Парс sock5 прокся");
			Console.WriteLine("");
		}

		private static ReaderWriterLockSlim Lock = new ReaderWriterLockSlim();
		private static List<string> SourceList = new List<string>();
		private static List<Thread> ThreadList = new List<Thread>();

		private static object printLock = new object();
		private static StreamWriter writer;
		private static int Completed = 0;
		private static int Parsed = 0;
	}
}
