using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetBDD
{
    class Velo
    {
        private string num_p;
        private string nomv;
        private string grandeur;
        private string ligne_p;
        private float prix_u;
        private string date_intro;
        private string date_disc;
        private int stock;
        public Velo(Connexion co, string num_p)
        {
            string[] data = co.requete("SELECT * FROM velo WHERE num_p=\"" + num_p + "\";");
            this.num_p = data[0];
            this.nomv = data[1];
            this.grandeur = data[2];
            this.ligne_p = data[3];
            this.prix_u = float.Parse(data[4]);
            this.date_intro = data[5];
            this.date_disc = data[6];
            this.stock = int.Parse(data[7]);
        }

        public string Num_p { get { return this.num_p; } }
        public string Grandeur { get { return this.grandeur; } }
        public string Ligne_p { get { return this.ligne_p; } }
        public string Nomv { get { return this.nomv; } }
        public string Date_intro { get { return this.date_intro; } }
        public string Date_disc { get { return this.date_disc; } }
        public int Stock { get { return this.stock; } }
        public float Prix_u { get { return this.prix_u; } }

    }
}
