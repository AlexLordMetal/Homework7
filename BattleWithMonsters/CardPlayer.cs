using System.Collections.Generic;

namespace BattleWithMonsters
{
    public class CardPlayer : Player
    {
        public List<Card> Cards { get; set; }

        public CardPlayer() { }

        public CardPlayer(string name, int money = 100)
        {
            Name = name;
            Money = money;
            Cards = new List<Card>();
        }

    }
}
