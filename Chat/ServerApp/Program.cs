using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // задаём кодировку консоли по-умолчанию
            Console.OutputEncoding = Encoding.UTF8;
            // сюда помещаем Recieve()
            int recv;
            // задаём объём данных
            byte[] data = new byte[1024];
            // создаём конечную точку адреса (задаётся пользователем программы)
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
            // инициализируем сокет
            Socket newsock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // биндим сокет на получаемый адрес
            newsock.Bind(ipep);
            // ожидание
            newsock.Listen(10);
            Console.WriteLine("!-> Ожидание клиента...");
            // подключаем клиента
            Socket client = newsock.Accept();
            IPEndPoint clientep = (IPEndPoint)client.RemoteEndPoint;
            Console.WriteLine("!-> Connected with {0} at port {1}", clientep.Address, clientep.Port);
            string welcome = "----- Добро пожаловать на сервер! -----";
            // задаём кодировку приветственного сообщения
            data = Encoding.UTF8.GetBytes(welcome);
            // подсоединяем клиента
            client.Send(data, data.Length, SocketFlags.None);
            // строка клиента
            string input;
            // создаём бесконечный цикл
            while (true)
            {
                // задаём объём данных
                data = new byte[1024];
                // создаём обработку данных
                recv = client.Receive(data);
                // делаем проверку на 0
                if (recv == 0)
                    break;
                // вводим сообщение клиента
                Console.WriteLine("-> Client: " + Encoding.UTF8.GetString(data, 0, recv));
                input = Console.ReadLine();
                // перемещаем курсор
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.WriteLine("-> You: " + input);
                // получаем байтовый размер
                client.Send(Encoding.UTF8.GetBytes(input));
            }
            // отсоединение
            Console.WriteLine("!-> Disconnected from {0}", clientep.Address);
            client.Close();
            newsock.Close();
            Console.ReadLine();
        }
    }
}
