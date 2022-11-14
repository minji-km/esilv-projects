using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetBDD
{
    class Fidelio
    {
        private string nom;
        private string prenom;
        private string desc_f;
        private string date_expiration;
        private string date_paiement;
        private int num_prog;
        private int rabais;
        private int cout;
        public Fidelio(Connexion co, string nom, string prenom)
        {
            string[] data = co.requete("SELECT * FROM fidelio WHERE nom=\"" + nom + "\" AND prenom = \"" + prenom +"\";");
            this.num_prog = Convert.ToInt32(data[0]);
            this.rabais = Convert.ToInt32(data[1]);
            this.desc_f = data[2];
            this.date_expiration = data[3];
            this.cout = Convert.ToInt32(data[4]);
            this.date_paiement = data[5];
            this.nom = data[6];
            this.prenom = data[7];
        }

        public int Num_prog { get { return this.num_prog; } }
        public int Rabais { get { return this.rabais; } }
        public string Desc_f { get { return this.desc_f; } }
        public string Date_expiration { get { return this.date_expiration; } }
        public int Cout { get { return this.cout; } }
        public string Date_paiement { get { return this.date_paiement; } }
        public string Nom { get { return this.nom; } }
        public string Prenom { get { return this.prenom; } }
    }
}
