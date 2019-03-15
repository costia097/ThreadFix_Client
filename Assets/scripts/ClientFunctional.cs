using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Local")]
[SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Global")]
public class ClientFunctional
{
    private Queue<string> queueToRead = new Queue<string>();
    
    private NetworkStream networkStream;
    private StreamReader streamReader;
    private StreamWriter streamWriter;
        
    private void eventChecker()
    {
        var thread = new Thread(start);
        thread.Start();
    }

    private void start()
    {
        while (true)
        {
            Console.WriteLine("CHECKER WORK");
            var value = streamReader.ReadLine();
            if (value != null)
            {
                queueToRead.Enqueue(value);
            }
        }
    }


    public void StartClient()
    {
        var tcpClient = new TcpClient();
        tcpClient.Connect("127.0.0.1", 27015);

        networkStream = tcpClient.GetStream();
            
        streamReader = new StreamReader(networkStream);
        streamWriter = new StreamWriter(networkStream) {AutoFlush = true};
        eventChecker();
        startProcessing();
    }

    private void startProcessing()
    {
        var thread = new Thread(() =>
        {
            //TODO fix it please
            while (true)
            {
                if (queueToRead.Count > 0)
                {
                    var element = queueToRead.Dequeue();
                    Debug.Log(element);
                }
                Thread.Sleep(1000);
            }   
        });
        thread.Start();
    }

    public void sendMessage(EventType eventType, object message)
    {
        switch (eventType)
        {
            case EventType.Movement:
                
                break;
        }
        
        
        
//        StreamWriter.WriteLine(message);
    }
}