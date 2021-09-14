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

        async public Task<List<Player>> GetChosenPlayers()
        {
            List<Player> allPlayers = await GetPlayerAsync();
            var chosendPlayers = from player in allPlayers
                                 where player.Choosed == true
                                 select player;
            List<Player> chosenPlayersList = chosendPlayers.ToList();
            return chosenPlayersList;
        }

        public Task<int> SavePersonAsync(Player person)
        {
            if (person.Id != 0)
            {
                // Update an existing note.
                return _database.UpdateAsync(person);
            }
            else
            {
                // Save a new note.
                return _database.InsertAsync(person);
            }
        }

        public Task<int> DeletePlayerAsync(Player person)
        {
            // Delete a note.
            return _database.DeleteAsync(person);
        }
    }
}
