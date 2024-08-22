using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace movieManager.Models
{
    public class Book
    {
        // Kitap ID'si
        public int Id { get; set; }

        // Kitap ismi
        public string Name { get; set; }

        // Kitap türü
        public string Type { get; set; }

        // Kitap fiyatı
        public float Price { get; set; }

        // Yazar
        public string Author { get; set; }

        // Yayın yılı
        public int PublicationYear { get; set; }

        // ISBN numarası
        public string ISBN { get; set; }

        // Sayfa sayısı
        public int PageCount { get; set; }

        // Yayıncı
        public string Publisher { get; set; }

        // Durum (örneğin, kullanılabilir, ödünç alındı vb.)
        public string Status { get; set; }

        // Kütüphanedeki yeri
        public string Location { get; set; }

        // Alan kişi
        public int VisiterId { get; set; }

        // Ödünç verilmesinden itibaren geçen süre
        public int Total { get; set; }

        // Ceza durumu
        public bool Criminal { get; set; }

        public Book(int id, string name, string type, float price, string author, int publicationYear, string isbn, int pageCount, string publisher, string status, string location, int visiterId, int total, bool criminal)
        {
            this.Id = id;
            this.Name = name;
            this.Type = type;
            this.Price = price;
            this.Author = author;
            this.PublicationYear = publicationYear;
            this.ISBN = isbn;
            this.PageCount = pageCount;
            this.Publisher = publisher;
            this.Status = status;
            this.Location = location;
            this.VisiterId = visiterId;
            this.Total = total;
            this.Criminal = criminal;
        }

        public override string ToString()
        {
            return $"{Id} - {Name} - {Type} - {Price} - {Author}";
        }
    }
}
