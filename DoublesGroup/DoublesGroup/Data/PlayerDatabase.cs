using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DoublesGroup
{
    public class PlayerDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public PlayerDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Player>().Wait();
        }

        public Task<List<Player>> GetPlayerAsync()
        {
            return _database.Table<Player>().ToListAsync();
        }

        public Task<Player> GetPlayerAsync(int id)
        {
            return _database.Table<Player>()
                            .Where(i => i.Id == id)
                            .FirstOrDefaultAsync();
        }

        async public Task<List<Player>> GetChosenPlayers()
        {
            List<Player> allPlayers = await GetPlayerAsync();
            var chosendPlayers = from player in allPlayers
                                 where player.Choosed == true
                                 select player;
            List<Player> chosenPlayersList = chosendPlayers.ToList();
            return chosenPlayersList;
        }

        public Task<int> SavePlayerAsync(Player person)
        {
            if (person.Id != 0)
            {
                return _database.UpdateAsync(person);
            }
            else
            {
                return _database.InsertAsync(person);
            }
        }

        public Task<int> DeletePlayerAsync(Player person)
        {
            return _database.DeleteAsync(person);
        }
    }
}
