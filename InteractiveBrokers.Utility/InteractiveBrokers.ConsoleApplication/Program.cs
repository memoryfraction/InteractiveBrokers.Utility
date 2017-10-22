using InteractiveBrokers.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractiveBrokers.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            InteractiveBrokersUtility IBUtility = new InteractiveBrokersUtility();
            IBUtility.AccountSummaryFetched += IBUtility_AccountSummaryFetched1; 
            IBUtility.GetAccountSummary();
        }


        private static void IBUtility_AccountSummaryFetched1(object sender, EventArgs e)
        {
            AccountSummary summary = (AccountSummary)sender;
            Console.WriteLine("IBBrokers account summary got.");
        }

    }
}
