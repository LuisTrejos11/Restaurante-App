using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO; 

namespace RestauranteApp.Modulos.PRODUCTOS
{
    public partial class Productos_rest : Form
    {
        public static int id_grupo; 
        public Productos_rest()
        {
            InitializeComponent();
        }

        
        private void Productos_rest_Load(object sender, EventArgs e)
        {
            DibujarGrupos();
        }

        private void DibujarGrupos()
        {
            try {

                Panel_Grupos.Controls.Clear();

                Conexion.CONEXIONMAESTRA.abrir();
                SqlCommand cmd = new SqlCommand("select * from Grupo_de_Productos", Conexion.CONEXIONMAESTRA.conectar);
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read()) {

                    Label b = new Label();
                    Panel p1 = new Panel();
                    Panel p2 = new Panel();
                    PictureBox I1 = new PictureBox();

                    b.Text = rdr["Grupo"].ToString();
                    b.Name = rdr["IdLine"].ToString();
                    b.Size = new System.Drawing.Size(119, 25);
                    b.Font = new System.Drawing.Font("Microsoft Sans Serif", 13);
                    b.BackColor = Color.Transparent;
                    b.ForeColor = Color.White;
                    b.Dock = DockStyle.Fill;
                    b.TextAlign = ContentAlignment.MiddleCenter;
                    b.Cursor = Cursors.Hand;

                    p1.Size = new System.Drawing.Size(140, 133);
                    p1.BorderStyle = BorderStyle.FixedSingle;
                    p1.Dock = DockStyle.Bottom;
                    p1.BackColor = Color.FromArgb(43,43,43);
                    p1.Name = rdr["IdLine"].ToString();


                    p2.Size = new System.Drawing.Size(140, 25);
                    p2.BorderStyle = BorderStyle.None;
                    p2.Dock = DockStyle.Top;
                    p2.BackColor = Color.Transparent;

                    I1.Size = new System.Drawing.Size(140, 76);
                    I1.Dock = DockStyle.Top;
                    I1.BackgroundImage = null;
                    byte[] bi = (Byte[])rdr["Icono"];
                    MemoryStream ms = new MemoryStream(bi);
                    I1.Image = Image.FromStream(ms);
                    I1.SizeMode = PictureBoxSizeMode.Zoom;
                    I1.Cursor = Cursors.Hand;
                    I1.Tag = rdr["IdLine"].ToString();


                    MenuStrip Menustrip = new MenuStrip();
                    Menustrip.BackColor = Color.Transparent;
                    Menustrip.AutoSize = false;
                    Menustrip.Size = new System.Drawing.Size(28, 24);
                    Menustrip.Dock = DockStyle.Right; 
                    Menustrip.Name = rdr["IdLine"].ToString();
                    ToolStripMenuItem toolStripPRINCIPAL = new ToolStripMenuItem();
                    ToolStripMenuItem toolStripEDITAR = new ToolStripMenuItem();
                    ToolStripMenuItem toolStripELIMINAR= new ToolStripMenuItem();
                    ToolStripMenuItem toolStripRESTAURAR = new ToolStripMenuItem();

                    toolStripPRINCIPAL.Image = Properties.Resources.menuCajas_claro;
                    toolStripPRINCIPAL.BackColor = Color.Transparent;

                    toolStripEDITAR.Text = "Editar";
                    toolStripEDITAR.Name = rdr["Grupo"].ToString();
                    toolStripEDITAR.Tag = rdr["IdLine"].ToString();

                    toolStripELIMINAR.Text = "Eliminar";
                    toolStripELIMINAR.Tag = rdr["IdLine"].ToString();

                    toolStripRESTAURAR.Text = "Restaurar";
                    toolStripRESTAURAR.Tag = rdr["IdLine"].ToString();

                    Menustrip.Items.Add(toolStripPRINCIPAL);
                    toolStripPRINCIPAL.DropDownItems.Add(toolStripEDITAR);
                    toolStripPRINCIPAL.DropDownItems.Add(toolStripELIMINAR);
                    toolStripPRINCIPAL.DropDownItems.Add(toolStripRESTAURAR);


                    p2.Controls.Add(Menustrip);


                    p1.Controls.Add(b);

                    if (rdr["Estado_de_icono"].ToString() != "VACIO")
                    {

                        p1.Controls.Add(I1);
                    }
                   

                    p1.Controls.Add(p2);

                    b.BringToFront();
                    p2.SendToBack();
                    Panel_Grupos.Controls.Add(p1);
                    b.Click += new EventHandler(miEventoLabel);
                    I1.Click += new EventHandler(miEventoImagen);


                }
                Conexion.CONEXIONMAESTRA.cerrar();
            }
            catch (Exception ex) {
                Conexion.CONEXIONMAESTRA.cerrar();
                MessageBox.Show(ex.StackTrace);
            
            }
        
        }


        private void miEventoLabel(System.Object sender, EventArgs e) {

            id_grupo =Convert.ToInt32(((Label)sender).Name);
            ver_productos_por_grupo();
            Seleccionar_Deseleccionar_grupos(); 

        }
        private void ver_productos_por_grupo() {

            panelBienvenida.Visible = false;
            panel3.Visible = true;
            panel3.Dock = DockStyle.Fill;
            DibujarProductos(); 
        }

        private void Seleccionar_Deseleccionar_grupos() {

            // sin seleccionar
            foreach (Panel panelP1 in Panel_Grupos.Controls) {

                if (panelP1 is Panel) {

                    foreach (Label PanLateral2 in panelP1.Controls) {

                        if (PanLateral2 is Label) {

                            panelP1.BackColor = Color.FromArgb(43,43,43);
                            break; 
                        }
                    }

                }
            }

            // selccionado
            foreach (Panel panelP2 in Panel_Grupos.Controls)
            {

                if (panelP2 is Panel)
                {

                    if (panelP2.Name == Convert.ToString(id_grupo)) {

                        panelP2.BackColor = Color.Black; 
                    }

                }
            }


        }

        private void miEventoImagen(System.Object sender, EventArgs e)
        {

            id_grupo = Convert.ToInt32(((PictureBox)sender).Tag);
            ver_productos_por_grupo();
            Seleccionar_Deseleccionar_grupos();


        }

        private void DibujarProductos()
        {
            try
            {
                panelProductos.Controls.Clear();
                Conexion.CONEXIONMAESTRA.abrir();
                SqlCommand cmd = new SqlCommand("mostrar_Productos_por_grupo", Conexion.CONEXIONMAESTRA.conectar);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_grupo", id_grupo);
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read()) {

                    Label b = new Label();
                    Panel p1 = new Panel();
                    Panel p2 = new Panel();
                    PictureBox I1 = new PictureBox();


                    b.Text = rdr["Descripcion"].ToString();
                    b.Name = rdr["Id_Producto1"].ToString();
                    b.Size = new System.Drawing.Size(119, 25);
                    b.Font = new System.Drawing.Font("Microsoft Sans Serif", 13);
                    b.BackColor = Color.Transparent;
                    b.ForeColor = Color.White;
                    b.Dock = DockStyle.Fill;
                    b.TextAlign = ContentAlignment.MiddleCenter;
                    b.Cursor = Cursors.Hand;


                    p1.Size = new System.Drawing.Size(140, 133);
                    p1.BorderStyle = BorderStyle.FixedSingle;
                    p1.Dock = DockStyle.Bottom;
                    p1.BackColor = Color.FromArgb(43, 43, 43);

                    p2.Size = new System.Drawing.Size(140, 25);
                    p2.BorderStyle = BorderStyle.None;
                    p2.Dock = DockStyle.Top;
                    p2.BackColor = Color.Transparent;


                    I1.Size = new System.Drawing.Size(140, 76);
                    I1.Dock = DockStyle.Top;
                    I1.BackgroundImage = null;
                    byte[] bi = (Byte[])rdr["Imagen"];
                    MemoryStream ms = new MemoryStream(bi);
                    I1.Image = Image.FromStream(ms);
                    I1.SizeMode = PictureBoxSizeMode.Zoom;
                    I1.Cursor = Cursors.Hand;
                    I1.Tag = rdr["Id_Producto1"].ToString();


                    MenuStrip Menustrip = new MenuStrip();
                    Menustrip.BackColor = Color.Transparent;
                    Menustrip.AutoSize = false;
                    Menustrip.Size = new System.Drawing.Size(28, 24);
                    Menustrip.Dock = DockStyle.Right;
                    Menustrip.Name = rdr["Id_Producto1"].ToString();
                    ToolStripMenuItem toolStripPRINCIPAL = new ToolStripMenuItem();
                    ToolStripMenuItem toolStripEDITAR = new ToolStripMenuItem();
                    ToolStripMenuItem toolStripELIMINAR = new ToolStripMenuItem();
                    ToolStripMenuItem toolStripRESTAURAR = new ToolStripMenuItem();


                    toolStripPRINCIPAL.Image = Properties.Resources.menuCajas_claro;
                    toolStripPRINCIPAL.BackColor = Color.Transparent;

                    toolStripEDITAR.Text = "Editar";
                    toolStripEDITAR.Name = rdr["Descripcion"].ToString();
                    toolStripEDITAR.Tag = rdr["Id_Producto1"].ToString();

                    toolStripELIMINAR.Text = "Eliminar";
                    toolStripELIMINAR.Tag = rdr["Id_Producto1"].ToString();

                    toolStripRESTAURAR.Text = "Restaurar";
                    toolStripRESTAURAR.Tag = rdr["Id_Producto1"].ToString();

                    Menustrip.Items.Add(toolStripPRINCIPAL);
                    toolStripPRINCIPAL.DropDownItems.Add(toolStripEDITAR);
                    toolStripPRINCIPAL.DropDownItems.Add(toolStripELIMINAR);
                    toolStripPRINCIPAL.DropDownItems.Add(toolStripRESTAURAR);

                    p2.Controls.Add(Menustrip);


                    p1.Controls.Add(b);

                    if (rdr["Estado_imagen"].ToString() != "VACIO")
                    {

                        p1.Controls.Add(I1);
                    }
                    

                       
                    
                    p1.Controls.Add(p2);

                    b.BringToFront();
                    p2.SendToBack();
                    panelProductos.Controls.Add(p1);


                }

                Conexion.CONEXIONMAESTRA.cerrar(); 

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);


            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Modulos.PRODUCTOS.Grupos__de_productos frm = new Modulos.PRODUCTOS.Grupos__de_productos();
            frm.FormClosed += new FormClosedEventHandler(frmGrupos_formClosed);
            frm.ShowDialog(); 
        }

        private void frmGrupos_formClosed(Object sender, FormClosedEventArgs e) {

            DibujarGrupos(); 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Modulos.PRODUCTOS.Registro_de_productos frm = new Modulos.PRODUCTOS.Registro_de_productos();
            frm.FormClosed += new FormClosedEventHandler(frmRegistro_formClosed); 
            frm.ShowDialog(); 
        }

        public void frmRegistro_formClosed(Object sender, FormClosedEventArgs e) {

            DibujarProductos(); 
        }
    }


  
}
