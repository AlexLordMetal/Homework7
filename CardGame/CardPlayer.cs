using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class CardPlayer
    {
        public string Name { get; set; }
        public int Money { get; set; }
        public List<Card> Cards { get; set; }

        public CardPlayer(string name, int money = 100)
        {
            Name = name;
            Money = money;
            Cards = new List<Card>();
        }
    }
}
