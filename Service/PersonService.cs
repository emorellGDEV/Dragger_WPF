using Dragger_WPF.Entity;
using Dragger_WPF.Persistence;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragger_WPF.Service
{
    class PersonService
    {
        //Funcionalitat identica al CardService
        public static IMongoCollection<Person> GetPersons()
        {
            return DbContext.GetInstance().GetCollection<Person>("Persons");
        }

        public static List<Person> GetAll()
        {
            IMongoCollection<Person> persons = PersonService.GetPersons();

            var result = persons.AsQueryable<Person>().ToList();
            return result;
        }
    }
}

