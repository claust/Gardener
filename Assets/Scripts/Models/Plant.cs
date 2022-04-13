using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    public class Plant
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Stages { get; set; }
        public int Stage { get; set; }

        public Plant(string name, string description, int stages)
        {
            Name = name;
            Description = description;
            Stages = stages;
        }
    }

}
