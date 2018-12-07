﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleWithMonsters
{
    public class Item
    {
        public string Name { get; set; }
        public int Weight { get; set; }
        public int Cost { get; set; }

        public void ReportSellPrice()
        {
            Console.WriteLine($"{Name}\t(weight: {Weight})\tSelling Cost: {(Cost + 1) / 2} gold");
        }

    }
}
