using SQLite;

namespace DoublesGroup
{
    public class Player
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public bool Choosed { get; set; }

        public Player()
        {
            Choosed = false;
        }
    }
}