using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ProjetBDD
{
    class Jsonification
    {
        private string filename;
        private List<Object> liste;

        public Jsonification(string filename)
        {
            this.filename = filename;
            this.liste = new List<object>();
        }
        public void write_object(Object o)
        {
            liste.Add(o);
            if (File.Exists(this.filename))
            {
                string content = File.ReadAllText(this.filename);
            }
            using (StreamWriter w = File.CreateText(this.filename))
            {
                w.WriteLine(JsonConvert.SerializeObject(liste));
            }

        }

        public void xmlification_machiavelique()
        {
            string content = File.ReadAllText(this.filename);
            content = "{ velomax:" + content + "}";
            XNode node = JsonConvert.DeserializeXNode(content, "Root");
            File.WriteAllText("xmlification.xml", node.ToString());
            
        }

    }
}
