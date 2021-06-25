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
    public partial class Salones : Form
    {

        int idSalon; 
        public Salones()
        {
            InitializeComponent();
        }

        private void Salones_Load(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.None;
            textSalonEdicion.Focus(); 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            insertar_salon();
        }

        private void insertar_mesas_vacias() {

            for (int i = 1; i <= 80; i++)
            {
                try {

                    Conexion.CONEXIONMAESTRA.abrir();
                    SqlCommand cmd = new SqlCommand("insertar_mesa",Conexion.CONEXIONMAESTRA.conectar );
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mesa", "NULO");
                    cmd.Parameters.AddWithValue("@idsalon", idSalon);
                    cmd.ExecuteNonQuery();
                    Conexion.CONEXIONMAESTRA.cerrar(); 
                }
                catch (Exception ex) {
                    Conexion.CONEXIONMAESTRA.cerrar();
                    MessageBox.Show(ex.StackTrace); 
                }
            }
        
        }

        private void mostrar_id_salon_recien_ingresado() {

            SqlCommand com = new SqlCommand("mostrar_id_salon_recien_ingresado", Conexion.CONEXIONMAESTRA.conectar);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Salon", textSalonEdicion.Text);
            try {

                Conexion.CONEXIONMAESTRA.abrir();
                idSalon = Convert.ToInt32(com.ExecuteScalar());
                Conexion.CONEXIONMAESTRA.cerrar(); 

            }
            catch (Exception ex ) {
                Conexion.CONEXIONMAESTRA.cerrar();
                MessageBox.Show(ex.StackTrace); 
            }
        }
        private void insertar_salon()
        {
            try
            {
                Conexion.CONEXIONMAESTRA.abrir();
                SqlCommand cmd = new SqlCommand("insertar_salon", Conexion.CONEXIONMAESTRA.conectar);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Salon", textSalonEdicion.Text);
                cmd.ExecuteNonQuery();
                Conexion.CONEXIONMAESTRA.conectar.Close();
                mostrar_id_salon_recien_ingresado();
                insertar_mesas_vacias(); 
                Close(); 
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
                Conexion.CONEXIONMAESTRA.cerrar();
               
            }

        }
    }
}
