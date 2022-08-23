using System;
using System.Net.Sockets;

namespace DotFTP
{
    public class BaseFTPAgent
    {
        protected string lastErrorMessage = null;
        public string LastErrorMessage
        {
            get
            {
                return lastErrorMessage;
            }
        }

        #region CONNECTION ANALYSIS
        /// <summary>
        /// Testa se e' presente una connessione di rete attiva
        /// </summary>
        /// <returns>True se presente False altrimenti</returns>
        public bool IsNetworkConnectionAvailable()
        {
            return System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
        }

        /// <summary>
        /// Testa se e' possibile connettersi ad internet
        /// </summary>
        /// <returns>True se la connessiona ha successo false altrimenti</returns>
        public bool IsNetworkConnectionWorking()
        {
            Socket sk = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                sk.Connect("www.google.com", 80);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Testa se e' possibile connttersi ad un host remoto sulla porta 80 (HTTP)
        /// </summary>
        /// <param name="host">L'host a cui connettersi</param>
        /// <returns>True se la connessiona ha avuto successo, False altrimenti</returns>
        public bool CanReachRemoteHost(string host)
        {

            Socket sk = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                sk.Connect(host, 80);//, 21);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Testa se e' possibile connttersi ad un host remoto sulla porta passata come Parametro
        /// </summary>
        /// <param name="host">L'host a cui connettersi</param>
        /// <param name="port">La porta utilizzato dal servizio</param>
        /// <param name="waitFor">Tempo di Timeout espresso in secondi</param>
        /// <returns>True se la connessiona ha avuto successo, False altrimenti</returns>

        public bool CanReachRemoteHost(string host, int port, int waitFor)
        {
            Socket sk = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                sk.ReceiveTimeout = waitFor * 1000;
                sk.SendTimeout = waitFor * 1000;
                sk.Connect(host, port);//, 21);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool CanReachRemoteHost(string host, int secondi)
        {
            Socket sk = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                sk.ReceiveTimeout = secondi * 1000;
                sk.SendTimeout = secondi * 1000;
                sk.Connect(host, 80);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        protected string CheckAndFixPath(string path)
        {
            string result = path;
            result = result.Replace('\\', '/');

            if (result.StartsWith("/") == true)
            {
                result = result.Substring(1);
            }

            if (result.EndsWith("/") == true)
            {
                result = result.Substring(0, result.Length - 1);
            }

            return result;
        }


    }
}
