using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows;
using Dragger_WPF.Entity;
using System.Security.Cryptography;

namespace Dragger_WPF.Persistence
{
    class DbContext
    {
        private const string DBName = "database.sqlite";
        private const string SQLScript = "C:\\Users\\joanb\\source\\repos\\Dragger_WPF\\Util\\database.sql";
        private static bool IsDbRecentlyCreated = false;

        public static void Up()
        {
            // Crea la base de datos solo una vez
            if (!File.Exists(Path.GetFullPath(DBName)))
            {
                SQLiteConnection.CreateFile(DBName);
                IsDbRecentlyCreated = true;
            }

            using (var ctx = GetInstance())
            {
                // Crea la base de datos solo la primera vez
                if (IsDbRecentlyCreated)
                {
                    using (var reader = new StreamReader(Path.GetFullPath(SQLScript)))
                    {
                        var query = "";
                        var line = "";
                        while ((line = reader.ReadLine()) != null)
                        {
                            query += line;
                        }

                        using (var command = new SQLiteCommand(query, ctx))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
     
        public static void InsertCard(Entity.Card card)
        {
            try
            {
                using (var ctx = GetInstance())
                {
                    var insert = "INSERT INTO Cards VALUES (?,?,?,?,?,?,?,?)";

                    using (var command = new SQLiteCommand(insert, ctx))
                    {
                        command.Parameters.Add(new SQLiteParameter("id_card", card._id_card.ToString()));
                        command.Parameters.Add(new SQLiteParameter("fk_id_responsable", card._id_persona.ToString()));
                        command.Parameters.Add(new SQLiteParameter("description", card._description.ToString()));
                        command.Parameters.Add(new SQLiteParameter("color", card._color.ToString()));
                        command.Parameters.Add(new SQLiteParameter("priority", card._priority.ToString()));
                        command.Parameters.Add(new SQLiteParameter("goalDate", card._goalDate.ToString()));
                        command.Parameters.Add(new SQLiteParameter("creationDate", card._creationDate.ToString()));
                        command.Parameters.Add(new SQLiteParameter("position", card._position.ToString()));

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void UpdateCard(Entity.Card card)
        {
            try
            {
                using (var ctx = GetInstance())
                {
                    var update = "Update Cards set fk_id_responsable = ?, description = ?, color = ?, priority = ?," +
                        "goaldate = ?, creationDate = ?, position = ? where id_card = " + card._id_card.ToString();

                    using (var command = new SQLiteCommand(update, ctx))
                    {
                        command.Parameters.Add(new SQLiteParameter("fk_id_responsable", card._id_persona.ToString()));
                        command.Parameters.Add(new SQLiteParameter("description", card._description.ToString()));
                        command.Parameters.Add(new SQLiteParameter("color", card._color.ToString()));
                        command.Parameters.Add(new SQLiteParameter("priority", card._priority.ToString()));
                        command.Parameters.Add(new SQLiteParameter("goalDate", card._goalDate.ToString()));
                        command.Parameters.Add(new SQLiteParameter("creationDate", card._creationDate.ToString()));
                        command.Parameters.Add(new SQLiteParameter("position", card._position.ToString()));

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void DeleteCard(Entity.Card card)
        {
            try
            {
                using (var ctx = GetInstance())
                {
                    var delete = "Delete from Cards where id_card = " + card._id_card.ToString();

                    using (var command = new SQLiteCommand(delete, ctx))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void InsertPerson(Entity.Person person)
        {
            try
            {
                using (var ctx = GetInstance())
                {
                    var insert = "INSERT INTO Persons VALUES (?,?)";

                    using (var command = new SQLiteCommand(insert, ctx))
                    {
                        command.Parameters.Add(new SQLiteParameter("id_person", person._id_person.ToString()));
                        command.Parameters.Add(new SQLiteParameter("name", person._name.ToString()));


                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void DeletePerson(Entity.Person person)
        {
            try
            {
                using (var ctx = GetInstance())
                {
                    var delete = "Delete from Persons where id_person = " + person._id_person.ToString();

                    using (var command = new SQLiteCommand(delete, ctx))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void UpdatePerson(Entity.Person person)
        {
            try
            {
                using (var ctx = GetInstance())
                {
                    var update = "Update Persons set id_person = ?, name = ? where id_card = " + person._id_person.ToString();

                    using (var command = new SQLiteCommand(update, ctx))
                    {
                        command.Parameters.Add(new SQLiteParameter("id_person", person._id_person.ToString()));
                        command.Parameters.Add(new SQLiteParameter("name", person._name.ToString()));

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static int SelectMaxCard()
        {
            try
            {
                using (var ctx = GetInstance())
                {
                    var query = "select max(id_card) from cards";

                    using (var command = new SQLiteCommand(query, ctx))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                return reader.GetInt32(0);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return 0;
        }

        public static int SelectMaxPerson()
        {
            try
            {
                using (var ctx = GetInstance())
                {
                    var query = "select max(id_person) from persons";

                    using (var command = new SQLiteCommand(query, ctx))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                return reader.GetInt32(0);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return 0;
        }



        public static SQLiteConnection GetInstance()
        {
            var db = new SQLiteConnection(
                string.Format("Data Source={0};Version=3;", DBName)
            );

            db.Open();

            return db;
        }

    }
}
