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
    public class InteractiveBrokersUtility : IDisposable
    {
        EWrapperImpl testImpl;
        EClientSocket clientSocket;
        EReaderSignal readerSignal;
        public bool IsBusy { get; set; }
        /// <summary>
        /// AccountSummary会在该事件触发后得到
        /// </summary>
        public event EventHandler AccountSummaryFetched;

        public InteractiveBrokersUtility()
        {

        }

        public void GetAccountSummary()
        {
            IsBusy = true;

            //initiate 多次循环使用时，需要每次都new一个新的EWrapperImpl。否则会因为信号量问题报错;
            using (testImpl = new EWrapperImpl())
            {
                clientSocket = testImpl.ClientSocket;
                readerSignal = testImpl.Signal;

                //! [connect]
                if (clientSocket.IsConnected() == false)
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


        }

        private void InteractiveBrokersUtility_AccountSummaryFetched(object sender, EventArgs e)
        {
            clientSocket.eDisconnect();
            clientSocket.Close();
            IsBusy = false;
            this.AccountSummaryFetched(sender, e);
        }

        private static void accountOperations(EClientSocket client)
        {
            client.reqAccountSummary(9001, "All", AccountSummaryTags.GetAllTags());
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    testImpl.Dispose();
                    clientSocket.Close();
                    clientSocket.eDisconnect();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~InteractiveBrokersUtility() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion


    }
}
