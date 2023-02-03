using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows;
using Dragger_WPF.Entity;
using MongoDB.Driver;
using Dragger_WPF.Service;
using MongoDB.Bson;
using System.Linq.Expressions;
using System.Globalization;

namespace Dragger_WPF.Persistence
{
    class DbContext
    {

        public static void InsertCard(Entity.Card card)
        {
            IMongoCollection<Entity.Card> cards = CardService.GetCards();

            cards.InsertOne(card);
        }

        public static void UpdateCard(Entity.Card card)
        {
            IMongoCollection<Card> users = CardService.GetCards();

            var filter = Builders<Card>.Filter.Eq(s => s.id_card, card.id_card);

            users.ReplaceOne(filter, card);
        }

        public static void DeleteCard(Entity.Card card)
        {
            IMongoCollection<Card> users = CardService.GetCards();

            users.DeleteOne(s => s.id_card == card.id_card);
        }

        public static void InsertPerson(Entity.Person person)
        {
            IMongoCollection<Entity.Person> persons = PersonService.GetPersons();

            persons.InsertOne(person);
        }

        public static void DeletePerson(Entity.Person person)
        {
            IMongoCollection<Person> users = PersonService.GetPersons();

            users.DeleteOne(s => person.id_person == person.id_person);
        }

        public static void UpdatePerson(Entity.Person person)
        {
            IMongoCollection<Person> persons = PersonService.GetPersons();

            var filter = Builders<Person>.Filter.Eq(s => s.id_person, person.id_person);

            persons.ReplaceOne(filter, person);
        }

        public static async Task<int> SelectMaxCard()
        {
            IMongoCollection<Card> cards = CardService.GetCards();
            var result = await cards.Find(x => true)
                                     .SortByDescending(x => x.id_card)
                                     .Limit(1)
                                     .FirstOrDefaultAsync();
            return result?.id_card ?? 0;
        }

        public static async Task<int> SelectMaxPerson()
        {
            IMongoCollection<Person> persons = PersonService.GetPersons();
            var result = await persons.Find(x => true)
                                     .SortByDescending(x => x.id_person)
                                     .Limit(1)
                                     .FirstOrDefaultAsync();
            return result?.id_person ?? 0;
        }


        public static IMongoDatabase GetInstance()
        {
            string connectionString = "mongodb+srv://dragger:dragger@dragger.thzs3ma.mongodb.net/test";
            MongoClient mongoClient = new MongoClient(connectionString);
            return mongoClient.GetDatabase("Dragger");
        }

    }
}
