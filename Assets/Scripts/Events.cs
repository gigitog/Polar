using System;

namespace Polar
{
    public class RespondArgs : EventArgs
    {
        public string text;
        public RequestMethod method;
        public bool error;
    }
    public enum RequestMethod
    {
        Login,
        Register,
        Profile,
        Rating
    }
}
