using System;
using System.Text;
using Puzzle.Core;
using System.Text.RegularExpressions;
using System.Threading;

namespace Puzzle.CUI
{
    /// <summary>
    /// Console
    /// </summary>
    class ConsoleUI : Puzzle.IUserInterface
    {
        //Playing field
        private Field field;

        bool DisplayGeneration;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="field">Field object</param>
        public ConsoleUI(Field field)
        {
            field.DisplayPlaying += UpdateUI;
            //field.DisplayGeneration += delegate { UpdateUI(); System.Threading.Thread.Sleep(50); };
            this.field = field;
        }

        /// <summary>
        /// Start new game
        /// </summary>
        public void StartNewGame()
        {
            field.Generate();
            do
            {
                UpdateUI();
                if (field.State == GameState.SOLVED)
                {
                    Console.WriteLine("Vyhral si!");
                    field.Generate();
                }
                ProcessInput();
            } while (true);
        }

        /// <summary>
        /// Show current game state (delegated method)
        /// </summary>
        public void UpdateUI()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < field.ColumnCount; i++)
            {
                sb.Append("  " + i);
            }
            sb.AppendFormat("\n");
            char c = 'A';
            for (int i = 0; i < field.RowCount; i++)
            {
                sb.Append(c + " ");
                for (int j = 0; j < field.ColumnCount; j++)
                {
                    ValueTile valueTile = field.Tiles[i, j] as ValueTile;
                    if (valueTile != null)
                    {
                        if (valueTile.Value < 10)
                        {
                            sb.Append("0" + valueTile.Value);
                        }
                        else
                        {
                            sb.Append(valueTile.Value);
                        }
                        sb.Append(" ");
                    }
                    else
                    {
                        sb.Append("   ");
                    }
                }
                sb.AppendFormat("\n");
                c++;
            }
            Console.Clear();
            Console.WriteLine(sb.ToString());
        }

        /// <summary>
        /// Process user input
        /// </summary>
        private void ProcessInput()
        {
            Console.WriteLine("X EXIT\nN NEW\nMB4 MOVE");
            string input = Console.ReadLine();
            Regex rx = new Regex("^(?<exit>X|x)|(?<new>N|n)|(?<wasd>[wasd])|(?<gen>G|g)|(?<move>(M|m)(?<row>[a-zA-Z])(?<column>[0-9]))$");
            Match match = rx.Match(input);
            if (!match.Success)
            {
                return;
            }

            if (match.Groups["exit"].Value != "")
            {
                Environment.Exit(0);
            }
            else if (match.Groups["new"].Value != "")
            {
                field.Generate();
            }
            else if (match.Groups["move"].Value != "")
            {
                int row = Convert.ToInt32(match.Groups["row"].Value.ToLower()[0]) - 97;
                int col = Convert.ToInt32(match.Groups["column"].Value[0]) - 48;
                if (!field.MoveTile(field.Tiles[row, col]))
                {
                    Console.WriteLine("you cant do this move");
                }
            }
            else if (match.Groups["wasd"].Value != "")
            {
                char c = match.Groups["wasd"].Value[0];
                int row = field.GreyTile.Row;
                int col = field.GreyTile.Col;
                switch (c)
                {
                    case 'w':
                        if (!field.MoveTile(row - 1, col))
                            Console.WriteLine("you cant do this move");
                        break;
                    case 's':
                        if (!field.MoveTile(row + 1, col))
                            Console.WriteLine("you cant do this move");
                        break;
                    case 'a':
                        if (!field.MoveTile(row, col - 1))
                            Console.WriteLine("you cant do this move");
                        break;
                    case 'd':
                        if (!field.MoveTile(row, col + 1))
                            Console.WriteLine("you cant do this move");
                        break;
                }
            }
            else if (match.Groups["gen"].Value != "")
            {
                if (DisplayGeneration)
                {
                    field.DisplayGeneration -= delegate { UpdateUI(); System.Threading.Thread.Sleep(50); };
                    DisplayGeneration = false;
                }
                else
                {
                    field.DisplayGeneration += delegate { UpdateUI(); System.Threading.Thread.Sleep(50); };
                    DisplayGeneration = true;
                }
            }
        }
    }
}