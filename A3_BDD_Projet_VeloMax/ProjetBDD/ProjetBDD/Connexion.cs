using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace ProjetBDD
{
    public class Connexion
    {
        MySqlConnection co = null;
        public bool statut;
        public string txtco;
        public Connexion(string txtco)
        {
            this.txtco = txtco;
            try
            {
                co = new MySqlConnection(txtco);
                co.Open();
                statut = true;
                Console.WriteLine("Connexion réussie");
            }
            catch (MySqlException e)
            {
                Console.WriteLine(" Erreur connexion : " + e.ToString());
                return;
                statut = false;
            }
        }
        public string[] requete(string requete)
        {
            MySqlCommand command = co.CreateCommand();
            command.CommandText = requete;
            co.Close();
            co.Open();
            MySqlDataReader reader;
            reader = command.ExecuteReader();
            List<string> liste = new List<string>();
            string[] valueString = new string[reader.FieldCount];
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    liste.Add(reader.GetValue(i).ToString());
                    valueString[i] = reader.GetValue(i).ToString();
                    //Console.Write(valueString[i] + ", ");
                }
                //Console.WriteLine();
            }
            return liste.ToArray();
        }

        public void requeteA(string requete)
        {
            MySqlCommand command = co.CreateCommand();
            command.CommandText = requete;
            co.Close();
            co.Open();
            MySqlDataReader reader;
            reader = command.ExecuteReader();
            List<string> liste = new List<string>();
            string[] valueString = new string[reader.FieldCount];
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    liste.Add(reader.GetValue(i).ToString());
                    valueString[i] = reader.GetValue(i).ToString();
                    Console.Write(valueString[i] + ", ");
                }
                Console.WriteLine();
            }
        }

        public void mod(string type, string table, string values, string cond)
        {
            string requete = "Na";
            switch (type)
            {
                case "Add":
                    requete = insert(table, values);
                    break;
                case "Del":
                    requete = delete(table, cond);
                    break;
                case "Up":
                    requete = update(table, values, cond);
                    break;
            }
            if (requete != "Na")
            {
                MySqlCommand command = co.CreateCommand();
                command.CommandText = requete;
                Console.WriteLine(requete);
                MySqlDataReader reader;
                co.Close();
                co.Open();
                reader = command.ExecuteReader();
            }
        }
        public string insert(string table, string values)
        {
            string requete = "INSERT INTO " + table + " VALUES " + values + ";";
            return requete;
        }
        public string delete(string table, string cond)
        {
            string requete = "DELETE FROM " + table + " WHERE " + cond + ";";
            return requete;
        }
        public string update(string table, string values, string cond)
        {
            string requete = "UPDATE " + table + " SET " + values + " WHERE " + cond + ";";
            return requete;
        }
        public void existe(string table, string e_key, string c_key)
        {
            string requete = "SELECT count(*) FROM " + table + " WHERE " + e_key + "=" + c_key + ";";
        }

    }
}