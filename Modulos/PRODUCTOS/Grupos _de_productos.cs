using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient; 

namespace RestauranteApp.Modulos.PRODUCTOS
{
    public partial class Grupos__de_productos : Form
    {
        public Grupos__de_productos()
        {
            InitializeComponent();
        }

        string ESTADO_IMAGEN; 
        private void Grupos__de_productos_Load(object sender, EventArgs e)
        {
            ESTADO_IMAGEN = "VACIO"; 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Insertar_Grupo_de_Productos();
            Dispose(); 
        }


        private void Insertar_Grupo_de_Productos()
        {
            try
            {
                Conexion.CONEXIONMAESTRA.abrir();
                SqlCommand cmd = new SqlCommand("Insertar_Grupo_de_Productos", Conexion.CONEXIONMAESTRA.conectar);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Grupo", grupotxt.Text);
                cmd.Parameters.AddWithValue("@Por_defecto", "NO");
                cmd.Parameters.AddWithValue("@Estado", "ACTIVO");
                cmd.Parameters.AddWithValue("@Estado_de_icono", ESTADO_IMAGEN);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
                cmd.Parameters.AddWithValue("@Icono", ms.GetBuffer());

                cmd.ExecuteNonQuery();
                Conexion.CONEXIONMAESTRA.cerrar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void insertar_imagen() {

            dlg.InitialDirectory = "";
            dlg.Filter = "Imagenes|*.jpg;*.png";
            dlg.FilterIndex = 2;
            dlg.Title = "Cargador de imagenes";

            if (dlg.ShowDialog() == DialogResult.OK) {

                pictureBox1.BackgroundImage = null;
                pictureBox1.Image = new Bitmap(dlg.FileName);

                panel2.Visible = false;
                ESTADO_IMAGEN = "LLENO"; 
            
            }
        
        }

        private void label2_Click(object sender, EventArgs e)
        {
            insertar_imagen(); 
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            insertar_imagen(); 
        }
    }

   
}
