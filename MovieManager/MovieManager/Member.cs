using System;
using System.Collections.Generic;
using System.Text;

namespace MovieManager
    // The class Member models a library member.
    // Each registered member is represented by an object of the class Member.
    // Each member has a movie collection which stores the movies they have borrowed.
{
    class Member
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string address { get; set; }
        public int phone_number { get; set; }
        public int password { get; set; }
        public int movies_borrowed { get; set; }

        MovieCollection movie_collection = new MovieCollection();

        /// <summary>
        /// Borrow a movie
        /// </summary>
        /// <param name="movie">Movie to be borrowed</param>
        public void BorrowMovie(Movie movie)
        {
            movie_collection.AddMovie(movie);
        }

        /// <summary>
        /// Return a movie
        /// </summary>
        /// <param name="movie">Movie to be returned</param>
        public void ReturnMovie(Movie movie)
        {
            movie_collection.RemoveMovie(movie);
        }

        /// <summary>
        /// List currently borrowed movies by title
        /// </summary>
        public void ListBorrowedMovies()
        {
            movie_collection.DisplayMovieTitles(movie_collection.Root);
        }

        /// <summary>
        /// Check if the member is already borrowing the movie
        /// </summary>
        /// <param name="name">A string of the movie title</param>
        /// <returns>If the member has borrowed the movie</returns>
        public bool CheckBorrowed(string name)
        {
            bool hasMovie = false;

            if (movie_collection.searchMovie(name, movie_collection.Root) != null)
            {
                hasMovie = true;
            }

            return hasMovie;
        }

    }
}