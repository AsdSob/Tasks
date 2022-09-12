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

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            SetArguments(args);

            Task.Run(ProcessChecker);

            while (true)
            {
                if (Console.ReadKey().Key == ConsoleKey.Q) break;
            }
        }

        private static void ProcessChecker()
        {
            while (true)
            {
                var processes = Process.GetProcessesByName(_processName);

                foreach (var process in processes)
                {
                    if (!CheckLifeTime(process)) continue;

                    Console.WriteLine($"{process.ProcessName} ({process.MainWindowTitle}) was killed : {LogEnds[new Random().Next(0,LogEnds.Length)]} \n");
                    process.Kill();
                }

                Console.WriteLine(".");
                Thread.Sleep(_frequency * 100);
            }
        }

        private static void SetArguments(string[] args)
        {
            if (args.Length < 3) return;

            _processName = args[0];
            _processLifeTime = Convert.ToDouble(args[1]);
            _frequency = Convert.ToInt32(args[2]) * 60;

            Console.WriteLine($"name = {_processName} \nlife time = {_processLifeTime} min \nsleeping time = {_frequency} min \n");
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
