using System;
using System.Reflection.Metadata.Ecma335;

namespace MovieManager
    /// <summary>
    /// 
    /// Menu driven program application which manages the movie DVDs
    /// For a community library.
    /// 
    /// Author: Davina Tan
    /// Student Number: N9741127
    /// Due Date: 25th May 2020
    /// 
    /// </summary>
{
    class Program
    {
        static bool isInStaffMenu = false;
        static void Main(string[] args)
        {
            RunProgram();
        }

        /// <summary>
        /// Calls all the methods and runs the program
        /// </summary>
        static void RunProgram()
        {
            // Initialise MemberCollection
            MemberCollection member_collection = new MemberCollection();

            // Initialise MovieCollection
            MovieCollection movie_collection = new MovieCollection();

            string username;

            while (true)
            {
                // Main menu screen
                int main_menu_selection = MainMenu();

                // Staff menu
                if (main_menu_selection == 1)
                {
                    bool login_success = StaffLogin();
                    while (login_success)
                    {
                        isInStaffMenu = true;
                        while (isInStaffMenu)
                        {
                            int staff_menu_selection = StaffMenu();

                            // if choice is 0 then return to main menu
                            if (staff_menu_selection == 0)
                            {
                                isInStaffMenu = false;
                                login_success = false;
                            }
                            // if choice is 1 then add a movie
                            else if (staff_menu_selection == 1)
                            {
                                // Get move title
                                Console.WriteLine("Enter the movie title: ");
                                string movie_title = Console.ReadLine();

                                // Check if the movie already exists
                                if (movie_collection.HasMovie(movie_title))
                                {
                                    // Add more copies
                                    Console.WriteLine("Enter the number of copies you would like to add: ");
                                    string copies_added_input = Console.ReadLine();

                                    int copies_added = Int32.Parse(copies_added_input);

                                    movie_collection.searchMovie(movie_title, movie_collection.Root).Data.num_copies_available += copies_added;
                                    movie_collection.searchMovie(movie_title, movie_collection.Root).Data.original_copies_available += copies_added;

                                    Console.ForegroundColor = ConsoleColor.Green;
                                    if (copies_added > 1)
                                    {
                                        Console.WriteLine("\nAdded {0} more copies of {1}", copies_added, movie_title);
                                    }
                                    else if (copies_added == 1)
                                    {
                                        Console.WriteLine("\nAdded {0} more copy of {1}", copies_added, movie_title);
                                    } else
                                    {
                                        Console.WriteLine("\nNo copies were added");
                                    }
                                    Console.ResetColor();
                                }
                                else
                                {
                                    // Add a new movie
                                    Movie movie = generateAddMovie(movie_title);
                                    movie_collection.AddMovie(movie);

                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine("\nSuccessfully added {0}", movie_title);
                                    Console.ResetColor();
                                }
                            }
                            // if choice is 2 then remove a movie
                            else if (staff_menu_selection == 2)
                            {
                                Movie movie = generateRemoveMovie();

                                // Check if the movie exists
                                if (!movie_collection.HasMovie(movie.movie_title))
                                {
                                    PrintColorMessage(ConsoleColor.Red, "\nThat movie does not exist in the library");
                                }
                                // Check if all movies have been returned before removing it
                                else if(movie_collection.searchMovie(movie.movie_title, movie_collection.Root).Data.num_copies_available != movie_collection.searchMovie(movie.movie_title, movie_collection.Root).Data.original_copies_available)
                                {
                                    PrintColorMessage(ConsoleColor.Red, "\nNot all copies have been returned!");
                                } else {
                                    // Remove the movie
                                    movie_collection.RemoveMovie(movie);

                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine("\nSuccessfully removed {0}", movie.movie_title);
                                    Console.ResetColor();
                                }
                            }
                            // if choice is 3 then register a member
                            else if (staff_menu_selection == 3)
                            {
                                isInStaffMenu = member_collection.RegisterMember();
                            }
                            // if choice is 4 then find a member's phone number
                            else if (staff_menu_selection == 4)
                            {
                                member_collection.FindPhoneNumber();
                            }
                        }

                    }

                // Member menu
                }
                else if (main_menu_selection == 2)
                {
                    bool login_success = member_collection.MemberLogin();
                    if (login_success)
                    {
                        username = member_collection.getUserName(); // get the current member's username
                        while (true)
                        {
                            int member_menu_selection = MemberMenu();

                            // if choice is 0 then return to main menu
                            if (member_menu_selection == 0)
                            {
                                break;
                            } 
                            // if choice is 1 then display all movies
                            else if (member_menu_selection == 1)
                            {
                                movie_collection.DisplayMovies(movie_collection.Root);
                            }
                            // if choice is 2 then borrow a movie
                            else if (member_menu_selection == 2)
                            {
                                Console.WriteLine("Enter movie title: ");
                                string name = Console.ReadLine();

                                // Check if the movie exists
                                if (!movie_collection.HasMovie(name))
                                {
                                    PrintColorMessage(ConsoleColor.Red, "\nThat movie does not exist in the library");
                                }
                                else
                                {
                                    // Check if the movie still has copies available
                                    if (movie_collection.searchMovie(name, movie_collection.Root).Data.num_copies_available > 0)
                                    {
                                        // Check if the member has already borrowed it
                                        if (member_collection.getMemberByName(username).CheckBorrowed(name))
                                        {
                                            PrintColorMessage(ConsoleColor.Red, "\nYou're already borrowing this movie!");
                                        }
                                        // or else borrow the movie
                                        else
                                        {
                                            // check if the user has borrowed 10 movies
                                            if (member_collection.getMemberByName(username).movies_borrowed < 10)
                                            {
                                                member_collection.getMemberByName(username).BorrowMovie(movie_collection.searchMovie(name, movie_collection.Root).Data);

                                                movie_collection.searchMovie(name, movie_collection.Root).Data.num_copies_available--;
                                                movie_collection.searchMovie(name, movie_collection.Root).Data.num_times_borrowed++;
                                                member_collection.getMemberByName(username).movies_borrowed++;

                                                Console.ForegroundColor = ConsoleColor.Green;
                                                Console.WriteLine("\nYou have borrowed {0}", name);
                                                Console.ResetColor();
                                            }
                                            else
                                            {
                                                PrintColorMessage(ConsoleColor.Red, "\nYou can only borrow 10 movies");
                                            }
                                        }
                                    }
                                    // No more copies available
                                    else
                                    {
                                        PrintColorMessage(ConsoleColor.Red, "\nThere are no more copies available.");
                                    }
                                }
                            }
                            // if choice is 3 then return a movie
                            else if (member_menu_selection == 3)
                            {
                                Console.WriteLine("Enter movie title: ");
                                string name = Console.ReadLine();

                                // Check if the movie exists
                                if (!movie_collection.HasMovie(name))
                                {
                                    PrintColorMessage(ConsoleColor.Red, "\nThat movie does not exist in the library");
                                }
                                // Check if the user has borrowed the movie
                                else if (!member_collection.getMemberByName(username).CheckBorrowed(name))
                                {
                                    PrintColorMessage(ConsoleColor.Red, "\nYou are not currently borrowing this movie.");
                                } else
                                {
                                    // Return the movie
                                    member_collection.getMemberByName(username).ReturnMovie(movie_collection.searchMovie(name, movie_collection.Root).Data);
                                    movie_collection.searchMovie(name, movie_collection.Root).Data.num_copies_available++;
                                    member_collection.getMemberByName(username).movies_borrowed--;

                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine("\nYou have returned {0}", name);
                                    Console.ResetColor();
                                }
                            }
                            // if choice is 4 then list borrowed movies
                            else if (member_menu_selection == 4)
                            {
                                member_collection.getMemberByName(username).ListBorrowedMovies();
                            }
                            // if choice is 5 then display top 10 most popular movies
                            else if (member_menu_selection == 5)
                            {
                                movie_collection.DisplayTopTenMovies();
                            }
                        }
                    }


                    // Exit program
                }
                else if (main_menu_selection == 0)
                {
                    ExitProgram();
                    break;
                }
            }
        }

        /// <summary>
        /// Displays the main menu
        /// </summary>
        /// <returns>The user input</returns>
        static int MainMenu()
        {
            int user_input;

            Console.WriteLine("\nWelcome to the Community Library"
                            + "\n===========Main Menu==========="
                            + "\n 1. Staff Login"
                            + "\n 2. Member Login"
                            + "\n 0. Exit"
                            + "\n==============================="
                            + "\n\nPlease make a selection  (1-2 or 0 to exit):");

            // Get user input
            string input = Console.ReadLine();
            user_input = Int32.Parse(input);

            return user_input;
        }

        /// <summary>
        /// Checks staff login
        /// </summary>
        /// <returns>If login was a success</returns>
        static bool StaffLogin()
        {
            bool login_success = false;

            if (staffUsernameCheck())
            {
                if (staffPasswordCheck())
                {
                    login_success = true;
                }
            }
            return login_success;
        }

        /// <summary>
        /// Checks the staff username
        /// </summary>
        /// <returns>If the staff username was correct</returns>
        static bool staffUsernameCheck()
        {
            bool staff_check = false;

            Console.WriteLine("Enter username: ");

            // While staff check is false
            while (!staff_check) {

                // Get username
                string input = Console.ReadLine();

                // Check if input is 'Staff'
                if (input == "staff")
                {
                    staff_check = true;
                } else
                {
                    PrintColorMessage(ConsoleColor.Red, "Invalid username");
                    continue;
                }
            }
            return staff_check;
        }

        /// <summary>
        /// Checks the staff password
        /// </summary>
        /// <returns>If the staff password was correct</returns>
        static bool staffPasswordCheck()
        {
            bool staff_check = false;

            Console.WriteLine("Enter password: ");

            // While staff check is false
            while (!staff_check)
            {
                // Get password
                string input = Console.ReadLine();

                // Check if input is 'today123'
                if (input == "today123")
                {
                    staff_check = true;
                } else
                {
                    PrintColorMessage(ConsoleColor.Red, "Incorrect Password");
                    continue;
                }
            }
            return staff_check;
        }

        /// <summary>
        /// Displays the staff menu
        /// </summary>
        /// <returns>The user input</returns>
        static int StaffMenu()
        {
            int user_input;

            Console.WriteLine("\n===========Staff Menu==========="
                            + "\n 1. Add a new movie DVD"
                            + "\n 2. Remove a movie DVD"
                            + "\n 3. Register a new Member"
                            + "\n 4. Find a registered member's phone number"
                            + "\n 0. Return to main menu"
                            + "\n=============================="
                            + "\n\nPlease make a selection (1-4 or 0 to return to main menu)");

            // Get user input
            string input = Console.ReadLine();
            user_input = Int32.Parse(input);

            return user_input;
        }

        /// <summary>
        /// Displays the member menu
        /// </summary>
        /// <returns>The user input</returns>
        static int MemberMenu()
        {
            int user_input;

            Console.WriteLine("\n===========Member Menu==========="
                            + "\n 1. Display all movies"
                            + "\n 2. Borrow a movie DVD"
                            + "\n 3. Return a movie DVD"
                            + "\n 4. List current borrowed movie DVDs"
                            + "\n 5. Display top 10 most popular movies"
                            + "\n 0. Return to main menu"
                            + "\n=============================="
                            + "\n\nPlease make a selection (1-5 or 0 to return to main menu)");

            // Get user input
            string input = Console.ReadLine();
            user_input = Int32.Parse(input);

            return user_input;
        }

        /// <summary>
        /// Generate a new movie to add
        /// </summary>
        /// <param name="movie_title">A string of the movie title to be added</param>
        /// <returns>A movie</returns>
        static Movie generateAddMovie(string movie_title)
        {
            // Create new movie object
            Movie movie = new Movie();

            movie.movie_title = movie_title;

            // Get starring
            Console.WriteLine("Enter the starring actor(s): ");
            string starring = Console.ReadLine();

            movie.starring = starring;

            // Get director
            Console.WriteLine("Enter the director(s): ");
            string director = Console.ReadLine();

            movie.director = director;

            // Get duration
            Console.WriteLine("Enter the duration (minutes): ");
            string duration = Console.ReadLine();

            movie.duration = duration;

            // Get release year
            Console.WriteLine("Enter the release year: ");
            string release_year = Console.ReadLine();

            movie.release_year = release_year;

            // Select the genre
            Console.WriteLine("Select the genre: "
                            + "\n 1. Drama"
                            + "\n 2. Adventure"
                            + "\n 3. Family"
                            + "\n 4. Action"
                            + "\n 5. SciFi"
                            + "\n 6. Comedy"
                            + "\n 7. Thriller"
                            + "\n 8. Other"
                            + "\n Make selection (1-8): ");
            string genre_input = Console.ReadLine();
            int genre_selection = Int32.Parse(genre_input);


            switch (genre_selection)
            {
                case 1:
                    movie.genre = "Drama";
                    break;
                case 2:
                    movie.genre = "Adventure";
                    break;
                case 3:
                    movie.genre = "Family";
                    break;
                case 4:
                    movie.genre = "Action";
                    break;
                case 5:
                    movie.genre = "SciFi";
                    break;
                case 6:
                    movie.genre = "Comedy";
                    break;
                case 7:
                    movie.genre = "Thriller";
                    break;
                case 8:
                    movie.genre = "Other";
                    break;
            }

            // Select the classification
            Console.WriteLine("Select the classification: "
                            + "\n 1. General (G)"
                            + "\n 2. Parental Guidance (PG)"
                            + "\n 3. Mature (M)"
                            + "\n 4. Mature Accompanied (MA15+)"
                            + "\n Make selection (1-4): ");
            string classification_input = Console.ReadLine();
            int classification_selection = Int32.Parse(classification_input);


            switch (classification_selection)
            {
                case 1:
                    movie.classification = "General (G)";
                    break;
                case 2:
                    movie.classification = "Parental Guidance (PG)";
                    break;
                case 3:
                    movie.classification = "Mature (M)";
                    break;
                case 4:
                    movie.classification = "Mature Accompanied (MA15+)";
                    break;
            }

            // Get number of copies available
            Console.WriteLine("Enter the number of copies available: ");
            string copies_available_input = Console.ReadLine();

            int copies_available = Int32.Parse(copies_available_input);

            movie.num_copies_available = copies_available;
            movie.original_copies_available = copies_available;

            // Set number of times borrowed to be 0
            movie.num_times_borrowed = 0;
            
            return movie;
        }

        /// <summary>
        /// Generate a movie to remove
        /// </summary>
        /// <returns>A movie</returns>
        static Movie generateRemoveMovie()
        {
            Movie movie = new Movie();

            // Get movie title
            Console.WriteLine("Enter movie title: ");
            string movie_title = Console.ReadLine();

            movie.movie_title = movie_title;

            return movie;
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

        /// <summary>
        /// Exits the program gracefully
        /// </summary>
        static void ExitProgram()
        {
            Console.Write("Press any key to exit");
            Console.ReadKey();
        }
    }
}
