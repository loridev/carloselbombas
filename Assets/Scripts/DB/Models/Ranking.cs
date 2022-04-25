namespace DB.Models
{
    public class Ranking
    {
        public int id;
        public int world_num;
        public int level_num;
        public int time;
        public User user;

        public Ranking(int id, int world_num, int level_num, int time, User user)
        {
            this.id = id;
            this.world_num = world_num;
            this.level_num = level_num;
            this.time = time;
            this.user = user;
        }
    }
}