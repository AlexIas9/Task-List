using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tema_II_2
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            RefreshTaskList();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Tema_II_2.ServiceReference1.WebService1SoapClient service = new Tema_II_2.ServiceReference1.WebService1SoapClient();
            
            string title = textBox1.Text;
            string description = textBox2.Text;
            DateTime deadline = DateTime.Parse(textBox3.Text);

            service.AddTask(title, description, deadline);
            RefreshTaskList();
            ClearTaskDetails();

        }
        private void ClearTaskDetails()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            checkBox1.Checked = false;
        }


        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                Tema_II_2.ServiceReference1.WebService1SoapClient service = new Tema_II_2.ServiceReference1.WebService1SoapClient();

                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                int taskId = Convert.ToInt32(selectedRow.Cells["Id"].Value);
                string title = Convert.ToString(selectedRow.Cells["Title"].Value);
                string description = Convert.ToString(selectedRow.Cells["Description"].Value);
                DateTime deadline = Convert.ToDateTime(selectedRow.Cells["Deadline"].Value);
                bool isCompleted = Convert.ToBoolean(selectedRow.Cells["IsCompleted"].Value);

                // Verificăm dacă valorile din casetele de text sunt modificate și actualizăm doar acele valori care sunt modificate
                if (!string.IsNullOrEmpty(textBox1.Text))
                    title = textBox1.Text;
                if (!string.IsNullOrEmpty(textBox2.Text))
                    description = textBox2.Text;
                if (!string.IsNullOrEmpty(textBox3.Text))
                    deadline = Convert.ToDateTime(textBox3.Text);
                if (checkBox1.Checked)
                    isCompleted = true;

                service.UpdateTask(taskId, title, description, deadline, isCompleted);
                RefreshTaskList();
                ClearTaskDetails();
            }
            else
            {
                MessageBox.Show("Select a task to update.");
            }
        }


        private void RefreshTaskList()
        {
            Tema_II_2.ServiceReference1.WebService1SoapClient service = new Tema_II_2.ServiceReference1.WebService1SoapClient();
            dataGridView1.DataSource = service.GetTasks();


        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                Tema_II_2.ServiceReference1.WebService1SoapClient service = new Tema_II_2.ServiceReference1.WebService1SoapClient();
                int taskId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id"].Value); // Obținem Id-ul task-ului selectat

                service.DeleteTask(taskId); // Ștergem task-ul folosind Id-ul obținut
                RefreshTaskList(); // Reîmprospătăm lista de task-uri în DataGridView
                ClearTaskDetails(); // Curățăm detaliile task-ului pentru a pregăti pentru următoarea operație
            }
            else
            {
                MessageBox.Show("Select a task to delete.");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
