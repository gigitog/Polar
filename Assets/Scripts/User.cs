using System.Collections.Generic;

namespace Polar
{
    public class User
    {
        public List<Area> areas;
        public string email;
        public int score;
        public string username;

        public User(ProfileAnswer answer)
        {
            username = answer.info.username;
            email = answer.info.email;
            score = 0; // TODO
            areas = answer.data;
        }
    }

    public class Marker
    {
        public int id;

        public int locationId;
        public Story story;

        public string type;
    }

    public class Story
    {
        public int id;
        public string text;
    }
}