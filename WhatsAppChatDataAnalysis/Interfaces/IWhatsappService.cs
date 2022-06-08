using System;
using System.Collections.Generic;
using System.Text;

namespace WhatsAppChatDataAnalysis
{
	public interface IWhatsappService
	{
		public void PopulateMessageList();
		public void PopulatePeopleList();
		public void PrintPeople();
		public void PrintMessages();
		public void PrintDates();
		public List<string> CountMessagesByPerson();
		public void GetMessagesFromPastYear();
		public void GetMessagesFromYearBefore();
		public Dictionary<string, int> GetWordCounts();
	}
}
