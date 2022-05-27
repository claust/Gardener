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

        public TileSaved ToSaved()
        {
            return new TileSaved()
            {
                Type = Type,
                WaterLevel = WaterLevel,
                Plant = Plant?.ToSaved(),
            };
        }

        public static Tile FromSaved(TileSaved saved)
        {
            return new Tile(saved.Type)
            {
                Plant = saved.Plant == null ? null : Plant.FromSaved(saved.Plant),
                WaterLevel = saved.WaterLevel,
            };
        }
    }

    public enum TileType
    {
        Dirt,
        Grass,
        Stone,
    }
}
