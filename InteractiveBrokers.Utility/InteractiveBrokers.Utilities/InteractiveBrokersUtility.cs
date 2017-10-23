using IBApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InteractiveBrokers.Utilities
{
    /// <summary>
    /// 使用说明:初始化，调用GetAccountSummary(), 然后订阅AccountSummaryFetched事件;
    /// </summary>
    public class InteractiveBrokersUtility
    {
        EWrapperImpl testImpl;
        EClientSocket clientSocket;
        EReaderSignal readerSignal;
        /// <summary>
        /// AccountSummary会在该事件触发后得到
        /// </summary>
        public event EventHandler AccountSummaryFetched;

        public InteractiveBrokersUtility()
        {
            testImpl = new EWrapperImpl();
            clientSocket = testImpl.ClientSocket;
            readerSignal = testImpl.Signal;
        }

        public void GetAccountSummary()
        {
            //! [connect]
            clientSocket.eConnect("127.0.0.1", 7496, 0);
            //! [connect]
            //! [ereader]
            //Create a reader to consume messages from the TWS. The EReader will consume the incoming messages and put them in a queue
            var reader = new EReader(clientSocket, readerSignal);
            reader.Start();

            //get summary
            if (clientSocket.IsConnected())
                accountOperations(clientSocket);

            this.testImpl.AccountSummaryFetched += InteractiveBrokersUtility_AccountSummaryFetched;

            //Once the messages are in the queue, an additional thread need to fetch them
            Thread msgProcessThread = new Thread(() =>
            {
                while (clientSocket.IsConnected())
                {
                    readerSignal.waitForSignal();
                    reader.processMsgs();
                }
            });

            msgProcessThread.IsBackground = true;
            msgProcessThread.Start();

            
        }

        private void InteractiveBrokersUtility_AccountSummaryFetched(object sender, EventArgs e)
        {
            clientSocket.eDisconnect();
            this.AccountSummaryFetched(sender, e);
        }

        private static void accountOperations(EClientSocket client)
        {
            client.reqAccountSummary(9001, "All", AccountSummaryTags.GetAllTags());
        }

    }
}
