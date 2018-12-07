using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleWithMonsters
{
    public class Monster : Creature
    {
        private Random random = new Random();
        public int Difficulty { get; set; }
        public List<Item> Items { get; set; }

        public void Report()
        {
            Console.WriteLine($"Monster {Name}\t(HP: {HP}, MP: {MP}, STR: {STR}, INT: {INT}), Difficulty level - {Difficulty}, weapon - {Weapon} (Attack: {Weapon.MinDamage} - {Weapon.MaxDamage})");
        }

        public int GetMoney()
        {
            return Difficulty * random.Next(2, 6);
        }

        public List<Item> GetItems()
        {
            int counter = (int)(Difficulty * (random.Next(0, 4) / 2.0));
            var loot = new List<Item>();
            for (int count = 0; count < counter; count++)
            {
                loot.Add(Items[random.Next(0, Items.Count)]);
            }
            return loot;
        }

    }
}
