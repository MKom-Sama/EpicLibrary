using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EpicLibrary
{
    abstract class Book
    {
        int ID;
        string name;
        string author;
        double rating;
        int quantity;

        public Book(int ID,string name,string author,double rating,int quantity)
        {
            this.ID = ID;
            this.name = name;
            this.author = author;
            this.rating = rating;
            this.quantity = quantity;
        }
        public abstract double getPrice();
    }

    class RentedBook : Book
    {
        double rentPricePerDay;
        DateTime dateOfPurchase;

        public RentedBook(int ID,string name,string author,int quantity,double rating,double pricePerDay)
            : base(ID,name, author, rating,quantity)
        {
            this.rentPricePerDay = pricePerDay;
            this.dateOfPurchase = DateTime.Now;
        }

        public override double getPrice()
        {
            TimeSpan daysPassed = DateTime.Now.Subtract(dateOfPurchase);
            return daysPassed.TotalDays*rentPricePerDay;
        }
    }

    class BoughtBook : Book
    {
        double price;

        public BoughtBook(int ID,string name, string author, int quantity,double rating,double price)
       : base(ID,name, author, rating,quantity)
        {
            this.price = price;
        }
        public override double getPrice()
        {
            return price;
        }
    }
}
