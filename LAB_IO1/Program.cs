using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LAB_IO1
{
    class Program
    {

        //===================ZAD1==================
        static void ThreadProc1(Object stateInfo)
        {
            var timeToSleep = ((object[])stateInfo)[0];
            Thread.Sleep((int)timeToSleep);
            Console.WriteLine("[Watek1] Czekałem " + timeToSleep.ToString()+ "ms");
        }
        static void ThreadProc2(Object stateInfo)
        {
            var timeToSleep = ((object[])stateInfo)[0];
            Thread.Sleep((int)timeToSleep);
            Console.WriteLine("[Watek2] Czekałem " + timeToSleep.ToString() + "ms");
        }
        static void zad1()
        {
            ThreadPool.QueueUserWorkItem(ThreadProc1, new object[] { 300 });
            ThreadPool.QueueUserWorkItem(ThreadProc2, new object[] { 600 });
            Thread.Sleep(1000);
        }

        //==================ZAD2===================
        static void ThreadServer(Object stateInfo)
        {
            TcpListener server = new TcpListener(IPAddress.Parse("127.0.0.1"), 2599);
            server.Start();
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                byte[] buffer = new byte[1024];
                client.GetStream().Read(buffer, 0, 1024);
                //var str = System.Text.Encoding.Default.GetString(buffer);
                //Console.WriteLine(str);
                client.Close();
            }
        }
        static void ThreadClient1(Object stateInfo)
        {
            TcpClient client = new TcpClient();
            client.Connect(IPAddress.Parse("127.0.0.1"), 2599);
            client.GetStream().Write(new System.Text.ASCIIEncoding().GetBytes("[Klient1] polaczony"), 0, 19);

        }
        static void ThreadClient2(Object stateInfo)
        {
            TcpClient client = new TcpClient();
            client.Connect(IPAddress.Parse("127.0.0.1"), 2599);
            client.GetStream().Write(new System.Text.ASCIIEncoding().GetBytes("[Klient2] polaczony"), 0, 19);

        }
        static void zad2()
        {
            ThreadPool.QueueUserWorkItem(ThreadServer);
            ThreadPool.QueueUserWorkItem(ThreadClient1);
            ThreadPool.QueueUserWorkItem(ThreadClient2);
            Thread.Sleep(2000);
        }

        //=================ZAD3===================
        static void ConnectionAccepter(Object stateInfo)
        {
            
        }
        static void ThreadServer2(Object stateInfo)
        {
            TcpListener server = new TcpListener(IPAddress.Parse("127.0.0.1"), 2599);
            server.Start();
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                ThreadPool.QueueUserWorkItem(ConnectionAccepter);
                byte[] buffer = new byte[1024];
                client.GetStream().Read(buffer, 0, 1024);
                var str = System.Text.Encoding.Default.GetString(buffer);
                Console.WriteLine(str, Console.ForegroundColor = ConsoleColor.Green);
                client.GetStream().Write(new System.Text.ASCIIEncoding().GetBytes(str), 0, str.Length);
                client.Close();
            }

        }
        static void ThreadClient3(Object stateInfo)
        {
            TcpClient client = new TcpClient();
            byte[] buffer = new byte[1024];
            client.Connect(IPAddress.Parse("127.0.0.1"), 2599);
            client.GetStream().Write(new System.Text.ASCIIEncoding().GetBytes("[Klient3] polaczony"), 0, 19);
            NetworkStream stream = client.GetStream();
            stream.Read(buffer, 0, 1024);
            var str = System.Text.Encoding.Default.GetString(buffer);
            Console.WriteLine(str, Console.ForegroundColor = ConsoleColor.Red);
        }
        static void ThreadClient4(Object stateInfo)
        {
            TcpClient client = new TcpClient();
            byte[] buffer = new byte[1024];
            client.Connect(IPAddress.Parse("127.0.0.1"), 2599);
            client.GetStream().Write(new System.Text.ASCIIEncoding().GetBytes("[Klient4] polaczony"), 0, 19);
            NetworkStream stream = client.GetStream();
            stream.Read(buffer, 0, 1024);
            var str = System.Text.Encoding.Default.GetString(buffer);
            Console.WriteLine(str, Console.ForegroundColor = ConsoleColor.Red);
        }
        static void zad3()
        {
            ThreadPool.QueueUserWorkItem(ThreadServer2);
            ThreadPool.QueueUserWorkItem(ThreadClient3);
            ThreadPool.QueueUserWorkItem(ThreadClient4);
            Thread.Sleep(2000);
        }

        //================ZAD4===================
        public static Object thisLock = new object();
        static void ThreadServer3(Object stateInfo)
        {
            TcpListener server = new TcpListener(IPAddress.Parse("127.0.0.1"), 2599);
            server.Start();
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                ThreadPool.QueueUserWorkItem(ConnectionAccepter);
                byte[] buffer = new byte[1024];
                client.GetStream().Read(buffer, 0, 1024);
                var str = System.Text.Encoding.Default.GetString(buffer);
                lock (thisLock)
                {
                    Console.WriteLine(str, Console.ForegroundColor = ConsoleColor.Green);
                }
                client.GetStream().Write(new System.Text.ASCIIEncoding().GetBytes(str), 0, str.Length);
                client.Close();
            }

        }
        static void ThreadClient5(Object stateInfo)
        {
            TcpClient client = new TcpClient();
            byte[] buffer = new byte[1024];
            client.Connect(IPAddress.Parse("127.0.0.1"), 2599);
            client.GetStream().Write(new System.Text.ASCIIEncoding().GetBytes("[Klient5] polaczony"), 0, 19);
            NetworkStream stream = client.GetStream();
            stream.Read(buffer, 0, 1024);
            var str = System.Text.Encoding.Default.GetString(buffer);
            lock (thisLock)
            {
                Console.WriteLine(str, Console.ForegroundColor = ConsoleColor.Red);
            }
        }
        static void ThreadClient6(Object stateInfo)
        {
            TcpClient client = new TcpClient();
            byte[] buffer = new byte[1024];
            client.Connect(IPAddress.Parse("127.0.0.1"), 2599);
            client.GetStream().Write(new System.Text.ASCIIEncoding().GetBytes("[Klient6] polaczony"), 0, 19);
            NetworkStream stream = client.GetStream();
            stream.Read(buffer, 0, 1024);
            var str = System.Text.Encoding.Default.GetString(buffer);
            lock (thisLock)
            {
                Console.WriteLine(str, Console.ForegroundColor = ConsoleColor.Red);
            }
        }
        static void zad4()
        {
            ThreadPool.QueueUserWorkItem(ThreadServer3);
            ThreadPool.QueueUserWorkItem(ThreadClient5);
            ThreadPool.QueueUserWorkItem(ThreadClient6);
            Thread.Sleep(2000);
        }

        static void Main(string[] args)
        {
            //zad1();
            //zad2();
            //zad3();
            zad4();
        }
    }
}
