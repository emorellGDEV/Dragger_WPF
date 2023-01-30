using Dragger_WPF.Entity;
using Dragger_WPF.Persistence;
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
        public static IEnumerable<Card> GetAll()
        {
            var result = new List<Card>();

            using (var ctx = DbContext.GetInstance())
            {
                var query = "SELECT id_card, fk_id_responsable, description, color, " +
                    "priority, cast(goalDate as nvarchar(10)) as goalDate, cast(creationDate as nvarchar(10)) as creationDate, position FROM Cards";

                using (var command = new SQLiteCommand(query, ctx))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new Card
                            {
                                _id_card = Convert.ToInt32(reader["id_card"].ToString()),
                                _id_persona = Convert.ToInt32(reader["fk_id_responsable"].ToString()),
                                _description =  reader["description"].ToString(),
                                _color = reader["color"].ToString(),
                                _priority = Convert.ToInt32(reader["priority"].ToString()),
                                _goalDate = Convert.ToDateTime(reader["goalDate"].ToString()),
                                _creationDate = Convert.ToDateTime(reader["creationDate"].ToString()),
                                _position = Convert.ToInt32(reader["position"].ToString())
                            });
                        }
                    }
                }
            }
            return result;
        }
    }
}
