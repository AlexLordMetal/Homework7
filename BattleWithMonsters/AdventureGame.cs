using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BattleWithMonsters
{
    public class AdventureGame
    {
        private Random random = new Random();

        public void Start()
        {
            var player = CreatePlayer();
            Console.Clear();
            Console.Write($"Welcome to the AdventureGame, {player.Name}!\n\nPress any key to start.");
            Console.ReadKey();
            MainMenu(player);
        }

        public Player CreatePlayer()
        {
            Console.Write("What is your name, adventurer?\nType here: ");
            var name = Console.ReadLine();

            var fists = new Weapon()
            {
                Name = "Fists",
                Weight = 0,
                Cost = 0,
                MinDamage = 1,
                MaxDamage = 2
            };

            var player = new Player()
            {
                Name = name,
                HP = 50,
                MP = 0,
                STR = 1,
                INT = 1,
                LVL = 1,
                EXP = 0,
                Money = 0,
                Weapon = fists,
                Inventory = new List<Item>()
            };

            return player;
        }

        public void MainMenu(Player player)
        {
            var isContinue = true;

            while (isContinue)
            {
                Console.Clear();
                player.Report();

                Console.Write($"1 - Shop\n2 - Tavern\n3 - Change weapon\n4 - Quit game\n\nMake your choice: ");
                var choice = ConditionParse(4);

                switch (choice)
                {
                    case 1:
                        ShopMenu(player);
                        break;
                    case 2:
                        TavernMenu(player);
                        break;
                    case 3:
                        ChangeWeapon(player);
                        break;
                    default:
                        isContinue = false;
                        break;
                }
            }
            Console.Write($"\nGoodbye, {player.Name}...");
            Console.ReadKey();
        }

        public void ShopMenu(Player player)
        {
            Console.Clear();
            player.Report();
            Console.Write($"1 - Buy weapon\n2 - Sell items\n3 - To main menu\n\nMake your choice: ");
            var choice = ConditionParse(3);

            switch (choice)
            {
                case 1:
                    BuyWeapon(player);
                    break;
                case 2:
                    SellItems(player);
                    break;
                default:
                    break;
            }
        }

        public void TavernMenu(Player player)
        {
            Console.Clear();
            player.Report();
            Console.Write($"1 - Find adventure\n2 - Play blackjack\n3 - To main menu\n\nMake your choice: ");
            var choice = ConditionParse(3);

            switch (choice)
            {
                case 1:
                    Adventure(player);
                    break;
                case 2:
                    //BlackJackGame(player);
                    break;
                default:
                    break;
            }
        }

        public void ChangeWeapon(Player player)
        {
            Console.Clear();
            player.Report();

            var weapons = player.Inventory.OfType<Weapon>().OrderBy(x => x.Name).ToList();

            if (weapons.Count != 0)
            {
                WeaponsReport(weapons);
                Console.WriteLine($"{weapons.Count + 1} - To main menu");
                Console.Write("\nSelect weapon according to it's number: ");
                var index = ConditionParse(weapons.Count + 1) - 1;
                if (index != weapons.Count)
                {
                    player.Inventory.Add(player.Weapon);
                    player.Weapon = weapons[index];
                    player.Inventory.Remove(weapons[index]);
                }
            }
            else
            {
                Console.Write("You have no weapons in your inventory.\n\nPress any key for return to main menu");
                Console.ReadKey();
            }
        }

        public void BuyWeapon(Player player)
        {
            Console.Clear();
            player.Report();

            var weapons = WeaponsFromJson("Weapons.txt");

            if (weapons.Count != 0)
            {
                WeaponsReport(weapons);
                Console.WriteLine($"{weapons.Count + 1} - Return to shop");
                Console.Write("\nSelect weapon to buy according to it's number: ");
                var index = ConditionParse(weapons.Count + 1) - 1;
                if (index != weapons.Count)
                {
                    if (player.Money >= weapons[index].Cost)
                    {
                        player.Money -= weapons[index].Cost;
                        player.Inventory.Add(weapons[index]);
                        Console.WriteLine($"\n\nYou bought {weapons[index].Name}.\n");
                    }
                    else Console.WriteLine($"You don't have enough money to buy {weapons[index].Name}.\n");
                    Console.Write("Press any key for return to shop");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.Write("No weapons to buy.\n\nPress any key for return to shop");
                Console.ReadKey();
            }
            ShopMenu(player);
        }

        public void SellItems(Player player)
        {
            Console.Clear();
            player.Report();

            if (player.Inventory.Count != 0)
            {
                ItemsToSellReport(player.Inventory);
                Console.WriteLine($"{player.Inventory.Count + 1} - Sell all items in inventory");
                Console.WriteLine($"{player.Inventory.Count + 2} - Return to shop");
                Console.Write("\nSelect weapon to buy according to it's number: ");
                var index = ConditionParse(player.Inventory.Count + 2) - 1;
                if (index < player.Inventory.Count)
                {
                    player.Money += (player.Inventory[index].Cost + 1) / 2;
                    player.Inventory.RemoveAt(index);
                    Console.Write($"\n\nYou sold {player.Inventory[index].Name}.\n\nPress any key for return to shop");
                    Console.ReadKey();
                }
                else if (index == player.Inventory.Count)
                {
                    foreach (var item in player.Inventory)
                    {
                        player.Money += (item.Cost + 1) / 2;
                    }
                    player.Inventory = new List<Item>();
                    Console.Write($"\n\nYou sold all items in your inventory.\n\nPress any key for return to shop");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.Write("You have no items to sell.\n\nPress any key for return to shop");
                Console.ReadKey();
            }
            ShopMenu(player);
        }

        public void Adventure(Player player)
        {
            Console.Clear();
            player.Report();

            var monsters = MonstersFromJson("Monsters.txt");
            monsters = monsters.Where(x => x.Difficulty >= player.LVL - 2 && x.Difficulty <= player.LVL + 2).OrderBy(x => x.Difficulty).ToList();

            if (monsters.Count != 0)
            {
                MonstersReport(monsters);
                Console.WriteLine($"{monsters.Count + 1} - Return to tavern");
                Console.Write("\nSelect monster to fight according to it's number: ");
                var index = ConditionParse(monsters.Count + 1) - 1;
                if (index != monsters.Count)
                {
                    var monster = monsters[index];
                    var playerHP = player.HP;
                    var monsterHP = monster.HP;

                    var isWin = Battle(player, monster);
                    if (isWin)
                    {
                        var moneyLoot = monster.GetMoney();
                        var itemsLoot = monster.GetItems();
                        Console.WriteLine($"You won the {monster.Name}. Your loot is {moneyLoot} gold and these items:");
                        ItemsToSellReport(itemsLoot);
                        player.Money += moneyLoot;
                        player.Inventory.AddRange(itemsLoot);

                        player.EXP = monster.Difficulty * 100;
                        player.IfLvlUp();

                        player.HP = playerHP;
                        monster.HP = monsterHP;
                        
                        Console.Write("\nPress any key for return to tavern");
                    }
                    else
                    {
                        Console.WriteLine("You died...\n\nPress any key for exit the game");
                        Console.ReadKey();
                        Environment.Exit(0);
                    }
                }
            }
            else
            {
                Console.Write("No suitable monsters to fight.\n\nPress any key for return to tavern");
                Console.ReadKey();
            }
            TavernMenu(player);
        }

        public bool Battle(Creature player, Creature monster)
        {
            while (player.HP > 0 && monster.HP > 0)
            {
                Attack(player, monster);
                if (monster.HP > 0) Attack(monster, player);
            }
            return player.HP > 0 ? true : false;
        }

        private void Attack(Creature attacker, Creature defender)
        {
            var damage = AttackPower(attacker);

            Console.Write($"\t{attacker.Name} attacks {defender.Name} with {damage} damage. ");

            Defend(defender, damage);
        }

        private int AttackPower(Creature attacker)
        {
            return random.Next(attacker.Weapon.MinDamage, attacker.Weapon.MaxDamage + 1) + attacker.STR;
        }

        private void Defend(Creature defender, int damage)
        {
            defender.HP -= damage;

            if (defender.HP > 0) Console.WriteLine($"{defender.Name} has {defender.HP} HP.");
            else Console.WriteLine($"{defender.Name} died.");
        }

        private int ConditionParse(int condition = 2147483647)
        {
            var isCorrect = false;
            int number = 0;
            while (isCorrect != true)
            {
                isCorrect = Int32.TryParse(Console.ReadLine(), out number);
                if (number <= 0 || number > condition) isCorrect = false;
                if (isCorrect == false) Console.Write("Incorrect choice! Do it once again: ");
            }
            return number;
        }

        private void WeaponsReport(List<Weapon> weapons)
        {
            if (weapons.Count == 0) Console.Write("No weapons");
            for (int index = 0; index < weapons.Count; index++)
            {
                Console.WriteLine($"\t{index + 1} - {weapons[index].Name}\t(attack: {weapons[index].MinDamage} - {weapons[index].MaxDamage})\tCost: {weapons[index].Cost} gold");
            }
        }

        private void MonstersReport(List<Monster> monsters)
        {
            if (monsters.Count == 0) Console.Write("No monsters");
            for (int index = 0; index < monsters.Count; index++)
            {
                Console.Write($"\t{index + 1} - ");
                monsters[index].Report();
            }
        }

        private List<Weapon> WeaponsFromJson(string file)
        {
            var weapons = new List<Weapon>();
            if (File.Exists(file))
            {
                using (var readFile = new StreamReader(file))
                {
                    weapons = JsonConvert.DeserializeObject<List<Weapon>>(readFile.ReadLine());
                }
            }
            return weapons;
        }

        private List<Monster> MonstersFromJson(string file)
        {
            var monsters = new List<Monster>();
            if (File.Exists(file))
            {
                using (var readFile = new StreamReader(file))
                {
                    monsters = JsonConvert.DeserializeObject<List<Monster>>(readFile.ReadLine());
                }
            }
            return monsters;
        }

        private void ItemsToSellReport(List<Item> items)
        {
            if (items.Count == 0) Console.Write("No items");
            for (int index = 0; index < items.Count; index++)
            {
                Console.Write($"\t{index + 1} - ");
                items[index].ReportSellPrice();
            }
        }
    }
}
