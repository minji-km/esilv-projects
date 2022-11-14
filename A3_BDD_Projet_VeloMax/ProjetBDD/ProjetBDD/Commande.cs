using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetBDD
{
    class Commande
    {
        private string num_c;
        private string adresse_liv;
        private string date_liv;
        private string nom;
        private string prenom;
        private string nom_compagnie;
        public Commande(Connexion co, string num_c)
        {
            string[] data = co.requete("SELECT * FROM commande WHERE num_c=\"" + num_c + "\";");
            this.num_c = data[0];
            this.adresse_liv = data[1];
            this.date_liv = data[2];
            this.nom = data[3];
            this.prenom = data[4];
            this.nom_compagnie = data[5];
        }
    }
}
