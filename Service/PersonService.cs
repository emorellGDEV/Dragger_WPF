using Dragger_WPF.Entity;
using Dragger_WPF.Persistence;
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
        public static IEnumerable<Person> GetAll()
        {
            var result = new List<Person>();

            using (var ctx = DbContext.GetInstance())
            {
                var query = "SELECT * FROM Persons";

                using (var command = new SQLiteCommand(query, ctx))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new Person
                            {
                                _id_person = Convert.ToInt32(reader[0].ToString()),
                                _name = reader[1].ToString()
                            });
                        }
                    }
                }
            }
            return result;
        }
    }
}

