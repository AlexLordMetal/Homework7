using System;

namespace BattleWithMonsters
{
    public class Card
    {
        public Suits CardSuit { get; set; }
        public Values CardValue { get; set; }

        public void Report()
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("┌─────────┐");
            Console.Write("│");
            CardPropToConsole();
            Console.WriteLine("      │");
            for (int i = 0; i < 7; i++) Console.WriteLine("│         │");
            Console.Write("│      ");
            CardPropToConsole();
            Console.WriteLine("│");
            Console.WriteLine("└─────────┘");
            Console.ResetColor();
        }

        public void CardPropToConsole()
        {
            switch (CardSuit)
            {
                case Suits.Diamonds:
                    Console.ForegroundColor = ConsoleColor.Red;
                    CardValueToConsole();
                    Console.Write("♦");
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
                case Suits.Hearts:
                    Console.ForegroundColor = ConsoleColor.Red;
                    CardValueToConsole();
                    Console.Write("♥");
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
                case Suits.Spades:
                    Console.ForegroundColor = ConsoleColor.Black;
                    CardValueToConsole();
                    Console.Write("♠");
                    break;
                case Suits.Clubs:
                    Console.ForegroundColor = ConsoleColor.Black;
                    CardValueToConsole();
                    Console.Write("♣");
                    break;
            }
        }

        public void CardValueToConsole()
        {
            switch (CardValue)
            {
                case Values.Two:
                    Console.Write("2 ");
                    break;
                case Values.Three:
                    Console.Write("3 ");
                    break;
                case Values.Four:
                    Console.Write("4 ");
                    break;
                case Values.Five:
                    Console.Write("5 ");
                    break;
                case Values.Six:
                    Console.Write("6 ");
                    break;
                case Values.Seven:
                    Console.Write("7 ");
                    break;
                case Values.Eight:
                    Console.Write("8 ");
                    break;
                case Values.Nine:
                    Console.Write("9 ");
                    break;
                case Values.Ten:
                    Console.Write("10");
                    break;
                case Values.Jack:
                    Console.Write("J ");
                    break;
                case Values.Queen:
                    Console.Write("Q ");
                    break;
                case Values.King:
                    Console.Write("K ");
                    break;
                case Values.Ace:
                    Console.Write("T ");
                    break;
            }
        }

        public int ValueToInt()
        {
            switch (CardValue)
            {
                case Values.Two:
                    return 2;
                case Values.Three:
                    return 3;
                case Values.Four:
                    return 4;
                case Values.Five:
                    return 5;
                case Values.Six:
                    return 6;
                case Values.Seven:
                    return 7;
                case Values.Eight:
                    return 8;
                case Values.Nine:
                    return 9;
                case Values.Ten:
                    return 10;
                case Values.Jack:
                    return 10;
                case Values.Queen:
                    return 10;
                case Values.King:
                    return 10;
                case Values.Ace:
                    return 11;
            }
            return 0;
        }

    }
}
