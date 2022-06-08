using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WhatsAppChatDataAnalysis
{
	public class WhatsAppService : IWhatsappService
	{
		List<Message> MessageList { get; set; }
		List<string> PeopleList { get; set; }
		public void PopulateMessageList()
		{
			MessageList = new List<Message>();
			List<string> messageList = new List<string>();
			foreach (string line in File.ReadLines(@"../../../../WhatsAppChat.txt"))
			{
				messageList.Add(line);
			}
			;
			for(int i = 0; i < messageList.Count; i++)
			{
				string[] splitByHyphen = messageList[i].Split('-');
				Boolean hasDate = DateTime.TryParse(splitByHyphen[0], out DateTime result);
				DateTime dateTime = hasDate ? result : new DateTime();

				if (hasDate)
				{
					string messageContentWithPerson = "";
					string person = "";
					string messageContent = "";
					for (int j = 1; j < splitByHyphen.Length; j++)
					{
						messageContentWithPerson += $" {splitByHyphen[j]}";
					}
					string[] splitByColon = messageContentWithPerson.Split(':');
					person = splitByColon[0].Trim();
					if (splitByColon.Length > 1)
					{
						for (int j = 1; j < splitByColon.Length; j++)
						{
							messageContent += $" {splitByColon[j]}";
						}
					}
					if (!messageContent.Contains("Media omitted"))
					{
						messageContent = CleanMessage(messageContent);
						if (!string.IsNullOrWhiteSpace(messageContent) && PeopleList.Contains(person))
						{
							Message message = new Message() { DateAndTime = dateTime, Sender = person, Content = messageContent };
							MessageList.Add(message);
						}
					}
					
				}
				else
				{
					MessageList.LastOrDefault().Content += $" {messageList[i]}";
				}
			}
		}
		public void PopulatePeopleList()
		{
			PeopleList = new List<string>();
			foreach (string line in File.ReadLines(@"../../../../PeopleList.txt"))
			{
				PeopleList.Add(line);
			}
		}
		public void PrintPeople()
		{
			foreach (Message message in MessageList)
			{
				Console.WriteLine(message.Sender);
			}
		}
		public void PrintMessages()
		{
			foreach (Message message in MessageList)
			{
				Console.WriteLine(message.Content);
			}
		}
		public void PrintDates()
		{
			foreach (Message message in MessageList)
			{
				Console.WriteLine(message.DateAndTime);
			}
		}
		private static string CleanMessage(string message)
		{
			string cleanedMessage = new string(message.Where(c => Char.IsLetter(c) || Char.IsWhiteSpace(c)).ToArray());
			return cleanedMessage.ToLower().Trim();
		}
		public List<string> CountMessagesByPerson()
		{
			List<string> listPeople = new List<string>();
			foreach (string person in PeopleList)
			{
				int count = MessageList.Count(c => c.Sender == person);
				listPeople.Add($"{person}: {count}");
			}
			return listPeople;
		}
		public void GetMessagesFromPastYear()
		{
			MessageList = MessageList.Where(m => m.DateAndTime > new DateTime(2020, 12, 16)).ToList();
		}
		public void GetMessagesFromYearBefore()
		{
			MessageList = MessageList.Where(m => m.DateAndTime <= new DateTime(2020, 12, 16) && m.DateAndTime > new DateTime(2019, 12, 16)).ToList();
		}
		public Dictionary<string, int> GetWordCounts()
		{
			Dictionary<string, int> wordDict = new Dictionary<string, int>();
			foreach (Message message in MessageList)
			{
				string[] wordArray = message.Content.Split(' ');
				foreach (string word in wordArray)
				{
					string trimmedWord = word.Trim();
					if (wordDict.ContainsKey(trimmedWord))
					{
						wordDict[trimmedWord] += 1;
					}
					else
					{
						wordDict.Add(trimmedWord, 1);
					}
				}
			}
			return wordDict.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
		}
	}
}
