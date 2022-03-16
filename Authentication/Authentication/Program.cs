using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Authentication
{
    internal class Program
    {
        static List<User> Accounts = new List<User>();
        static void Main(string[] args)
        {
            string userInput;
            do
            {
                userInput = pemilihanMenu();
                prosesInput(userInput);
            } while (userInput != "5");
        }
        static string pemilihanMenu()
        {
            Console.WriteLine("**BASIC USER AUTHENTICATION**");
            Console.WriteLine("1. Create");
            Console.WriteLine("2. Show");
            Console.WriteLine("3. Search/Remove");
            Console.WriteLine("4. Login");
            Console.WriteLine("5. Exit");
            Console.Write("Input : "); return Console.ReadLine();
        }
        static void prosesInput(string input)
        {
            if (input == "1")
            {
                string firstName, lastName, userName, password, patternA, patternB, patternC, patternD;
                Boolean validPassword;
                Console.Clear();
                Console.WriteLine("==CREATE USER==");
                do
                {
                    Console.Write("First Name: ");
                    firstName = Console.ReadLine();
                    if (firstName.Contains(" "))
                    {
                        Console.WriteLine("Tidak boleh ada spasi.");
                    }
                    if (firstName.Length < 2)
                    {
                        Console.WriteLine("Tidak boleh kurang dari dua huruf");
                    }
                } while (firstName.Contains(" ") || firstName.Length < 2);
                do
                {
                    Console.Write("Last Name: ");
                    lastName = Console.ReadLine();
                    if (lastName.Contains(" "))
                    {
                        Console.WriteLine("Tidak boleh ada spasi.");
                    }
                    if (lastName.Length < 2)
                    {
                        Console.WriteLine("Tidak boleh kurang dari dua huruf");
                    }
                } while (lastName.Contains(" ") || lastName.Length < 2);
                Console.Write("Username: ");
                userName = $"{firstName.Substring(0, 2)}{lastName.Substring(0, 2)}";
                //add a check here for a potential duplicate username
                int duplicate = 0;
                for (int i = 0; i < Accounts.Count; i++)
                {
                    if (Accounts[i].getUserName() == userName)
                    {
                        duplicate += 1;
                    }
                }
                if (duplicate > 0)
                {
                    userName += duplicate;
                }
                Console.WriteLine(userName);
                //password pattern requirements
                patternA = @"[a-z]";
                patternB = @"[A-Z]";
                patternC = @"[0-9]";
                patternD = @"[^A-Za-z0-9]";
                do
                {
                    Console.WriteLine("Password (Minimal 8 karakter mengandung alfabet besar & kecil, numerik, dan non-alfanumerik) : ");
                    password = Console.ReadLine();
                    validPassword = password.Length >= 8
                                    && Regex.IsMatch(password, patternA)
                                    && Regex.IsMatch(password, patternB)
                                    && Regex.IsMatch(password, patternC)
                                    && Regex.IsMatch(password, patternD);
                    if (!validPassword)
                    {
                        Console.WriteLine("Error: password tidak valid.");
                    }
                } while (!validPassword);
                string hashPassword = BCrypt.Net.BCrypt.HashPassword(password);
                Accounts.Add(new User(firstName, lastName, userName, hashPassword));
            }
            else if (input == "2")
            {
                Console.Clear();
                Console.WriteLine("==SHOW USER==");
                for (int i = 0; i < Accounts.Count; i++)
                {
                    Console.WriteLine("==========================");
                    Console.Write("First Name: ");
                    Console.WriteLine(Accounts[i].getFirstName());
                    Console.Write("Last Name: ");
                    Console.WriteLine(Accounts[i].getLastName());
                    Console.Write("Username: ");
                    Console.WriteLine(Accounts[i].getUserName());
                    Console.Write("Password: ");
                    Console.WriteLine(Accounts[i].getPassword());
                    Console.WriteLine("==========================");
                }
                Console.ReadKey();
            }
            else if (input == "3")
            {
                string username, fname, lname;
                List<int> userIdx = new List<int>();
                Console.Clear();
                Console.WriteLine("==SEARCH/REMOVE_USER==");
                Console.WriteLine("Based on:");
                Console.WriteLine("1. Username");
                Console.WriteLine("2. First name/last name");
                Console.Write("\nInput: ");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.Write("Enter username: ");
                        username = Console.ReadLine();
                        for (int i = 0; i < Accounts.Count; i++)
                        {
                            if (Accounts[i].getUserName() == username)
                            {
                                userIdx.Add(i);
                            }
                        }
                        break;
                    case "2":
                        Console.Write("Enter first name: ");
                        fname = Console.ReadLine();
                        Console.Write("Enter last name: ");
                        lname = Console.ReadLine();
                        for (int i = 0; i < Accounts.Count; i++)
                        {
                            if (Accounts[i].getFirstName() == fname || Accounts[i].getLastName() == lname)
                            {
                                userIdx.Add(i);
                            }
                        }
                        break;
                }
                int foundCount = userIdx.Count;
                if (foundCount > 0)
                {
                    string ans, usrname;
                    switch (foundCount)
                    {
                        case 1:
                            User choosen = Accounts[userIdx[0]];
                            Console.WriteLine("==========================");
                            Console.Write("First Name: ");
                            Console.WriteLine(choosen.getFirstName());
                            Console.Write("Last Name: ");
                            Console.WriteLine(choosen.getLastName());
                            Console.Write("Username: ");
                            Console.WriteLine(choosen.getUserName());
                            Console.Write("Password: ");
                            Console.WriteLine(choosen.getPassword());
                            Console.WriteLine("==========================");
                            usrname = choosen.getUserName();
                            Console.WriteLine($"Apakah anda ingin menghapus {usrname}? (Y/y)Ya / (T/t)Tidak");
                            ans = Console.ReadLine();
                            if (ans.ToLower() == "y")
                            {
                                Accounts.Remove(choosen);
                                Console.WriteLine($"{usrname} Terhapus.");
                            }
                            break;
                        case int n when (n > 1):
                            for (int i = 0; i < userIdx.Count; i++)
                            {
                                Console.WriteLine($"({i + 1})");
                                Console.WriteLine("==========================");
                                Console.Write("First Name: ");
                                Console.WriteLine(Accounts[userIdx[i]].getFirstName());
                                Console.Write("Last Name: ");
                                Console.WriteLine(Accounts[userIdx[i]].getLastName());
                                Console.Write("Username: ");
                                Console.WriteLine(Accounts[userIdx[i]].getUserName());
                                Console.Write("Password: ");
                                Console.WriteLine(Accounts[userIdx[i]].getPassword());
                                Console.WriteLine("==========================");
                            }
                            Console.WriteLine("Apakah anda ingin menghapus salah satu data? (Y)Ya / (T)Tidak");
                            ans = Console.ReadLine();
                            if (ans.ToLower().Contains("y"))
                            {
                                Console.Write("Pilih nomor dari daftar yang ingin dihapus: ");
                                int choiceNum = int.Parse(Console.ReadLine());
                                choosen = Accounts[userIdx[choiceNum - 1]];
                                Console.WriteLine("==========================");
                                Console.Write("First Name: ");
                                Console.WriteLine(choosen.getFirstName());
                                Console.Write("Last Name: ");
                                Console.WriteLine(choosen.getLastName());
                                Console.Write("Username: ");
                                Console.WriteLine(choosen.getUserName());
                                Console.Write("Password: ");
                                Console.WriteLine(choosen.getPassword());
                                Console.WriteLine("==========================");
                                usrname = choosen.getUserName();
                                Console.WriteLine($"Apakah anda ingin menghapus {usrname}? (Y/y)Ya / (T/y)Tidak");
                                ans = Console.ReadLine();
                                if (ans.ToLower() == "y")
                                {
                                    Accounts.Remove(choosen);
                                    Console.WriteLine($"{usrname} Terhapus.");
                                }
                            }
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("No user found.");
                }
                Console.ReadKey();
            }
            else if (input == "4")
            {
                Console.Clear();
                if (Accounts.Count == 0)
                {
                    Console.WriteLine("No user exists.\n(Press ENTER to go back)");
                    Console.Read();
                    return;
                }
                string username, password;
                Console.WriteLine("==LOGIN_USER==");
                Console.WriteLine("(Enter 'x' to return to main menu)");
                Boolean usernameFound = false;
                do
                {
                    Console.Write("Enter username: ");
                    username = Console.ReadLine();
                    for (int i = 0; i < Accounts.Count; i++)
                    {
                        if (Accounts[i].getUserName() == username)
                        {
                            usernameFound = true;
                            break;
                        }
                    }
                    if (username.ToLower() == "x")
                    {
                        Console.Clear();
                        return;
                    }
                    else if (!usernameFound)
                    {
                        Console.WriteLine("username doesn't exist.");
                    }
                } while (!usernameFound);
                Boolean passwordMatch = false;
                do
                {
                    Console.Write("Enter password: ");
                    password = Console.ReadLine();
                    for (int i = 0; i < Accounts.Count; i++)
                    {
                        if (BCrypt.Net.BCrypt.Verify(password, Accounts[i].getPassword()))
                        {
                            passwordMatch = true;
                        }
                    }
                    if (password.ToLower() == "x")
                    {
                        Console.Clear();
                        return;

                    }
                    else if (!passwordMatch)
                    {
                        Console.WriteLine("Wrong password.");
                    }
                } while (!passwordMatch);
                Console.WriteLine("Login Success.");
                Console.ReadKey();
            }
            else if (input == "5")
            {
                Console.WriteLine("Selamat Tinggal.");
                return;
            }
            Console.Clear();
        }
    }
}
