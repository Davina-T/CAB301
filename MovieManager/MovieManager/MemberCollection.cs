using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Xml.Serialization;

namespace MovieManager
    // The class MemberCollection represents a collection of registered members
    // (a collection of the objects of the class Member)
    // MemberCollection uses an array as a class member to store the members
{
    class MemberCollection
    {
        Member[] members = new Member[100]; // Initialise members array

        string username; 

        /// <summary>
        /// Register a new member
        /// </summary>
        /// <returns>If the user is in the staff menu</returns>
        public bool RegisterMember()
        {
            // Create new member object
            Member member = new Member();

            // Get first name
            Console.WriteLine("Enter member's first name: ");
            string first_name = Console.ReadLine();

            member.first_name = first_name;

            // Get last name
            Console.WriteLine("Enter member's last name: ");
            string last_name = Console.ReadLine();

            member.last_name = last_name;

            // Check if username exists
            for (int i = 0; i < members.Length; i++)
            {
                if (members[i] != null)
                {
                    if (members[i].first_name + members[i].last_name == first_name + last_name)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n{0} {1} has already registered!", first_name, last_name);
                        Console.ResetColor();
                        
                        return false;
                    }
                }
            }

            // Get address
            Console.WriteLine("Enter member's address: ");
            string address = Console.ReadLine();

            member.address = address;

            // Get phone number
            Console.WriteLine("Enter member's phone number: ");
            string phone_num = Console.ReadLine();
            int user_phone_num = Int32.Parse(phone_num);

            member.phone_number = user_phone_num;

            // Get password
            Console.WriteLine("Enter member's password(4 digits): ");
            bool password_check = false;
            int user_password = 0;

            // Check if the password is valid
            while (!password_check)
            {
                string password = Console.ReadLine();

                // Check if input is 4 digits
                if (password.Length != 4)
                {
                    PrintColorMessage(ConsoleColor.Red, "Password must be 4 digits");
                    continue;
                }
                // Check if the password contains letters
                else if (!int.TryParse(password, out user_password)) {
                    PrintColorMessage(ConsoleColor.Red, "Password must only have numbers");
                    continue;
                }
                else
                {
                    password_check = true;
                }

                user_password = Int32.Parse(password);
            }

            member.password = user_password;

            // Add member to members array
            for (int i = 0; i < members.Length; i++)
            {
                // Use sequential search to check if member exists
                int member_exists = SequentialSearchMember(members, member);

                // If the position is empty and the member does not exist
                if ((members[i] == null) && (member_exists == -1))
                {
                    // Add member
                    members[i] = member;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nSuccessfully added {0} {1}", first_name, last_name);
                    Console.ResetColor();
                }
            }
            return true;
        }

        /// <summary>
        /// Find a member's phone number
        /// </summary>
        public void FindPhoneNumber()
        {
            string phone_number;
            int int_phone_number = 0;

            // Get member's first name
            Console.WriteLine("Enter member's first name: ");
            string first_name_input = Console.ReadLine();

            // Get member's last name
            Console.WriteLine("Enter member's last name: ");
            string last_name_input = Console.ReadLine();

            // Check if the member does not exist
            if (getMemberByName(first_name_input + last_name_input) == null)
            {
                PrintColorMessage(ConsoleColor.Red, "\nThat user does not exist");
            } else
            {
                // Search members array to find first name position
                int username_search_result = SequentialSearchUserName(members, first_name_input + last_name_input);

                // If the first name exists
                if (username_search_result != -1)
                {
                    // Get phone number
                    int_phone_number = members[username_search_result].phone_number;
                }

                // Convert phone number to string
                phone_number = int_phone_number.ToString();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n{0} {1}'s phone number is: {2}", first_name_input, last_name_input, phone_number);
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Sequential search members array for a member
        /// </summary>
        /// <param name="members">Members array</param>
        /// <param name="member">Member to search</param>
        /// <returns>Position of the member or -1 if the member is not found</returns>
        static int SequentialSearchMember(Member[] members, Member member)
        {
            int i = 0;
            while (i < members.Length && members[i] != member)
            {
                i++;
            }
            if (i < members.Length)
            {
                return i;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Get a member's details object by their full name
        /// </summary>
        /// <param name="name">String of member's full name</param>
        /// <returns>A member or null if member is not found</returns>
        public Member getMemberByName(string name)
        {
            for (int i = 0; i < members.Length; i++)
            {
                if (members[i] != null && members[i].first_name + members[i].last_name ==  name)
                {
                    return members[i];
                }
            }
            return null; 
        }

        /// <summary>
        /// Get the current member's username
        /// </summary>
        /// <returns>Member's username (full name)</returns>
        public string getUserName()
        {
            return username;
        }

        /// <summary>
        /// Login for members
        /// </summary>
        /// <returns>If login was a success</returns>
        public bool MemberLogin()
        {
            bool login_success = false;

            int username_position = MemberUsernameCheck();

            if (username_position == -1)
            {
                PrintColorMessage(ConsoleColor.Red, "\nUsername does not exist!");
            } else if (MemberPasswordCheck(username_position))
            {
                login_success = true;
            }

            return login_success;
        }

        /// <summary>
        /// Checks the member's username
        /// </summary>
        /// <returns>Position of the member in the members array or -1 if the username does not exist</returns>
        public int MemberUsernameCheck()
        {
            Console.WriteLine("Enter username (FirstnameLastname): ");

            // Get username
            username = Console.ReadLine();

            int username_check = -1;

            // if the member does not exist
            if (getMemberByName(username) == null)
            {
                return username_check;
            }
            else
            {
                // Check where the username is in the members array
                username_check = SequentialSearchUserName(members, username);

                return username_check;
            }
        }

        /// <summary>
        /// Checks the member's password
        /// </summary>
        /// <param name="position">The position of the member's username in the members array</param>
        /// <returns>If the password is correct</returns>
        public bool MemberPasswordCheck(int position)
        {
            bool password_check = false;

            Console.WriteLine("Enter password (4-digits): ");

            while (!password_check)
            {
                // Get password
                string input = Console.ReadLine();
                int password_input;

                // Check if the password is 4 digits
                if (input.Length != 4)
                {
                    PrintColorMessage(ConsoleColor.Red, "Password must be 4 digits");
                    continue;
                }  
                // Check if the password contains letters
                else if (!int.TryParse(input, out password_input))
                {
                    PrintColorMessage(ConsoleColor.Red, "Password must only have numbers");
                    continue;
                }

                password_input = Int32.Parse(input);

                // Search for password in the user position
                int user_password = SearchPassword(members, position);

                // Check if password input is the same
                if (password_input == user_password)
                {
                    password_check = true;
                } else
                {
                    PrintColorMessage(ConsoleColor.Red, "Incorrect password");
                    continue;
                }
            }

            return password_check;
        }

        /// <summary>
        /// Search member's array for password
        /// </summary>
        /// <param name="members">Members array</param>
        /// <param name="position">Position to check password in members array</param>
        /// <returns>Password in the position of the members array or 0 if it is an incorrect password</returns>
        static int SearchPassword(Member[] members, int position)
        {
            int password = 0;

            for (int i = 0; i < members.Length; i++)
            {
                if (members[i] == members[position])
                {
                    return members[i].password;
                }
            }

            return password;
        }

        /// <summary>
        /// Sequential search members array for username
        /// </summary>
        /// <param name="members">Members array</param>
        /// <param name="name">String of the member's username (full name)</param>
        /// <returns>Position of the member or -1 if the username does not exist</returns>
        static int SequentialSearchUserName(Member[] members, String name)
        {
            int i = 0;
            while (i < members.Length && members[i].first_name + members[i].last_name != name)
            {
                i++;
            }
            if (i < members.Length)
            {
                return i;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Print colour messages
        /// </summary>
        /// <param name="colour">Colour of the text</param>
        /// <param name="message">String to be printed</param>
        static void PrintColorMessage(ConsoleColor colour, string message)
        {
            Console.ForegroundColor = colour;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
