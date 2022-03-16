using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication
{
    internal class User
    {
        private string firstName;
        private string lastName;
        private string userName;
        private string password;
        public User(string fname, string lname, string username, string password)
        {
            this.firstName = fname;
            this.lastName = lname;
            this.userName = username;
            this.password = password;
        }

        public string getFirstName()
        {
            return firstName;
        }
        public string getLastName()
        {
            return lastName;
        }
        public string getUserName()
        {
            return userName;
        }
        public string getPassword()
        {
            return password;
        }
        public void setFirstName(string fname)
        {
            this.firstName = fname;
        }
        public void setLastName(string lname)
        {
            this.lastName = lname;
        }
        public void setUserName(string username)
        {
            this.userName = username;
        }
        public void setPassword(string password)
        {
            this.password = password;
        }
    }
}
