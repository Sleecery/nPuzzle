using System;
using Puzzle.Core;
using System.Threading;
using Puzzle.CUI;

namespace Puzzle
{
    /// <summary>
    /// Starts game
    /// </summary>
    class PuzzleGame
    {
        private ConsoleUI userInterface = null;

        public PuzzleGame()
        {
            Field field = new Field(4, 4);
            userInterface = new ConsoleUI(field);
            userInterface.StartNewGame();
        }
        static int Main(string[] args)
        {
            PuzzleGame pg = new PuzzleGame();
            return 0;
        }
    }
}
