using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;


namespace RestauranteApp.Conexion
{
    public partial class CONEXION_MANUAL : Form
    {

        private Librerias.AES aes = new Librerias.AES();
        int idTabla; 
        public CONEXION_MANUAL()
        {
            InitializeComponent();
        }

        public void SavetoXML(Object dbcnString)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("ConnectionString.xml");
            XmlElement root = doc.DocumentElement;
            root.Attributes[0].Value = Convert.ToString(dbcnString);
            XmlTextWriter writer = new XmlTextWriter("ConnectionString.xml", null);
            writer.Formatting = Formatting.Indented;
            doc.Save(writer);
            writer.Close();
        }

        string dbcnString;
        public void ReadfromXML()
        {

            try
            {

                XmlDocument doc = new XmlDocument();
                doc.Load("ConnectionString.xml");
                XmlElement root = doc.DocumentElement;
                dbcnString = root.Attributes[0].Value;
                txtCnString.Text = (aes.Decrypt(dbcnString, Librerias.Desencryptacion.appPwdUnique, int.Parse("256")));
            }
            catch (System.Security.Cryptography.CryptographicException ex)
            {

            }
        }


        
        private void CONEXION_MANUAL_Load(object sender, EventArgs e)
        {
            ReadfromXML(); 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            comprobar_conexion();
        }

        private void comprobar_conexion()
        {
            SqlConnection con = new SqlConnection();
            try
            {
              
                con.ConnectionString = txtCnString.Text;
                SqlCommand com = new SqlCommand("Select * from SALON", con);

                con.Open();
                idTabla = Convert.ToInt32(com.ExecuteScalar());
                con.Close();
                SavetoXML(aes.Encrypt(txtCnString.Text, Librerias.Desencryptacion.appPwdUnique, int.Parse("256")));
                MessageBox.Show("Conexion realizada correctamente", "CONEXION", MessageBoxButtons.OK, MessageBoxIcon.Information  );
                Application.Exit(); 
            }
            catch (Exception ex) {
                MessageBox.Show("Sin conexion", "CONEXION FALLIDA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close(); 
            }
        }
    }
}
