DROP DATABASE IF EXISTS velomax;
CREATE DATABASE velomax;
USE velomax;

DROP TABLE IF EXISTS velo;
DROP TABLE IF EXISTS pieces;
DROP TABLE IF EXISTS fournit_p;
DROP TABLE IF EXISTS fournisseur;
DROP TABLE IF EXISTS composition;
DROP TABLE IF EXISTS commande;
DROP TABLE IF EXISTS commande_p;
DROP TABLE IF EXISTS commande_v;
DROP TABLE IF EXISTS client_etp;
DROP TABLE IF EXISTS client_p;
DROP TABLE IF EXISTS fidelio;

CREATE TABLE IF NOT EXISTS velo(
num_p INT PRIMARY KEY,
nom_v VARCHAR(40), 
grandeur VARCHAR(40),
ligne_p VARCHAR(40),
prix_u FLOAT,
date_intro VARCHAR(40),
date_disc VARCHAR(40),
stock INT);

CREATE TABLE IF NOT EXISTS pieces(
num_p VARCHAR(40) PRIMARY KEY,
desc_p VARCHAR(40),
date_intro_p VARCHAR(40),
date_disc_p VARCHAR(40),
stock INT,
prix_u FLOAT);

CREATE TABLE IF NOT EXISTS fournisseur(
SIRET VARCHAR(40) PRIMARY KEY,
nom_fournisseur VARCHAR(40),
adresse VARCHAR(100),
contact VARCHAR(40),
libelle INT NULL CHECK (libelle <= 4));



CREATE TABLE IF NOT EXISTS fournit_p(
num_p VARCHAR(40), FOREIGN KEY (num_p) references pieces(num_p),
SIRET VARCHAR(40), FOREIGN KEY (SIRET) references fournisseur(SIRET),
prix FLOAT,
delai VARCHAR(40),
nump_catalogue VARCHAR(40),
stock INT,
PRIMARY KEY(num_p, SIRET));




CREATE TABLE IF NOT EXISTS composition(
num_v INT, FOREIGN KEY(num_v) references velo(num_p),
num_p VARCHAR(40), FOREIGN KEY (num_p) references pieces(num_p),
desc_p VARCHAR(40),
PRIMARY KEY(num_v, num_p));

CREATE TABLE IF NOT EXISTS client_etp(
nom_compagnie VARCHAR(40) PRIMARY KEY,
nom_contact VARCHAR(40),
adresse VARCHAR(40),
tel VARCHAR(40),
courriel VARCHAR(40));

CREATE TABLE IF NOT EXISTS client_p(
nom VARCHAR(40),
prenom VARCHAR(40),
nom_contact VARCHAR(40),
adresse VARCHAR(40),
tel VARCHAR(40),
courriel VARCHAR(40),
PRIMARY KEY(nom, prenom));

CREATE TABLE IF NOT EXISTS commande(
num_c VARCHAR(40) PRIMARY KEY,
adresse_liv VARCHAR(40),
date_liv VARCHAR(40),
nom VARCHAR(40) NULL,
prenom VARCHAR(40) NULL, 
FOREIGN KEY(nom, prenom) references client_p(nom, prenom),
nom_compagnie VARCHAR(40) NULL, FOREIGN KEY(nom_compagnie) references client_etp(nom_compagnie));

CREATE TABLE IF NOT EXISTS commande_p(
num_c VARCHAR(40), FOREIGN KEY(num_c) references commande(num_c),
num_p VARCHAR(40), FOREIGN KEY(num_p) references pieces(num_p),
quantite INT,
PRIMARY KEY(num_p, num_c));

CREATE TABLE IF NOT EXISTS commande_v(
num_c VARCHAR(40), FOREIGN KEY(num_c) references commande(num_c),
num_v INT, FOREIGN KEY(num_v) references velo(num_p),
quantite INT,
PRIMARY KEY(num_v, num_c));

CREATE TABLE IF NOT EXISTS fidelio(
num_prog INT,
rabais INT,
desc_f VARCHAR(40),
date_expiration VARCHAR(40),
cout INT,
date_paiement VARCHAR(40),
nom VARCHAR(40) references client_p(nom),
prenom VARCHAR(40) references client_p(prenom),
PRIMARY KEY(nom, prenom));

/* INSERTION DE VALEURS */
INSERT INTO velo VALUES(101, "Kilimandjaro", "Adultes", "VTT", 569.0, "2014", "toujours en production", 200);
INSERT INTO velo VALUES(102, "NorthPole", "Adultes", "VTT", 329.0, "2002", "2015", 600);
INSERT INTO velo VALUES(103, "MontBlanc", "Jeunes", "VTT", 399.0, "2003", "2009", 500);
INSERT INTO velo VALUES(104, "Hooligan", "Jeunes", "VTT", 199.0, "2004", "toujours en production", 2);
INSERT INTO velo VALUES(105, "Orléans", "Hommes", "Vélo de course", 229.0, "2005", "2020", 220);
INSERT INTO velo VALUES(106, "Orléans", "Dames", "Vélo de course", 229.0, "2006", "2020", 550);
INSERT INTO velo VALUES(107, "BlueJay", "Hommes", "Vélo de course", 349.0, "2007", "2021", 460);
INSERT INTO velo VALUES(108, "BlueJay", "Dammes", "Vélo de course", 349.0, "2008", "2021", 780);
INSERT INTO velo VALUES(109, "Trail Explorer", "Filles", "Classique", 129.0, "2010", "toujours en production", 4320);
INSERT INTO velo VALUES(110, "Trail Explorer", "Garçon", "Classique", 129.0, "2010", "toujours en production", 6850);
INSERT INTO velo VALUES(111, "Night Hawk", "Jeunes", "Classique", 189.0, "2011", "2021", 1);
INSERT INTO velo VALUES(112, "Tierra Verde", "Hommes", "Classique", 199.0, "2012", "toujours en production", 8570);
INSERT INTO velo VALUES(113, "Tierra Verde", "Dames", "Classique", 199.0, "2013", "toujours en production", 4000);
INSERT INTO velo VALUES(114, "Mud Zinger I", "Jeunes", "BMX", 279.0, "2014", "2021", 0);
INSERT INTO velo VALUES(115, "Mud Zinger II", "Adultes", "BMX", 359.0, "2015", "toujours en production", 2200);


INSERT INTO pieces VALUES("C32", "Cadre", "1995-08-26", "2022-05-04", 2, 20); 
INSERT INTO pieces VALUES("C34", "Cadre","1995-08-26", "2022-05-04", 500, 20); 
INSERT INTO pieces VALUES("C76", "Cadre", "1995-08-26", "2022-05-04", 0, 20); 
INSERT INTO pieces VALUES("C43", "Cadre", "1995-08-26", "2022-05-04", 500, 20); 
INSERT INTO pieces VALUES("C43f", "Cadre","1995-08-26", "2022-05-04", 500, 20); 
INSERT INTO pieces VALUES("C44f", "Cadre","1995-08-26", "2022-05-04", 500, 20); 
INSERT INTO pieces VALUES("C01", "Cadre","1995-08-26", "2022-05-04", 500, 20); 
INSERT INTO pieces VALUES("C02", "Cadre", "1995-08-26", "2022-05-04", 500, 20); 
INSERT INTO pieces VALUES("C15", "Cadre", "1995-08-26", "2022-05-04", 500, 20); 
INSERT INTO pieces VALUES("C87", "Cadre", "1995-08-26", "2022-05-04", 500, 20); 
INSERT INTO pieces VALUES("C87f", "Cadre", "1995-08-26", "2022-05-04", 500, 20); 
INSERT INTO pieces VALUES("C25", "Cadre", "1995-08-26", "2022-05-04", 500, 20); 
INSERT INTO pieces VALUES("C26", "Cadre", "1995-08-26", "2022-05-04", 500, 20); 

INSERT INTO pieces VALUES("G7", "Guidon", "1995-08-26", "2022-05-04", 500, 20); 
INSERT INTO pieces VALUES("G9", "Guidon", "1995-08-26", "2022-05-04", 500, 20); 
INSERT INTO pieces VALUES("G12", "Guidon", "1995-08-26", "2022-05-04", 500, 20); 

INSERT INTO pieces VALUES("F3", "Freins", "1995-08-26", "2022-05-04", 500, 20); 
INSERT INTO pieces VALUES("F9", "Freins", "1995-08-26", "2022-05-04", 500, 20); 

INSERT INTO pieces VALUES("S88", "Selle", "1995-08-26", "2022-05-04", 500, 20); 
INSERT INTO pieces VALUES("S37", "Selle", "1995-08-26", "2022-05-04", 500, 20); 
INSERT INTO pieces VALUES("S35", "Selle", "1995-08-26", "2022-05-04", 500, 20); 
INSERT INTO pieces VALUES("S02", "Selle", "1995-08-26", "2022-05-04", 500, 20); 
INSERT INTO pieces VALUES("S03", "Selle", "1995-08-26", "2022-05-04", 500, 20); 
INSERT INTO pieces VALUES("S36", "Selle", "1995-08-26", "2022-05-04", 500, 20); 
INSERT INTO pieces VALUES("S34", "Selle", "1995-08-26", "2022-05-04", 500, 20); 
INSERT INTO pieces VALUES("S87", "Selle", "1995-08-26", "2022-05-04", 500, 20); 

INSERT INTO pieces VALUES("DV133", "Dérailleur avant", "1995-08-26", "2022-05-04", 500, 20); 
INSERT INTO pieces VALUES("DV17", "Dérailleur avant", "1995-08-26", "2022-05-04", 500, 20); 
INSERT INTO pieces VALUES("DV87", "Dérailleur avant", "1995-08-26", "2022-05-04", 1, 20); 
INSERT INTO pieces VALUES("DV57", "Dérailleur avant", "1995-08-26", "2022-05-04", 500, 20); 
INSERT INTO pieces VALUES("DV15", "Dérailleur avant", "1995-08-26", "2022-05-04", 500, 20); 
INSERT INTO pieces VALUES("DV41", "Dérailleur avant", "1995-08-26", "2022-05-04", 1, 20); 
INSERT INTO pieces VALUES("DV132", "Dérailleur avant", "1995-08-26", "2022-05-04", 500, 20);

INSERT INTO pieces VALUES("DR56", "Dérailleur arrière", "1995-08-26", "2022-05-04", 500, 20); 
INSERT INTO pieces VALUES("DR87", "Dérailleur arrière", "1995-08-26", "2022-05-04", 500, 20); 
INSERT INTO pieces VALUES("DR86", "Dérailleur arrière", "1995-08-26", "2022-05-04", 500, 20); 
INSERT INTO pieces VALUES("DR23", "Dérailleur arrière", "1995-08-26", "2022-05-04", 500, 20); 
INSERT INTO pieces VALUES("DR76", "Dérailleur arrière", "1995-08-26", "2022-05-04", 500, 20); 
INSERT INTO pieces VALUES("DR52", "Dérailleur arrière", "1995-08-26", "2022-05-04", 2, 20); 

INSERT INTO pieces VALUES("R45", "Roue", "1995-08-26", "2022-05-04", 1, 20); 
INSERT INTO pieces VALUES("R48", "Roue", "1995-08-26", "2022-05-04", 500, 20); 
INSERT INTO pieces VALUES("R12", "Roue", "1995-08-26", "2022-05-04", 2, 20); 
INSERT INTO pieces VALUES("R19", "Roue", "1995-08-26", "2022-05-04", 500, 20); 
INSERT INTO pieces VALUES("R1", "Roue", "1995-08-26", "2022-05-04", 500, 20); 
INSERT INTO pieces VALUES("R11", "Roue", "1995-08-26", "2022-05-04", 500, 20); 
INSERT INTO pieces VALUES("R44", "Roue", "1995-08-26", "2022-05-04", 500, 20); 

INSERT INTO pieces VALUES("R46", "Roue", "1995-08-26", "2022-05-04", 500, 20); 
INSERT INTO pieces VALUES("R47", "Roue", "1995-08-26", "2022-05-04", 500, 20); 
INSERT INTO pieces VALUES("R32", "Roue", "1995-08-26", "2022-05-04", 500, 20); 
INSERT INTO pieces VALUES("R18", "Roue", "1995-08-26", "2022-05-04", 500, 20); 
INSERT INTO pieces VALUES("R2", "Roue", "1995-08-26", "2022-05-04", 500, 20);

INSERT INTO pieces VALUES("R02", "Réflecteurs", "1995-08-26", "2022-05-04", 500, 20); 
INSERT INTO pieces VALUES("R09", "Réflecteurs", "1995-08-26", "2022-05-04", 500, 20); 
INSERT INTO pieces VALUES("R10", "Réflecteurs", "1995-08-26", "2022-05-04", 500, 20); 

INSERT INTO pieces VALUES("P12", "Pédaliers", "1995-08-26", "2022-05-04", 1, 20); 
INSERT INTO pieces VALUES("P34", "Pédaliers", "1995-08-26", "2022-05-04", 500, 20); 
INSERT INTO pieces VALUES("P1", "Pédaliers", "1995-08-26", "2022-05-04", 0, 20); 
INSERT INTO pieces VALUES("P15", "Pédaliers", "1995-08-26", "2022-05-04", 500, 20); 

INSERT INTO pieces VALUES("O2", "Ordinateur", "1995-08-26", "2022-05-04", 500, 20); 
INSERT INTO pieces VALUES("O4", "Ordinateur", "1995-08-26", "2022-05-04", 0, 20); 

INSERT INTO pieces VALUES("S01", "Panier", "1995-08-26", "2022-05-04", 2, 20); 
INSERT INTO pieces VALUES("S05", "Panier", "1995-08-26", "2022-05-04", 2, 20); 
INSERT INTO pieces VALUES("S74", "Panier", "1995-08-26", "2022-05-04", 2, 20); 
INSERT INTO pieces VALUES("S73", "Panier", "1995-08-26", "2022-05-04", 2, 20); 





INSERT INTO composition VALUES("101", "C32", "Cadre");
INSERT INTO composition VALUES("101", "G7", "Guidon");
INSERT INTO composition VALUES("101", "F3", "Freins");
INSERT INTO composition VALUES("101", "S88", "Selle");
INSERT INTO composition VALUES("101", "DV133", "Dérailleur avant");
INSERT INTO composition VALUES("101", "DR56", "Dérailleur arrière");
INSERT INTO composition VALUES("101", "R45", "Roue avant");
INSERT INTO composition VALUES("101", "R46", "Roue arrière");
INSERT INTO composition VALUES("101", "P12", "Pédaliers");
INSERT INTO composition VALUES("101", "O2", "Ordinateur");

INSERT INTO composition VALUES("102", "C34", "Cadre");
INSERT INTO composition VALUES("102", "G7", "Guidon");
INSERT INTO composition VALUES("102", "F3", "Freins");
INSERT INTO composition VALUES("102", "S88", "Selle");
INSERT INTO composition VALUES("102", "DV17", "Dérailleur avant");
INSERT INTO composition VALUES("102", "DR87", "Dérailleur arrière");
INSERT INTO composition VALUES("102", "R48", "Roue avant");
INSERT INTO composition VALUES("102", "R47", "Roue arrière");
INSERT INTO composition VALUES("102", "P12", "Pédaliers");

INSERT INTO composition VALUES("103", "C76", "Cadre");
INSERT INTO composition VALUES("103", "G7", "Guidon");
INSERT INTO composition VALUES("103", "F3", "Freins");
INSERT INTO composition VALUES("103", "S88", "Selle");
INSERT INTO composition VALUES("103", "DV17", "Dérailleur avant");
INSERT INTO composition VALUES("103", "DR87", "Dérailleur arrière");
INSERT INTO composition VALUES("103", "R48", "Roue avant");
INSERT INTO composition VALUES("103", "R47", "Roue arrière");
INSERT INTO composition VALUES("103", "P12", "Pédaliers");
INSERT INTO composition VALUES("103", "O2", "Ordinateur");

INSERT INTO composition VALUES("104", "C76", "Cadre");
INSERT INTO composition VALUES("104", "G7", "Guidon");
INSERT INTO composition VALUES("104", "F3", "Freins");
INSERT INTO composition VALUES("104", "S88", "Selle");
INSERT INTO composition VALUES("104", "DV87", "Dérailleur avant");
INSERT INTO composition VALUES("104", "DR86", "Dérailleur arrière");
INSERT INTO composition VALUES("104", "R12", "Roue avant");
INSERT INTO composition VALUES("104", "R32", "Roue arrière");
INSERT INTO composition VALUES("104", "P12", "Pédaliers");

INSERT INTO composition VALUES("105", "C43", "Cadre");
INSERT INTO composition VALUES("105", "G9", "Guidon");
INSERT INTO composition VALUES("105", "F9", "Freins");
INSERT INTO composition VALUES("105", "S37", "Selle");
INSERT INTO composition VALUES("105", "DV57", "Dérailleur avant");
INSERT INTO composition VALUES("105", "DR86", "Dérailleur arrière");
INSERT INTO composition VALUES("105", "R19", "Roue avant");
INSERT INTO composition VALUES("105", "R18", "Roue arrière");
INSERT INTO composition VALUES("105", "R02", "Réflecteurs");
INSERT INTO composition VALUES("105", "P34", "Pédaliers");

INSERT INTO composition VALUES("106", "C44f", "Cadre");
INSERT INTO composition VALUES("106", "G9", "Guidon");
INSERT INTO composition VALUES("106", "F9", "Freins");
INSERT INTO composition VALUES("106", "S35", "Selle");
INSERT INTO composition VALUES("106", "DV57", "Dérailleur avant");
INSERT INTO composition VALUES("106", "DR86", "Dérailleur arrière");
INSERT INTO composition VALUES("106", "R19", "Roue avant");
INSERT INTO composition VALUES("106", "R18", "Roue arrière");
INSERT INTO composition VALUES("106", "R02", "Réflecteurs");
INSERT INTO composition VALUES("106", "P34", "Pédaliers");

INSERT INTO composition VALUES("107", "C43", "Cadre");
INSERT INTO composition VALUES("107", "G9", "Guidon");
INSERT INTO composition VALUES("107", "F9", "Freins");
INSERT INTO composition VALUES("107", "S37", "Selle");
INSERT INTO composition VALUES("107", "DV57", "Dérailleur avant");
INSERT INTO composition VALUES("107", "DR87", "Dérailleur arrière");
INSERT INTO composition VALUES("107", "R19", "Roue avant");
INSERT INTO composition VALUES("107", "R18", "Roue arrière");
INSERT INTO composition VALUES("107", "R02", "Réflecteurs");
INSERT INTO composition VALUES("107", "P34", "Pédaliers");
INSERT INTO composition VALUES("107", "O4", "Ordinateur");

INSERT INTO composition VALUES("108", "C43f", "Cadre");
INSERT INTO composition VALUES("108", "G9", "Guidon");
INSERT INTO composition VALUES("108", "F9", "Freins");
INSERT INTO composition VALUES("108", "S35", "Selle");
INSERT INTO composition VALUES("108", "DV57", "Dérailleur avant");
INSERT INTO composition VALUES("108", "DR87", "Dérailleur arrière");
INSERT INTO composition VALUES("108", "R19", "Roue avant");
INSERT INTO composition VALUES("108", "R18", "Roue arrière");
INSERT INTO composition VALUES("108", "R02", "Réflecteurs");
INSERT INTO composition VALUES("108", "P34", "Pédaliers");
INSERT INTO composition VALUES("108", "O4", "Ordinateur");

INSERT INTO composition VALUES("109", "C01", "Cadre");
INSERT INTO composition VALUES("109", "G12", "Guidon");
INSERT INTO composition VALUES("109", "S02", "Selle");
INSERT INTO composition VALUES("109", "R1", "Roue avant");
INSERT INTO composition VALUES("109", "R2", "Roue arrière");
INSERT INTO composition VALUES("109", "R09", "Réflecteurs");
INSERT INTO composition VALUES("109", "P1", "Pédaliers");
INSERT INTO composition VALUES("109", "S01", "Panier");

INSERT INTO composition VALUES("110", "C02", "Cadre");
INSERT INTO composition VALUES("110", "G12", "Guidon");
INSERT INTO composition VALUES("110", "S03", "Selle");
INSERT INTO composition VALUES("110", "R1", "Roue avant");
INSERT INTO composition VALUES("110", "R2", "Roue arrière");
INSERT INTO composition VALUES("110", "R09", "Réflecteurs");
INSERT INTO composition VALUES("110", "P1", "Pédaliers");
INSERT INTO composition VALUES("110", "S05", "Panier");

INSERT INTO composition VALUES("111", "C15", "Cadre");
INSERT INTO composition VALUES("111", "G12", "Guidon");
INSERT INTO composition VALUES("111", "F9", "Freins");
INSERT INTO composition VALUES("111", "S36", "Selle");
INSERT INTO composition VALUES("111", "DV15", "Dérailleur avant");
INSERT INTO composition VALUES("111", "DR23", "Dérailleur arrière");
INSERT INTO composition VALUES("111", "R11", "Roue avant");
INSERT INTO composition VALUES("111", "R12", "Roue arrière");
INSERT INTO composition VALUES("111", "R10", "Réflecteurs");
INSERT INTO composition VALUES("111", "P15", "Pédaliers");
INSERT INTO composition VALUES("111", "S74", "Panier");

INSERT INTO composition VALUES("112", "C87", "Cadre");
INSERT INTO composition VALUES("112", "G12", "Guidon");
INSERT INTO composition VALUES("112", "F9", "Freins");
INSERT INTO composition VALUES("112", "S36", "Selle");
INSERT INTO composition VALUES("112", "DV41", "Dérailleur avant");
INSERT INTO composition VALUES("112", "DR76", "Dérailleur arrière");
INSERT INTO composition VALUES("112", "R11", "Roue avant");
INSERT INTO composition VALUES("112", "R12", "Roue arrière");
INSERT INTO composition VALUES("112", "R10", "Réflecteurs");
INSERT INTO composition VALUES("112", "P15", "Pédaliers");
INSERT INTO composition VALUES("112", "S74", "Panier");

INSERT INTO composition VALUES("113", "C87f", "Cadre");
INSERT INTO composition VALUES("113", "G12", "Guidon");
INSERT INTO composition VALUES("113", "F9", "Freins");
INSERT INTO composition VALUES("113", "S34", "Selle");
INSERT INTO composition VALUES("113", "DV41", "Dérailleur avant");
INSERT INTO composition VALUES("113", "DR76", "Dérailleur arrière");
INSERT INTO composition VALUES("113", "R11", "Roue avant");
INSERT INTO composition VALUES("113", "R12", "Roue arrière");
INSERT INTO composition VALUES("113", "R10", "Réflecteurs");
INSERT INTO composition VALUES("113", "P15", "Pédaliers");
INSERT INTO composition VALUES("113", "S73", "Panier");

INSERT INTO composition VALUES("114", "C25", "Cadre");
INSERT INTO composition VALUES("114", "G7", "Guidon");
INSERT INTO composition VALUES("114", "F3", "Freins");
INSERT INTO composition VALUES("114", "S87", "Selle");
INSERT INTO composition VALUES("114", "DV132", "Dérailleur avant");
INSERT INTO composition VALUES("114", "DR52", "Dérailleur arrière");
INSERT INTO composition VALUES("114", "R44", "Roue avant");
INSERT INTO composition VALUES("114", "R47", "Roue arrière");
INSERT INTO composition VALUES("114", "P12", "Pédaliers");

INSERT INTO composition VALUES("115", "C26", "Cadre");
INSERT INTO composition VALUES("115", "G7", "Guidon");
INSERT INTO composition VALUES("115", "F3", "Freins");
INSERT INTO composition VALUES("115", "S87", "Selle");
INSERT INTO composition VALUES("115", "DV133", "Dérailleur avant");
INSERT INTO composition VALUES("115", "DR52", "Dérailleur arrière");
INSERT INTO composition VALUES("115", "R44", "Roue avant");
INSERT INTO composition VALUES("115", "R47", "Roue arrière");
INSERT INTO composition VALUES("115", "P12", "Pédaliers");

INSERT INTO client_etp VALUES("", "", "", "", "");
INSERT INTO client_p VALUES("", "", "", "", "", "");


SELECT nom_fournisseur, num_p FROM Fournisseur NATURAL JOIN fournit_p;

SELECT nom,prenom, nom_compagnie, sum(commande_p.quantite*pieces.prix_u) + sum(commande_v.quantite*velo.prix_u) 
FROM commande NATURAL JOIN commande_p JOIN commande_v ON commande_v.num_c = commande.num_c NATURAL JOIN pieces 
JOIN velo ON velo.num_p = commande_v.num_v
GROUP BY nom, prenom, nom_compagnie ORDER BY sum(commande_p.quantite*pieces.prix_u) + sum(commande_v.quantite*velo.prix_u) DESC LIMIT 3;

SELECT num_p, sum(quantite), sum(quantite*prix_u) FROM commande_p NATURAL JOIN pieces GROUP BY pieces.num_p 
ORDER BY(SELECT sum(quantite*prix_u) FROM commande_p NATURAL JOIN pieces GROUP BY pieces.num_p) desc;


INSERT INTO client_p VALUES ("Leboucher", "Maxime", "ESILV", "Lyon", "01 02 01 02 01", "maxime.leboucher@edu.devinci.fr");
INSERT INTO client_p VALUES ("Benazet", "Marie", "ESILV", "Toulouse", "01 02 08 72 01", "marie.benazet@edu.devinci.fr");
INSERT INTO client_p VALUES ("Houte", "Axel", "ESILV", "Paris", "01 54 47 02 01", "axel.houte@edu.devinci.fr");

INSERT INTO fidelio VALUES (4, 12, "Fidélio Diamant", "2020-09-06", 100, "2019-09-06", "Leboucher", "Maxime");
INSERT INTO fidelio VALUES (3, 10, "Fidélio Platine", "2017-08-20", 100, "2015-07-20", "Houte", "Axel");

INSERT INTO fournisseur VALUES ("0021C", "Fournisseur cadre 2", "Rennes", "", 2);
INSERT INTO fournisseur VALUES ("0022C", "Fournisseur cadre", "Montpellier", "", 3);
INSERT INTO fournisseur VALUES ("0123R", "Fournisseur roue", "Strasbourg", "", 4);

INSERT INTO fournit_p VALUES("C34", "0022C", 25, "12 jours", "01C", 7800);
INSERT INTO fournit_p VALUES("C32", "0022C", 28, "12 jours", "02C", 2500);
INSERT INTO fournit_p VALUES("C76", "0022C", 19, "12 jours", "03C", 420);
INSERT INTO fournit_p VALUES("C43", "0022C", 20, "12 jours", "04C", 675);

INSERT INTO fournit_p VALUES("C34", "0021C", 22, "8 jours", "13", 480);
INSERT INTO fournit_p VALUES("C32", "0021C", 26, "8 jours", "21", 2600);
INSERT INTO fournit_p VALUES("C76", "0021C", 21, "8 jours", "48", 145);
INSERT INTO fournit_p VALUES("C43", "0021C", 23, "8 jours", "98", 675);

INSERT INTO fournit_p VALUES("R45", "0123R", 25, "5 jours", "01R", 4600);
INSERT INTO fournit_p VALUES("R48", "0123R", 28, "5 jours", "02R", 280);
INSERT INTO fournit_p VALUES("R12", "0123R", 19, "5 jours", "03R", 1605);
INSERT INTO fournit_p VALUES("R19", "0123R", 20, "5 jours", "04R", 900);
