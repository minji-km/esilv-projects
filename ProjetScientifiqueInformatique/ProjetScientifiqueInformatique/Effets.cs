using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Media;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace ProjetScientifiqueInformatique
{
    class Effets
    {
        MyImage td;

        public Effets(MyImage td)
        {
            this.td = td;
        }

        public void Dupliquer(byte[] image)
        {
            File.WriteAllBytes("./Duppliqué.bmp", image);
        }

        public byte[] Rotation90Gauche(byte[] image)
        {
            int nvlarge = td.HauteurImage(image);
            int nvlhaut = td.LargeurImage(image);
            Couleur[,] mat = td.MatriceRGB(image);
            Couleur[,] mat2 = new Couleur[nvlhaut, nvlarge];
            for (int i = 0; i < mat2.GetLength(0); i++)
                for (int j = 0; j < mat2.GetLength(1); j++)
                {
                    mat2[i, j] = mat[j, i];
                }


            byte[] rotation = td.From_Mat_Color_To_Image(image, mat2);

            for (int i = 18; i < 22; i++)
                rotation[i] = td.Convertir_Int_To_Endian(nvlarge)[i - 18];
            for (int i = 22; i < 26; i++)
                rotation[i] = td.Convertir_Int_To_Endian(nvlhaut)[i - 22];

            return rotation;
        }

        public byte[] Rotation180(byte[] image)
        {
            Couleur[,] mat = td.MatriceRGB(image);
            Couleur[,] mat2 = new Couleur[mat.GetLength(0), mat.GetLength(1)];
            for (int i = 0; i < mat2.GetLength(0); i++)
                for (int j = 0; j < mat2.GetLength(1); j++)
                {
                    mat2[i, j] = mat[mat.GetLength(0) - 1 - i, mat.GetLength(1) - 1 - j];
                }

            byte[] rotation = td.From_Mat_Color_To_Image(image, mat2);

            return rotation;
        }

        public byte[] Rotation90Droite(byte[] image)
        {
            return Rotation90Gauche(Rotation180(image));
        }

        public byte[] Mirror(byte[] image, Couleur[,] mat)
        {
            Couleur[,] mirror = new Couleur[mat.GetLength(0), mat.GetLength(1)];
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                for (int j = 0; j < mat.GetLength(1); j++)
                {
                    mirror[i, j] = mat[i, mat.GetLength(1) - j - 1];
                }
            }
            return td.From_Mat_Color_To_Image(image, mirror);
        }

        public byte[] RotationDegréAvecBord(byte[] image, int degre)
        {
            double angle = -degre * Math.PI / 180;
            double tcos = Math.Cos(angle);
            double tsin = Math.Sin(angle);

            Couleur[,] im = td.MatriceRGB(image);
            int largeur = td.LargeurImage(image);
            int hauteur = td.HauteurImage(image);

            int nlarg = Convert.ToInt32(largeur * Math.Abs(tcos) + hauteur * Math.Abs(tsin));
            int nhaut = Convert.ToInt32(largeur * Math.Abs(tsin) + hauteur * Math.Abs(tcos));

            int centre_larg = largeur / 2;
            int centre_haut = hauteur / 2;
            int ncentre_haut = nhaut / 2;
            int ncentre_larg = nlarg / 2;



            Couleur[,] rot = new Couleur[nhaut, nlarg];
            for (int i = 0; i < nhaut; i++)
                for (int j = 0; j < nlarg; j++)
                    rot[i, j] = new Couleur(255, 255, 255);

            for (int i = 0; i < nhaut; i++)
                for (int j = 0; j < nlarg; j++)
                {
                    int phaut = Convert.ToInt32(tcos * (i - ncentre_haut) + tsin * (j - ncentre_larg) + centre_haut);
                    int plarg = Convert.ToInt32(-tsin * (i - ncentre_haut) + tcos * (j - ncentre_larg) + centre_larg);
                    if (degre == 270) plarg--;
                    else if (degre == 90) phaut--;


                    if ((plarg >= 0) && (phaut >= 0) && (plarg < largeur) && (phaut < hauteur))
                    {
                        rot[i, j] = im[phaut, plarg];
                    }
                }

            List<byte> liste = new List<byte>();
            for (int i = 0; i < 54; i++)
                liste.Add(image[i]);

            byte[] taille = td.Convertir_Int_To_Endian(nlarg * nhaut + 54);
            for (int i = 2; i < 6; i++)
                liste[i] = taille[i - 2];

            byte[] larg = td.Convertir_Int_To_Endian(nlarg);

            for (int i = 18; i < 22; i++)
                liste[i] = larg[i - 18];

            byte[] haut = td.Convertir_Int_To_Endian(nhaut);
            for (int i = 22; i < 26; i++)
                liste[i] = haut[i - 22];

            byte[] tailleim = td.Convertir_Int_To_Endian(nlarg * nhaut);

            for (int i = 34; i < 38; i++)
                liste[i] = tailleim[i - 34];

            for (int i = 0; i < rot.GetLength(0); i++)
                for (int j = 0; j < rot.GetLength(1); j++)
                {
                    liste.Add(rot[i, j].B);
                    liste.Add(rot[i, j].G);
                    liste.Add(rot[i, j].R);
                }

            return liste.ToArray();
        }

        public byte[] RotationDegré(byte[] image, int degre)
        {
            double angle = -degre * Math.PI / 180;
            double tcos = Math.Cos(angle);
            double tsin = Math.Sin(angle);

            Couleur[,] im = td.MatriceRGB(image);
            int largeur = td.LargeurImage(image);
            int hauteur = td.HauteurImage(image);


            int centre_larg = largeur / 2;
            int centre_haut = hauteur / 2;

            Couleur[,] rot = new Couleur[hauteur, largeur];
            for (int i = 0; i < hauteur; i++)
                for (int j = 0; j < largeur; j++)
                    rot[i, j] = new Couleur(255, 255, 255);

            for (int i = 0; i < hauteur; i++)
                for (int j = 0; j < largeur; j++)
                {
                    int phaut = Convert.ToInt32(tcos * (i - centre_larg) + tsin * (j - centre_haut) + centre_haut);
                    int plarg = Convert.ToInt32(-tsin * (i - centre_larg) + tcos * (j - centre_haut) + centre_larg);

                    if ((plarg >= 0) && (phaut >= 0) && (plarg < largeur) && (phaut < hauteur))
                    {
                        rot[i, j] = im[phaut, plarg];
                    }
                }
            return td.From_Mat_Color_To_Image(image, rot);
        }

        public byte[] Resize(byte[] image, double agrandissement)
        {
            Couleur[,] matRGB = td.MatriceRGB(image);
            int hauteur = td.HauteurImage(image);
            int largeur = td.LargeurImage(image);

            int newHaut = Convert.ToInt32(agrandissement * hauteur);
            int newLarg = Convert.ToInt32(agrandissement * largeur);

            int[,] imageR = new int[hauteur, largeur];
            int[,] imageG = new int[hauteur, largeur];
            int[,] imageB = new int[hauteur, largeur];

            for (int i = 0; i < matRGB.GetLength(0); i++)
            {
                for (int j = 0; j < matRGB.GetLength(1); j++)
                {
                    imageR[i, j] = matRGB[i, j].R;
                    imageG[i, j] = matRGB[i, j].G;
                    imageB[i, j] = matRGB[i, j].B;
                }
            }

            int[,] newR = new int[newHaut, newLarg];
            int[,] newG = new int[newHaut, newLarg];
            int[,] newB = new int[newHaut, newLarg];

            for (int newI = 0; newI < newHaut; newI++)
            {
                int oldI = (int)Math.Truncate(Convert.ToDouble(newI * 1 / agrandissement));
                if (oldI < 0)
                {
                    oldI = 0;
                }
                if (oldI >= hauteur)
                {
                    oldI = hauteur - 1;
                }

                for (int newJ = 0; newJ < newLarg; newJ++)
                {
                    int oldJ = (int)Math.Truncate(Convert.ToDouble(newJ * 1 / agrandissement));
                    if (oldJ < 0)
                    {
                        oldJ = 0;
                    }
                    if (oldJ >= largeur)
                    {
                        oldJ = largeur - 1;
                    }

                    newR[newI, newJ] = imageR[oldI, oldJ];
                    newG[newI, newJ] = imageG[oldI, oldJ];
                    newB[newI, newJ] = imageB[oldI, oldJ];
                }
            }

            Couleur[,] newMatRGB = new Couleur[newR.GetLength(0), newR.GetLength(1)];
            for (int k = 0; k < newMatRGB.GetLength(0); k++)
            {
                for (int l = 0; l < newMatRGB.GetLength(1); l++)
                {
                    newMatRGB[k, l] = new Couleur(Convert.ToByte(newR[k, l]), Convert.ToByte(newG[k, l]), Convert.ToByte(newB[k, l]));
                }
            }

            List<byte> liste = new List<byte>();
            for (int i = 0; i < 54; i++)
                liste.Add(image[i]);

            byte[] taille = td.Convertir_Int_To_Endian(newLarg * newHaut + 54);
            for (int i = 2; i < 6; i++)
                liste[i] = taille[i - 2];

            byte[] larg = td.Convertir_Int_To_Endian(newLarg);

            for (int i = 18; i < 22; i++)
                liste[i] = larg[i - 18];

            byte[] haut = td.Convertir_Int_To_Endian(newHaut);
            for (int i = 22; i < 26; i++)
                liste[i] = haut[i - 22];

            byte[] tailleim = td.Convertir_Int_To_Endian(newLarg * newHaut);

            for (int i = 34; i < 38; i++)
                liste[i] = tailleim[i - 34];

            for (int i = 0; i < newMatRGB.GetLength(0); i++)
                for (int j = 0; j < newMatRGB.GetLength(1); j++)
                {
                    liste.Add(newMatRGB[i, j].B);
                    liste.Add(newMatRGB[i, j].G);
                    liste.Add(newMatRGB[i, j].R);
                }

            return liste.ToArray();
        }

    }
}
