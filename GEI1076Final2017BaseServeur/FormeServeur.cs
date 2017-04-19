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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GEI1076Final2017BaseServeur
{
    public partial class FormeServeur : Form
    {
        private const int taille = 1024;

        private bool enFonction = false;
        private Thread thread = null;
        private TcpListener ecouteTCP = null;
        private byte[] tamponServeurReception = new byte[taille];
        public FormeServeur()
        {
            InitializeComponent();
        }

        private void DemarrerEcoute()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress adresseLocale = null;
            bool trouve = false;
            for (int i = 0; !trouve && (i < ipHostInfo.AddressList.Length); ++i)
            {
                if (ipHostInfo.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                {
                    adresseLocale = ipHostInfo.AddressList[i];
                    trouve = true;
                }
            }

            if (!trouve) return;

            ecouteTCP = new TcpListener(adresseLocale, 666);

            ecouteTCP.Start();

            thread = new Thread(new ThreadStart(GereEcoute));
            enFonction = true;
            thread.Start();
        }

        private void ArreterEcoute()
        {
            if (!enFonction) return;

            enFonction = false;
            while (thread.IsAlive) Thread.Sleep(50);
            ecouteTCP = null;
        }

        // Méthode à compléter
        private void GereEcoute()
        {
            while (enFonction)
            {
                if (ecouteTCP.Pending())
                {

                    TcpClient clientTcp = ecouteTCP.AcceptTcpClient();
                    NetworkStream stream = clientTcp.GetStream();


                    stream.Read(tamponServeurReception, 0, taille);

                    // Format imposé
                    string [] coords = ASCIIEncoding.ASCII.GetString(tamponServeurReception).Split(':');


                    int x = int.Parse(coords[0]);
                    int y = int.Parse(coords[1]);
                    int numero = 0;

                    // Utilisez cette méthode pour accéder les contrôles du Thread principal de manière sécuritaire
                    this.Invoke(new Action(delegate()
                    {
                        BoiteImage bi = BoiteImage.CreerBoiteImage(this, x, y);
                        numero = bi.Numero = (new Random()).Next(1, 100);
                        bi.Refresh();
                    }));

                    byte[] reponse = BitConverter.GetBytes(numero);

                    stream.Write(reponse, 0, 1);

                    clientTcp.Close();
                }
                else
                    Thread.Sleep(20);
            }
            ecouteTCP.Stop();
        }

        private void FormeServeur_FormClosing(object sender, FormClosingEventArgs e)
        {
            ArreterEcoute();

            if (thread != null && thread.IsAlive)
                thread.Abort();
        }

        private void FormeServeur_Load(object sender, EventArgs e)
        {
            DemarrerEcoute();
        }
    }
}
