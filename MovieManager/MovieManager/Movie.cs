using System;
using System.Collections.Generic;
using System.Text;

namespace MovieManager
    // The class Movie models a movie DVD
    // Each movie DVD is represented by an object of the class Movie
{
    class Movie
    {
        public string movie_title { get; set; }
        public string starring { get; set; }
        public string director { get; set; }
        public string duration { get; set; }
        public string genre { get; set; }
        public string release_year { get; set; }
        public string classification { get; set; }
        public int original_copies_available { get; set; }
        public int num_copies_available { get; set; } 
        public int num_times_borrowed { get; set; }
    }
}
