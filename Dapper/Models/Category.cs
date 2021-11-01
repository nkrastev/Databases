namespace BookShop.Models
{
    using System.Collections.Generic;

    public class Category
    {
        public Category()
        {
            this.BooksCategories = new HashSet<BookCategory>();
        }

        public int CategoryId { get; set; }

        public string Name { get; set; }

        public ICollection<BookCategory> BooksCategories { get; set; }
    }
}