using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace movieManager.Models
{
    public class Movie
    {
        // Film ID'si
        public int Id { get; set; }

        // Film ismi
        public string Title { get; set; }

        // Film türü
        public string Type { get; set; }

        // Yönetmen
        public string Director { get; set; }

        // Yayın yılı
        public int ReleaseYear { get; set; }

        // IMDb numarası
        public string IMDb { get; set; }

        // Süre (dakika)
        public int Duration { get; set; }

        // Yapımcı
        public string Producer { get; set; }

        public Movie(int id, string title, string type, string director, int releaseYear, string imdb, int duration, string producer)
        {
            this.Id = id;
            this.Title = title;
            this.Type = type;
            this.Director = director;
            this.ReleaseYear = releaseYear;
            this.IMDb = imdb;
            this.Duration = duration;
            this.Producer = producer;
        }

        public override string ToString()
        {
            return $"{Id} - {Title} - {Type} - {Director}";
        }
    }
}
