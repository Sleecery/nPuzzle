using System;
using System.Text;
using System.Threading;
using Puzzle.CUI;
using Puzzle;

namespace Puzzle.Core
{
    /// <summary>
    /// Game state
    /// </summary>
    public enum GameState
    {
        GENERATION,
        PLAYING,
        SOLVED
    }

    /// <summary>
    /// Field logic
    /// </summary>
    public class Field
    {
        //Shift selected tile to grey tile
        private const int MOVE_LEFT_TILE = 0;
        private const int MOVE_RIGHT_TILE = 1;
        private const int MOVE_UP_TILE = 2;
        private const int MOVE_DOWN_TILE = 3;

        ///Used in field generation
        private const int SHUFFLE_MOVEMENTS = 100;

        //Field dimension
        public int ColumnCount { get; set; }
        public int RowCount { get; set; }

        //Field of tiles
        public Tile[,] Tiles { get; set; }

        //Game state
        public GameState State { get; set; }

        public GreyTile GreyTile { get; private set; }

        /// <summary>
        /// Field constructor
        /// </summary>
        /// <param name="rowCount">Rows count</param>
        /// <param name="columnCount">Columns count</param>
        public Field(int rowCount, int columnCount)
        {
            RowCount = rowCount;
            ColumnCount = columnCount;
            Tiles = new Tile[rowCount, columnCount];

            Init();
        }

        /// <summary>
        /// Field initialization
        /// </summary>
        private void Init()
        {
            GreyTile = new GreyTile();
            GreyTile.Col = ColumnCount - 1;
            GreyTile.Row = RowCount - 1;
            int value = 1;
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    Tiles[i, j] = new ValueTile(value);
                    Tiles[i, j].Row = i;
                    Tiles[i, j].Col = j;
                    value++;
                }
            }
            Tiles[GreyTile.Row, GreyTile.Col] = GreyTile;
        }

        /// <summary>
        /// Field generator
        /// </summary>
        public void Generate()
        {
            //Set game state
            State = GameState.GENERATION;

            //Shuffle field's tiles
            Random rnd = new Random();
            int direction = MOVE_DOWN_TILE;
            for (int shifts = 0; shifts < SHUFFLE_MOVEMENTS; shifts++)
            {
                int newDirection;
                // It is possible to define next value as follows (it selects another direction from previous one): 
                //       while (direction == (newDirection = rnd.Next(0, 4)));
                // However, this generator is not so sophisticated. Use next one to get better result (field shuffling)
                newDirection = direction == 0 || direction == 1 ? rnd.Next(2, 4) : rnd.Next(0, 2);

                bool moved = false;
                switch (newDirection)
                {
                    case MOVE_LEFT_TILE: moved = MoveTile(GreyTile.Row, GreyTile.Col - 1); break;
                    case MOVE_RIGHT_TILE: moved = MoveTile(GreyTile.Row, GreyTile.Col + 1); break;
                    case MOVE_UP_TILE: moved = MoveTile(GreyTile.Row - 1, GreyTile.Col); break;
                    case MOVE_DOWN_TILE: moved = MoveTile(GreyTile.Row + 1, GreyTile.Col); break;
                }


                if (moved)
                {
                    direction = newDirection;
                }
                else
                {
                    shifts--;
                }
            }

            //Set game state
            State = GameState.PLAYING;
        }

        /// <summary>
        /// Moves specified tile identified by column and row index
        /// </summary>
        /// <param name="row">Row</param>
        /// <param name="col">Column</param>
        /// <returns>True if tile have been moved, false otherwise</returns>
        public bool MoveTile(int row, int col)
        {
            if (row >= 0 && row < RowCount && col >= 0 && col < ColumnCount)
            {
                return MoveTile(Tiles[row, col]);
            }
            return false;
        }

        /// <summary>
        /// Moves specified tile
        /// </summary>
        /// <param name="tile">Tile</param>
        /// <returns>True if tile have been moved, false otherwise</returns>
        public bool MoveTile(Tile tile)
        {
            if (Math.Abs(tile.Row - GreyTile.Row) <= 1 &&
                Math.Abs(tile.Col - GreyTile.Col) <= 1)
            {
                if (Math.Abs(tile.Row - GreyTile.Row) == 1 &&
                Math.Abs(tile.Col - GreyTile.Col) == 1)
                {
                    return false;
                }
                int x = GreyTile.Row;
                int y = GreyTile.Col;
                Tiles[x, y] = tile;
                Tiles[tile.Row, tile.Col] = GreyTile;
                GreyTile.Row = tile.Row;
                GreyTile.Col = tile.Col;
                tile.Row = x;
                tile.Col = y;
                if (IsSolved())
                    State = GameState.SOLVED;
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Tests if the game is solved
        /// </summary>
        /// <returns>True if the game is in final state, false otherwise</returns>
        private bool IsSolved()
        {
            if (State.Equals(GameState.PLAYING))
            {
                for (int i = 0; i < RowCount; i++)
                {
                    for (int j = 0; j < ColumnCount; j++)
                    {
                        ValueTile valueTile = Tiles[i, j] as ValueTile;
                        if (valueTile != null)
                        {
                            if (valueTile.Value != i * ColumnCount + j + 1)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (i != RowCount - 1 &&
                                j != ColumnCount - 1)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
    }
}