using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEditor.Compilation;
using UnityEngine;
using TMPro;
using System.Collections;

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
        public readonly object connModeLock = new object();
        public int connectionMode = 0;
        [SerializeField] private TextMeshProUGUI voiceInfo;
        private double t = 0;
        private double frame = 0;
        private bool resultTimeExeFlg;


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

            connectionMode = 0;//-1:無動作 0:待機 1:StartPythonProc 2:RecordStart 3:RecordingVoice 4:voiceAnalyzationStart 5:voiceAnalyzation
        
            voiceInfo.text = "";
            t = 0;
            frame = 0;
            resultTimeExeFlg = false;
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
                        lock (connModeLock)
                        {
                            connectionMode = 2;
                        }
                    }
                    else if (msg == "VOICE_END")
                    {
                        lock (connModeLock)
                        {
                            connectionMode = 4;
                        }
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

        /// <summary>
        /// main thread process
        /// </summary>


        public void PCUpdate(double deltaTime)
        {
            switch (connectionMode)
            {
                case 0:
                    Debug.Log("Start");
                    resultTimeExeFlg = false;
                    lock (connModeLock)
                    {
                        connectionMode = 1;
                    }
                    SendStart();
                    voiceInfo.text = "";
                    t = 0;
                    break;
                
                case 1:                    
                    break; 
                case 2:
                    Debug.Log("🎤 Voice Recording...");
                    lock (connModeLock)
                    {
                        connectionMode = 3;
                    }

                    voiceInfo.text = "録音中";
                    break;
                
                case 3:
                    voiceInfo.text = "録音中";
                    t += deltaTime;
                    if (t >= 0.25f)                             {
                        t -= 0.25f;
                        frame = (frame + 1) % 4;
                    }
                    for(int i=0; i<frame; i++)
                    {
                        voiceInfo.text += ".";
                    }

                    break;
                
                case 4:
                    Debug.Log("🛑 Voice End and analyzing");
                    lock (connModeLock)
                    {
                        connectionMode = 5;
                    }

                    voiceInfo.text = "分析中";
                    t += deltaTime;
                    if (t >= 0.25f)                             {
                        t -= 0.25f;
                        frame = (frame + 1) % 4;
                    }
                    for(int i=0; i<frame; i++)
                    {
                        voiceInfo.text += ".";
                    }

                    break;
                
                case 5:
                    voiceInfo.text = "分析中";
                    t += deltaTime;
                    if (t >= 0.25f)                             {
                        t -= 0.25f;
                        frame = (frame + 1) % 4;
                    }
                    for(int i=0; i<frame; i++)
                    {
                        voiceInfo.text += ".";
                    }

                    break;
                
                case 6:
                    voiceInfo.text = "検出！！";
                    if (!resultTimeExeFlg)
                    {
                        resultTimeExeFlg = true;
                        StartCoroutine(ResultTime());
                    }
                    break;
                
                case 7:
                    voiceInfo.text = "検出できなかった...";
                    if (!resultTimeExeFlg)
                    {
                        resultTimeExeFlg = true;
                        StartCoroutine(ResultTime());
                    }
                    break;
                
                default:
                    break;

            }
        }

        private IEnumerator ResultTime()
        {
            yield return new WaitForSeconds(2f);
            lock (connModeLock)
            {
                connectionMode = 0;
            }
        }


        public void SendStart()
        {
            writer.WriteLine("START");
        }

        public void EndGame()
        {
            lock (connModeLock)
            {
                connectionMode = -1;
            }
        }

        public void ClearConn(bool flg)
        {
            lock (connModeLock)
            {
                if(connectionMode != 6 && !flg)connectionMode = 7;
                else if(flg)connectionMode = 6;
            }
        }

        public void OnDestroy()
        {
            receiveThread?.Abort();
            client?.Close();
        }
    }
}
