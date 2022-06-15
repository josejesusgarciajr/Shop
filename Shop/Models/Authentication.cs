using System;
namespace Shop.Models
{
    public class Authentication
    {
        public int Key { get; set; }
        private bool State { get; set; }

        public Authentication()
        {
            State = false;
        }

        public void LogIn()
        {
            State = true;
        }

        public void LogOut()
        {
            State = false;
        }

        public bool IsLoggedIn()
        {
            return State;
        }

    }
}
