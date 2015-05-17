namespace Puzzle.Core
{
    /// <summary>
    /// Tile of game field
    /// </summary>
    public abstract class Tile
    {
        /// <summary>
        /// Tile's row index
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// Tile's column index
        /// </summary>
        public int Col { get; set; }
    }
    public class ValueTile : Tile
    {
        public int Value { get; set; }
        public ValueTile(int value)
        {
            Value = value;
        }
    }

    public class GreyTile : Tile
    {

    }

 
}
