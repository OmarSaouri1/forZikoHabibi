using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace TpCalculette
{
    public partial class frmCalculette : Form
    {
        private bool createButtonClicked = false;
        private bool deleteButtonClicked = false;
        private dbAccess dbAccess = new dbAccess();

        public frmCalculette()
        {
            InitializeComponent();
            this.Load += frmCalculette_Load;
            this.FormClosing += frmCalculette_FormClosing;
            
    }

        private void frmCalculette_Load(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;

            comboBox1.Enabled = true;
            textBox1.Enabled = false;
            textBox2.Enabled = false;

            button1.Click += button1_Click;
            button5.Click += button5_Click;
            button4.Click += button4_Click;
            button2.Click += button2_Click;
            button3.Click += button3_Click;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            comboBox1.KeyPress += comboBox1_KeyPress;
        }
        private void frmCalculette_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to close the window?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                e.Cancel = true; // Cancel the closing event
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;

            textBox1.Enabled = true;
            textBox2.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;

            createButtonClicked = true;
            comboBox1.DropDownStyle = ComboBoxStyle.Simple;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (deleteButtonClicked)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this book?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if (dbAccess.DeleteBookFromDatabase(comboBox1.Text))
                    {
                        MessageBox.Show("Book deleted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete book from the database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    EnableInitialState();
                }
            }
            else if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrEmpty(textBox2.Text))
            {
                if (createButtonClicked)
                {
                    if (dbAccess.InsertBookIntoDatabase(comboBox1.Text, textBox1.Text, textBox2.Text))
                    {
                        MessageBox.Show("Book added successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Failed to add book to the database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    if (dbAccess.EditBookInDatabase(comboBox1.Text, textBox1.Text, textBox2.Text))
                    {
                        MessageBox.Show("Book edited successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Failed to edit book in the database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                EnableInspectState();
            }
            else
            {
                MessageBox.Show("Please fill in all inputs.", "Incomplete Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            deleteButtonClicked = false;
            createButtonClicked = false;
        }

        private void EnableInitialState()
        {
            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;

            comboBox1.Enabled = true;

            textBox1.Enabled = false;
            textBox2.Enabled = false;

            comboBox1.DropDownStyle = ComboBoxStyle.DropDown;

            ClearInputs();
            comboBox1.Text = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ClearInputs();
            EnableInspectState();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;

            textBox1.Enabled = true;
            textBox2.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;

            comboBox1.DropDownStyle = ComboBoxStyle.Simple;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            deleteButtonClicked = true;

            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = true;
            button5.Enabled = true;

            comboBox1.Enabled = false;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button2.Enabled = (comboBox1.SelectedItem != null);
            button3.Enabled = (comboBox1.SelectedItem != null);

            if (comboBox1.SelectedItem != null)
            {
                dbAccess.FetchBookInformation(textBox1, textBox2, comboBox1.Text);
            }
            else
            {
                ClearInputs();
            }
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetterOrDigit(e.KeyChar))
            {
                dbAccess.LoadBookSuggestions(comboBox1, comboBox1.Text + e.KeyChar);
            }
        }

        private void EnableInspectState()
        {
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = false;
            button5.Enabled = false;

            comboBox1.Enabled = true;

            textBox1.Enabled = false;
            textBox2.Enabled = false;

            comboBox1.DropDownStyle = ComboBoxStyle.DropDown;
        }
        private void ClearInputs()
        {
            textBox1.Text = "";
            textBox2.Text = "";
        }
    }
}
