using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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

        void Start()
        {
            Bctrl = FindFirstObjectByType<ButtonController>();

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
            string mark = "";
            List<string>selectedMark = new List<string>();

            foreach (char c in emojis)
            {
                if(c == ',')
                {
                    selectedMark.Add(mark);
                    mark = "";
                    continue;
                }
                mark += c;
            }
            selectedMark.Add(mark);
            Bctrl.AudioResponse(selectedMark);
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
