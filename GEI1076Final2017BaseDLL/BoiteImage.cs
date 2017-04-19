using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// À enlever avec les références
using System.Windows.Forms;
using System.Drawing;

namespace GEI1076Final2017BaseDLL
{
    namespace Controles
    {
        public class BoiteImage : PictureBox
        {
            private static int numeroUnique = 0;

            private static Font police = new Font("Courrier New", 10.0f);

            public int Numero {get; set; }

            private BoiteImage()
            {
                Numero = 0;

                Click += Clique;
                Paint += Dessine;

            }

            // Méthode à compléter
            public static BoiteImage CreerBoiteImage(Form f, int x, int y)
            {
                BoiteImage bi = new BoiteImage();

                bi.Size = new System.Drawing.Size(100, 100);
                bi.TabStop = false;
                bi.BorderStyle = BorderStyle.FixedSingle;
                bi.Name = "BI" + numeroUnique++;
                bi.Location = new Point(x, y);
                

                f.Controls.Add(bi);

                return bi;
            }

            private void Clique(object sender, EventArgs e)
            {
                MessageBox.Show(Numero.ToString(), "Voilà");
            }

            private void Dessine(object sender, PaintEventArgs e)
            {
                e.Graphics.Clear(this.BackColor);
                e.Graphics.DrawString(Numero.ToString(), police, Brushes.Black, 0, 0);
            }
        }
    }
}
