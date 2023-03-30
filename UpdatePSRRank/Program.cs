
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Transactions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

using System.Data.SqlClient;
using System.Linq;
using Microsoft.Data.SqlClient;
class Program
{
    static void Main(string[] args)
    {
        string connectionString = "Data Source=KSK;Initial Catalog=KSK;User Id=sa;Password=Only1TimeKSK;" +
                                  "Trusted_Connection=False;TrustServerCertificate=True;" +
                                  " Connection Timeout=600;"; 
        string tableName = "total_category_item";
        string columnName = "Rank";
        int id = 1;
        int newValue = 10;

        try
        {
        
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = $"UPDATE {tableName} SET {columnName} " +
                               $"= @newValue WHERE Id = @id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@newValue", newValue);
                    command.Parameters.AddWithValue("@id", id);

                    int rowsAffected = command.ExecuteNonQuery();

                    Console.WriteLine($"Updated {rowsAffected} rows.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        Console.ReadLine();
    }
    
     static int GetPsrAdvFrequency(decimal input)
        {
            if (input >= -30 && input < -27)
            {
                return -27;
            }
            else if (input >= -27 && input < -24)
            {
                return -24;
            }
            else if (input >= -24 && input < -21)
            {
                return -21;
            }
            else if (input >= -21 && input < -18)
            {
                return -18;
            }
            else if (input >= -18 && input < -15)
            {
                return -15;
            }
            else if (input >= -15 && input < -12)
            {
                return -12;
            }
            else if (input >= -12 && input < -9)
            {
                return -9;
            }
            else if (input >= -9 && input < -6)
            {
                return -6;
            }
            else if (input >= -6 && input < -3)
            {
                return -3;
            }
            else if (input >= -3 && input < 0)
            {
                return -1;
            }
            else if (input >= 0 && input < 3)
            {
                return 1;
            }
            else if (input >= 3 && input < 6)
            {
                return 3;
            }
            else if (input >= 6 && input < 9)
            {
                return 6;
            }
            else if (input >= 9 && input < 12)
            {
                return 9;
            }
            else if (input >= 12 && input < 15)
            {
                return 12;
            }
            else if (input >= 15 && input < 18)
            {
                return 15;
            }
            else if (input >= 18 && input < 21)
            {
                return 18;
            }
            else if (input >= 21 && input < 24)
            {
                return 21;
            }
            else if (input >= 24 && input < 27)
            {
                return 24;
            }
            else if (input >= 27 && input <= 30)
            {
                return 27;
            }
            else
            {
                return 1;

            }
        }

}