using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace WhatsAppChatDataAnalysis
{
	class Program
	{
		static async Task Main(string[] args)
		{
			IWhatsappService service = new WhatsAppService();
			service.PopulatePeopleList();
			service.PopulateMessageList();
			service.GetMessagesFromYearBefore();
			Dictionary<string, int> wordDict = service.GetWordCounts();
			using StreamWriter file = new StreamWriter("../../../../CCWords.txt");
			//foreach (KeyValuePair<string,int> kvp in wordDict)
			//{
			//	await file.WriteLineAsync($"{kvp.Key},{kvp.Value}");
			//}
			List<string> messageCount = service.CountMessagesByPerson();
			foreach (string line in messageCount)
			{
				Console.WriteLine(line);
			}
		}
	}
}
