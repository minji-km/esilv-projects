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
    class MyImage
    {


        //lit une fichier .bmp et le transforme en instance de la classe MyImage
        public MyImage()
        {
        }

        //prend une instance de MyImage et la transforme en fichier binaire respectant la structure du fichier .bmp
        public void From_Image_To_File(string file)
        {
            byte[] image = File.ReadAllBytes(file);
            string text = "";
            for (int i = 0; i < image.Length; i++)
            {
                text = text + image[i] + " ";
            }
            File.WriteAllText("fichierbinaire.txt", text);
        }

        //Convertit une séquence d'octets au format little endian en entier
        public int Convertir_Endian_To_Int(byte[] tab)
        {
            double resultat = 0;
            for (int i = 0; i < tab.Length; i++)
            {
                resultat += tab[i] * Math.Pow(256, i);
            }
            return Convert.ToInt32(resultat);
        }

        //Convertit un entier en séquence d'octets au format little endian
        public byte[] Convertir_Int_To_Endian(int val)
        {
            double stock = val;
            byte[] endian = new byte[4];
            for (int i = 3; i >= 0; i--)
            {
                double reste = stock % Math.Pow(256, i);
                endian[i] = Convert.ToByte((stock - reste) / Math.Pow(256, i));
                stock = stock - endian[i] * Math.Pow(256, i);
            }
            return endian;
        }

        public string TypeImage(byte[] image)
        {
            string type = "";
            for (int i = 0; i < 2; i++)
                type += Encoding.ASCII.GetString(new byte[] { image[i] });
            Console.Write(type);
            return type;
        }

        public int TailleOffset(byte[] image)
        {
            double taille = 0;
            for (int i = 10; i < 14; i++)
                taille += image[i] * Math.Pow(256, i - 10);

            return Convert.ToInt32(taille);
        }

        public int TailleFichier(byte[] image)
        {
            double taille = 0;
            for (int i = 2; i < 6; i++)
                taille += image[i] * Math.Pow(256, i - 2);

            return Convert.ToInt32(taille);
        }

        public int LargeurImage(byte[] image)
        {
            double largeur = 0;
            for (int i = 18; i < 22; i++)
                largeur += image[i] * Math.Pow(256, i - 18);
            return Convert.ToInt32(largeur);
        }

        public int HauteurImage(byte[] image)
        {
            double hauteur = 0;
            for (int i = 22; i < 26; i++)
                hauteur += image[i] * Math.Pow(256, i - 22);
            return Convert.ToInt32(hauteur);
        }

        public Couleur[,] MatriceRGB(byte[] image)
        {
            int hauteur = HauteurImage(image);
            int largeur = LargeurImage(image);
            Couleur[,] mat = new Couleur[hauteur, largeur];

            Queue<Couleur> liste = new Queue<Couleur>();
            for (int i = 54; i < image.Length; i += 3)
            {
                Couleur pixel = new Couleur(image[i + 2], image[i + 1], image[i]);
                liste.Enqueue(pixel);
            }

            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    mat[i, j] = liste.Dequeue();
                }
            }
            return mat;
        }

        //Convertit une matrice de couleurs en image bitmap
        public byte[] From_Mat_Color_To_Image(byte[] image, Couleur[,] matRGB)
        {
            List<byte> liste = new List<byte>();
            for (int i = 0; i < 54; i++)
                liste.Add(image[i]);
            for (int i = 0; i < matRGB.GetLength(0); i++)
            {
                for (int j = 0; j < matRGB.GetLength(1); j++)
                {
                    liste.Add(matRGB[i, j].B);
                    liste.Add(matRGB[i, j].G);
                    liste.Add(matRGB[i, j].R);
                }
            }
            if (image.Length == liste.Count) return liste.ToArray();
            else
            {
                Console.WriteLine("ERREUR");
                return liste.ToArray();
            }
        }


    }
}
