using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEditor.Compilation;
using UnityEngine;

namespace Unknown_Project
{
    public class PythonClient : MonoBehaviour
    {
        TcpClient client;
        StreamReader reader;
        StreamWriter writer;
        Thread receiveThread;
        private ButtonController Bctrl;
        public bool response = false;
        public List<string>selectedMark;
        public readonly object markLock = new object();

        void Start()
        {
            Bctrl = FindFirstObjectByType<ButtonController>();

            response = false;

            client = new TcpClient("127.0.0.1", 50007);
            NetworkStream stream = client.GetStream();

            reader = new StreamReader(stream, Encoding.UTF8);
            writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

            receiveThread = new Thread(ReceiveLoop);
            receiveThread.Start();
        }

        void ReceiveLoop()
        {
            while (true)
            {
                try
                {
                    string msg = reader.ReadLine();
                    if (msg == null) continue;

                    Debug.Log("From Python: " + msg);

                    if (msg == "VOICE_START")
                    {
                        // UI表示・SEなど
                        Debug.Log("🎤 Voice Recording...");
                    }
                    else if (msg == "VOICE_END")
                    {
                        Debug.Log("🛑 Voice End");
                    }
                    else if (msg.StartsWith("RESULT"))
                    {
                        string result = msg.Replace("RESULT ", "");
                        HandleResult(result);
                    }
                }
                catch
                {
                    break;
                }
            }
        }

        void HandleResult(string emojis)
        {
            var localList = new List<string>();
            string mark = "";

            foreach (char c in emojis)
            {
                if (c == ',')
                {
                    localList.Add(mark);
                    mark = "";
                    continue;
                }
                mark += c;
            }
            localList.Add(mark);

            lock (markLock)
            {
                selectedMark = localList;
                response = true;
            }

        }
        public void SendStart()
        {
            writer.WriteLine("START");
        }

        public void OnDestroy()
        {
            receiveThread?.Abort();
            client?.Close();
        }
    }
}
