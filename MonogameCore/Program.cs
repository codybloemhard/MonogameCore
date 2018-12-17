using System;
using MonogameCore.Test;
using Core;

namespace MonogameCore
{
    public static class Program
    {        
        [STAThread]
        static void Main()
        {
            RunGame game = new RunGame();
        }
    }
    
    public class RunGame
    {
        private GameWindow game;

        private void Test()
        {
            object legacyWMPCheck = Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\Software\Microsoft\Active Setup\Installed Components\{22d6f312-b0f6-11d0-94ab-0080c74c7e95}", "IsInstalled", null);
            if (legacyWMPCheck == null || legacyWMPCheck.ToString() != "1")
            {
                /*Console.WriteLine("It appears that you don't have Windows Media Player installed. This game needs system features bound to Windows Media Player. Please install the Media Feature Pack corresponding to your Windows version to run this game:"
                    + "Windows 10: https://www.microsoft.com/en-US/download/details.aspx?id=48231"
                */
                Console.WriteLine("wmp not here");
                return;
            }
        }

        public RunGame()
        {
            Test();
            game = new GameWindow(1600);
            game.SetLoad(Load);
            Debug.ProfilingMode();
            game.Run();
        }
        
        private void Load()
        {
            TextureManager.LoadTexture("block", "block");
            TextureManager.LoadTexture("suprise", "suprise");
            TextureManager.LoadTexture("dude", "player");
            TextureManager.LoadTexture("animNumbers", "animatie0", 4, 2);
            TextureManager.LoadTexture("animLetters", "animatie1", 5, 2);
            TextureManager.LoadTexture("tiletest", "block", 0, 0);
            AudioManager.LoadEffect("bleep", "blocklock");
            AudioManager.LoadTrack("music", "beethoven");
            TestMenu testMenu = new TestMenu();
            TestGame testGame = new TestGame();
            game.states.AddState("menu", testMenu);
            game.states.AddState("game", testGame);
            game.states.SetStartingState("menu");
        }
    }
}