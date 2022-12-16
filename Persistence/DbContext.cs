using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;


namespace Dragger_WPF.Persistence
{
    class DbContext
    {
        private const string DBName = "database.sqlite";
        private const string SQLScript = @"..\..\Util\database.sql";
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
            SQLiteCommand insertSQL = new SQLiteCommand("INSERT INTO Cards VALUES (?,?,?,?,?,?)");
            insertSQL.Parameters.Add(card._id_card);
            insertSQL.Parameters.Add(card._id_persona);
            insertSQL.Parameters.Add(card._description);
            insertSQL.Parameters.Add(card._color);
            insertSQL.Parameters.Add(card._goalDate);
            insertSQL.Parameters.Add(card._creationDate);
            try
            {
                insertSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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
