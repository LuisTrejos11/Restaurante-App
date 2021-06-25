using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace RestauranteApp.Modulos.Mesas_Salones
{
    public partial class Configurar__mesas : Form
    {

        int idSalon;
        string estado;
       public static string nombre_mesa;
       public static int idMesa; 
        public Configurar__mesas()
        {
            InitializeComponent();
        }

        private void Configurar__mesas_Load(object sender, EventArgs e)
        {
            PanelBienvenida.Dock = DockStyle.Fill;
            dibujarSalones(); 
        }

        private void dibujarMESAS()
        {
            try
            {
                
                PanelMesas.Controls.Clear();
                Conexion.CONEXIONMAESTRA.abrir();
                SqlCommand cmd = new SqlCommand("mostrar_mesas_por_salon", Conexion.CONEXIONMAESTRA.conectar);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_salon", idSalon);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Button b = new Button();
                    Panel panel = new Panel();
                    int alto = Convert.ToInt32(rdr["y"].ToString());
                    int ancho = Convert.ToInt32(rdr["x"].ToString());
                    int tamanio_letra = Convert.ToInt32(rdr["Tamanio_letra"].ToString());
                    Point tamanio = new Point(ancho, alto);

                    panel.BackgroundImage = Properties.Resources.mesa_vacia;
                    panel.BackgroundImageLayout = ImageLayout.Zoom;
                    panel.Cursor = Cursors.Hand;
                    panel.Tag = rdr["Id_mesa"].ToString();
                    panel.Size = new System.Drawing.Size(tamanio);


                    b.Text = rdr["Mesa"].ToString();
                    b.Name = rdr["Id_mesa"].ToString();

                    if (b.Text != "NULO")
                    {
                        b.Size = new System.Drawing.Size(tamanio);
                        b.BackColor = Color.FromArgb(5, 179, 90);
                        b.Font = new System.Drawing.Font("Microsoft Sans Serif", tamanio_letra);
                        b.FlatStyle = FlatStyle.Flat;
                        b.FlatAppearance.BorderSize = 0;
                        b.ForeColor = Color.White;
                        PanelMesas.Controls.Add(b);
                    }
                    else
                    {
                        PanelMesas.Controls.Add(panel);
                    }


                    b.Click += new EventHandler(miEvento);
                    panel.Click += new EventHandler(miEvento_panel_click); 

                }


                Conexion.CONEXIONMAESTRA.cerrar();

            }
            catch (Exception ex)
            {
                Conexion.CONEXIONMAESTRA.cerrar();
                MessageBox.Show(ex.StackTrace);
            }

        }


        private void miEvento(System.Object sender, EventArgs e) {

            nombre_mesa = ((Button)sender).Text;
            idMesa = Convert.ToInt32(((Button)sender).Name);
            Agregar_mesa_ok frm = new Agregar_mesa_ok();
            frm.FormClosed += new FormClosedEventHandler(frm_Agregar_mesa_ok_FormClosed);
            frm.ShowDialog(); 
        
        }


        private void miEvento_panel_click(System.Object sender, EventArgs e) {

            idMesa = Convert.ToInt32(((Panel)sender).Tag);
            Agregar_mesa_ok frm = new Agregar_mesa_ok();
            frm.FormClosed += new FormClosedEventHandler(frm_Agregar_mesa_ok_FormClosed); 
            frm.ShowDialog();

        }

        private void frm_Agregar_mesa_ok_FormClosed(Object sender, FormClosedEventArgs e) {

            dibujarMESAS(); 
        }
        private void dibujarSalones()
        {
            try
            {
                flowLayoutPanel1.Controls.Clear();

                Conexion.CONEXIONMAESTRA.abrir();
                SqlCommand cmd = new SqlCommand("mostrar_SALON", Conexion.CONEXIONMAESTRA.conectar);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@buscar", textSalon.Text);

                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read()) {

                    Button b = new Button();
                    Panel panelC1 = new Panel();

                    b.Text = rdr["Salon"].ToString();
                    b.Name = rdr["Id_salon"].ToString();
                    b.Dock = DockStyle.Fill;
                    b.BackColor = Color.Transparent;
                    b.Font = new System.Drawing.Font("Microsoft Sans Serif", 12);
                    b.FlatStyle = FlatStyle.Flat;
                    b.FlatAppearance.BorderSize = 0;
                    b.FlatAppearance.MouseDownBackColor = Color.FromArgb(64,64,64);
                    b.FlatAppearance.MouseOverBackColor = Color.FromArgb(43, 43, 43);
                    b.TextAlign = ContentAlignment.MiddleLeft;
                    b.Tag = rdr["Estado"];


                    panelC1.Size = new System.Drawing.Size(290, 58);
                    panelC1.Name = rdr["Id_salon"].ToString();

                    string estado;
                    estado = rdr["Estado"].ToString();

                    if (estado == "ELIMINADO")
                    {

                        b.Text = rdr["Salon"].ToString() + "-Eliminado";
                        b.ForeColor = Color.FromArgb(231, 63, 67);

                    }
                    else {

                        b.ForeColor = Color.White; 
                    }

                    panelC1.Controls.Add(b);
                    flowLayoutPanel1.Controls.Add(panelC1);
                    b.Click += new EventHandler(miEvento_salon_click);

                }
                Conexion.CONEXIONMAESTRA.cerrar(); 
            }
            catch (Exception ex) {
                Conexion.CONEXIONMAESTRA.cerrar();
                MessageBox.Show(ex.StackTrace); 
            
            }    
            
        }

        private void miEvento_salon_click(System.Object sender, EventArgs e) {

            PanelBienvenida.Visible = false;
            PanelBienvenida.Dock = DockStyle.None;
            PanelMesas.Visible = true;
            PanelMesas.Dock = DockStyle.Fill;
            idSalon = Convert.ToInt32(((Button)sender).Name);
            estado = Convert.ToString(((Button)sender).Tag);
            dibujarMESAS(); 

            foreach (Panel panelC1 in flowLayoutPanel1.Controls) {

                if (panelC1 is Panel) {

                    foreach (Button boton in panelC1.Controls) {

                        if (boton is Button) {

                            boton.BackColor = Color.Transparent;
                            break; 
                        }
                    
                    }
                
                }
            
            }

            string NOMBRE = Convert.ToString(((Button)sender).Name);

            foreach (Panel panelC1 in flowLayoutPanel1.Controls) {

                if (panelC1 is Panel) {

                    foreach (Button boton in panelC1.Controls) {
                        if (boton is Button) {

                            if (boton.Name == NOMBRE) {

                                boton.BackColor = Color.OrangeRed;
                                break; 
                            }
                        
                        }
                        
                    
                    }
                
                }
            
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Modulos.Mesas_Salones.Salones frm = new Modulos.Mesas_Salones.Salones();
         
            frm.FormClosed += new FormClosedEventHandler(frm_FormClosed);
            frm.ShowDialog();
        }

        void frm_FormClosed(Object sender, FormClosedEventArgs e) {

            dibujarSalones(); 
        }

        public void configurar_mesas_FormClosed(Object sender, FormClosedEventArgs e) { 
        
                    
        }

        private void button4_Click(object sender, EventArgs e)
        {
            aumentar_tamaño_mesa(); 
        }

        internal void aumentar_tamaño_mesa()
        {
            try
            {
                 SqlCommand cmd = new SqlCommand();
                Conexion.CONEXIONMAESTRA.abrir();
                cmd = new SqlCommand("aumentar_tamanio_mesa", Conexion.CONEXIONMAESTRA.conectar);
                cmd.ExecuteNonQuery();
                Conexion.CONEXIONMAESTRA.cerrar();
                dibujarMESAS(); 

            }
            catch (Exception ex) {

                MessageBox.Show(ex.StackTrace); 
            
            }
        
        }

        internal void disminuir_tamaño_mesa() {

            try
            {
                SqlCommand cmd = new SqlCommand();
                Conexion.CONEXIONMAESTRA.abrir();
                cmd = new SqlCommand("disminuir_tamanio_mesa", Conexion.CONEXIONMAESTRA.conectar);
                cmd.ExecuteNonQuery();
                Conexion.CONEXIONMAESTRA.cerrar();
                dibujarMESAS();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            disminuir_tamaño_mesa(); 
        }


        internal void aumentar_tamaño_letra() {

            try
            {
                SqlCommand cmd = new SqlCommand();
                Conexion.CONEXIONMAESTRA.abrir();
                cmd = new SqlCommand("aumentar_tamanio_letra", Conexion.CONEXIONMAESTRA.conectar);
                cmd.ExecuteNonQuery();
                Conexion.CONEXIONMAESTRA.cerrar();
                dibujarMESAS();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.StackTrace);

            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            aumentar_tamaño_letra(); 
        }

        internal void disminuir_tamaño_letra() {

            try
            {
                SqlCommand cmd = new SqlCommand();
                Conexion.CONEXIONMAESTRA.abrir();
                cmd = new SqlCommand("disminuir_tamanio_letra", Conexion.CONEXIONMAESTRA.conectar);
                cmd.ExecuteNonQuery();
                Conexion.CONEXIONMAESTRA.cerrar();
                dibujarMESAS();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            disminuir_tamaño_letra(); 
        }
    }
}
