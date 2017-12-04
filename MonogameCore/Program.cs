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
            game = new GameWindow(1920);
            game.SetLoad(Load);
            game.Run();
        }

        private void Load()
        {
            TextureManager.LoadTexture("block", "block", false);
            TextureManager.LoadTexture("suprise", "suprise", false);
            TextureManager.LoadTexture("dude", "player", false);
            TestMenu testMenu = new TestMenu();
            TestGame testGame = new TestGame();
            game.states.AddState("menu", testMenu);
            game.states.AddState("game", testGame);
            game.states.SetStartingState("menu");
        }
    }
}