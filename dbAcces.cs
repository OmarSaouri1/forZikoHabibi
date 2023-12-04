using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace TpCalculette
{
    public class dbAccess
    {
        private string connstr = "server=localhost;uid=root;database=library";

        public bool DeleteBookFromDatabase(string bookName)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connstr))
                {
                    con.Open();

                    string sql = "DELETE FROM books WHERE bookname = @bookName";
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@bookName", bookName);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool InsertBookIntoDatabase(string bookName, string bookPrice, string bookYear)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connstr))
                {
                    con.Open();

                    string sql = "INSERT INTO books (bookname, bookprice, bookyear) VALUES (@bookName, @bookPrice, @bookYear)";
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@bookName", bookName);
                    cmd.Parameters.AddWithValue("@bookPrice", bookPrice);
                    cmd.Parameters.AddWithValue("@bookYear", bookYear);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool EditBookInDatabase(string bookName, string bookPrice, string bookYear)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connstr))
                {
                    con.Open();

                    string sql = "UPDATE books SET bookprice = @bookPrice, bookyear = @bookYear WHERE bookname = @bookName";
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@bookName", bookName);
                    cmd.Parameters.AddWithValue("@bookPrice", bookPrice);
                    cmd.Parameters.AddWithValue("@bookYear", bookYear);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public void LoadBookSuggestions(ComboBox comboBox, string partialBookName)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connstr))
                {
                    con.Open();

                    string sql = "SELECT bookname FROM books WHERE bookname LIKE @partialBookName";
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@partialBookName", partialBookName + "%");

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        comboBox.Items.Clear();

                        while (reader.Read())
                        {
                            comboBox.Items.Add(reader["bookname"].ToString());
                        }
                    }
                }

                comboBox.Select(comboBox.Text.Length, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void FetchBookInformation(TextBox textBox1, TextBox textBox2, string selectedBook)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connstr))
                {
                    con.Open();

                    string sql = "SELECT bookprice, bookyear FROM books WHERE bookname = @selectedBook";
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@selectedBook", selectedBook);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            textBox1.Text = reader["bookprice"].ToString();
                            textBox2.Text = reader["bookyear"].ToString();
                        }
                        else
                        {
                            textBox1.Clear();
                            textBox2.Clear();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
