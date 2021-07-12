using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // на сервере всё тоже самое
            Console.OutputEncoding = Encoding.UTF8;
            byte[] data = new byte[1024];
            string input, stringData;       // ввод, считываемые данные
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9050);
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // создаём точку соединения и делаем проверку на ex
            try
            {
                server.Connect(ipep);
            }
            catch (SocketException e)
            {
                Console.WriteLine("!-> Не удалось подключиться.");
                Console.WriteLine(e.ToString());
                return;
            }
            int recv = server.Receive(data);
            // задаём кодировку для считываемых данных
            stringData = Encoding.UTF8.GetString(data, 0, recv);
            Console.WriteLine(stringData);
            while (true)
            {
                input = Console.ReadLine();
                // задаём команду выхода
                if (input == "--exit")
                    break;

                Console.WriteLine("-> You: " + input);
                server.Send(Encoding.UTF8.GetBytes(input));
                data = new byte[1024];
                recv = server.Receive(data);
                stringData = Encoding.UTF8.GetString(data, 0, recv);
                byte[] utf8string = System.Text.Encoding.UTF8.GetBytes(stringData);
                Console.WriteLine("-> Server: " + stringData);
            }
            Console.WriteLine("!-> Отключение от сервера...");
            server.Shutdown(SocketShutdown.Both);
            server.Close();
            Console.WriteLine("!-> Соединение завершено!");
            Console.ReadLine();
        }
    }
}
