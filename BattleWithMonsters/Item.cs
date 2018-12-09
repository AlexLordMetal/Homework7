using System;

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
