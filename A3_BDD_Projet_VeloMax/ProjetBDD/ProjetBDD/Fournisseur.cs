using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetBDD
{
    class Fournisseur
    {
        private string siret;
        private string nom_fournisseur;
        private string adresse;
        private string contact;
        private int libelle;
        public Fournisseur(Connexion co, string siret)
        {
            string[] data = co.requete("SELECT * FROM fournisseur WHERE SIRET=\"" + siret + "\";");
            this.siret = data[0];
            this.nom_fournisseur = data[1];
            this.adresse = data[2];
            this.contact = data[3];
            this.libelle = Convert.ToInt32(data[4]);
        }

        public string SIRET { get { return this.siret; } }
        public string Nom_fournisseur { get { return this.nom_fournisseur; } }
        public string Adresse { get { return this.adresse; } }
        public string Contact { get { return this.contact; } }
        public int Libelle { get { return this.libelle; } }
    }
}
