using System;
using System.Collections.Generic;

namespace Polar
{
    public class RegisterRequest
    {
        public string Email;
        public string Password;
        public string UserName;
    }

    public class RegisterAnswer
    {
        public List<Error> errors;
        public bool succeeded;
    }

    public class LoginRequest
    {
        public string login;
        public string password;
    }

    public class LoginAnswer
    {
        public bool succeeded;
        public string token;
        public Error[] errors;
    }

    public class MarkersAnswer
    {
        public bool succeeded;
        public List<AreaSuper> data;
    }

    public class ProfileAnswer
    {
        public List<Area> data;
        public Info info;
    }

    public class Info
    {
        public string email;
        public string username;
    }

    [Serializable]
    public class Area
    {
        public List<ServerMarker> markers;
        public string name;
        public int totalMarkers;
    }

    [Serializable]
    public class AreaSuper
    {
        public List<MarkerSuper> markers;
        public string name;

        public List<ServerMarker> GetMarkersCompressed()
        {
            var result = new List<ServerMarker>();

            foreach (var t in markers)
                result.Add(new ServerMarker() {id = t.id, type = t.type});

            return result;
        }
    }

    public class ServerMarker
    {
        public int id;
        public string type;
    }

    [Serializable]
    public class MarkerSuper
    {
        public int id;
        public string type;
        public string qrCode;
        public int storyId;
        public string storyText;
    }

    public class RatingAnswer
    {
        public List<Competitor> rating;
        public int userscore;
        public int userplace;
        public bool succeeded;
    }

    public class Competitor
    {
        public int score;
        public string username;
    }

    public class Error
    {
        public string code;
        public string description;
    }
}