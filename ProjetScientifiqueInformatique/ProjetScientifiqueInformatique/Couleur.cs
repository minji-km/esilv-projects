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
    class Couleur
    {
        byte r;
        byte g;
        byte b;


        public Couleur(byte r, byte g, byte b)
        {
            this.r = r;
            this.g = g;
            this.b = b;

        }

        public byte R
        {
            get { return this.r; }
            set { if ((value >= 0) && (value <= 255)) this.r = value; }
        }

        public byte G
        {
            get { return this.g; }
            set { if ((value >= 0) && (value <= 255)) this.g = value; }
        }

        public byte B
        {
            get { return this.b; }
            set { if ((value >= 0) && (value <= 255)) this.b = value; }
        }


    }
}
