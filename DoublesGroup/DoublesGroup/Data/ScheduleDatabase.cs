using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DoublesGroup
{
    public class ScheduleDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public ScheduleDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Schedule>().Wait();
        }

        public Task<List<Schedule>> GetPlayersAsync()
        {
            return _database.Table<Schedule>().ToListAsync();
        }

        public async Task UpdateSchedue(List<Schedule> schedules)
        {
            await _database.DeleteAllAsync<Schedule>();
            await _database.InsertAllAsync(schedules);
        }

        public Task<int> SaveScheduleAsync(Schedule schedule)
        {
            return _database.UpdateAsync(schedule);

        }
    }
}
