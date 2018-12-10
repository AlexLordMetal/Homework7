using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Threading;

namespace BattleWithMonsters
{
    public class AdventureGame
    {
        private Random random = new Random();

        public List<Weapon> Weapons { get; set; }
        public List<Monster> Monsters { get; set; }

        public void Start()
        {
            Weapons = WeaponsFromJson("Weapons.json");
            Monsters = MonstersFromJson("Monsters.json");
            AddMonstersItems();

            string[] savFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.sav");
            if (savFiles.Length > 0)
            {
                Console.WriteLine("What do you want to do?\n1 - Start new game\n2 - Load game from save file");
                if (ConditionParse(2) == 1) NewGame();
                else LoadGame(savFiles);
            }
            else NewGame();
        }

        public void NewGame()
        {
            var player = CreatePlayer();
            Console.Clear();
            Console.Write($"Welcome to the AdventureGame, {player.Name}!\n\nPress any key to start.");
            Console.ReadKey();
            MainMenu(player);
        }

        public void LoadGame(string[] savFiles)
        {
            Console.WriteLine("\nSave files:");
            for (int i = 0; i < savFiles.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {savFiles[i]}");
            }
            Console.Write("Select number of the save file to load:");
            var player = new CardPlayer();
            using (var readFile = new StreamReader(savFiles[ConditionParse(savFiles.Length) - 1]))
            {
                player = JsonConvert.DeserializeObject<CardPlayer>(readFile.ReadLine());
            }
            Console.WriteLine("Game has loaded.\n\nPress any key to continue");
            Console.ReadKey();
            MainMenu(player);
        }

        public CardPlayer CreatePlayer()
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

            var player = new CardPlayer()
            {
                Name = name,
                HP = 20,
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
            SaveAndExit(player);            
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
                    var cardGame = new CardGame();
                    var cardPlayer = player as CardPlayer;
                    cardGame.BlackJackGames(cardPlayer);
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

            if (Weapons.Count != 0)
            {
                WeaponsReport(Weapons);
                Console.WriteLine($"{Weapons.Count + 1} - Return to shop");
                Console.Write("\nSelect weapon to buy according to it's number: ");
                var index = ConditionParse(Weapons.Count + 1) - 1;
                if (index != Weapons.Count)
                {
                    if (player.Money >= Weapons[index].Cost)
                    {
                        player.Money -= Weapons[index].Cost;
                        player.Inventory.Add(Weapons[index]);
                        Console.WriteLine($"\n\nYou bought {Weapons[index].Name}.\n");
                    }
                    else Console.WriteLine($"You don't have enough money to buy {Weapons[index].Name}.\n");
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
                    Console.Write($"\n\nYou sold {player.Inventory[index].Name}.\n\nPress any key for return to shop");
                    player.Inventory.RemoveAt(index);
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

            var monsters = Monsters.Where(x => x.Difficulty >= player.LVL - 2 && x.Difficulty <= player.LVL + 2).OrderBy(x => x.Difficulty).ToList();

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

                    Console.Clear();
                    Console.WriteLine($"You fight with monster {monster.Name}:");
                    var isWin = Battle(player, monster);
                    if (isWin)
                    {
                        var moneyLoot = monster.GetMoney();
                        var itemsLoot = monster.GetItems();
                        Console.WriteLine($"\nYou won the {monster.Name}. Your loot is {moneyLoot} gold and these items:");
                        ItemsToSellReport(itemsLoot);
                        
                        player.Money += moneyLoot;
                        player.Inventory.AddRange(itemsLoot);

                        player.EXP += monster.Difficulty * 100;
                        player.HP = playerHP;
                        monster.HP = monsterHP;
                        player.IfLvlUp();

                        Console.Write("\nPress any key for return to tavern");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("\nYou died...\n\nPress any key for exit the game");
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
                Thread.Sleep(500);
                Attack(player, monster);
                Thread.Sleep(500);
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

        public List<Item> ItemsFromJson(string file)
        {
            var items = new List<Item>();
            if (File.Exists(file))
            {
                using (var readFile = new StreamReader(file))
                {
                    items = JsonConvert.DeserializeObject<List<Item>>(readFile.ReadLine());
                }
            }
            return items;
        }

        public List<Weapon> WeaponsFromJson(string file)
        {
            var weapons = new List<Weapon>();
            if (File.Exists(file))
            {
                using (var readFile = new StreamReader(file))
                {
                    weapons = JsonConvert.DeserializeObject<List<Weapon>>(readFile.ReadLine());
                }
            }
            weapons = weapons.OrderBy(x => x.Cost).ToList();
            return weapons;
        }

        public List<Monster> MonstersFromJson(string file)
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

        public void AddMonstersItems()
        {
            var items = ItemsFromJson("Items.json");

            foreach (var monster in Monsters)
            {
                var counter = random.Next(15, 25);
                for (int count = 0; count < counter; count++)
                {
                    monster.Items.Add(items[random.Next(0, items.Count)]);
                }

                counter = random.Next(0, 6);
                for (int count = 0; count < counter; count++)
                {
                    monster.Items.Add(Weapons[random.Next(0, Weapons.Count)]);
                }
            }
        }

        private void ItemsToSellReport(List<Item> items)
        {
            if (items.Count == 0) Console.WriteLine("No items");
            for (int index = 0; index < items.Count; index++)
            {
                Console.Write($"\t{index + 1} - ");
                items[index].ReportSellPrice();
            }
        }

        private void SaveAndExit(Player player)
        {
            Console.Clear();
            Console.WriteLine("Do you want to save your game? All unsaved data will be lost..\n1 - Yes\n2 - No");
            if (ConditionParse(2) == 1)
            {
                var cardPlayer = player as CardPlayer;
                SaveFarmGame(cardPlayer);
            }
            Console.Write($"\nGoodbye, {player.Name}...");
            Console.ReadKey();
        }

        public void SaveFarmGame(CardPlayer player)
        {
            var saved = false;
            while (saved != true)
            {
                Console.Write("Enter a name of save file: ");
                string saveName = Console.ReadLine() + ".sav";
                if (File.Exists(saveName) == true)
                {
                    Console.WriteLine("A file with this name already exists. Do you Want to overwrite it??\n1 - Yes\n2 - No");
                    if (ConditionParse(2) == 1)
                    {
                        using (StreamWriter writer = new StreamWriter(saveName))
                        {
                            writer.Write(JsonConvert.SerializeObject(player));
                        }
                        saved = true;
                    }
                }
                else
                {
                    using (StreamWriter writer = new StreamWriter(saveName))
                    {
                        writer.Write(JsonConvert.SerializeObject(player));
                    }
                    saved = true;
                }
            }
        }

    }
}
