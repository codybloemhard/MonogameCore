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

        public RunGame()
        {
            game = new GameWindow(1200);
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
            TestMenu testMenu = new TestMenu();
            TestGame testGame = new TestGame();
            game.states.AddState("menu", testMenu);
            game.states.AddState("game", testGame);
            game.states.SetStartingState("menu");
        }
    }
}