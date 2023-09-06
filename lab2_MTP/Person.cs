using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2_MTP
{
    public enum Category_
    {
        Prieteni,
        Colegi,
        Rude,
        Diversi
    };

    class Person
    {
        private string name_;
        private DateTime birthDate_;
        private string phoneNumber_;
        private string address_;
        private Category_ category_;

        public Person(string name, DateTime birthDate, string phoneNumber, string address, Category_ category)
        {
            name_ = name;
            birthDate_ = birthDate;
            phoneNumber_ = phoneNumber;
            address_ = address;
            category_ = category;
        }

        [Description("Numele complet al persoanei"), Category("Date personale")]
        public string Nume { get { return name_; } }

        [Description("Data nasterii"), Category("Date personale")]
        public DateTime DataNasterii { get { return birthDate_; } }

        [Description("Categorie"), Category("Categorie")]
        public Category_ Categorie { get { return category_; } set { category_ = value; } }

        [Description("Telefonul persoanei"), Category("Date personale")]
        public string NumarDeTelefon { get { return phoneNumber_; } set { phoneNumber_ = value; } }

        [Description("Adresa"), Category("Date personale")]
        public string Adresa { get { return address_; } set { address_ = value; } }
    }
}