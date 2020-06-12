using System;
using System.Collections.Generic;
using System.Text;

namespace MovieManager
    // The Class MovieCollection repesents a collection of movie DVDs 
    // (a collection of the objects of the class Movie)
    // MovieCollection uses Binary Search Tree as a class member to store the movie DVDs
{
    class Node
    {
        public Node LeftNode { get; set; }
        public Node RightNode { get; set; }
        public Movie Data { get; set; }
    }
    class MovieCollection
    {
        public Node Root { get; set; } // create root node

        /// <summary>
        /// Adds a movie to the movie collection in alphabetical order using a BST
        /// </summary>
        /// <param name="movie">Movie to add into the BST</param>
        /// <returns>The movie added</returns>
        public bool AddMovie(Movie movie)
        {
            Node before = null, after = Root;

            while (after != null)
            {
                before = after;
                if (movie.movie_title.CompareTo(after.Data.movie_title) < 0)
                {
                    after = after.LeftNode;
                }
                else if (movie.movie_title.CompareTo(after.Data.movie_title) > 0)
                {
                    after = after.RightNode;
                }
                else
                {
                    return false;
                }
            }

            Node newNode = new Node(); // new node to insert into the tree
            newNode.Data = movie; // set the node data to the movie object

            if (Root == null) 
            {
                Root = newNode;
            }
            else
            {
                if (movie.movie_title.CompareTo(before.Data.movie_title) < 0)
                {
                    before.LeftNode = newNode;
                }
                else
                {
                    before.RightNode = newNode;
                }
            }
            return true;
        }

        /// <summary>
        /// Calls Remove() to remove a movie
        /// </summary>
        /// <param name="movie">Movie to be removed</param>
        public void RemoveMovie(Movie movie)
        {
            Root = Remove(Root, movie);
        }

        /// <summary>
        /// Removes a movie
        /// </summary>
        /// <param name="root"></param>
        /// <param name="movie"></param>
        /// <returns>Node to be removed</returns>
        Node Remove(Node root, Movie movie)
        {
            // if the BST is empty return it
            if (root == null) return root;

            // Otherwise repeat down the BST 
            if (movie.movie_title.CompareTo(root.Data.movie_title) < 0)
                root.LeftNode = Remove(root.LeftNode, movie);
            else if (movie.movie_title.CompareTo(root.Data.movie_title) > 0)
                root.RightNode = Remove(root.RightNode, movie);

            // if movie is the same as the root movie, then set this as the node to remove 
            else
            {
                // node with only one child or no child  
                if (root.LeftNode == null)
                    return root.RightNode;
                else if (root.RightNode == null)
                    return root.LeftNode;

                // node with two children: Get the inorder successor (smallest in the right subtree)
                root.Data = minValue(root.RightNode);

                // Remove the inorder successor  
                root.RightNode = Remove(root.RightNode, root.Data);
            }
            return root;
        }

        /// <summary>
        /// Gets the smallest value
        /// </summary>
        /// <param name="root">Root node</param>
        /// <returns>Movie with the smallest value</returns>
        Movie minValue(Node root)
        {
            Movie minv = root.Data;
            while (root.LeftNode != null)
            {
                minv = root.LeftNode.Data;
                root = root.LeftNode;
            }
            return minv;
        }

        /// <summary>
        /// Search for a movie by movie title
        /// </summary>
        /// <param name="name">String of the movie title</param>
        /// <param name="parent">Parent node</param>
        /// <returns>Node of the movie or null if the movie does not exist</returns>
        public Node searchMovie(string name, Node parent)
        {
            if (parent != null)
            {
                if (name.Equals(parent.Data.movie_title)) return parent;
                if (name.CompareTo(parent.Data.movie_title) < 0)
                    return searchMovie(name, parent.LeftNode);
                else
                    return searchMovie(name, parent.RightNode);
            }

            return null;
        }

        /// <summary>
        /// Display the details of all the movies
        /// </summary>
        /// <param name="root">Root node</param>
        public void DisplayMovies(Node root)
        {
            if (root == null)
            {
                return;
            } else
            {
                DisplayMovies(root.LeftNode);
                Console.WriteLine("\nTitle : " + root.Data.movie_title
                                + "\nStarring: " + root.Data.starring
                                + "\nDirector: " + root.Data.director
                                + "\nDuration: " + root.Data.duration
                                + "\nRelease year: " + root.Data.release_year
                                + "\nGenre: " + root.Data.genre
                                + "\nClassification: " + root.Data.classification
                                + "\nCopies Available: " + root.Data.num_copies_available
                                + "\nTimes Borrowed: " + root.Data.num_times_borrowed);
                DisplayMovies(root.RightNode);
            }
        }

        /// <summary>
        /// Display the movie titles
        /// </summary>
        /// <param name="root">Root node</param>
        public void DisplayMovieTitles(Node root)
        {
            if (root == null)
            {
                return;
            }
            else
            {
                DisplayMovieTitles(root.LeftNode);
                Console.WriteLine("Title : " + root.Data.movie_title);
                DisplayMovieTitles(root.RightNode);
            }
        }

        /// <summary>
        /// Check if the movie already exists
        /// </summary>
        /// <param name="name">String of the movie title</param>
        /// <returns>If the movie exists or not</returns>
        public bool HasMovie(string name)
        {
            bool hasMovie = false;

            if (searchMovie(name, Root) != null)
            {
                hasMovie = true;
            }

            return hasMovie;
        }

        Movie[] movies = new Movie[10];
        int counter = 0;
    
        /// <summary>
        /// Bubble sorting algorithm to sort the movies by number of times borrowed
        /// </summary>
        /// <param name="movies">Movies array</param>
        /// <param name="n">Counter integer</param>
        void BubbleSort(Movie[] movies, int n)
        {
            for (int i = 0; i < n - 1; i++)
                for (int j = 0; j < n - i - 1; j++)
                    if (movies[j].num_times_borrowed < movies[j + 1].num_times_borrowed)
                    {
                        // swap temp and arr[i] 
                        Movie temp = movies[j];
                        movies[j] = movies[j + 1];
                        movies[j + 1] = temp;
                    }
        }
        
        /// <summary>
        /// Use in order traversal for the movies collection
        /// </summary>
        /// <param name="parent">Parent node</param>
        public void TraverseInOrder(Node parent)
        {
            if (parent != null)
            {
                TraverseInOrder(parent.LeftNode);
                movies[counter] = parent.Data;
                counter++;
                TraverseInOrder(parent.RightNode);
            }
        }

        /// <summary>
        /// Display the top ten movies by number of times borrowed using in order traversal and bubble sort
        /// </summary>
        public void DisplayTopTenMovies()
        {
            TraverseInOrder(Root);
            BubbleSort(movies, counter);

            Console.WriteLine("\nCurrent Top Ten Movies\n");

            for (int i = 0; i < counter; i++)
            {
                Console.WriteLine(movies[i].movie_title);
            }

            for (int j = 0; j < counter; j++)
            {
                movies[j] = null;
            }

            counter = 0;
        }
    }
}
