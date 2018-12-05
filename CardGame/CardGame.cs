using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame
{
    public class CardGame
    {
        public List<Card> Deck { get; set; }

        public void Start()
        {
            //First Star
            //CreateDeck();
            //ShuffleDeck();
            //DeckReport(5);
            //CardsValueReport(Values.Jack);
            //CardsSuitReport(Suits.Clubs);

            //Second Star
            //EmptyCardReport();
            //DeckInRowReport(6);
            BlackJackGames();

        }

        public void CreateDeck()
        {
            Deck = new List<Card>();
            foreach (Suits suit in Enum.GetValues(typeof(Suits)))
            {
                foreach (Values value in Enum.GetValues(typeof(Values)))
                {
                    Deck.Add(new Card { CardSuit = suit, CardValue = value });
                }
            }
        }

        public void ShuffleDeck()
        {
            Deck = Deck.OrderBy(x => Guid.NewGuid()).ToList();
        }

        public void DeckReport(int numberOfCards = -1)
        {
            if (numberOfCards == -1) Console.WriteLine("All cards in deck:");
            else if (numberOfCards > 0) Console.WriteLine($"First {numberOfCards} card(s) in deck:");
            cardsReport(Deck, numberOfCards);
        }

        private void cardsReport(List<Card> cards, int numberOfCards = -1)
        {
            if (numberOfCards == -1) numberOfCards = cards.Count;
            for (int index = 0; index < numberOfCards; index++)
            {
                cards[index].Report();
            }
        }

        public void CardsValueReport(Values minValue = Values.Two, Values maxValue = Values.Ace)
        {
            Console.WriteLine($"Cards with value >= {minValue} and <= {maxValue}:");
            cardsReport(Deck.Where(x => x.CardValue >= minValue && x.CardValue <= maxValue).ToList());
        }

        public void CardsSuitReport(Suits suit)
        {
            Console.WriteLine($"Cards with {suit} suit in deck:");
            cardsReport(Deck.Where(x => x.CardSuit == suit).ToList());
        }

        public void EmptyCardReport()
        {
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("┌─────────┐");
                for (int i = 0; i < 9; i++) Console.WriteLine("│▒▒▒▒▒▒▒▒▒│");
                Console.WriteLine("└─────────┘");
                Console.ResetColor();
            }
        }

        public void CardsInRowReport(List<Card> cards, int numberOfCards = -1)
        {
            if (numberOfCards == -1) numberOfCards = cards.Count;

            if (numberOfCards > 0)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                for (int index = 0; index < numberOfCards; index++)
                {
                    Console.Write("┌─────────┐");
                }
                Console.WriteLine();
                for (int index = 0; index < numberOfCards; index++)
                {
                    Console.Write("│");
                    cards[index].CardPropToConsole();
                    Console.Write("      │");
                }
                Console.WriteLine();

                for (int i = 0; i < 7; i++)
                {
                    for (int index = 0; index < numberOfCards; index++)
                    {
                        Console.Write("│         │");
                    }
                    Console.WriteLine();
                }

                for (int index = 0; index < numberOfCards; index++)
                {
                    Console.Write("│      ");
                    cards[index].CardPropToConsole();
                    Console.Write("│");
                }
                Console.WriteLine();

                for (int index = 0; index < numberOfCards; index++)
                {
                    Console.Write("└─────────┘");
                }
                Console.WriteLine();

                Console.ResetColor();
            }
        }

        public void PlayerCardsReport(CardPlayer player)
        {
            Console.WriteLine($"{player.Name}'s cards:");
            CardsInRowReport(player.Cards);
        }

        public void PlayerFirstCardsOpenReport(CardPlayer player)
        {
            Console.WriteLine($"{player.Name}'s cards:");
            var count = player.Cards.Count;

            if (count > 0)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;

                for (int index = 0; index < count; index++) Console.Write("┌─────────┐");
                Console.WriteLine();

                Console.Write("│");
                player.Cards[0].CardPropToConsole();
                Console.Write("      │");
                for (int index = 1; index < count; index++) Console.Write("│▒▒▒▒▒▒▒▒▒│");
                Console.WriteLine();

                for (int i = 0; i < 7; i++)
                {
                    Console.Write("│         │");
                    for (int index = 1; index < count; index++) Console.Write("│▒▒▒▒▒▒▒▒▒│");
                    Console.WriteLine();
                }

                Console.Write("│      ");
                player.Cards[0].CardPropToConsole();
                Console.Write("│");
                for (int index = 1; index < count; index++) Console.Write("│▒▒▒▒▒▒▒▒▒│");
                Console.WriteLine();

                for (int index = 0; index < count; index++) Console.Write("└─────────┘");
                Console.WriteLine();

                Console.ResetColor();
            }
        }

        public void BlackJackGames()
        {
            var player = new CardPlayer("Player");
            var croupier = new CardPlayer("Croupier");

            while (player.Money > 0 && croupier.Money > 0)
            {
                Console.Clear();
                Console.WriteLine(player.Money);
                Console.WriteLine(croupier.Money);
                BlackJackGame(player, croupier);
            }

            if (player.Money <= 0) Console.WriteLine("You lost all your money. GAME OVER.");
            if (croupier.Money <= 0) Console.WriteLine($"You won all {croupier.Name} money. GAME OVER.");
        }

        public void BlackJackGame(CardPlayer player, CardPlayer croupier)
        {
            CreateDeck();
            ShuffleDeck();
            player.Cards = new List<Card>();
            croupier.Cards = new List<Card>();

            var bet = GetPlayerBet();

            DealCards(player, 2);
            DealCards(croupier, 2);

            PlayerFirstCardsOpenReport(croupier);
            PlayerCardsReport(player);

            if (CardsValueSum(player.Cards) == 21) Console.WriteLine("You have 21.");
            else
            {
                BlackJackPlayerTurn(player);
                if (CardsValueSum(player.Cards) <= 21)
                {
                    BlackJackCroupierTurn(croupier, CardsValueSum(player.Cards));                    
                }
            }

            BlackJackGameResult(player, croupier, bet);

            Console.Write("Press any key to coninue...");
            Console.ReadKey();
        }

        private int GetPlayerBet()
        {
            Console.WriteLine("Your bet?\n1 - 5\n2 - 10\n3 - 25");
            switch (ConditionParse(3))
            {
                case 1:
                    return 5;
                case 2:
                    return 10;
                case 3:
                    return 25;
            }
            return 0;
        }

        private int ConditionParse(int condition = 2147483647)
        {
            var isCorrect = false;
            int number = 0;
            while (isCorrect != true)
            {
                isCorrect = Int32.TryParse(Console.ReadLine(), out number);
                if (number <= 0 || number > condition) isCorrect = false;
                if (isCorrect == false) Console.Write("Ввод некорректен! Еще раз: ");
            }
            return number;
        }

        private void DealCards(CardPlayer player, int numberOfCards = 1)
        {
            if (Deck.Count < numberOfCards)
            {
                Console.WriteLine("Not enough cards in deck");
            }
            else
            {
                player.Cards.AddRange(Deck.GetRange(0, numberOfCards));
                Deck.RemoveRange(0, numberOfCards);
            }
        }

        private int CardsValueSum(List<Card> cards)
        {
            var sum = 0;
            foreach (var card in cards)
            {
                sum += card.ValueToInt();
            }
            return sum;
        }

        private void BlackJackPlayerTurn(CardPlayer player)
        {
            while (CardsValueSum(player.Cards) <= 21)
            {
                Console.WriteLine($"Sum of your cards is {CardsValueSum(player.Cards)}.");
                Console.WriteLine("1 - One more card\n2 - Enough");
                if (ConditionParse(2) == 1)
                {
                    DealCards(player);
                    PlayerCardsReport(player);
                }
                else break;
            }
        }

        private void BlackJackCroupierTurn(CardPlayer player, int firstPlayerCardsSum)
        {
            PlayerCardsReport(player);
            while (CardsValueSum(player.Cards) < firstPlayerCardsSum && CardsValueSum(player.Cards) < 21)
            {
                DealCards(player);
                PlayerCardsReport(player);
            }
        }

        private void BlackJackGameResult(CardPlayer player, CardPlayer croupier, int bet)
        {
            if (CardsValueSum(player.Cards) > 21)
            {
                Console.WriteLine($"Sum of your cards more than 21 ({CardsValueSum(player.Cards)}).\n\nYOU LOSE!");
                player.Money -= bet;
                croupier.Money += bet;
            }
            else if (CardsValueSum(croupier.Cards) > 21)
            {
                Console.WriteLine($"Sum of {croupier.Name} cards more than 21 ({CardsValueSum(croupier.Cards)}).\n\nYOU WIN!");
                player.Money += bet;
                croupier.Money -= bet;
            }
            else if (CardsValueSum(player.Cards) > CardsValueSum(croupier.Cards))
            {
                Console.WriteLine($"Sum of your cards ({CardsValueSum(player.Cards)}) more than sum of {croupier.Name} cards ({CardsValueSum(croupier.Cards)})\n\nYOU WON!");
                player.Money += bet;
                croupier.Money -= bet;
            }
            else if (CardsValueSum(player.Cards) < CardsValueSum(croupier.Cards))
            {
                Console.WriteLine($"Sum of your cards ({CardsValueSum(player.Cards)}) less than sum of {croupier.Name} cards ({CardsValueSum(croupier.Cards)})\n\nYOU LOSE!");
                player.Money -= bet;
                croupier.Money += bet;
            }
            else if (CardsValueSum(player.Cards) == CardsValueSum(croupier.Cards))
            {
                Console.WriteLine($"Sum of your cards ({CardsValueSum(player.Cards)}) equal to sum of {croupier.Name} cards ({CardsValueSum(croupier.Cards)})\n\nDRAW!");
            }
        }

    }


}
