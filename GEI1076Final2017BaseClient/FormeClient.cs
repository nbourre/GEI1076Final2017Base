using GEI1076Final2017BaseDLL.Controles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GEI1076Final2017BaseClient
{
    public partial class FormeClient : Form
    {
        private IPAddress adresseIP;
        private byte[] reponse = new byte[1];
        Socket socketEnvoi;

        public FormeClient()
        {
            InitializeComponent();
        }

        // Méthode à compléter
        private void FormeClient_MouseClick(object sender, MouseEventArgs e)
        {
            // Ce format est imposé
            string coordonnees = e.X + ":" + e.Y + ":";
            byte[] tamponClient = ASCIIEncoding.ASCII.GetBytes(coordonnees);

            BoiteImage bi = BoiteImage.CreerBoiteImage(this, e.X, e.Y);

            adresseIP = IPAddress.Parse("172.16.200.195");
            // À compléter, le port de TCP sera toujours 666
            IPEndPoint IPDestination = new IPEndPoint(adresseIP, 666);

            socketEnvoi = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Utilisez BitConvert pour convertir de  [] byte à entier 
            try
            {
                socketEnvoi.Connect(IPDestination);
                socketEnvoi.Send(Encoding.ASCII.GetBytes(coordonnees));
                socketEnvoi.Receive(reponse);

                bi.Numero = reponse[0];
                
                socketEnvoi.Dispose();

            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur");
                bi.Dispose();
            }
        }
    }
}
