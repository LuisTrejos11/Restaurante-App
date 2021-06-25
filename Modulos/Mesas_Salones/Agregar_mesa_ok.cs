using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient; 

namespace RestauranteApp.Modulos.Mesas_Salones
{
    public partial class Agregar_mesa_ok : Form
    {
        public Agregar_mesa_ok()
        {
            InitializeComponent();
        }

        private void Agregar_mesa_ok_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            txtMesaEdicion.Text = Configurar__mesas.nombre_mesa; 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtMesaEdicion.Text != "") {

                editar_mesa();
            }
        }

        private void editar_mesa()
        {
            try {
                Conexion.CONEXIONMAESTRA.abrir();
                SqlCommand cmd = new SqlCommand("editar_mesa", Conexion.CONEXIONMAESTRA.conectar);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mesa", txtMesaEdicion.Text);
                cmd.Parameters.AddWithValue("@id_mesa", Configurar__mesas.idMesa);
                cmd.ExecuteNonQuery();
                Conexion.CONEXIONMAESTRA.cerrar();
                Close(); 



            }
            catch (Exception ex) {
                Conexion.CONEXIONMAESTRA.cerrar();
                MessageBox.Show(ex.Message); 
            }
        
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close(); 
        }
    }
}
