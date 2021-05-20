using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlTools.XmlMapper;

namespace UnitTests.XmlMapper
{
   

    class Example
    {
        public static Example CreateSample()
        {
            return new Example()
            {
                SchoolName = "George Washington",
                SchoolNumber = 343,
                SchoolState = "Washington",
                InscriptionDate = new DateTime(2021, 05, 03),
                Alumn = new Person()
                {
                    FirstName = "Erick",
                    LastName = "Doe",
                    Age = 17,
                    Address = new Address()
                    {
                        Street = "3rd Street",
                        Number = "78A",
                        Phone = new Phone()
                        {
                            Code = "58",
                            Number = "555-687858"
                        }
                    }
                },
                Parent = new Person()
                {
                    FirstName = "Susana",
                    LastName = "Doe",
                    Age = 17,
                    Address = new Address()
                    {
                        Street = "1st Street",
                        Number = "95B",
                        Phone = new Phone()
                        {
                            Code = "25",
                            Number = "555-5354585"
                        }
                    }
                }
            };
        }

        [XmlPath("/example/school", Required = true)]
        public string SchoolName { get; set; }

        [XmlPath("/example/schoolNumber")]
        public int SchoolNumber { get; set; }
        
        [XmlPath("/example/schoolState")]
        public string SchoolState { get; set; }

        [XmlPath("/example/inscriptionDate")]
        public DateTime InscriptionDate { get; set; }

        [XmlPath("/example/alumn")]
        public Person Alumn { get; set; }

        [XmlPath("/example/parent")]
        public Person Parent { get; set; }
    }

    public class Person
    {
        [XmlPath("/firstName")]
        public string FirstName { get; set; }

        [XmlPath("/lastName")]
        public string LastName { get; set; }

        [XmlPath("/age")]
        public int Age { get; set; }

        [XmlPath("/address")]
        public Address Address { get; set; }
    }

    public class Address
    {
        [XmlPath("/street")]
        public string Street { get; set; }

        [XmlPath("/number")]
        public string Number { get; set; }

        [XmlPath("/phone")]
        public Phone Phone { get; set; }
    }

    public class Phone
    {
        [XmlPath("/number")]
        public string Number { get; set; }
        [XmlPath("/countryCode")]
        public string Code { get; set; }
    }
}
