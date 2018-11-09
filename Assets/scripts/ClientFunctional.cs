using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class ClientFunctional
{
    private Queue<string> QueueToRead = new Queue<string>();
    private NetworkStream NetworkStream;
    private StreamReader StreamReader;
    private StreamWriter StreamWriter;
        
    private void EventChecker()
    {
        Thread thread = new Thread(Start);
        thread.Start();
    }

    private void Start()
    {
        while (true)
        {
            Console.WriteLine("CHECKER WORK");
            var value = StreamReader.ReadLine();
            if (value != null)
            {
                QueueToRead.Enqueue(value);
            }
            Thread.Sleep(300);
        }
    }


    public void StartClient()
    {
        TcpClient tcpClient = new TcpClient();
        tcpClient.Connect("127.0.0.1", 27015);

        NetworkStream = tcpClient.GetStream();
            
        StreamReader = new StreamReader(NetworkStream);
        StreamWriter = new StreamWriter(NetworkStream) {AutoFlush = true};
        EventChecker();
        StartProcessing();
    }

    private void StartProcessing()
    {
        Thread thread = new Thread(() =>
        {
            while (true)
            {
                if (QueueToRead.Count > 0)
                {
                    var element = QueueToRead.Dequeue();
                    Debug.Log(element);
                }

                SendMessage(EventType.Movement, "test");
                Thread.Sleep(1000);
            }   
        });
        thread.Start();
    }

    public void SendMessage(EventType eventType, object message)
    {
        switch (eventType)
        {
            case EventType.Movement:
                
                break;
        }
        
        
        
//        StreamWriter.WriteLine(message);
    }
}