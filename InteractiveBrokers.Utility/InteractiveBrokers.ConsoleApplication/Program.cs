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
            for (int i = 0; i < 999; i++)
            {
                using (InteractiveBrokersUtility IBUtility = new InteractiveBrokersUtility())
                {
                    //订阅事件
                    IBUtility.AccountSummaryFetched += IBUtility_AccountSummaryFetched;

                    Console.WriteLine("index:" + i);
                    //开始请求数据，结果会在IBUtility_AccountSummaryFetched中得到
                    IBUtility.GetAccountSummary();

                    //如果正在处理数据，就等待;
                    while (IBUtility.IsBusy)
                        continue;
                }
            }

            //阻塞主线程，并等待
            Console.WriteLine("Request Processing, Pls Wait.");
            Console.ReadKey();
        }

        private static void IBUtility_AccountSummaryFetched(object sender, EventArgs e)
        {

            AccountSummary summary = (AccountSummary)sender;
            Console.WriteLine("IBBrokers account summary got.");
        }


    }
}
