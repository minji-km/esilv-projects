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
    class Filtre
    {
        MyImage td;

        public Filtre(MyImage td)
        {
            this.td = td;
        }

        public static int[] SommePourConvolution(int[,] noyau, Couleur[,] matrice, int ligne, int colonne, bool flou) //effectue la somme descouleurs pour la révolution
        {
            int[] somme = { 0, 0, 0 };
            int ligneNoyau = noyau.GetLength(0);
            int colonneNoyau = noyau.GetLength(1);

            for (int i = 0; i < ligneNoyau; i++)
            {
                for (int j = 0; j < colonneNoyau; j++)
                {
                    int x = i + (ligne - ligneNoyau / 2);
                    if (x < 0) x = matrice.GetLength(0) + x;
                    if (x >= matrice.GetLength(0)) x = x - matrice.GetLength(0);
                    int y = j + (colonne - colonneNoyau / 2);
                    if (y < 0) y = matrice.GetLength(1) + y;
                    if (y >= matrice.GetLength(1)) y = y - matrice.GetLength(1);
                    somme[0] += noyau[i, j] * matrice[x, y].B; //somme des bleus
                    somme[1] += noyau[i, j] * matrice[x, y].G; //somme des vert
                    somme[2] += noyau[i, j] * matrice[x, y].R; //somme des rouges
                }
            }
            if (flou)
            {
                for (int k = 0; k < somme.Length; k++)
                {
                    if (somme[k] < 0)
                    {
                        somme[k] = 0;
                    }
                    else
                    {
                        somme[k] = somme[k] / 9;
                    }
                }
            }
            else
            {
                for (int k = 0; k < somme.Length; k++)
                {
                    if (somme[k] < 0)
                    {
                        somme[k] = 0;
                    }
                    if (somme[k] > 255)
                    {
                        somme[k] = 255;
                    }
                }
            }
            return somme;
        }

        public Couleur[,] MatriceDeConvolution(Couleur[,] mat, int[,] noyau, bool flou)
        {
            Couleur[,] rendu = new Couleur[mat.GetLength(0), mat.GetLength(1)];

            for (int i = 0; i < mat.GetLength(0); i++)
            {
                for (int j = 0; j < mat.GetLength(1); j++)
                {
                    int[] color = SommePourConvolution(noyau, mat, i, j, flou);
                    rendu[i, j] = new Couleur(Convert.ToByte(color[2]), Convert.ToByte(color[1]), Convert.ToByte(color[0]));
                }
            }
            return rendu;
        }

        public byte[] DetectionDeContour(byte[] image, Couleur[,] mat)
        {
            int[,] noyau = { { 0, 1, 0 }, { 1, -4, 1 }, { 0, 1, 0 } };
            Couleur[,] detection = MatriceDeConvolution(mat, noyau, false);
            return td.From_Mat_Color_To_Image(image, detection);
        }

        public byte[] RenforcementDesBords(byte[] image, Couleur[,] mat)
        {
            int[,] noyau = { { 0, 0, 0 }, { -1, 1, 0 }, { 0, 0, 0 } };
            Couleur[,] renforcement = MatriceDeConvolution(mat, noyau, false);
            return td.From_Mat_Color_To_Image(image, renforcement);
        }

        public byte[] Flou(byte[] image, Couleur[,] mat)
        {
            int[,] noyau = { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
            Couleur[,] flou = MatriceDeConvolution(mat, noyau, true);
            return td.From_Mat_Color_To_Image(image, flou);
        }

        public byte[] Repoussage(byte[] image, Couleur[,] mat)
        {
            int[,] noyau = { { -2, -1, 0 }, { -1, 1, 1 }, { 0, 1, 2 } };
            Couleur[,] repoussage = MatriceDeConvolution(mat, noyau, false);
            return td.From_Mat_Color_To_Image(image, repoussage);
        }

        public byte[] BlackGreyWhite(byte[] image, Couleur[,] mat)
        {
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                for (int j = 0; j < mat.GetLength(1); j++)
                {
                    Couleur mycolor = mat[i, j];
                    byte nuance = Convert.ToByte((mycolor.B + mycolor.G + mycolor.R) / 3);
                    mat[i, j] = new Couleur(nuance, nuance, nuance);
                }
            }
            return td.From_Mat_Color_To_Image(image, mat);
        }

        public byte[] Negatif(byte[] image, Couleur[,] mat)
        {
            for (int i = 0; i < td.HauteurImage(image); i++)
            {
                for (int j = 0; j < td.LargeurImage(image); j++)
                {
                    Couleur mycolor = mat[i, j];
                    mat[i, j] = new Couleur(Convert.ToByte(255 - mycolor.R), Convert.ToByte(255 - mycolor.G), Convert.ToByte(255 - mycolor.B));
                }
            }
            return td.From_Mat_Color_To_Image(image, mat);
        }

    }
}
