using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace clientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            const string GIVE = "GIVE";
            const string ADD = "ADD";
            const string UPDATE = "UPDATE";
            const string REMOVE = "REMOVE";

            while (true)
            {
                var request = new Request();

                Console.WriteLine("1. Получить данные\n2. Добавить данные \n3.Обновить данные \n4.Удалить данные");
                string action = Console.ReadLine();
                switch(action)
                {
                    
                    case "1":
                        request.Action = GIVE;
                        request.Path = "user";
                        break;
                    case "2":
                        request.Action = ADD;
                        request.Path = "user";
                        Console.WriteLine("Введите Имя пользователя");
                        request.Value = Console.ReadLine();
                        break;
                    case "3":
                        request.Action = UPDATE;
                        request.Path = "user";
                        Console.WriteLine("Введите Id пользователя");
                        request.Value = Console.ReadLine();
                        Console.WriteLine("Введите новое Имя пользователя");
                        request.NewData = Console.ReadLine();
                        break;                       
                    case "4":
                        request.Action = REMOVE;
                        request.Path = "user";
                        Console.WriteLine("Введите Id пользователя");
                        request.Value = Console.ReadLine();
                        break;
                    default:
                        Console.WriteLine("Вы ошиблись");
                        break;
                }
                var json = JsonConvert.SerializeObject(request);
                using (var client = new TcpClient())
                {
                    client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3231));
                    using (var stream = client.GetStream())
                    {
                        var data = Encoding.UTF8.GetBytes(json);
                        stream.Write(data, 0, data.Length);

                        var resultText = string.Empty;
                        do
                        {
                            var buffer = new byte[128];
                            stream.Read(buffer, 0, buffer.Length);
                            resultText += System.Text.Encoding.UTF8.GetString(buffer);
                        }
                        while (stream.DataAvailable);

                        
                    }
                };
            }
        }
    }
}
