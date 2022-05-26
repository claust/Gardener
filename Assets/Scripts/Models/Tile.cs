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

        public Plant Plant { get; set; }
        public float WaterLevel { get; set; }

        public float Dryness { get { return 1 - WaterLevel; } }

        public Tile(TileType type)
        {
            this.Type = type;
            this.WaterLevel = 0;
        }

        public void Water()
        {
            WaterLevel = 1;
        }
    }

    public enum TileType
    {
        Dirt,
        Grass,
    }
}
