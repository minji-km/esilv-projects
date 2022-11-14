using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace ProjetBDD
{
    class Program
    {
        static string ajoutV(Connexion co)
        {
            string b;
            bool safety = true;
            int i = 0;

            Console.WriteLine("Spécifier le numéro de modèle");
            string num_p = Console.ReadLine();

            Console.WriteLine("'nom','grandeur','ligne de produit', prix,'date intro','date disc',stock");
            b = "(" + num_p + "," + Console.ReadLine() + ")";

            co.mod("Add", "velo", b, "Na");

            Console.WriteLine("renseigner les pièces composant le vélo");
            b = "oui";

            string[] req = co.requete("SELECT num_p FROM pieces");
            while (b == "oui")
            {
                Console.WriteLine("Numéro de pièce :");
                b = "\"" + Console.ReadLine() + "\"";
                i = 0;
                safety = true;
                Console.WriteLine(req.GetLength(0));
                while (i < req.GetLength(0) && safety == true)
                {
                    if ("\"" + req[i] + "\"" == b)
                    {
                        safety = false;
                    }
                    i++;
                }
                Console.WriteLine("Renseigner sa description :");
                string d = b + ",\"" + Console.ReadLine() + "\"";
                if (safety == true)
                {
                    b = ajoutP(co, b, d);
                }
                b = "(" + num_p + "," + d + ")";
                co.mod("Add", "composition", b, "Na");
                Console.WriteLine("renseigner une autre pièce pour le vélo?");
                b = Console.ReadLine();
            }
            Console.WriteLine("ajouter un fournisseur pour le vélo?");
            b = Console.ReadLine();
            string dataF = "";
            while (b == "oui")
            {
                Console.WriteLine("Entrez le SIRET du fournisseur");
                //si fournisseur déjà existant
                string SIRET = "\"" + Console.ReadLine() + "\"";
                string[] newf = co.requete("SELECT SIRET FROM fournisseur;");
                bool existe = false;
                for (int j = 0; j < newf.Length && existe == false; j++)
                {
                    if ("\"" + newf[i] + "\"" == SIRET)
                    {
                        existe = true;
                    }
                }
                if (existe == true)
                {
                    dataF = SIRET;
                }
                else //si nouveau fournisseur
                {
                    dataF = ajoutF(co, SIRET);

                }
                Console.WriteLine("Spécifier prix,délai,numéro de catalogue, stock");
                b = "(" + num_p + "," + dataF + ")";
                co.mod("Add", "fournit_p", b, "Na");
                Console.WriteLine("ajouter un autre fournisseur pour le vélo?");
                b = Console.ReadLine();
            }
            while (b == "non")
            {
                Console.WriteLine("Ajouter un fournisseur déjà existant ?");
                b = Console.ReadLine();
                while (b == "oui")
                {
                    b = "(" + num_p + "," + dataF + ")";
                    co.mod("Add", "fournit_p", b, "Na");
                }
            }
            return num_p;
        }

        static void assemblification(Connexion co)
        {
            Console.WriteLine("Entrer le numéro du vélo à assembler :");
            string num_v = Console.ReadLine();
            string stock = "SELECT num_p,stock FROM Composition NATURAL JOIN pieces WHERE num_v=" + num_v + ";";
            string[] ratio = co.requete(stock);
            bool test = true;
            int i = 1;
            while (test == true && i < ratio.GetLength(0))
            {
                if (int.Parse(ratio[i]) < 1)
                {
                    test = false;
                }
                if(int.Parse(ratio[i])<2)
                {
                    Console.WriteLine("Le stock pour la pièce " + ratio[i - 1] + " est bientôt épuisé. Stock : " + ratio[i]);
                }
                i = i + 2;
            }
            i = 0;
            if (test == false)
            {
                Console.WriteLine("Stock insuffisant, état des stocks de pièces pour le vélo demandé :");
                while (i < ratio.GetLength(0))
                {
                    Console.WriteLine(ratio[i] + ": " + ratio[i + 1]);
                    i = i + 2;
                }
            }
            if (test == true)
            {
                while (i < ratio.GetLength(0))
                {
                    stock = "stock=" + (int.Parse(ratio[i + 1]) - 1);
                    co.mod("Up", "pieces", stock, "num_p=\"" + ratio[i]+"\"");
                    i = i + 2;
                }
                ratio = co.requete("SELECT stock FROM velo WHERE num_p=" + num_v);
                stock = "stock=" + (int.Parse(ratio[0]) + 1);
                co.mod("Up", "velo", stock, "num_p=" + num_v);
            }
        }

        static void supCommande(Connexion co, string num_c, string nom, string prenom)
        {

           
            string cond = "num_c=" + num_c;
            if (num_c == "Na")
            {

                string[] valuestring = co.requete("SELECT num_c FROM commande WHERE nom_compagnie=" + nom);
                if (prenom != "Na")
                {
                    string[] valuestring2 = co.requete("SELECT num_c FROM commande WHERE nom=" + nom + " AND prenom=" + prenom + ";");
                    valuestring = valuestring2;
                }

                int s = valuestring.Length;
                if (valuestring.Length<0 && valuestring != null && valuestring[0] != null)
                {
                    string[] lespieces = co.requete("SELECT num_p FROM commande_p WHERE num_c =" + num_c + ";");
                    string[] quantite_p = co.requete("SELECT quantite FROM commande_p WHERE num_c =" + num_c + ";");
                    string[] lesvelos = co.requete("SELECT num_v FROM commande_v WHERE num_c =" + num_c + ";");
                    string[] quantite_v = co.requete("SELECT quantite FROM commande_v WHERE num_c =" + num_c + ";");
                    for (int i = 0; i < valuestring.GetLength(0); i++)
                    {
                        cond = "num_c=" + valuestring[i];

                        for (int j = 0; j < lespieces.Length && lespieces.Length>0; j++)
                        {
                            majStock(co, lespieces[j], "pieces", Convert.ToInt32(quantite_p[j]));
                        }
                        //maj stock piece 

                        co.mod("Del", "commande_p", "Na", cond);
                        for (int j = 0; j < lesvelos.Length && lesvelos.Length>0; j++)
                        {
                            majStock(co, lesvelos[j], "velo", Convert.ToInt32(quantite_v[j]));
                        }
                        //maj stock velo
                        co.mod("Del", "commande_v", "Na", cond);

                        co.mod("Del", "commande", "Na", cond);
                    }
                }
            }
            else
            {
                string[] lespieces = co.requete("SELECT num_p FROM commande_p WHERE num_c =" + num_c + ";");
                string[] quantite_p = co.requete("SELECT quantite FROM commande_p WHERE num_c =" + num_c + ";");
                string[] lesvelos = co.requete("SELECT num_v FROM commande_v WHERE num_c =" + num_c + ";");
                string[] quantite_v = co.requete("SELECT quantite FROM commande_v WHERE num_c =" + num_c + ";");
                cond = "num_c=" + num_c;
                for (int j = 0; j < lespieces.Length && lespieces.Length > 0; j++)
                {
                    majStock(co, lespieces[j], "pieces", Convert.ToInt32(quantite_p[j]));
                }
                //maj stock piece 

                co.mod("Del", "commande_p", "Na", cond);
                for (int j = 0; j < lesvelos.Length && lesvelos.Length > 0; j++)
                {
                    majStock(co, lesvelos[j], "velo", Convert.ToInt32(quantite_v[j]));
                }
                //maj stock velo
                co.mod("Del", "commande_p", "Na", cond);
                co.mod("Del", "commande_v", "Na", cond);
                co.mod("Del", "commande", "Na", cond);
            }
            
        }
        static void ajoutCommande(Connexion co)
        {
            //Numéro de commande
            Console.WriteLine("Entrez le numéro de la commande");
            string num_c = "\"" + Console.ReadLine() + "\"";

            //Type de clientèle
            Console.WriteLine("particulier ou entreprise ?");
            string id = Console.ReadLine().ToLower();
            string np = "";
            bool estclient = false;
            int i = 0;
            while(id!="particulier" && id!="entreprise")
            {
                Console.WriteLine("Entrez une catégorie valide : particulier ou entreprise");
                id = Console.ReadLine();
            }
            switch (id)
            {
                case ("particulier"):
                    //adresse
                    Console.WriteLine("adresse du client");
                    string adresse = "\"" + Console.ReadLine() + "\"";

                    //date de livraison
                    Console.WriteLine("date de livraison de la commande (format[YYYY-MM-DD])");
                    string date_livraison = Console.ReadLine();
                    if (date_livraison.Length == 10 && Char.IsNumber(date_livraison, 0) && Char.IsNumber(date_livraison, 1) && Char.IsNumber(date_livraison, 2) && Char.IsNumber(date_livraison, 3) && date_livraison[4] == '-' && Char.IsNumber(date_livraison, 5) && Char.IsNumber(date_livraison, 6) && date_livraison[7] == '-' && Char.IsNumber(date_livraison, 8) && Char.IsNumber(date_livraison, 9))
                    {
                        date_livraison = new DateTime(Convert.ToInt32(date_livraison.Split('-')[0]), Convert.ToInt32(date_livraison.Split('-')[1]), Convert.ToInt32(date_livraison.Split('-')[2])).ToString("d");
                    }

                    np = "(" + num_c + "," + adresse + "," + "\"" + date_livraison + "\",";

                    //nom
                    Console.WriteLine("nom du client");
                    string nom = "\"" + Console.ReadLine() + "\"";

                    //prenom
                    Console.WriteLine("prenom' du client");
                    string prenom = "\"" + Console.ReadLine() + "\"";
                    np = np + nom  + "," +  prenom  +", \"\")";

                    string[] req = co.requete("SELECT nom, prenom FROM client_p");
                    i = 0;
                    while(i<req.Length && estclient==false && req.Length>0)
                    {
                        //Console.WriteLine("nom " + req[i]);
                        //Console.WriteLine("prenom" + req[j]);
                        if ("\"" + req[i].ToLower() + "\"" ==  nom.ToLower()  && "\"" + req[i+1].ToLower() + "\""  == prenom.ToLower())
                        {
                            estclient = true;
                        }
                        i+=2;
                    }
                    if(estclient == false)
                    {
                        string np2 =  nom + "," + prenom;
                        Console.WriteLine("contact");
                        np2 = np2 + "," + "\"" + Console.ReadLine() + "\"" + "," + adresse + ",";
                        Console.WriteLine("tel");
                        np2 = np2 + "\"" + Console.ReadLine() + "\",";
                        Console.WriteLine("courriel");
                        np2 = np2 + "\"" + Console.ReadLine() + "\"";
                        co.mod("Add", "client_p", "(" + np2 +")", "Na");
                        ajoutFidelio(co, "," + nom + "," + prenom);
                    }

                    break;
                case ("entreprise"):
                    Console.WriteLine("adresse");
                    adresse = "\"" + Console.ReadLine() + "\"";
                    np = "(" + num_c + "," + adresse + ",";
                    Console.WriteLine("'date de livraison' de la commande");
                    np = np + "\"" + Console.ReadLine() + "\"" + "," + "\"\", \"\",";
                    Console.WriteLine("nom de la compagnie' de l'entreprise");
                    string entreprise = "\"" + Console.ReadLine() + "\"";
                    np = np + entreprise + ")";

                    string[] listetp = co.requete("SELECT nom_compagnie FROM client_etp");

                    while (i < listetp.Length && estclient == false && listetp.Length > 0)
                    {
                        if ("\"" + listetp[i] + "\"" == entreprise)
                        {
                            estclient = true;
                        }
                        i++;
                    }
                    if (estclient == false)
                    {
                        string np2 = "(" + entreprise + ",";
                        Console.WriteLine("contact");
                        np2 = np2 + "\"" + Console.ReadLine() + "\"" + "," + adresse + ",";
                        Console.WriteLine("tel");
                        np2 = np2 + "\"" + Console.ReadLine() + "\",";
                        Console.WriteLine("courriel");
                        np2 = np2 + "\"" + Console.ReadLine() + "\")";
                        co.mod("Add", "client_etp", np2, "Na");
                    }

                    break;
            }
            
            co.mod("Add", "commande", np, "Na");
            int qtite = 0;
            int dispo = 0;

            Console.WriteLine("Commande de pièces?");
            np = Console.ReadLine();
            if (np == "oui");
            while (np == "oui")
            {
                Console.WriteLine("Entrez le 'numéro de pièce'");
                np = "\"" + Console.ReadLine() + "\"";
                string[] listetp = co.requete("SELECT stock FROM pieces WHERE num_p = " + np);
                Console.WriteLine("Entrez la quantité voulue");
                qtite = Convert.ToInt32(Console.ReadLine());
                dispo = Convert.ToInt32(listetp[0]);
                if (dispo < qtite)
                {
                    Console.WriteLine("Il n'y a pas assez de pièces " + np + " en stock");
                }
                else
                {
                    co.mod("Up", "pieces", "stock=" + (dispo - qtite), "num_p = " + np);
                    np = "(" + num_c + "," + np + "," + qtite + ")";
                    co.mod("Add", "commande_p", np, "Na");

                }

                Console.WriteLine("Commander une autre pièce?");
                np = Console.ReadLine();
            }
            Console.WriteLine("Commande de vélos?");
            np = Console.ReadLine();
            if (np == "oui") ;
            while (np == "oui")
            {
                Console.WriteLine("Entrez le 'numéro du modèle'");
                np = Console.ReadLine();
                string[] listetp = co.requete("SELECT stock FROM velo WHERE num_p = " + np);
                dispo = Convert.ToInt32(listetp[0]);
                Console.WriteLine("Entrez la quantité voulue");
                qtite = Convert.ToInt32(Console.ReadLine());
                if (dispo < qtite)
                {
                    Console.WriteLine("Il n'y a pas assez de velo " + np + " en stock");
                }
                else
                {
                    co.mod("Up", "velo", "stock=" + (dispo - qtite), "num_p = " + np);
                    np = "(" + num_c + "," + np + "," + qtite + ")";
                    co.mod("Add", "commande_v", np, "Na");
                }
                Console.WriteLine("Commander un autre vélo ?");
                np = Console.ReadLine();
            }
        }

        static void updateCommande(Connexion co)
        {
            Console.WriteLine("Renseigener numÃ©ro de commande puis nouvelle adresse puis nouvelle date de livraison");
            string num_c = "num_c=\"" + Console.ReadLine() + "\"";
            string data = "adresse=\"" + Console.ReadLine() + "\"";
            data = ",date_liv=\"" + Console.ReadLine() + "\"";
            co.mod("Up", "commande", data, num_c);
        }
        static void ajoutClient(Connexion co, string nom = null, string prenom = null, string infos = null)
        {

            if(nom==null)
            {
                Console.WriteLine("Nom");
                nom = "\"" + Console.ReadLine() + "\"";
                Console.WriteLine("prénom du client");
                prenom = "\"" + Console.ReadLine() + "\"";
            }

            bool estclient = false;
            string[] req = co.requete("SELECT nom, prenom FROM client_p");
            int i = 0;
            while (i < req.Length && estclient == false && req.Length > 0)
            {
                //Console.WriteLine("nom " + req[i]);
                //Console.WriteLine("prenom" + req[j]);
                if ("\"" + req[i].ToLower() + "\"" == nom.ToLower() && "\"" + req[i + 1].ToLower() + "\"" == prenom.ToLower())
                {
                    estclient = true;
                }
                i += 2;
            }
            if (estclient == false)
            {
                string np2 = nom + "," + prenom + ",";
                Console.WriteLine("contact");
                np2 = np2 + "\"" + Console.ReadLine() + "\",";
                Console.WriteLine("adresse");
                np2 = np2 + "\"" + Console.ReadLine() + "\",";
                Console.WriteLine("tel");
                np2 = np2 + "\"" + Console.ReadLine() + "\",";
                Console.WriteLine("courriel");
                np2 = np2 + "\"" + Console.ReadLine() + "\"";
                co.mod("Add", "client_p", "(" + np2 + ")", "Na");
                ajoutFidelio(co, "," + nom + "," + prenom);
            }
            else
            {
                Console.WriteLine(nom + " " + prenom + " est déjà client.");
            }

        }

        static void ajoutClientETP(Connexion co)
        {
            bool estclient = true;
            Console.WriteLine("nom de la compagnie' de l'entreprise");
            string entreprise = "\"" + Console.ReadLine() + "\"";

            string[] listetp = co.requete("SELECT nom_compagnie FROM client_etp");
            int i = 0;
            while (i < listetp.Length && estclient == false && listetp.Length > 0)
            {
                if ("\"" + listetp[i] + "\"" == entreprise)
                {
                    estclient = true;
                }
                i++;
            }
            if (estclient == false)
            {
                Console.WriteLine("nom de la compagnie");
                string np = "\"" + Console.ReadLine() + "\"";
                Console.WriteLine("contact");
                np = "(" + np + "," + "\"" + Console.ReadLine() + "\",";
                Console.WriteLine("adresse");
                np = np + "\"" + Console.ReadLine() + "\",";
                Console.WriteLine("tel");
                np = np + "\"" + Console.ReadLine() + "\",";
                Console.WriteLine("courriel");
                np = np + "\"" + Console.ReadLine() + "\"" + ")";
                co.mod("Add", "client_etp", np, "Na");
            }
            else
            {
                Console.WriteLine("Ce client est déjà enregistré dans la base");
            }
            
        }

        static void ajoutFidelio(Connexion co, string np)
        {
            Console.WriteLine("Entrer le numéro du programme Fidélio");
            string b = Console.ReadLine();
            Console.WriteLine("Entrer la date de paiement ou  [Entrée] si non souscrit au programme Fidélio (format obligatoire ! [YY-MM-DD])");
            string c = Console.ReadLine();
            bool paye = true;
            if (c.Length < 1 || c.Length > 10 || !Char.IsNumber(c, 0) || !Char.IsNumber(c, 1) || !Char.IsNumber(c, 2) || !Char.IsNumber(c, 3) || c[4] != '-' || !Char.IsNumber(c, 5) || !Char.IsNumber(c, 6) || c[7] != '-' || !Char.IsNumber(c, 8) || !Char.IsNumber(c, 9))
            { 
                paye = false;
            }
            string[] c2 = new string[10];


            switch(paye)
            {
                case true:
                    c2 = c.Split('-');
                    int[] date_paiement = new int[c2.Length];
                    for (int i = 0; i < c2.Length; i++) {date_paiement[i] = Convert.ToInt32(c2[i]);}
                    DateTime date_p = new DateTime(date_paiement[0], date_paiement[1], date_paiement[2]);
                    DateTime date_exp;
                    string z = "0";
                    switch (b)
                    {
                        case "1":
                            date_exp = date_p.AddYears(1);
                            b = "(" + b + ",5,'Fidélio',\"" + date_exp.Year + "-";
                            if (date_exp.Month < 10) b = b + z;
                            b = b + date_exp.Month + "-";
                            if (date_exp.Day < 10) b = b + z;
                            b = b + date_exp.Day + "\",15," + "\"" + c + "\"" + np + ")";

                            if (DateTime.Compare(DateTime.Now, date_p) < 0 || DateTime.Compare(date_exp, DateTime.Now) < 0) break;
                            else
                            {
                                co.mod("Add", "fidelio", b, "Na");
                                break;
                            }
                        case "2":
                            date_exp = date_p.AddYears(2);
                            b = "(" + b + ",8,'Fidélio Or',\"" + date_exp.Year + "-";
                            if (date_exp.Month < 10) b = b + z;
                            b = b+ date_exp.Month + "-";
                            if (date_exp.Day < 10) b = b + z;
                            b = b + date_exp.Day + "\",25," + "\"" + c + "\"" + np + ")";
                            if (DateTime.Compare(DateTime.Now, date_p) < 0 || DateTime.Compare(date_exp, DateTime.Now) < 0) break;
                            else
                            {
                                co.mod("Add", "fidelio", b, "Na");
                                break;
                            }
                        case "3":
                            date_exp = date_p.AddYears(2);
                            b = "(" + b + ",10,'Fidélio  ',\"" + date_exp.Year + "-";
                            if (date_exp.Month < 10) b = b + z;
                            b = b+date_exp.Month + "-";
                            if (date_exp.Day < 10) b = b + z;
                            b = b + date_exp.Day + "\",60," + "\"" + c + "\"" + np + ")";
                            if (DateTime.Compare(DateTime.Now, date_p) < 0 || DateTime.Compare(date_exp, DateTime.Now) < 0) break;
                            else
                            {
                                co.mod("Add", "fidelio", b, "Na");
                                break;
                            }
                        case "4":
                            date_exp = date_p.AddYears(3);
                            b = "(" + b + ",12,'Fidélio Diamant',\"" + date_exp.Year + "-";
                            if (date_exp.Month < 10) b = b + z;
                            b = b+date_exp.Month + "-";
                            if (date_exp.Day < 10) b = b + z;
                            b = b+ date_exp.Day + "\",100," + "\"" + c + "\"" + np + ")";
                            if (DateTime.Compare(DateTime.Now, date_p) < 0 || DateTime.Compare(date_exp, DateTime.Now) < 0) break;
                            else
                            {
                                co.mod("Add", "fidelio", b, "Na");
                                break;
                            }
                        default:
                            Console.WriteLine("Pas de fidélio pas de rabais, pas de rabais pas de sous, pas de sous pas de sous");
                            break;
                    }
                    break;
                case false:
                    switch (b)
                    {
                        case "1":
                            b = "(" + b + ",5,'Fidélio',\"\",15,\"\"" + np + ")";
                            co.mod("Add", "fidelio", b, "Na");
                            break;
                        case "2":
                            b = "(" + b + ",8,'Fidélio Or',\"\",25,\"\"" + np + ")";
                            co.mod("Add", "fidelio", b, "Na");
                            break;
                        case "3":
                            b = "(" + b + ",10,'Fidélio Platine',\"\",60,\"\"" + np + ")";
                            co.mod("Add", "fidelio", b, "Na");
                            break;
                        case "4":
                            b = "(" + b + ",12,'Fidélio Diamant',\"\",100,\"\"" + np + ")";
                            co.mod("Add", "fidelio", b, "Na");
                            break;
                        default:
                            Console.WriteLine("Pas de fidélio pas de rabais, pas de rabais pas de sous, pas de sous pas de sous");
                            break;
                    }
                    break;
            }
        }

        static void supFidelio(Connexion co, string n, string p)
        {
            string cond = "nom=" + n + " AND prenom=" + p;
            co.mod("Del", "fidelio", "Na", cond);
        }
        static void UpdateClient(Connexion co)
        {
            Console.WriteLine("Nom du client");
            string n = "\"" + Console.ReadLine() + "\"";
            Console.WriteLine("Prénom du client");
            string p = "\"" + Console.ReadLine() + "\"";
            Console.WriteLine("Modifier les coordonnées?");
            string adresse = Console.ReadLine();
            if (adresse == "oui")
            {
                Console.Write("Spécifier le nouveau contact : ");
                string nom_contact = "\"" + Console.ReadLine() + "\"";
                Console.Write("Spécifier la nouvelle adresse : ");
                adresse = "\"" + Console.ReadLine() + "\"";
                Console.Write("Spécifier le nouveau tel : ");
                string tel = "\"" + Console.ReadLine() + "\"";
                Console.Write("Spécifier le nouveau courriel : ");
                string courriel = "\"" + Console.ReadLine() + "\"";
                string ndata = "nom_contact=" + nom_contact + ",adresse=" + adresse + ",courriel=" + courriel + ",tel=" + tel;
                n = "nom=" + n + " AND prenom=" + p;
                co.mod("Up", "client_p", ndata, n);
            }
            Console.WriteLine("Souscrire un nouveau Fidélio?");
            adresse = Console.ReadLine();
            if (adresse == "oui")
            {
                supFidelio(co, n, p);
                n = n + "," + p;
                ajoutFidelio(co, n);
            }
        }

        static void UpdateClientETP(Connexion co)
        {
            Console.WriteLine("Nom de la compagnie");
            string n = "\"" + Console.ReadLine() + "\"";
            Console.WriteLine("Modifier les coordonnées?");
            string adresse = Console.ReadLine();
            if (adresse == "oui")
            {
                Console.Write("Spécifier le nouveau contact : ");
                string nom_contact = "\"" + Console.ReadLine() + "\"";
                Console.Write("Spécifier la nouvelle adresse : ");
                adresse = "\"" + Console.ReadLine() + "\"";
                Console.Write("Spécifier le nouveau tel : ");
                string tel = "\"" + Console.ReadLine() + "\"";
                Console.Write("Spécifier le nouveau courriel : ");
                string courriel = "\"" + Console.ReadLine() + "\"";
                string ndata = "nom_contact=" + nom_contact + ",adresse=" + adresse + ",courriel=" + courriel + ",tel=" + tel;
                n = "nom_compagnie=" + n;
                co.mod("Up", "client_etp", ndata, n);
            }

        }

        static void supV(Connexion co)
        {
            Console.WriteLine("Modèle à supprimer");
            string b = Console.ReadLine();
            string c = "num_p=" + b;
            b = "num_v=" + b;
            
            co.mod("Del", "composition", "Na", b);
            co.mod("Del", "velo", "Na", c);
        }
        static void supClient(Connexion co)
        {
            Console.Write("Nom du client à supprimer ");
            string n = "\"" + Console.ReadLine() + "\"";
            Console.Write("Prénom du client à supprimer ");
            string p = "\"" + Console.ReadLine() + "\"";
            string cond = "nom=" + n + " AND prenom=" + p;
            supCommande(co, "Na", n, p);
            co.mod("Del", "fidelio", "Na", cond);
            co.mod("Del", "client_p", "Na", cond);
        }

        static void supClientETP(Connexion co)
        {
            Console.Write("Nom de la compagnie client Ã  supprimer ");
            string n = "\"" + Console.ReadLine() + "\"";
            string cond = "nom_compagnie=" + n;
            supCommande(co, "Na", n, "Na");
            co.mod("Del", "client_etp", "Na", cond);
        }

        static void updateV(Connexion co)
        {
            Console.WriteLine("Modèle à modifier");
            string b = "num_p=" + Console.ReadLine();
            Console.WriteLine("Nouveau prix");
            string c = "prix_u=" + Console.ReadLine();
            co.mod("Up", "velo", c, b);
        }
        static void supP(Connexion co)
        {
            Console.WriteLine("Numéro de la pièce à supprimer");
            string b = "num_p=\"" + Console.ReadLine() + "\"";
            co.mod("Del", "composition", "Na", b);
            co.mod("Del", "fournit_p", "Na", b);
            co.mod("Del", "pieces", "Na", b);
        }
        static void updateP(Connexion co)
        {

            Console.WriteLine("Pièce à modifier");
            string b = Console.ReadLine();
            Console.WriteLine("modifier stock ou prix?");
            string c = Console.ReadLine();
            if (c == "prix")
            {
                Console.WriteLine("Nouveau prix");
                c = "prix_u=" + Console.ReadLine();
                b = "num_p=" + b;
                co.mod("Up", "pieces", c, b);
            }
            if (c == "stock")
            {
                Console.WriteLine("Renseigner la quantité de pièces livrées :");
                int bah = Convert.ToInt32(Console.ReadLine());
                majStock(co, b, "pieces", bah);
            }
            else
            {
                Console.WriteLine("entrée invalide");
            }
        }


        static string ajoutF(Connexion co, string SIRET = null)
        {
            string d;
            if (SIRET == null)
            {
                Console.WriteLine("SIRET du fournisseur");
                SIRET = "\"" + Console.ReadLine() + "\"";
            }
                Console.WriteLine("nom");
                d = "(" + SIRET + "," + "\"" + Console.ReadLine() + "\",";
                Console.WriteLine("adresse");
                d = d + "\"" + Console.ReadLine() + "\",";
                Console.WriteLine("contact");
                d = d + "\"" + Console.ReadLine() + "\",";
                Console.WriteLine("libelle");
                d = d + Console.ReadLine() + ")";
            co.mod("Add", "fournisseur", d, "Na");
            return SIRET;
        }
        static void supF(Connexion co)
        {
            Console.WriteLine("Entrez le 'SIRET' du fournisseur Ã  supprimer");
            string cond = "\"" + Console.ReadLine() + "\"";
            cond = "SIRET=" + cond;
            co.mod("Del", "fournit_p", "Na", cond);
            co.mod("Del", "fournisseur", "Na", cond);
        }

        static void majStock(Connexion co, string num, string table, int bah, string siret = "Na")
        {
            if (siret == "Na")
            {
                string[] maitregims = co.requete("SELECT stock FROM" + table + "WHERE num_p=\"" + num + "\"");
                maitregims[0] = (int.Parse(maitregims[0]) + bah).ToString();
                co.mod("Up", table, "stock=" + maitregims[0], "num_p=\"" + num + "\"");
            }
            else
            {
                co.mod("Up", table, "stock=" + bah, "num_p=\"" + num + "\"" + " AND SIRET=\"" + siret + "\"");
            }
        }

        static void updateF(Connexion co)
        {
            Console.WriteLine("Entrer le 'SIRET'");
            string siret = Console.ReadLine();
            Console.WriteLine("Modifier coorodonnées ou stock?");
            string ac = Console.ReadLine();
            if (ac == "coordonnées")
            {
                Console.WriteLine("Entrez les nouveaux 'adresse' puis 'contact' du fournisseur");
                ac = "adresse=" + Console.ReadLine();
                ac = ac + ",contact=" + Console.ReadLine();
                co.mod("Up", "fournisseur", ac, siret);
            }
            if (ac == "stock")
            {
                ac = "oui";
                while (ac == "oui")
                {
                    Console.WriteLine("Entrez le numéro de la pièce à mettre à jour :");
                    string num = Console.ReadLine();
                    Console.WriteLine("Entrez la nouvelle quantité");
                    int bah = Convert.ToInt32(Console.ReadLine());
                    majStock(co, num, "fournit_f", bah, siret);
                }
            }
        }
        static string ajoutP(Connexion co, string e, string d)
        {
            string b;
            Console.WriteLine("date d'introduction de la pièce de la commande (format[YYYY-MM-DD])");
            string date_intro = Console.ReadLine();
            if (date_intro.Length == 10 && Char.IsNumber(date_intro, 0) && Char.IsNumber(date_intro, 1) && Char.IsNumber(date_intro, 2) && Char.IsNumber(date_intro, 3) && date_intro[4] == '-' && Char.IsNumber(date_intro, 5) && Char.IsNumber(date_intro, 6) && date_intro[7] == '-' && Char.IsNumber(date_intro, 8) && Char.IsNumber(date_intro, 9))
            {
                date_intro = new DateTime(Convert.ToInt32(date_intro.Split('-')[0]), Convert.ToInt32(date_intro.Split('-')[1]), Convert.ToInt32(date_intro.Split('-')[2])).ToString("d");
            }
            Console.WriteLine("date de discontinuation de la pièce de la commande (format[YYYY-MM-DD])");
            string date_disc = Console.ReadLine();
            if (date_disc.Length == 10 && Char.IsNumber(date_disc, 0) && Char.IsNumber(date_disc, 1) && Char.IsNumber(date_disc, 2) && Char.IsNumber(date_disc, 3) && date_disc[4] == '-' && Char.IsNumber(date_disc, 5) && Char.IsNumber(date_disc, 6) && date_disc[7] == '-' && Char.IsNumber(date_disc, 8) && Char.IsNumber(date_disc, 9))
            {
                date_disc = new DateTime(Convert.ToInt32(date_disc.Split('-')[0]), Convert.ToInt32(date_disc.Split('-')[1]), Convert.ToInt32(date_disc.Split('-')[2])).ToString("d");
            }

            Console.WriteLine("stock");
            b = "(" + d + "," + "\"" + date_intro + "\"" + "," + "\"" + date_disc + "\"" + "," + Console.ReadLine() + ",";
            Console.WriteLine("prix");
            b = b + Console.ReadLine() + ")";
            co.mod("Add", "pieces", b, "Na");
            Console.WriteLine("nouveaux fournisseurs pour la pièce?");
            b = Console.ReadLine();
            while (b == "oui")
            {
                string dataF = ajoutF(co);
                Console.WriteLine("Entrer le prix de la pièce");
                string delai = Console.ReadLine();
                Console.WriteLine("délai de livraison de la pièce");
                delai = delai + "," + "\"" + Console.ReadLine() + "\",";
                Console.WriteLine("numéro de la pièce dans le catalogue");
                delai = delai + "\"" + Console.ReadLine() + "\",";
                Console.WriteLine("stock de la pièce");
                delai = delai + Console.ReadLine();
                b = "(" + e + "," + dataF + "," + delai + ")";
                co.mod("Add", "fournit_p", b, "Na");
                Console.WriteLine("poursuivre?");
                b = Console.ReadLine();
            }
            return d;
        }

        static void modifier(Connexion co)
        {
            Console.WriteLine("Entrer le type d'élément à modifier : client particulier/client entreprise/fournisseur/commande");
            string a = "";
            string b = "";
            while (a != "velo" || a != "pieces" || a != "client particulier" || a != "client entreprise" || a != "fournisseur" || a != "commande")
            {
                a = Console.ReadLine();
                switch (a)
                {
                    case "velo":
                        Console.WriteLine("Sélectionner un type d'opération");
                        b = Console.ReadLine();
                        while (b != "Add" && b != "Del" && b != "Up" && b == "stop")
                        {
                            Console.WriteLine("Type de commande invalide, seuls les arguments Add, Del et Up sont acceptés");
                            b = Console.ReadLine();
                        }
                        switch (b)
                        {
                            case "Add":
                                string num_p = ajoutV(co);
                                break;
                            case "Del":
                                supV(co);
                                break;
                            case "Up":
                                updateV(co);
                                break;
                        }
                        break;
                    case "pieces":
                        Console.WriteLine("Sélectionner un type d'opération");
                        b = Console.ReadLine();
                        while (b != "Add" && b != "Del" && b != "Up")
                        {
                            Console.WriteLine("Type de commande invalide, seuls les arguments Add, Del et Up sont acceptés");
                            b = Console.ReadLine();
                        }
                        switch (b)
                        {
                            case "Add":
                                Console.WriteLine("Numéro de pièce :");
                                b = "\"" + Console.ReadLine() + "\"";
                                Console.WriteLine("Renseigner sa description :");
                                string d = b + ",\"" + Console.ReadLine() + "\"";
                                ajoutP(co, b, d);
                                break;
                            case "Del":
                                supP(co);
                                break;
                            case "Up":
                                updateP(co);
                                break;
                        }
                        break;
                    case "client particulier":
                        Console.WriteLine("Sélectionner un type d'opération");
                        b = Console.ReadLine();
                        while (b != "Add" && b != "Del" && b != "Up")
                        {
                            Console.WriteLine("Type de commande invalide, seuls les arguments Add, Del et Up sont acceptés");
                            b = Console.ReadLine();
                        }
                        switch (b)
                        {
                            case "Add":
                                ajoutClient(co);
                                break;
                            case "Del":
                                supClient(co);
                                break;
                            case "Up":
                                UpdateClient(co);
                                break;
                        }
                        break;
                    case "client entreprise":
                        Console.WriteLine("Sélectionner un type d'opération");
                        b= Console.ReadLine();
                        while (b != "Add" && b != "Del" && b != "Up")
                        {
                            Console.WriteLine("Type de commande invalide, seuls les arguments Add, Del et Up sont acceptés");
                            b = Console.ReadLine();
                        }
                        switch (b)
                        {
                            case "Add":
                                ajoutClientETP(co);
                                break;
                            case "Del":
                                supClientETP(co);
                                break;
                            case "Up":
                                UpdateClientETP(co);
                                break;
                        }
                        break;
                    case "fournisseur":
                        Console.WriteLine("Sélectionner un type d'opération");
                        b = Console.ReadLine();
                        while (b != "Add" && b != "Del" && b != "Up")
                        {
                            Console.WriteLine("Type de commande invalide, seuls les arguments Add, Del et Up sont acceptés");
                            b = Console.ReadLine();
                        }
                        switch (b)
                        {
                            case "Add":
                                ajoutF(co);
                                break;
                            case "Del":
                                supF(co);
                                break;
                            case "Up":
                                updateF(co);
                                break;
                        }
                        break;
                    case "commande":
                        Console.WriteLine("Sélectionner un type d'opération");
                        b = Console.ReadLine();
                        while (b != "Add" && b != "Del" && b != "Up")
                        {
                            Console.WriteLine("Type de commande invalide, seuls les arguments Add, Del et Up sont acceptés");
                            b = Console.ReadLine();
                        }
                        switch (b)
                        {
                            case "Add":
                                ajoutCommande(co);
                                break;
                            case "Del":
                                Console.WriteLine("numéro de la commande à supprimer");
                                string num_c = Console.ReadLine();
                                supCommande(co, num_c, "Na", "Na");
                                break;
                            case "Up":
                                updateCommande(co);
                                break;
                        }
                        break;
                }break;
            }
        }

        static void modStatistique(Connexion co, string info = null, string data = null)
        {
            Console.WriteLine("MODULE STATISTIQUE");
            Console.WriteLine("Sélectionner le type d'informations");
            if (info == null) info = Console.ReadLine();
            else Console.WriteLine(info);
            if (info == "client")
            {
                Console.WriteLine("Statistique demandée :");
                if (data == null) data = Console.ReadLine();
                else Console.WriteLine(data);
                switch (data)
                {
                    case ("liste"):
                        co.requeteA("SELECT * FROM client_p;");
                        co.requete("SELECT * FROM client_etp");
                        break;
                    case ("nombre"):
                        int nb = Convert.ToInt32(co.requete("SELECT count(*) FROM client_p")[0]) + Convert.ToInt32(co.requete("SELECT count(*) FROM client_etp")[0]) - 2;
                        Console.WriteLine("Il y a au total " + nb + " clients");
                        break;
                    case ("fidélio"):
                        Console.WriteLine("Sélectionnez la fidélio en question");
                        data = Console.ReadLine();
                        if (data != "Fidélio Or" && data != "Fidélio Platine" && data != "Fidélio Diamant" && data != "Fidélio")
                        {
                            co.requeteA("SELECT * FROM fidelio;");
                        }
                        else
                        {
                            co.requeteA("SELECT * FROM fidelio WHERE desc_f=\"" + data + "\";");
                        }
                        break;
                    case ("baleine"):
                        Console.WriteLine("Liste des plus gros consommateurs de pièces");
                        co.requeteA("SELECT nom,prenom, nom_compagnie, sum(commande_p.quantite*pieces.prix_u) + sum(commande_v.quantite*velo.prix_u) FROM commande NATURAL JOIN commande_p JOIN commande_v ON commande_v.num_c = commande.num_c NATURAL JOIN pieces JOIN velo ON velo.num_p = commande_v.num_v GROUP BY nom, prenom, nom_compagnie ORDER BY sum(commande_p.quantite*pieces.prix_u) + sum(commande_v.quantite*velo.prix_u) DESC LIMIT 3; ");
                        
                        break;
                    case ("cumul commandes"):
                        Console.WriteLine("Liste des clients et le cumul de leur commandes en euros (client non affiché si aucune commande enregistée)");
                        co.requeteA("SELECT nom,prenom, nom_compagnie, sum(commande_p.quantite*pieces.prix_u) + sum(commande_v.quantite*velo.prix_u) FROM commande NATURAL JOIN commande_p JOIN commande_v ON commande_v.num_c = commande.num_c NATURAL JOIN pieces JOIN velo ON velo.num_p = commande_v.num_v GROUP BY nom, prenom, nom_compagnie; ");
                        break;
                    case ("expiration"):
                        Console.WriteLine("membres et expiration de leur fidelio");
                        co.requeteA("SELECT nom, prenom, date_expiration FROM fidelio; ");
                        break;
                    default:
                        break;

                }
            }
            else if (info == "ventes")
            {
                Console.WriteLine("Statistique demandée");
                if (data == null) data = Console.ReadLine();
                else Console.WriteLine(data);
                switch (data)
                {
                    case ("pieces"):
                        Console.WriteLine("Entrez la pièce demandée");
                        data = Console.ReadLine();
                        if (data == "tout")
                        {
                            Console.WriteLine("classement des pièces rapportant le plus d'argent");
                            co.requeteA("SELECT commande_p.num_p,sum(quantite),sum(quantite*prix_u) FROM commande_p JOIN pieces ON pieces.num_p = commande_p.num_p GROUP BY num_p ORDER BY sum(quantite) desc;");
                            Console.WriteLine("classement des pièces les plus vendues");
                            co.requeteA("SELECT num_p,sum(quantite) FROM commande_p GROUP BY num_p ORDER BY sum(quantite) desc;");
                        }
                        else
                        {
                            co.requeteA("SELECT sum(quantite),sum(quantite*prix_u), stock FROM commande_p JOIN pieces ON pieces.num_p = commande_p.num_p WHERE commande_p.num_p=\"" + data + "\";");
                        }
                        break;
                    case ("velo"):
                        Console.WriteLine("Entrez le vélo demandé");
                        data = Console.ReadLine();
                        if (data == "tout")
                        {
                            Console.WriteLine("classement des vélos rapportant le plus d'argent");
                            co.requeteA("SELECT num_v,sum(quantite), sum(quantite*prix_u) FROM commande_v JOIN velo ON velo.num_p = commande_v.num_v GROUP BY commande_v.num_v ORDER BY sum(quantite*prix_u) desc;");
                            Console.WriteLine("classement des vélos les plus vendus");
                            co.requeteA("SELECT num_v,sum(quantite) FROM commande_v GROUP BY num_v ORDER BY sum(quantite) desc;");
                        }
                        else
                        {
                            co.requeteA("SELECT sum(quantite),sum(quantite*prix_u),velo.stock FROM commande_v JOIN velo ON velo.num_p = commanve_v.num_v WHERE commande_v.num_v=\"" + data + "\";");
                        }
                        break;
                    case ("fournisseur"):
                        Console.WriteLine("Liste des fournisseurs et les pièces qu'ils fournissent"); //Pas pour les vélos parce que le cahier des charges indique que les fournisseurs fournissent uniquement des pièces l'assmblage est fait par un employé
                        co.requeteA("SELECT nom_fournisseur, num_p FROM Fournisseur NATURAL JOIN fournit_p;");
                        break;
                }
            }
        }

        static void Main(string[] args)
        {
            string txtco = "SERVER=localhost;PORT=3306;" + "DATABASE=velomax;UID=root;PASSWORD=root";
            Connexion co = new Connexion(txtco);

            #region Modifier la base de données
            Console.WriteLine("Voulez vous-modifier des données ?");
            string entree = Console.ReadLine();

            while(entree=="oui")
            {
                modifier(co);
                Console.WriteLine("Souhaitez-vous faire une autre opération ? Entrez oui pour continuer ou une autre touche pour arrêter");
                entree = Console.ReadLine();

            }
            #endregion

            #region Module statistique
            



                Console.WriteLine("appuyer sur une touche pour continuer");
                Console.ReadLine();
                modStatistique(co, "ventes", "velo");

                Console.WriteLine("appuyer sur une touche pour continuer");
                Console.ReadLine();
                modStatistique(co, "ventes", "pieces");


                Console.WriteLine("appuyer sur une touche pour continuer");
                Console.ReadLine();
                modStatistique(co, "client", "nombre");

                Console.WriteLine("appuyer sur une touche pour continuer");
                Console.ReadLine();
                modStatistique(co, "client", "cumul commandes");

                Console.WriteLine("appuyer sur une touche pour continuer");
                Console.ReadLine();
                modStatistique(co, "ventes", "fournisseur");
                #endregion

                #region 7. Export en Json stock faible 
                Console.WriteLine("appuyer sur une touche pour continuer");
                Console.ReadLine();
                string[] p = co.requete("SELECT num_p FROM pieces WHERE stock<=2");

                Console.WriteLine("Les pièces dont le stock est faible");
                co.requeteA("SELECT num_p FROM pieces WHERE stock<=2");
                //Export en Json pour le stock faible des pièces
                Jsonification pieces_stock_insuffisant = new Jsonification("pieces_stock_fabile.json");
                for (int i = 0; i < p.Length; i++)
                {
                    string[] f = co.requete("SELECT SIRET FROM fournisseur NATURAL JOIN fournit_p WHERE num_p =\"" + p[i] + "\";");
                    Piece piece = new Piece(co, p[i]);
                    pieces_stock_insuffisant.write_object(piece);
                    for (int j = 0; j < f.Length; j++)
                    {
                        Fournisseur fournisseur = new Fournisseur(co, f[j]);
                        co.requeteA("SELECT nom_fournisseur, num_p, stock FROM fournisseur NATURAL JOIN fournit_p WHERE num_p =\"" + p[i] + "\";");
                        pieces_stock_insuffisant.write_object(fournisseur);
                    }

                }
                Console.WriteLine("Export du stock faible des pièces réussi ! Le fichier \"velos_stock_faible.json\" se situe dans bin/debug");

                Console.WriteLine("appuyer sur une touche pour continuer");
                Console.ReadLine();

                string[] v = co.requete("SELECT num_p FROM velo WHERE stock<=2");
                Console.WriteLine("Les vélos dont le stock est faible");
                co.requeteA("SELECT num_p FROM velo WHERE stock<=2");
                //Export en Json pour le stock faible des vélos
                Jsonification velos_stock_insuffisant = new Jsonification("velos_stock_faible.json");
                for (int j = 0; j < v.Length; j++)
                {
                    Velo velo = new Velo(co, v[j]);
                    velos_stock_insuffisant.write_object(velo);
                }
                Console.WriteLine("Export du stock faible des vélos réussi ! Le fichier \"velos_stock_faible.json\" se situe dans bin/debug");

                Console.WriteLine("appuyer sur une touche pour continuer");
                Console.ReadLine();




                Jsonification fidelio_exp = new Jsonification("fidelio_exp.json");
                string[] fid = co.requete("SELECT nom, prenom, date_expiration FROM fidelio");
                Console.WriteLine("Les fidélio qui arrivent à expiration dans moins de deux mois");

                for (int i = 2; i < fid.Length; i += 3)
                {
                    DateTime exp;
                    if (fid[i].Length == 10 && Char.IsNumber(fid[i], 0) && Char.IsNumber(fid[i], 1) && Char.IsNumber(fid[i], 2) && Char.IsNumber(fid[i], 3) && fid[i][4] == '-' && Char.IsNumber(fid[i], 5) && Char.IsNumber(fid[i], 6) && fid[i][7] == '-' && Char.IsNumber(fid[i], 8) && Char.IsNumber(fid[i], 9))
                    {
                        exp = new DateTime(Convert.ToInt32(fid[i].Split('-')[0]), Convert.ToInt32(fid[i].Split('-')[1]), Convert.ToInt32(fid[i].Split('-')[2]));


                        if (DateTime.Compare(exp, DateTime.Now.AddMonths(2)) < 0 && DateTime.Compare(exp, DateTime.Now) > 0)
                        {
                            Fidelio fidelio = new Fidelio(co, fid[i - 2], fid[i - 1]);
                            fidelio_exp.write_object(fidelio);

                        }
                    }


                }
                Console.WriteLine("Export des fiédlios arrivant à expiration dans moins de deux mois réussi ! Le fichier \"fidelio_exp.json\" se situe dans bin/debug");
            #endregion

            Console.WriteLine("Souhaitez-vous consulter le module statistique ?");
            entree = Console.ReadLine();
            while (entree == "oui")
            {
                modStatistique(co);
                Console.WriteLine("Ouvrir le module statistique à nouveau ? Entrez oui pour continuer ou une autre touche pour arrêter");
                entree = Console.ReadLine();
            }


                Console.WriteLine("Entree pour arrêter le programme");
                Console.ReadLine();
            
        }
    }
}
