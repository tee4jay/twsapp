using IBApi;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace TwsClient
{
    public class MarketData : IMarketData
    {
        private EWrapperImpl _eWrapper;
        private BlockingCollection<string> _messages;

        public BlockingCollection<string> Messages
        {
            get { return _messages; }
        }

        public MarketData()
        {
            _messages = new BlockingCollection<string>();
            _eWrapper = new EWrapperImpl(_messages);
        }

        public void Start()
        {
            EClientSocket clientSocket = _eWrapper.ClientSocket;
            EReaderSignal readerSignal = _eWrapper.Signal;

            //! [connect]
            clientSocket.eConnect("127.0.0.1", 7497, 0);
            //! [connect]
            //! [ereader]
            //Create a reader to consume messages from the TWS. The EReader will consume the incoming messages and put them in a queue
            var reader = new EReader(clientSocket, readerSignal);
            reader.Start();
            //Once the messages are in the queue, an additional thread can be created to fetch them
            new Thread(() => { while (clientSocket.IsConnected()) { readerSignal.waitForSignal(); reader.processMsgs(); } }) { IsBackground = true }.Start();
            //! [ereader]
            /*************************************************************************************************************************************************/
            /* One (although primitive) way of knowing if we can proceed is by monitoring the order's nextValidId reception which comes down automatically after connecting. */
            /*************************************************************************************************************************************************/
            while (_eWrapper.NextOrderId <= 0) { }

            //! [futcontract]
            Contract contract = new Contract();
            contract.Symbol = "MES";
            contract.SecType = "FUT";
            contract.Exchange = "GLOBEX";
            contract.Currency = "USD";
            contract.LastTradeDateOrContractMonth = "20200320";

            clientSocket.reqMktData(1001, contract, "233", false, false, null);
        }

        public void Stop()
        {
            _eWrapper.ClientSocket.cancelMktData(1001);
            _eWrapper.ClientSocket.eDisconnect();
            _messages.CompleteAdding();
        }
    }
}