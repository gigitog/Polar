using System.Collections.Generic;

namespace Polar
{
    public class User
    {
        public string username;
        public string email;
        public int score;
        public List<Marker> markers;
        public List<Area> areas;

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

        public string type;
        public Story story;

        public int locationId;
    }

    public class Story
    {
        public int id;
        public string text;
    }
}

