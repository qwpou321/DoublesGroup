using SQLite;

namespace DoublesGroup
{
    public class Schedule
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Players { get; set; }
        public bool Played { get; set; }
    }
}
