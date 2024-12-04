using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

namespace KaskadenCounter
{
    class Program
    {

        
        static void Main(string[] args)
        {
            Console.WriteLine("Jodhnx_coding");
            CounterController controller = new CounterController(new CounterConfig(2, 59, 1, 23));
            controller.Start();
        }
    }

 

    class CounterController
    {
        private readonly CounterConfig _config;
        private readonly List<ConsoleColor> _progressBarColors;
        private int _colorIndex;

        public CounterController(CounterConfig config)
        {
            _config = config;
            _progressBarColors = new List<ConsoleColor>
            {
                ConsoleColor.Green,
                ConsoleColor.Yellow,
                ConsoleColor.Cyan,
                ConsoleColor.Magenta,
                ConsoleColor.Red
            };
            _colorIndex = 0;
        }

        public void Start()
        {
            while (true)
            {
                for (int i = 0; i < _config.NumStandardCycles; i++)
                {
                    RunCounter(0, _config.MaxStandardCount, $"Durchgang {i + 1}");
                    ChangeProgressBarColor();
                }

                RunCounter(0, _config.MaxHourCount, "Stunden");
                ChangeProgressBarColor();
                ShowResetMessage();
            }
        }

        private void RunCounter(int start, int end, string label)
        {
            for (int i = start; i <= end; i++)
            {
                DisplayCounter(i, label, end);
                Thread.Sleep(100);
            }
        }

        private void DisplayCounter(int value, string label, int max)
        {
            Console.Clear();
            DrawHeader(label);
            DrawProgressBar(value, max);
            DrawFooter(value);
        }

        private void DrawHeader(string label)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("====================");
            Console.WriteLine($"   {label.ToUpper()}   ");
            Console.WriteLine("====================");
            Console.ResetColor();
        }

        private void DrawProgressBar(int current, int max)
        {
            int totalBlocks = 30;
            int filledBlocks = (int)((double)current / max * totalBlocks);

            Console.ForegroundColor = _progressBarColors[_colorIndex]; 
            Console.Write("[");
            Console.Write(new string('█', filledBlocks));
            Console.Write(new string(' ', totalBlocks - filledBlocks));
            Console.WriteLine("]");
            Console.ResetColor();
        }

        private void DrawFooter(int value)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"  Aktueller Wert: {value:D2}");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"  Zeit: {DateTime.Now:HH:mm:ss}");
            Console.WriteLine("  (Drücke STRG+C zum Beenden)");
            Console.ResetColor();
        }

        private void ChangeProgressBarColor()
        {
            _colorIndex = (_colorIndex + 1) % _progressBarColors.Count; // Farbindex rotieren
        }

        private void ShowResetMessage()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\nZähler wird zurückgesetzt. Neuer Durchgang beginnt...");
            Thread.Sleep(2000);
            Console.ResetColor();
        }
    }

    class CounterConfig
    {
        public int NumStandardCycles { get; }
        public int MaxStandardCount { get; }
        public int MaxHourCount { get; }

        public CounterConfig(int numStandardCycles, int maxStandardCount, int hourCycles, int maxHourCount)
        {
            NumStandardCycles = numStandardCycles;
            MaxStandardCount = maxStandardCount;
            MaxHourCount = maxHourCount;
        }
    }
}
