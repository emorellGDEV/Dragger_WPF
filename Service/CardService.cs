using Dragger_WPF.Entity;
using Dragger_WPF.Persistence;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
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
                var query = "SELECT * FROM Cards";

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
                                _description = reader["description"].ToString(),
                                _color = reader["color"].ToString(),
                                _priority = Convert.ToInt32(reader["priority"].ToString()),
                                _goalDate = ConvertToDateTime(reader["goalDate"].ToString()),
                                _creationDate = ConvertToDateTime(reader["creationDte"].ToString())
                            });
                        }
                    }
                }
            }
            return result;

        }

        public static DateOnly ConvertToDateTime(string str)
        {
            string pattern = @"(\d{4})-(\d{2})-(\d{2})";
            if (Regex.IsMatch(str, pattern))
            {
                Match match = Regex.Match(str, pattern);
                int year = Convert.ToInt32(match.Groups[1].Value);
                int month = Convert.ToInt32(match.Groups[2].Value);
                int day = Convert.ToInt32(match.Groups[3].Value);
                return new DateOnly(year, month, day);
            }
            else
            {
                throw new Exception("Unable to parse.");
            }
        }
    }
}
