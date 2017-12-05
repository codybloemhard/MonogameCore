using System;
using System.Collections.Generic;

namespace Core
{
    public enum DEBUGMODE
    {
        RELEASE,
        DEBUG,
        PROFILING
    }

    public static class Debug
    {
        private static float timer;
        private static DEBUGMODE mode;

        internal static int dynamicObjects;
        internal static int staticObjects;

        public static DEBUGMODE Mode { get { return mode; } }
        public static float printInterval = 1.0f;
        public static bool printErrors = true;
        public static bool drawLines = false;
        public static bool printData = false;
        public static bool printNotifications = false;
        public static bool printPerformance = false;
        
        public static void ReleaseMode()
        {
            mode = DEBUGMODE.RELEASE;
            printErrors = false;
            drawLines = false;
            printData = false;
            printNotifications = false;
            printPerformance = false;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[Engine] :: Release Mode activated!");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void FullDebugMode()
        {
            mode = DEBUGMODE.DEBUG;
            printErrors = true;
            drawLines = true;
            printData = true;
            printNotifications = true;
            printPerformance = true;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[Engine] :: Debug Mode activated!");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void ProfilingMode()
        {
            mode = DEBUGMODE.PROFILING;
            printErrors = true;
            drawLines = false;
            printData = false;
            printNotifications = false;
            printPerformance = true;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[Engine] :: Profiling Mode activated!");
            Console.ForegroundColor = ConsoleColor.White;
        }

        internal static void PrintError(string error)
        {
            if (!printErrors) return;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[ERROR] :: " + error);
            Console.ForegroundColor = ConsoleColor.White;
        }

        internal static void PrintError<T>(string error, T val)
        {
            if (!printErrors) return;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[ERROR] :: " + error);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(val);
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void PrintDebug<T>(string msg, T val)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("[Debug] :: " + msg);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(val);
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void PrintDebug(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[Debug] :: " + msg);
            Console.ForegroundColor = ConsoleColor.White;
        }

        internal static void PrintNotification(string msg)
        {
            if (!printNotifications) return;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[Notification] :: " + msg);
            Console.ForegroundColor = ConsoleColor.White;
        }

        internal static void PrintNotification<T>(string msg, T val)
        {
            if (!printNotifications) return;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("[Notification] :: " + msg);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(val);
            Console.ForegroundColor = ConsoleColor.White;
        }

        internal static void Update(float time)
        {
            timer += time;
            if (timer >= printInterval)
                timer = 0.0f;
            else return;
            if (printData)
            {
                PrintDebug("Dynamics: ", dynamicObjects);
                PrintDebug("Statics: ", staticObjects);
            }
            if (printPerformance)
            {
                PrintDebug("FPS: ", Time.Fps);
                PrintDebug("Memory: ", GC.GetTotalMemory(false));
            }
        }
    }
}