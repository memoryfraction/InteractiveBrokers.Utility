using InteractiveBrokers.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractiveBrokers.ConsoleApplication
{
    // 平均每次调用耗时: 1.13S
    // 只支持单线程调用;
    class Program
    {
        static int i;
        static Stopwatch sw = new Stopwatch();
        static void Main(string[] args)
        {
            InteractiveBrokersUtility IBUtility = new InteractiveBrokersUtility();
            sw.Start();
            for (i = 0; i < 100; i++)
            {
                //订阅事件
                IBUtility.AccountSummaryFetched += IBUtility_AccountSummaryFetched;

                Console.WriteLine("index:" + i);
                //开始请求数据，结果会在IBUtility_AccountSummaryFetched中得到
                IBUtility.GetAccountSummary();

                //如果正在处理数据，就等待;
                while (IBUtility.IsBusy)
                    continue;
                IBUtility.Dispose();
            };
            sw.Stop();

            //阻塞主线程，并等待
            Console.WriteLine("Request Processing, Pls Wait.");
            Console.ReadKey();
        }

        private static void IBUtility_AccountSummaryFetched(object sender, EventArgs e)
        {
            AccountSummary summary = (AccountSummary)sender;
            double avgSecConsumed = sw.Elapsed.TotalSeconds / (double)i;
            Console.WriteLine("IBBrokers account summary got.index:" + i + "\r\n average second per time:" + avgSecConsumed);
        }
    }
}