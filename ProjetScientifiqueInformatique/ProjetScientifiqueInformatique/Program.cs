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
    class Program
    {
        static public int SaisieNombre()
        {
            int result;
            while (!int.TryParse(Console.ReadLine(), out result))
            { }
            return result;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Souhaitez-vous générer un QR code ou faire du traitement d'images ?\n" 
                + "1 : traitement d'images\n" 
                + "2 : lecteur QR code");
            int choix = SaisieNombre();

            switch (choix)
            {
                case 2:
                    Console.WriteLine("Entrez un message pour le QR code");
                    string message = Console.ReadLine();
                    Settings QRcode = new Settings(message);
                    int[] alphanum = QRcode.AlphaNum(QRcode.ValeurNum());
                    int[] correction = QRcode.Correction();
                    foreach (int i in alphanum) Console.Write(i);
                    foreach (int i in correction) Console.Write(i);
                    Console.WriteLine();
                    QRcode.Remplissage();
                    break;
                case 1:
                    ConsoleKeyInfo cki;
                    string fichier = "";
                    do
                    {
                        Console.Clear();
                        Console.WriteLine("Menu :\n"
                                         + "1 : Une pomme\n"
                                         + "2 : Photo en noir et blanc d'une femme\n"
                                         + "3 : Perroquet\n"
                                         + "Sélectionnez l'image désirée ou\n"
                                         + "4 : Fractale");
                        int numero = SaisieNombre();
                        MyImage td = new MyImage();
                        switch (numero)
                        {
                            case 1:
                                fichier = "pomme.bmp";
                                break;
                            case 2:
                                fichier = "lena.bmp";
                                break;
                            case 3:
                                fichier = "coco.bmp";
                                break;
                            case 4:
                                fichier = "im.bmp";
                                byte[] image = File.ReadAllBytes(fichier);
                                Création_d_images fract = new Création_d_images(td);
                                byte[] fractale = fract.Mandelbrot(image);
                                File.WriteAllBytes("fractale.bmp", fractale);
                                Process.Start(new ProcessStartInfo("fractale.bmp") { UseShellExecute = true });
                                break;
                            default:
                                Console.WriteLine("Cette option n'est pas possible");
                                break;
                        }
                        Console.Clear();
                        if (numero != 4)
                        {
                            Console.WriteLine("Menu :\n"
                                             + "4 : Appliquer le filtre négatif\n"
                                             + "5 : Duppliquer l'image\n"
                                             + "6 : Applique l'effet mirroir\n"
                                             + "7 : Rotation d'image selon un angle choisi\n"
                                             + "8 : Redimensionne l'image selon un coefficient d'agrandissement choisi\n"
                                             + "9 : Image en noir et blanc\n"
                                             + "10 : Renforce les bords de l'image\n"
                                             + "11 : Détection de contours de l'image\n"
                                             + "12 : Repoussage de l'image\n"
                                             + "13 : Histogramme\n"
                                             + "Sélectionnez l'option désirée ");
                            int num = SaisieNombre();


                            Filtre filter = new Filtre(td);
                            Effets effect = new Effets(td);
                            byte[] image = File.ReadAllBytes(fichier);

                            switch (num)
                            {
                                case 1:
                                    byte[] rot90D = effect.Rotation90Droite(image);
                                    File.WriteAllBytes("rot90D.bmp", rot90D);
                                    Process.Start(new ProcessStartInfo("rot90D.bmp") { UseShellExecute = true });
                                    break;
                                case 2:
                                    byte[] rot90G = effect.Rotation90Gauche(image);
                                    File.WriteAllBytes("rot90G.bmp", rot90G);
                                    Process.Start(new ProcessStartInfo("rot90G.bmp") { UseShellExecute = true });
                                    break;
                                case 3:
                                    byte[] rot180 = effect.Rotation180(image);
                                    File.WriteAllBytes("rot180.bmp", rot180);
                                    Process.Start(new ProcessStartInfo("rot180.bmp") { UseShellExecute = true });
                                    break;
                                case 4:
                                    byte[] negatif = filter.Negatif(image, td.MatriceRGB(image));
                                    File.WriteAllBytes("negatif.bmp", negatif);
                                    Process.Start(new ProcessStartInfo("negatif.bmp") { UseShellExecute = true });
                                    break;
                                case 5:
                                    effect.Dupliquer(image);
                                    break;
                                case 6:
                                    byte[] miroir = effect.Mirror(image, td.MatriceRGB(image));
                                    File.WriteAllBytes("miroir.bmp", miroir);
                                    Process.Start(new ProcessStartInfo("miroir.bmp") { UseShellExecute = true });
                                    break;
                                case 7:
                                    Console.WriteLine("Entrez le degré de rotation");
                                    int degre = Convert.ToInt32(Console.ReadLine());
                                    byte[] rot = effect.RotationDegréAvecBord(image, degre);
                                    File.WriteAllBytes("rot.bmp", rot);
                                    Process.Start(new ProcessStartInfo("rot.bmp") { UseShellExecute = true });
                                    break;
                                case 8:
                                    Console.WriteLine("Entrez un coefficient d'agrandissement entier");
                                    int agrandissement = Convert.ToInt32(Console.ReadLine());
                                    byte[] resize = effect.Resize(image, agrandissement);
                                    File.WriteAllBytes("resize.bmp", resize);
                                    Process.Start(new ProcessStartInfo("resize.bmp") { UseShellExecute = true });
                                    break;
                                case 9:
                                    byte[] grey = filter.BlackGreyWhite(image, td.MatriceRGB(image));
                                    File.WriteAllBytes("grey.bmp", grey);
                                    Process.Start(new ProcessStartInfo("grey.bmp") { UseShellExecute = true });
                                    break;
                                case 10:
                                    byte[] bord = filter.RenforcementDesBords(image, td.MatriceRGB(image));
                                    File.WriteAllBytes("renforcement_des_bords.bmp", bord);
                                    Process.Start(new ProcessStartInfo("renforcement_des_bords.bmp") { UseShellExecute = true });
                                    break;
                                case 11:
                                    byte[] det_contour = filter.DetectionDeContour(image, td.MatriceRGB(image));
                                    File.WriteAllBytes("détection_contour.bmp", det_contour);
                                    Process.Start(new ProcessStartInfo("détection_contour.bmp") { UseShellExecute = true });
                                    break;
                                case 12:
                                    byte[] repoussage = filter.Repoussage(image, td.MatriceRGB(image));
                                    File.WriteAllBytes("repoussage.bmp", repoussage);
                                    Process.Start(new ProcessStartInfo("repoussage.bmp") { UseShellExecute = true });
                                    break;
                                case 13:
                                    Création_d_images histo = new Création_d_images(td);
                                    byte[] historouge = histo.Histogramme(image, td.MatriceRGB(image), 1);
                                    byte[] histovert = histo.Histogramme(image, td.MatriceRGB(image), 2);
                                    byte[] histobleu = histo.Histogramme(image, td.MatriceRGB(image), 3);
                                    File.WriteAllBytes("historouge.bmp", historouge);
                                    File.WriteAllBytes("histovert.bmp", histovert);
                                    File.WriteAllBytes("histobleu.bmp", histobleu);
                                    Process.Start(new ProcessStartInfo("historouge.bmp") { UseShellExecute = true });
                                    Process.Start(new ProcessStartInfo("histobleu.bmp") { UseShellExecute = true });
                                    Process.Start(new ProcessStartInfo("histovert.bmp") { UseShellExecute = true });
                                    break;
                                default:
                                    Console.WriteLine("Cette option n'est pas proposée");
                                    break;
                            }
                        }
                        Console.WriteLine("Tapez Entrer pour sortir ou une autre touche pour recommencer");
                        cki = Console.ReadKey();
                    } while (cki.Key != ConsoleKey.Enter);

                    break;
                default:
                    Console.WriteLine("Ce choix n'est pas disponible");
                    break;
            }
            
        }
    }
}
