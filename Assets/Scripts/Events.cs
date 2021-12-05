using System;

namespace Polar
{
    public class RespondArgs : EventArgs
    {
        public bool error;
        public RequestMethod method;
        public string text;
    }

    public enum RequestMethod
    {
        Login,
        Register,
        Profile,
        Rating,
        Markers
    }

    public class UserArgs : EventArgs
    {
        public int markersCount;
        public User user;
    }

    public class ServerConnection : EventArgs
    {
        public bool hasConnection;
    }
}