using Dragger_WPF.Entity;
using Dragger_WPF.Persistence;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dragger_WPF.Service
{
    class CardService
    {
        //Proporciona una colecció Mongo de Cards
        public static IMongoCollection<Card> GetCards()
        {
            return DbContext.GetInstance().GetCollection<Card>("Cards");
        }

        //Transforma la colecció en una llista.
        public static List<Card> GetAll()
        {
            IMongoCollection<Card> cards = CardService.GetCards();

            var result = cards.AsQueryable<Card>().ToList();
            return result;
        }
    }
}
