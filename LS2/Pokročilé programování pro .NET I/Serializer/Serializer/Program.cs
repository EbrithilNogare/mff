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
    public delegate string SerializerHelperPointingToVariableValue<T>(T person); // TODO change name
    public class RootDescriptor<T>
    {
        SerializerHelperPointingToVariableValue<T>[] variables;
        string ownName;
        public void Serialize(TextWriter writer, T instance)
        {
            writer.WriteLine($"<{ownName}>");
            foreach (var item in variables)
            {
                writer.WriteLine(item(instance));
            }
            writer.WriteLine($"</{ownName}>");
        }
        public string Serialize(string name, T instance)
        {
            string result = String.Empty;

            if (variables.Length == 0)
            {
                result += $"<{name}>";
                result += $"{instance}";
                result += $"</{name}>";
            }
            else
            {
                result += $"<{name}>\n";
                foreach (var item in variables)
                {
                    result += item(instance).ToString();
                    result += "\n";
                }
                result += $"</{name}>";
            }
            return result;
        }
        public RootDescriptor(string ownName, SerializerHelperPointingToVariableValue<T>[] variables)
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

            RootDescriptor<String> stringDesc = GetStringDescriptor();
            RootDescriptor<Int32> int32Desc = GetInt32Descriptor();
            RootDescriptor<Address> adressDesc = GetAdressDescriptor();
            RootDescriptor<Country> countryDesc = GetCountryDescriptor();
            RootDescriptor<PhoneNumber> phoneNumberDesc = GetPhoneNumberDescriptor();

            SerializerHelperPointingToVariableValue<Person>[] variables = new SerializerHelperPointingToVariableValue<Person>[]
            {
                new SerializerHelperPointingToVariableValue<Person>(FirstName),
                new SerializerHelperPointingToVariableValue<Person>(LastName),
                new SerializerHelperPointingToVariableValue<Person>(HomeAddress),
                new SerializerHelperPointingToVariableValue<Person>(WorkAddress),
                new SerializerHelperPointingToVariableValue<Person>(CitizenOf),
                new SerializerHelperPointingToVariableValue<Person>(MobilePhone),
            };

            string FirstName(Person person)     => stringDesc.Serialize(nameof(FirstName),person.FirstName);
            string LastName(Person person)      => stringDesc.Serialize(nameof(LastName), person.LastName);
            string HomeAddress(Person person)   => adressDesc.Serialize(nameof(HomeAddress), person.HomeAddress);
            string WorkAddress(Person person)   => adressDesc.Serialize(nameof(WorkAddress), person.WorkAddress);
            string CitizenOf(Person person)     => countryDesc.Serialize(nameof(CitizenOf), person.CitizenOf);
            string MobilePhone(Person person)   => phoneNumberDesc.Serialize(nameof(MobilePhone), person.MobilePhone);

            return new RootDescriptor<Person>(ownName, variables);
        }
        static RootDescriptor<Address> GetAdressDescriptor()
        {
            string ownName = nameof(Address);

            RootDescriptor<String> stringDesc = GetStringDescriptor();

            SerializerHelperPointingToVariableValue<Address>[] variables = new SerializerHelperPointingToVariableValue<Address>[]
            {
                new SerializerHelperPointingToVariableValue<Address>(Street),
                new SerializerHelperPointingToVariableValue<Address>(City),
            };

            string Street(Address address) => stringDesc.Serialize(nameof(Street), address.Street);
            string City(Address address) => stringDesc.Serialize(nameof(City), address.City);

            return new RootDescriptor<Address>(ownName, variables);
        }
        static RootDescriptor<PhoneNumber> GetPhoneNumberDescriptor()
        {
            string ownName = nameof(PhoneNumber);

            RootDescriptor<Country> adressDesc = GetCountryDescriptor();
            RootDescriptor<Int32> int32Desc = GetInt32Descriptor();

            SerializerHelperPointingToVariableValue<PhoneNumber>[] variables = new SerializerHelperPointingToVariableValue<PhoneNumber>[]
            {
                new SerializerHelperPointingToVariableValue<PhoneNumber>(Country),
                new SerializerHelperPointingToVariableValue<PhoneNumber>(Number),
            };

            string Country(PhoneNumber phoneNumber) => adressDesc.Serialize(nameof(Country), phoneNumber.Country);
            string Number(PhoneNumber phoneNumber) => int32Desc.Serialize(nameof(Number), phoneNumber.Number);

            return new RootDescriptor<PhoneNumber>(ownName, variables);
        }
        static RootDescriptor<Country> GetCountryDescriptor()
        {
            string ownName = nameof(Country);

            RootDescriptor<String> stringDesc = GetStringDescriptor();
            RootDescriptor<Int32> int32Desc = GetInt32Descriptor();

            SerializerHelperPointingToVariableValue<Country>[] variables = new SerializerHelperPointingToVariableValue<Country>[]
            {
                new SerializerHelperPointingToVariableValue<Country>(Name),
                new SerializerHelperPointingToVariableValue<Country>(AreaCode),
            };

            string Name(Country country) => stringDesc.Serialize(nameof(Name), country.Name);
            string AreaCode(Country country) => int32Desc.Serialize(nameof(AreaCode), country.AreaCode);

            return new RootDescriptor<Country>(ownName, variables);
        }

        static RootDescriptor<String> GetStringDescriptor()
        {
            string ownName = nameof(String);
            SerializerHelperPointingToVariableValue<String>[] variables = new SerializerHelperPointingToVariableValue<String>[] { };

            var rootDesc = new RootDescriptor<String>(ownName, variables);

            return rootDesc;
        }
        static RootDescriptor<Int32> GetInt32Descriptor()
        {
            string ownName = nameof(Int32);
            SerializerHelperPointingToVariableValue<Int32>[] variables = new SerializerHelperPointingToVariableValue<Int32>[] { };

            var rootDesc = new RootDescriptor<Int32>(ownName, variables);

            return rootDesc;
        }
    }
}