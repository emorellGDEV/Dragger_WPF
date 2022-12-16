﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows;

namespace Dragger_WPF.Persistence
{
    class DbContext
    {
        private const string DBName = "database.sqlite";
        private const string SQLScript = @"C:\Users\eduar\OneDrive\DAM2\M13 _Projecte\Projecte\Dragger - WPF\Dragger_WPF\Util\database.sql";
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
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
