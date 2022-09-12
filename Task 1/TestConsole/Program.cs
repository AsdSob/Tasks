using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace monitor
{
    class Program
    {
        static string _processName;
        static double _processLifeTime;
        static int _frequency;

        static async Task Main(string[] args)  // async Task Main
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            if (!SetArguments(args)) return;

            // добавить CancellationToken 
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;

            // async () => ProcessChecker(cancellationToken);
            Task task = new Task(async () => ProcessChecker(token), token);

            task.Start();

            while (!token.IsCancellationRequested)
            {
                if (Console.ReadKey().Key == ConsoleKey.Q)
                {
                    cancellationTokenSource.Cancel();
                }
            }
        }

        private static void ProcessChecker(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var processes = Process.GetProcessesByName(_processName);

                foreach (var process in processes)
                {
                    if (!CheckLifeTime(process)) continue;

                    Console.WriteLine($"{process.ProcessName} ({process.MainWindowTitle}) was killed : {LogEnds[new Random().Next(0,LogEnds.Length)]} \n");
                    process.Kill();
                }

                Console.WriteLine(".");

                //Thread.Sleep(_frequency * 100);

                // Task.Delay + CancelationToken
                Task.Delay(2000, token).Start();
            }
        }

        private static bool SetArguments(string[] args) // Возвращать bool и если параметры не заданы верно выводить сообщение как правильно запускать и завершить работу
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Not enough details");
                return false;
            }

            _processName = args[0];

            // double.TryParse
            if (!double.TryParse(args[1], out _processLifeTime))
            {
                Console.WriteLine("Cannot convert process life time");
                return false;
            };

            // int.TryParse
            if (!Int32.TryParse(args[2], out _frequency))
            {
                Console.WriteLine("Cannot convert frequency time");
                return false;
            };

            //_frequency *= 60;
            Console.WriteLine($"name = {_processName} \nlife time = {_processLifeTime} min \nsleeping time = {_frequency/100} min \n");

            return true;
        }

        private static bool CheckLifeTime(Process process)
        {
            return (DateTime.Now - process.StartTime).TotalSeconds > _processLifeTime;
        }

        //extra fun
        static readonly string[] LogEnds = {
            "He was two days away from retirement",
            "I miss you bro",
            "Rest in peace",
            "I will avenge you",
            "He had beautiful life",
            "Hope its not Corona",
            "Valhalla calling you",
            "This is Thanos' work",
            "V for vendetta",
        };

    }
}
