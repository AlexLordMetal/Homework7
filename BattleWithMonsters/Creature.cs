namespace BattleWithMonsters
{
    public class Creature
    {
        private int hp;

        public string Name { get; set; }
        public int HP
        {
            get
            {
                return hp;
            }
            set
            {
                if (value >= 0) hp = value;
                else hp = 0;
            }
        }
        public int MP { get; set; }
        public int STR { get; set; }
        public int INT { get; set; }
        public Weapon Weapon { get; set; }
    }
}
