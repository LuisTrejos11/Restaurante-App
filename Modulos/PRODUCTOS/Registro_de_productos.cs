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
    public partial class Registro_de_productos : Form
    {
        public Registro_de_productos()
        {
            InitializeComponent();
        }

        String ESTADO_IMAGEN; 
        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void Registro_de_productos_Load(object sender, EventArgs e)
        {
            ESTADO_IMAGEN = "VACIO"; 
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (txtdescripcion.Text != "") {

                if (txtprecioventa.Text != "") {

                    Insertar_Producto1();
                }
            
            }
           
        }


        private void Insertar_Producto1()
        {
            try
            {
                Conexion.CONEXIONMAESTRA.abrir();
                SqlCommand cmd = new SqlCommand("Insertar_Producto1", Conexion.CONEXIONMAESTRA.conectar);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Descripcion", txtdescripcion.Text);
                cmd.Parameters.AddWithValue("@Id_grupo", PRODUCTOS.Productos_rest.id_grupo);
                cmd.Parameters.AddWithValue("@Precio_de_venta", txtprecioventa.Text);
                cmd.Parameters.AddWithValue("@Estado_imagen", ESTADO_IMAGEN);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                ImagenProducto.Image.Save(ms, ImagenProducto.Image.RawFormat);
                cmd.Parameters.AddWithValue("@Imagen", ms.GetBuffer());

                cmd.ExecuteNonQuery();
                Conexion.CONEXIONMAESTRA.cerrar();
                Dispose(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void insertar_imagen()
        {

            dlg.InitialDirectory = "";
            dlg.Filter = "Imagenes|*.jpg;*.png";
            dlg.FilterIndex = 2;
            dlg.Title = "Cargador de imagenes";

            if (dlg.ShowDialog() == DialogResult.OK)
            {

                ImagenProducto.BackgroundImage = null;
                ImagenProducto.Image = new Bitmap(dlg.FileName);

                panel3.Visible = false;
                ESTADO_IMAGEN = "LLENO";

            }

        }

        private void label4_Click(object sender, EventArgs e)
        {
            insertar_imagen(); 
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            insertar_imagen(); 
        }
    }
}
