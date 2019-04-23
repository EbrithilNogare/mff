using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace UnreflectedSerializer
{

    public class RootDescriptor<T>
    {
        string[] variables;
        string ownName;
        public void Serialize(TextWriter writer, T instance)
        {
            writer.WriteLine($"<{ownName}>");
            foreach (string item in variables)
            {
                writer.WriteLine($"<{item}>");
                string test = (string)instance.GetType().GetField(item).GetValue(instance);
                writer.WriteLine();
                writer.WriteLine($"</{item}>");
            }
            writer.WriteLine($"</{ownName}>");
        }
        public RootDescriptor(string ownName, string[] variables)
        {
            this.variables = variables;
            this.ownName = ownName;
        }
    }

    class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
    }

    class Country
    {
        public string Name { get; set; }
        public int AreaCode { get; set; }
    }

    class PhoneNumber
    {
        public Country Country { get; set; }
        public int Number { get; set; }
    }

    class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Address HomeAddress { get; set; }
        public Address WorkAddress { get; set; }
        public Country CitizenOf { get; set; }
        public PhoneNumber MobilePhone { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            RootDescriptor<Person> rootDesc = GetPersonDescriptor();

            var czechRepublic = new Country { Name = "Czech Republic", AreaCode = 420 };
            var person = new Person
            {
                FirstName = "David",
                LastName = "Napravnik",
                HomeAddress = new Address { Street = "Vetrna", City = "Predboj" },
                WorkAddress = new Address { Street = "U kastanu", City = "Prague" },
                CitizenOf = czechRepublic,
                MobilePhone = new PhoneNumber { Country = czechRepublic, Number = 987654321 }
            };

            rootDesc.Serialize(Console.Out, person);
            Console.ReadKey();
        }

        static RootDescriptor<Person> GetPersonDescriptor()
        {
            string ownName = nameof(Person);
            string[] variables = new string[]
            {
                nameof(Person.FirstName),
                nameof(Person.LastName),
                //nameof(Person.HomeAddress),
                //nameof(Person.WorkAddress),
                //nameof(Person.CitizenOf),
                //nameof(Person.MobilePhone),
            };

            var rootDesc = new RootDescriptor<Person>(ownName, variables);

            return rootDesc;
        }
    }
}