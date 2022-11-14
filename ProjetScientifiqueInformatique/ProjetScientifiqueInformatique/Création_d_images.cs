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
    class Création_d_images
    {
        MyImage td;
        public Création_d_images(MyImage td)
        {
            this.td = td;
        }

        public byte[] Mandelbrot(byte[] image)
        {
            double x1 = -2.1;
            double x2 = 0.6;
            double y1 = -1.2;
            double y2 = 1.2;
            int itmax = 50;
            int larg = td.LargeurImage(image);
            int haut = td.HauteurImage(image);

            double c_r = 0;
            double c_i = 0;
            double z_r = 0;
            double z_i = 0;
            int it = 0;

            double zoom_x = haut / (x2 - x1);
            double zoom_y = larg / (y2 - y1);

            Couleur[,] matrice = td.MatriceRGB(image);
            for (int i = 0; i < haut; i++)
            {
                for (int j = 0; j < larg; j++)
                {
                    c_r = (i / zoom_x) + x1;
                    c_i = (j / zoom_y) + y1;
                    z_r = 0;
                    z_i = 0;
                    it = 0;

                    do
                    {
                        double temp = z_r;
                        z_r = (z_r * z_r) - (z_i * z_i) + c_r;
                        z_i = 2 * z_i * temp + c_i;
                        it++;
                    } while ((z_r * z_r + z_i * z_i < 4) && (it < itmax));

                    if (it == itmax)
                    {
                        matrice[i, j] = new Couleur(0, 0, 0);
                    }
                    else
                    {
                        matrice[i, j] = new Couleur(Convert.ToByte(it * 255 / itmax), 0, 0);
                    }
                }
            }

            byte[] fractale = td.From_Mat_Color_To_Image(image, matrice);
            return fractale;

        }

        public byte[] ImagePourHisto(Couleur[,] matRGB, byte[] image)
        {
            int newHaut = matRGB.GetLength(0);
            int newLarg = matRGB.GetLength(1);

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

            for (int i = matRGB.GetLength(0) - 1; i > -1; i--)
            {
                for (int j = 0; j < matRGB.GetLength(1); j++)
                {
                    liste.Add(matRGB[i, j].B);
                    liste.Add(matRGB[i, j].G);
                    liste.Add(matRGB[i, j].R);
                }
            }

            return liste.ToArray();
        }
        public Couleur[,] TabHistToMatRGB(int[] histo, int color)
        {

            Couleur[,] histoMat = new Couleur[histo.Max(), 256];
            for (int i = 0; i < histoMat.GetLength(0); i++)
            {
                for (int j = 0; j < histoMat.GetLength(1); j++)
                {
                    if (histoMat.GetLength(0) - 1 - i <= histo[j])
                    {
                        switch (color)
                        {
                            case 1:
                                histoMat[i, j] = new Couleur(255, 0, 0);
                                break;
                            case 2:
                                histoMat[i, j] = new Couleur(0, 255, 0);
                                break;
                            case 3:
                                histoMat[i, j] = new Couleur(0, 0, 255);
                                break;
                        }

                    }
                    else
                    {
                        histoMat[i, j] = new Couleur(255, 255, 255);
                    }
                }
            }

            return histoMat;
        }
        public byte[] Histogramme(byte[] image, Couleur[,] mat, int couleur)
        {

            int[] histo = new int[256];

            for (int k = 0; k < 256; k++)
            {
                histo[k] = 0;
            }

            for (int i = 0; i < mat.GetLength(0); i++)
            {
                for (int j = 0; j < mat.GetLength(1); j++)
                {
                    switch (couleur)
                    {
                        case 1:
                            histo[mat[i, j].R]++;
                            break;

                        case 2:
                            histo[mat[i, j].G]++;
                            break;

                        case 3:
                            histo[mat[i, j].B]++;
                            break;
                    }
                }
            }

            Couleur[,] histoMat = TabHistToMatRGB(histo, couleur);

            byte[] histogramme = ImagePourHisto(histoMat, image);

            return histogramme;
        }



    }
}
