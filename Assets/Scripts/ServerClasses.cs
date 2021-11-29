using System;
using UnityEditor;

namespace Polar
{
    using System.Collections.Generic;

    public class RegisterRequest
    {
        public string UserName;
        public string Email;
        public string Password;
    }

    public class RegisterAnswer
    {
        public bool succeeded;
        public List<Polar.Error> errors;
    }

    public class LoginRequest
    {
        public string login;
        public string password;
    }
    
    [Serializable]
    public class LoginAnswer
    {
        public bool succeeded;

        public Error[] errors;

        public string token;
    }

    public class ProfileAnswer
    {
        public Info info;
        public List<Area> data;
    }

    public class Info
    {
        public string username;
        public string email;
    }

    public class Area
    {
        public string name;
        public List<ServerMarker> markers;
        public int totalMarkers;
    }

    public class ServerMarker
    {
        public int id;
        public string type;
    }

    public class RatingAnswer
    {
        public bool succeeded;
        public List<Competitor> competitors;
    }

    public class Competitor
    {
        public string username;
        public int score;
    }

    public class Error
    {
        public string code;
        public string description;
    }
}
