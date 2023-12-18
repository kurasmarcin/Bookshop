namespace Bookshop.Models
{
    public class Book
    {
            public int Id { get; set; }
            public string Tytul { get; set; }
            public string ImieAutor { get; set; }
            public string NazwiskoAutor { get; set; }
            public string Gatunek { get; set; }
            public string Opis { get; set; }
            public int RokWydania { get; set; }
            public decimal Cena { get; set; }
            public string Okladka { get; set; }
    }
}
