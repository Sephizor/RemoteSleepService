using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace RemoteSleepService
{
    public partial class RemoteSleep : ServiceBase
    {

        [DllImport("Powrprof.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool SetSuspendState(bool hibernate, bool forceCritical, bool disableWakeEvent);

        private Thread _runThread;

        public RemoteSleep()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _runThread = new Thread(() =>
            {
                var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                var localEndPoint = new IPEndPoint(IPAddress.Any, 15000);

                try
                {
                    socket.Bind(localEndPoint);
                    socket.Listen(10);

                    while (true)
                    {
                        var clientSocket = socket.Accept();
                        string receivedData = null;

                        while (true)
                        {
                            var buffer = new byte[1024];
                            var receivedBytes = clientSocket.Receive(buffer);

                            receivedData = Encoding.ASCII.GetString(buffer, 0, receivedBytes);

                            if (IsValidCommand(receivedData))
                            {
                                clientSocket.Send(Encoding.ASCII.GetBytes("Command accepted\n"));
                                clientSocket.Close();
                                break;
                            }
                        }

                        var powerFunctions = new Dictionary<string, Action>
                        {
                            {
                                "sleep", () =>
                                {
                                    SetSuspendState(false, true, true);
                                }
                            },
                            {
                                "shutdown", () =>
                                {
                                    Process.Start("shutdown", "/s /t 0");
                                }
                            },
                            {
                                "hibernate", () =>
                                {
                                    SetSuspendState(true, true, true);
                                }
                            }
                        };

                        Action actionToRun;
                        powerFunctions.TryGetValue(receivedData, out actionToRun);
                        actionToRun?.Invoke();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);

                }
            });
            _runThread.Start();
        }

        private bool IsValidCommand(string data)
        {
            return data.Contains("lock") ||
                   data.Contains("sleep") ||
                   data.Contains("hibernate") ||
                   data.Contains("shutdown");
        }
    }
}
