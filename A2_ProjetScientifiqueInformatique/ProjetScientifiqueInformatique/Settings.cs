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
    class Settings
    {
        bool valide;
        string message;
        static int version;
        public Settings(string message)
        {
            this.message = message;
            version = 0;
            if (this.message.Length < 25) version = 1; //152 bits
            else if (this.message.Length < 48) version = 2; //324 bits

        }

        public int[] ValeurNum()
        {
            valide = true;
            int[] messageTabNum = new int[message.Length];
            for (int i = 0; i < message.Length && valide; i++)
            {
                switch (message[i])
                {
                    case '0':
                        messageTabNum[i] = 0;
                        break;
                    case '1':
                        messageTabNum[i] = 1;
                        break;
                    case '2':
                        messageTabNum[i] = 2;
                        break;
                    case '3':
                        messageTabNum[i] = 3;
                        break;
                    case '4':
                        messageTabNum[i] = 4;
                        break;
                    case '5':
                        messageTabNum[i] = 5;
                        break;
                    case '6':
                        messageTabNum[i] = 6;
                        break;
                    case '7':
                        messageTabNum[i] = 7;
                        break;
                    case '8':
                        messageTabNum[i] = 8;
                        break;
                    case '9':
                        messageTabNum[i] = 9;
                        break;
                    case 'A':
                        messageTabNum[i] = 10;
                        break;
                    case 'B':
                        messageTabNum[i] = 11;
                        break;
                    case 'C':
                        messageTabNum[i] = 12;
                        break;
                    case 'D':
                        messageTabNum[i] = 13;
                        break;
                    case 'E':
                        messageTabNum[i] = 14;
                        break;
                    case 'F':
                        messageTabNum[i] = 15;
                        break;
                    case 'G':
                        messageTabNum[i] = 16;
                        break;
                    case 'H':
                        messageTabNum[i] = 17;
                        break;
                    case 'I':
                        messageTabNum[i] = 18;
                        break;
                    case 'J':
                        messageTabNum[i] = 19;
                        break;
                    case 'K':
                        messageTabNum[i] = 20;
                        break;
                    case 'L':
                        messageTabNum[i] = 21;
                        break;
                    case 'M':
                        messageTabNum[i] = 22;
                        break;
                    case 'N':
                        messageTabNum[i] = 23;
                        break;
                    case 'O':
                        messageTabNum[i] = 24;
                        break;
                    case 'P':
                        messageTabNum[i] = 25;
                        break;
                    case 'Q':
                        messageTabNum[i] = 26;
                        break;
                    case 'R':
                        messageTabNum[i] = 27;
                        break;
                    case 'S':
                        messageTabNum[i] = 28;
                        break;
                    case 'T':
                        messageTabNum[i] = 29;
                        break;
                    case 'U':
                        messageTabNum[i] = 30;
                        break;
                    case 'V':
                        messageTabNum[i] = 31;
                        break;
                    case 'W':
                        messageTabNum[i] = 32;
                        break;
                    case 'X':
                        messageTabNum[i] = 33;
                        break;
                    case 'Y':
                        messageTabNum[i] = 34;
                        break;
                    case 'Z':
                        messageTabNum[i] = 35;
                        break;
                    case ' ':
                        messageTabNum[i] = 36;
                        break;
                    case '$':
                        messageTabNum[i] = 37;
                        break;
                    case '%':
                        messageTabNum[i] = 38;
                        break;
                    case '*':
                        messageTabNum[i] = 39;
                        break;
                    case '+':
                        messageTabNum[i] = 40;
                        break;
                    case '-':
                        messageTabNum[i] = 41;
                        break;
                    case '.':
                        messageTabNum[i] = 42;
                        break;
                    case '/':
                        messageTabNum[i] = 43;
                        break;
                    case ':':
                        messageTabNum[i] = 44;
                        break;
                    default:
                        valide = false;
                        break;
                }
            }
            return messageTabNum;
        }

        public bool EntreeCorrecte()
        {
            return valide;
        }


        public int[] AlphaNum(int[] messageTabNum)
        {
            List<int> tabAlphaNum = new List<int>();
            //Indicateur de mode
            tabAlphaNum.Add(0);
            tabAlphaNum.Add(0);
            tabAlphaNum.Add(1);
            tabAlphaNum.Add(0);

            double longueur = message.Length;
            //Indicateur du nombre de caractères
            for (int j = 8; j >= 0; j--)
            {
                if (longueur - Math.Pow(2, j) >= 0)
                {
                    longueur = longueur - Math.Pow(2, j);
                    tabAlphaNum.Add(1);
                }
                else tabAlphaNum.Add(0);
            }

            if (messageTabNum.Length%2 == 0)
            {
                for(int i = 0; i<messageTabNum.Length-1;i+=2)
                {
                    double nombre = 45 * messageTabNum[i] + messageTabNum[i + 1];
                    for(int j = 10; j>=0; j--)
                    {
                        if (nombre - Math.Pow(2, j) >= 0)
                        {
                            nombre = nombre - Math.Pow(2, j);
                            tabAlphaNum.Add(1);
                        }
                        else tabAlphaNum.Add(0);
                    }
                }
            }
            else
            {
                double nombre = 0;
                for (int i = 0; i < messageTabNum.Length-1; i += 2)
                {
                    nombre = 45 * messageTabNum[i] + messageTabNum[i + 1];
                    for (int j = 10; j >= 0; j--)
                    {
                        if (nombre - Math.Pow(2, j) >= 0)
                        {
                            nombre = nombre - Math.Pow(2, j);
                            tabAlphaNum.Add(1);
                        }
                        else tabAlphaNum.Add(0);
                    }
                }
                nombre = messageTabNum[message.Length - 1];
                for (int j = 5; j >= 0; j--)
                {
                    
                    if (nombre - Math.Pow(2, j) >= 0)
                    {
                        nombre = nombre - Math.Pow(2, j);
                        tabAlphaNum.Add(1);
                    }
                    else tabAlphaNum.Add(0);
                }


            }

            int capacite1 = 152;
            int capacite2 = 324;
            int capacite = 0;
            if (version == 1) capacite = capacite1;
            if (version == 2) capacite = capacite2;
            if (capacite - tabAlphaNum.Count < 4)
            {
                switch (capacite - tabAlphaNum.Count)
                {
                    case 1:
                        tabAlphaNum.Add(0);
                        break;
                    case 2:
                        tabAlphaNum.Add(0);
                        tabAlphaNum.Add(0);
                        break;
                    case 3:
                        tabAlphaNum.Add(0);
                        tabAlphaNum.Add(0);
                        tabAlphaNum.Add(0);
                        break;
                }
            }
            else
            {
                tabAlphaNum.Add(0);
                tabAlphaNum.Add(0);
                tabAlphaNum.Add(0);
                tabAlphaNum.Add(0);

                if ((tabAlphaNum.Count % 8 != 0) && (capacite - tabAlphaNum.Count > 0))
                {
                    int mult8 = (tabAlphaNum.Count / 8 + 1) * 8 - tabAlphaNum.Count;
                    for (int i = 1; i <= mult8; i++) tabAlphaNum.Add(0);
                }
                if (tabAlphaNum.Count % 8 != 0) Console.WriteLine("ERREUR");
                int nbpaves = (capacite - tabAlphaNum.Count) / 8;
                if (nbpaves % 2 == 0)
                {
                    nbpaves = nbpaves / 2;
                    for (int i = 1; i <= nbpaves; i++)
                    {
                        tabAlphaNum.Add(1);
                        tabAlphaNum.Add(1);
                        tabAlphaNum.Add(1);
                        tabAlphaNum.Add(0);
                        tabAlphaNum.Add(1);
                        tabAlphaNum.Add(1);
                        tabAlphaNum.Add(0);
                        tabAlphaNum.Add(0);

                        tabAlphaNum.Add(0);
                        tabAlphaNum.Add(0);
                        tabAlphaNum.Add(0);
                        tabAlphaNum.Add(1);
                        tabAlphaNum.Add(0);
                        tabAlphaNum.Add(0);
                        tabAlphaNum.Add(0);
                        tabAlphaNum.Add(1);
                    }
                }
                else
                {
                    nbpaves = (nbpaves - 1) / 2;
                    for (int i = 1; i <= nbpaves; i++)
                    {
                        tabAlphaNum.Add(1);
                        tabAlphaNum.Add(1);
                        tabAlphaNum.Add(1);
                        tabAlphaNum.Add(0);
                        tabAlphaNum.Add(1);
                        tabAlphaNum.Add(1);
                        tabAlphaNum.Add(0);
                        tabAlphaNum.Add(0);

                        tabAlphaNum.Add(0);
                        tabAlphaNum.Add(0);
                        tabAlphaNum.Add(0);
                        tabAlphaNum.Add(1);
                        tabAlphaNum.Add(0);
                        tabAlphaNum.Add(0);
                        tabAlphaNum.Add(0);
                        tabAlphaNum.Add(1);
                    }
                    tabAlphaNum.Add(1);
                    tabAlphaNum.Add(1);
                    tabAlphaNum.Add(1);
                    tabAlphaNum.Add(0);
                    tabAlphaNum.Add(1);
                    tabAlphaNum.Add(1);
                    tabAlphaNum.Add(0);
                    tabAlphaNum.Add(0);
                }


            }



            return tabAlphaNum.ToArray();
        }

        public int[] Correction()
        {
            Encoding u8 = Encoding.UTF8;
            int nbEC = 0;
            if (version == 1) nbEC = 7;
            else if (version == 2) nbEC = 10;
            byte[] bytes = u8.GetBytes(message);
            byte[] result = ReedSolomonAlgorithm.Encode(bytes, nbEC, ErrorCorrectionCodeType.QRCode);
            int[] corrige = new int[result.Length];
            for (int i = 0; i < corrige.Length; i++) corrige[i] = Convert.ToInt32(result[i]);
            List<int> correction = new List<int>();
            foreach(int b in corrige)
            {
                double nombre = b;
                for (int j = 10; j >= 0; j--)
                {
                    if (nombre - Math.Pow(2, j) >= 0)
                    {
                        nombre = nombre - Math.Pow(2, j);
                        correction.Add(1);
                    }
                    else correction.Add(0);
                }
            }

            return correction.ToArray();
        }

        public Couleur[,] MatriceQR()
        {
            Couleur[,] matrice;
            int taille = 0;
            string fichier = "";
            if (version == 1) taille = 24;
            else if (version == 2) taille = 28;

            matrice = new Couleur[taille, taille];
            Couleur noir = new Couleur(0, 0, 0);
            Couleur blanc = new Couleur(255, 255, 255);
            switch (version)
            {
                case 1:
                    fichier = "21.bmp";
                    for (int i = 0; i < 21; i++)
                    {
                        for (int j = 0; j < 21; j++)
                            matrice[i, j] = new Couleur(139, 69, 19);
                    }

                    for (int i = 0; i < 9; i++)
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            matrice[i, j] = new Couleur(0,0,255);
                            matrice[i, j + 13] = new Couleur(0, 0, 255);
                            matrice[i + 13, j] = new Couleur(0, 0, 255);
                        }
                    }

                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            matrice[i, j] = blanc;
                            matrice[i, j + 13] = blanc;
                            matrice[i + 13, j] = blanc;
                        }
                    }
                    for (int i = 0; i < 7; i++)
                    {
                        for (int j = 0; j < 7; j++)
                        {
                            matrice[i, j] = noir;
                            matrice[i, j + 14] = noir;
                            matrice[i + 14, j] = noir;
                        }
                    }
                    for (int i = 1; i < 6; i++)
                    {
                        for (int j = 1; j < 6; j++)
                        {
                            matrice[i, j] = blanc;
                            matrice[i, j + 14] = blanc;
                            matrice[i + 14, j] = blanc;
                        }
                    }
                    for (int i = 2; i < 5; i++)
                    {
                        for (int j = 2; j < 5; j++)
                        {
                            matrice[i, j] = noir;
                            matrice[i, j + 14] = noir;
                            matrice[i + 14, j] = noir;
                        }
                    }


                    for (int i = 6; i < 15; i++)
                    {
                        if (i % 2 == 0)
                        {
                            matrice[6, i] = noir;
                            matrice[i, 6] = noir;
                        }
                        else
                        {
                            matrice[6, i] = blanc;
                            matrice[i, 6] = blanc; 
                        }
                    }
                    for (int i = 0; i < taille; i++)
                        for (int j = 21; j < taille; j++)
                        {
                            matrice[i, j] = blanc;
                            matrice[j, i] = blanc;
                        }
                    for(int j = 14; j<21; j++)
                    {
                        matrice[8, j] = new Couleur(0, 0, 255);
                        matrice[j, 8] = new Couleur(0, 0, 255);
                    }
                    matrice[8, 13] = noir;
                    break;
                case 2:
                    fichier = "25.bmp";
                    for (int i = 0; i < 25; i++)
                    {
                        for (int j = 0; j < 25; j++)
                            matrice[i, j] = new Couleur(139, 69, 19);
                    }

                    for (int i = 0; i < 9; i++)
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            matrice[i, j] = new Couleur(0, 0, 255);
                            matrice[i, j + 17] = new Couleur(0, 0, 255);
                            matrice[i + 17, j] = new Couleur(0, 0, 255);
                        }
                    }

                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            matrice[i, j] = blanc;
                            matrice[i, j + 17] = blanc;
                            matrice[i + 17, j] = blanc;
                        }
                    }
                    for (int i = 0; i < 7; i++)
                    {
                        for (int j = 0; j < 7; j++)
                        {
                            matrice[i, j] = noir;
                            matrice[i, j + 18] = noir;
                            matrice[i + 18, j] = noir;
                        }
                    }
                    for (int i = 1; i < 6; i++)
                    {
                        for (int j = 1; j < 6; j++)
                        {
                            matrice[i, j] = blanc;
                            matrice[i, j + 18] = blanc;
                            matrice[i + 18, j] = blanc;
                        }
                    }
                    for (int i = 2; i < 5; i++)
                    {
                        for (int j = 2; j < 5; j++)
                        {
                            matrice[i, j] = noir;
                            matrice[i, j + 18] = noir;
                            matrice[i + 18, j] = noir;
                        }
                    }

                    for (int i = 6; i < 19; i++)
                    {
                        if (i % 2 == 0)
                        {
                            matrice[6, i] = noir;
                            matrice[i, 6] = noir;
                        }
                        else
                        {
                            matrice[6, i] = blanc;
                            matrice[i, 6] = blanc;
                        }
                    }
                    for (int i = 0; i < taille; i++)
                        for (int j = 25; j < taille; j++)
                        {
                            matrice[i, j] = blanc;
                            matrice[j, i] = blanc;
                        }
                    for(int i = 16; i<21;i++)
                    {
                        for(int j = 16; j<21;j++)
                        {
                            matrice[i, j] = noir;
                        }
                    }
                    for (int i = 17; i < 20; i++)
                    {
                        for (int j = 17; j < 20; j++)
                        {
                            matrice[i, j] = blanc;
                        }
                    }
                    for (int j = 18; j < 25; j++)
                    {
                        matrice[8, j] = new Couleur(0, 0, 255);
                        matrice[j, 8] = new Couleur(0, 0, 255);
                    }
                    matrice[18, 18] = noir;
                    matrice[8, 17] = noir;
                    break;
            }

            
            return matrice;
        }

        public Queue<int> CaseBleue()
        {
            Queue<int> masque = new Queue<int>();
            masque.Enqueue(1);
            masque.Enqueue(1);
            masque.Enqueue(1);
            masque.Enqueue(0);
            masque.Enqueue(1);
            masque.Enqueue(1);
            masque.Enqueue(1);
            masque.Enqueue(1);
            masque.Enqueue(1);
            masque.Enqueue(0);
            masque.Enqueue(0);
            masque.Enqueue(0);
            masque.Enqueue(1);
            masque.Enqueue(0);
            masque.Enqueue(0);

            masque.Enqueue(1);
            masque.Enqueue(1);
            masque.Enqueue(1);
            masque.Enqueue(0);
            masque.Enqueue(1);
            masque.Enqueue(1);
            masque.Enqueue(1);
            masque.Enqueue(1);
            masque.Enqueue(1);
            masque.Enqueue(0);
            masque.Enqueue(0);
            masque.Enqueue(0);
            masque.Enqueue(1);
            masque.Enqueue(0);
            masque.Enqueue(0);

            return masque;
        }

        public Couleur[,] Remplissage()
        {
            string fichier = "";
            if (version == 1) fichier = "21.bmp";
            else if (version == 2) fichier = "25.bmp";
            byte[] image = File.ReadAllBytes(fichier);
            MyImage td = new MyImage();
            Effets effect = new Effets(td);
            int[] alphanum = AlphaNum(ValeurNum());
            int[] correction = Correction();
            Queue<int> casebleue = CaseBleue();
            Queue<int> code = new Queue<int>();
            foreach (int i in alphanum) code.Enqueue(i);
            foreach (int i in correction) code.Enqueue(i);

            Couleur noir = new Couleur(0, 0, 0);
            Couleur blanc = new Couleur(255, 255, 255);

            Couleur[,] matrice = MatriceQR();
            File.WriteAllBytes("matrice.bmp", td.From_Mat_Color_To_Image(image, matrice));

            int taille = matrice.GetLength(0) - 4;
            for(int i = taille; i>=0; i-=2)
            {
                if (i % 4 == 0)
                {
                    for (int j = taille; j >= 0; j --)
                    {
                        if (matrice[i, j].R == 139)
                        {
                            if (code.Peek() == 0)
                            {

                                matrice[i, j] = blanc;
                            }
                            else matrice[i, j] = noir;
                            code.Dequeue();
                        }

                        if ((i>0) && (matrice[i - 1, j].R == 139))
                        {
                            if (code.Peek() == 0)
                            {

                                matrice[i-1, j] = blanc;
                            }
                            else matrice[i-1, j] = noir;
                            code.Dequeue();
                        }
                    }
                }
                else
                {
                    for (int j = 0; j <= taille; j++)
                    {
                        if (matrice[i, j].R == 139)
                        {
                            if (code.Peek() == 0)
                            {

                                matrice[i, j] = blanc;
                            }
                            else matrice[i, j] = noir;
                            code.Dequeue();
                        }

                        if ((i>0) && matrice[i - 1, j].R == 139)
                        {
                            if (code.Peek() == 0)
                            {

                                matrice[i-1, j] = blanc;
                            }
                            else matrice[i-1, j] = noir;
                            code.Dequeue();
                        }
                    }
                }
            }
            

            if(taille == 20)
            {

                for (int i = taille; i > taille - 8; i--)
                {
                    if (casebleue.Peek() == 0) matrice[i, 8] = blanc;
                    else matrice[i, 8] = noir;

                    casebleue.Dequeue();
                }
                for (int j = 14; j<=taille; j++)
                {
                    if (casebleue.Peek() == 0) matrice[8, j] = blanc;
                    else matrice[8, j] = noir;

                    casebleue.Dequeue();
                }
                for (int j = 0; j < 9; j++)
                {
                    if (matrice[8, j].R == 0 && matrice[8, j].G == 0 && matrice[8, j].B == 255)
                    {
                        if (casebleue.Peek() == 0) matrice[8, j] = blanc;
                        else matrice[8,j ] = noir;

                        casebleue.Dequeue();
                    }

                }
                for (int i = 7; i>=0; i--)
                {
                    if (matrice[i, 8].R == 0 && matrice[i,8].G == 0 && matrice[i,8].B == 255)
                    {
                        if (casebleue.Peek() == 0) matrice[i, 8] = blanc;
                        else matrice[i, 8] = noir;
                        casebleue.Dequeue();
                    }
                }
            }
            else if(taille == 24)
            {
                
                    for (int i = taille; i > taille - 8; i--)
                    {
                        if (casebleue.Peek() == 0) matrice[i, 8] = blanc;
                        else matrice[i, 8] = noir;

                        casebleue.Dequeue();
                    }
                    for (int j = 18; j <= taille; j++)
                    {
                        if (casebleue.Peek() == 0) matrice[8, j] = blanc;
                        else matrice[8, j] = noir;

                        casebleue.Dequeue();
                    }
                    for (int j = 0; j < 9; j++)
                    {
                        if (matrice[8, j].R == 0 && matrice[8, j].G == 0 && matrice[8, j].B == 255)
                        {
                            if (casebleue.Peek() == 0) matrice[8, j] = blanc;
                            else matrice[8, j] = noir;

                            casebleue.Dequeue();
                        }

                    }
                    for (int i = 7; i >= 0; i--)
                    {
                        if (matrice[i, 8].R == 0 && matrice[i, 8].G == 0 && matrice[i, 8].B == 255)
                        {
                            if (casebleue.Peek() == 0) matrice[i, 8] = blanc;
                            else matrice[i, 8] = noir;
                            casebleue.Dequeue();
                        }
                    }
                
            }

            
            File.WriteAllBytes("qr.bmp", effect.RotationDegréAvecBord(td.From_Mat_Color_To_Image(image, matrice), 270));
            Process.Start(new ProcessStartInfo("qr.bmp") { UseShellExecute = true });

            Couleur[,] masque = Masque();
            for (int i = 9; i < taille - 7; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (masque[i, j] == matrice[i, j]) matrice[i, j] = blanc;
                    else matrice[i, j] = noir;

                    if (masque[j, i] == matrice[j, i]) matrice[j, i] = blanc;
                    else matrice[j, i] = noir;

                    if (masque[i + 8, j + 9] == matrice[i + 8, j + 9]) matrice[i + 8, j + 9] = blanc;
                    else matrice[i + 8, j + 9] = noir;
                }
            }

            for (int i = 7; i < taille - 7; i++)
            {
                for (int j = 7; j < taille - 7; j++)
                {
                    if (i != 16 || j != 16)
                    {
                        if (i == 7 || i == 8)
                        {
                            switch (j)
                            {
                                case 7:
                                    break;
                                case 8:
                                    break;
                                default:
                                    if (masque[i, j] == matrice[i, j]) matrice[i, j] = blanc;
                                    else matrice[i, j] = noir;
                                    break;
                            }
                        }
                    }
                }
            }
            for (int i = 9; i < 16; i++)
                for (int j = 17; j <= taille; j++)
                    if (masque[i, j] == matrice[i, j]) matrice[i, j] = blanc;
                    else matrice[i, j] = noir;
            for (int i = 16; i <= taille; i++)
                for (int j = 19; j <= taille; j++)
                    if (masque[i, j] == matrice[i, j]) matrice[i, j] = blanc;
                    else matrice[i, j] = noir;
            for (int i = 21; i <= taille; i++)
                for (int j = 15; j < 19; j++)
                    if (masque[i, j] == matrice[i, j]) matrice[i, j] = blanc;
                    else matrice[i, j] = noir;
            for (int i = 17; i < 21; i++)
                if (masque[i, 15] == matrice[i, 15]) matrice[i, 15] = blanc;
                else matrice[i, 15] = noir;



            File.WriteAllBytes("qr_mask.bmp", effect.RotationDegréAvecBord(td.From_Mat_Color_To_Image(image, matrice), 270));
            Process.Start(new ProcessStartInfo("qr_mask.bmp") { UseShellExecute = true });
            return matrice;
        }

        public Couleur[,] Masque()
        {
            int taille = 0;
            if (version == 1) taille = 24;
            else if (version == 2) taille = 28;
            Couleur[,] masque = new Couleur[taille, taille];
            Couleur blanc = new Couleur(255, 255, 255);
            Couleur noir = new Couleur(0, 0, 0);
            for (int i = 0; i < taille; i++)
                for (int j = 0; j < taille; j++)
                    masque[i, j] = blanc;
            for (int i = 0; i < taille-3; i++)
                for (int j = 0; j < taille-3; j++)
                {
                    if ((i + j) % 2 != 0) masque[i, j] = blanc;
                    else masque[i, j] = noir;
                }
            return masque;
        }
    }
}

    



