using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    public class Tile
    {
        public TileType Type { get; }

        public Tile(TileType type)
        {
            this.Type = type;
        }

    }
    public enum TileType
    {
        Dirt,
        Grass,
    }
}
