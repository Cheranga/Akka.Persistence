using System;
using System.Linq;

namespace Console
{
    public static class DisplayHelper
    {
        public static void Show(string message, ConsoleColor textColor)
        {
            System.Console.ForegroundColor = textColor;
            System.Console.WriteLine($"{DateTime.Now:HH:mm:ss}\t{message}");
            System.Console.ResetColor();
        }

        public static void ShowInfo(string message)
        {
            Show(message, ConsoleColor.Cyan);
        }

        public static void ShowWarning(string message)
        {
            Show(message, ConsoleColor.Yellow);
        }

        public static void ShowError(string message)
        {
            Show(message, ConsoleColor.Red);
        }

        public static void ShowInfo(params object[] messages)
        {
            if (messages != null && messages.Any())
            {
                var message = string.Join(" ", messages);
                ShowInfo(message);
            }
        }

        public static void ShowWarning(params object[] messages)
        {
            if (messages != null && messages.Any())
            {
                var message = string.Join(" ", messages);
                ShowWarning(message);
            }
        }

        public static void ShowError(params object[] messages)
        {
            if (messages != null && messages.Any())
            {
                var message = string.Join(" ", messages);
                ShowError(message);
            }
        }
    }
}