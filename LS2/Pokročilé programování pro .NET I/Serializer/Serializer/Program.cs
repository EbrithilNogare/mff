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
    abstract class RootDescriptor{
            
    }
    public class RootDescriptor<T>
    {
        object[] variables;
        string ownName;
        public void Serialize(TextWriter writer, T instance)
        {
            writer.WriteLine($"<{ownName}>");
            foreach (string item in variables)
            {
                item.ser
            }
            writer.WriteLine($"</{ownName}>");
        }
        public RootDescriptor(string ownName, object[] variables)
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
            object[] variables = new object[]
            {
                nameof(Person.FirstName),
                nameof(Person.LastName),
                nameof(Person.HomeAddress),
                nameof(Person.WorkAddress),
                nameof(Person.CitizenOf),
                nameof(Person.MobilePhone),
            };

            var rootDesc = new RootDescriptor<Person>(ownName, variables);

            return rootDesc;
        }
        static RootDescriptor<Address> GetAdressDescriptor()
        {
            string ownName = nameof(Address);
            string[] variables = new string[]
            {
                nameof(Address.Street),
                nameof(Address.City),
            };

            var rootDesc = new RootDescriptor<Address>(ownName, variables);

            return rootDesc;
        }
        static RootDescriptor<PhoneNumber> GetPhoneNumberDescriptor()
        {
            string ownName = nameof(PhoneNumber);
            object[] variables = new object[]
            {
                nameof(PhoneNumber.Country),
                nameof(PhoneNumber.Number),
            };

            var rootDesc = new RootDescriptor<PhoneNumber>(ownName, variables);

            return rootDesc;
        }
        static RootDescriptor<Country> GetCountryDescriptor()
        {
            string ownName = nameof(Country);
            string[] variables = new string[]
            {
                nameof(Country.Name),
                nameof(Country.AreaCode),
            };

            var rootDesc = new RootDescriptor<Country>(ownName, variables);

            return rootDesc;
        }
        static RootDescriptor<String> GetStringDescriptor()
        {
            string ownName = nameof(String);
            string[] variables = new string[] { };

            var rootDesc = new RootDescriptor<String>(ownName, variables);

            return rootDesc;
        }
        static RootDescriptor<Int32> GetInt32Descriptor()
        {
            string ownName = nameof(Int32);
            string[] variables = new string[] { };

            var rootDesc = new RootDescriptor<Int32>(ownName, variables);

            return rootDesc;
        }
    }
}