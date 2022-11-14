using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace ProjetBDD
{
    [XmlInclude(typeof(Connexion))]
    public class Piece
    {
        private string num_p;
        private string desc_p;
        private string date_intro;
        private string date_disc;
        private int stock;
        private float prix_u;

        
        public Piece() { }

        public Piece(Connexion co, string num_p)
        {
            string[] feu = co.requete("SELECT * FROM pieces WHERE num_p=\"" + num_p + "\";");
            this.num_p = feu[0];
            this.desc_p = feu[1];
            this.date_intro = feu[2];
            this.date_disc = feu[3];
            this.stock = int.Parse(feu[4]);
            this.prix_u = float.Parse(feu[5]);
        }

        public string Num_p { get { return this.num_p; } }
        public string Desc_p { get { return this.desc_p; } }
        public string Date_intro { get { return this.date_intro; } }
        public string Date_disc { get { return this.date_disc; } }
        public int Stock { get { return this.stock; } }
        public float Prix_u { get { return this.prix_u; } }

    }
}
