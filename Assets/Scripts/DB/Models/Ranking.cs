using System;
using Newtonsoft.Json;

namespace DB.Models
{
    public class Ranking : IComparable
    {
        public int id;
        public int world_num;
        public int level_num;
        public int time;
        public User user;

        [JsonConstructor]
        public Ranking(int id, int world_num, int level_num, int time, User user)
        {
            this.id = id;
            this.world_num = world_num;
            this.level_num = level_num;
            this.time = time;
            this.user = user;
        }

        public Ranking(int time, User user)
        {
            this.time = time;
            this.user = user;
        }

        public int CompareTo(object obj)
        {
            Ranking rankingCompare = obj as Ranking;
            
            if (rankingCompare != null)
            {
                return this.time.CompareTo(rankingCompare.time);
            }

            return 1;
        }
    }
}