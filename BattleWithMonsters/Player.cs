using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleWithMonsters
{
    public class Player : Creature
    {
        public int EXP { get; set; }
        public int LVL { get; set; }
        public List<Item> Inventory { get; set; }
        public int Money { get; set; }

        public bool IfLvlUp()
        {
            var ifUp = false;
            switch (LVL)
            {
                case 1:
                    if (EXP >= 1000) { lvlUp(1000); ifUp = true; }
                    break;
                case 2:
                    if (EXP >= 2000) { lvlUp(2000); ifUp = true; }
                    break;
                case 3:
                    if (EXP >= 4000) { lvlUp(4000); ifUp = true; }
                    break;
                case 4:
                    if (EXP >= 7000) { lvlUp(7000); ifUp = true; }
                    break;
                case 5:
                    if (EXP >= 10000) { lvlUp(10000); ifUp = true; }
                    break;
                case 6:
                    if (EXP >= 15000) { lvlUp(15000); ifUp = true; }
                    break;
                default:
                    break;
            }
            if (ifUp) IfLvlUp();
            return ifUp;
        }

        private void lvlUp(int lvlExp)
        {
            EXP -= lvlExp;
            LVL++;
            Console.WriteLine($"Congrats! Now your LVL is {LVL}"); //!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        }

        public void Report()
        {
            Console.WriteLine($"Your name is {Name}, level - {LVL}, weapon - {Weapon.Name} (attack: {Weapon.MinDamage} - {Weapon.MaxDamage}), and you have {Money} gold.\n");
        }
    }
}
