
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
using MathNet.Numerics.Statistics;

using System.Data.SqlClient;
using System.Linq;
using Microsoft.Data.SqlClient;


class HistogramGenerator
{
	static void Main(string[] args)
	{

		string connectionString = "Data Source=KSK;Initial Catalog=KSK;User Id=sa;Password=Only1TimeKSK;" +
		                          "Trusted_Connection=False;TrustServerCertificate=True;" +
		                          " Connection Timeout=600;"; 

		 SqlConnection connection = new SqlConnection(connectionString);
		//
		// // Set up a SQL query to select the column data
		 string query = "SELECT Rank FROM total_category_item where category_name LIKE'%PsrAdvRaw%' AND rank <> 88";

//		WHERE([t0].[category_name] LIKE @p0) AND([t0].[rank] <> @p1)

		//
		//Set up a SqlDataAdapter to retrieve the data from the database
		DataSet dataset = new DataSet();
		SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
	
		 adapter.Fill(dataset, "Rank");
		//
		// // Retrieve the column data from the DataSet
		DataTable table = new DataTable("Data");
		table.Columns.Add("Rank", typeof(double));

		DataTable dataTable = dataset.Tables["Rank"];
		 foreach (DataRow row in dataTable.Rows)
		 {
		     object columnData = row["Rank"];
		     table.Rows.Add(columnData);
			// Do something with the column data...
		}
		dataset.Tables.Add(table);


		//DataTable table = new DataTable("Data");
		//table.Columns.Add("Rank", typeof(double));
		//table.Rows.Add(-86);
		//table.Rows.Add(-58);
		// Add more rows with the data you need

		

		// Extract data from the DataSet
		double[] ranks = dataset.Tables["Data"].AsEnumerable().Select(row => row.Field<double>("Rank")).ToArray();

		int n = ranks.Length;
		double minValue = MathNet.Numerics.Statistics.ArrayStatistics.Minimum(ranks);
		double maxValue = MathNet.Numerics.Statistics.ArrayStatistics.Maximum(ranks);
		double standardDeviation = ArrayStatistics.StandardDeviation(ranks);
		double interquartileRange = ArrayStatistics.InterquartileRangeInplace(ranks);

		// Square root rule
		int numBinsSqrt = (int)Math.Ceiling(Math.Sqrt(n));

		// Sturges' formula
		int numBinsSturges = (int)Math.Ceiling(Math.Log(n, 2) + 1);

		// Rice rule
		int numBinsRice = (int)Math.Ceiling(2 * Math.Pow(n, 1.0 / 3.0));

		// Scott's rule
		double binWidthScott = 3.5 * standardDeviation * Math.Pow(n, -1.0 / 3.0);
		int numBinsScott = (int)Math.Ceiling((maxValue - minValue) / binWidthScott);

		// Freedman-Diaconis rule
		double binWidthFD = 2 * interquartileRange * Math.Pow(n, -1.0 / 3.0);
		int numBinsFD = (int)Math.Ceiling((maxValue - minValue) / binWidthFD);

		int binSize = numBinsFD;
		double minRank = minValue = -20;
		double maxRank = maxValue = 20;


		var histogram = new Histogram(ranks, 30, minRank, maxRank);

		Console.WriteLine("Histogram:");
		for (int i = 0; i < histogram.BucketCount; i++)
		{
			Console.WriteLine($"Bin {histogram[i].LowerBound} to {histogram[i].UpperBound}: {histogram[i].Count}");
		}
		
		int[] binAssignments = ranks.Select(x => AssignBin(x, histogram)).ToArray();
		//binAssignments.Dump();
	}
	
	public static int AssignBin(double value, MathNet.Numerics.Statistics.Histogram histogram)
	{
		for (int i = 0; i < histogram.BucketCount; i++)
		{
			if (histogram[i].LowerBound <= value && value < histogram[i].UpperBound)
			{
				return i;
			}
		}

		// If value is equal to the maximum upper bound, assign it to the last bin
		if (value == histogram[histogram.BucketCount - 1].UpperBound)
		{
			return histogram.BucketCount - 1;
		}

		// If value is outside the bin range, return -1
		return -1;
	}

}

// // Set up a connection to the SQL Server database
// string connectionString = "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;";
// SqlConnection connection = new SqlConnection(connectionString);
//
// // Set up a SQL query to select the column data
// string query = "SELECT columnName FROM tableName";
//
// // Set up a SqlDataAdapter to retrieve the data from the database
// SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
//
// // Set up a DataSet to hold the retrieved data
// DataSet dataSet = new DataSet();
//
// // Fill the DataSet with the data from the database
// adapter.Fill(dataSet, "tableName");
//
// // Retrieve the column data from the DataSet
// DataTable dataTable = dataSet.Tables["tableName"];
// foreach (DataRow row in dataTable.Rows)
// {
//     object columnData = row["columnName"];
//     // Do something with the column data...
// }
//


// // Set up a connection to the SQL Server database
// string connectionString = "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;";
// SqlConnection connection = new SqlConnection(connectionString);
//
// // Set up a SQL query to select the column data
// string query = "SELECT columnName FROM tableName";
//
// // Create a SqlCommand object with the SQL query and SqlConnection object
// SqlCommand command = new SqlCommand(query, connection);
//
// // Open the connection to the database
// connection.Open();
//
// // Execute the SQL query and retrieve the data using LINQ
// var columnData = command.ExecuteReader()
//     .Cast<IDataRecord>()
//     .Select(x => (object)x["columnName"])
//     .ToList();
//
// // Close the connection to the database
// connection.Close();
//
// // Do something with the column data...
// foreach (var data in columnData)
// {
//     // Do something with the column data...
// }


// class HistogramGenerator
// {
//     static void Main(string[] args)
//     {
//          string connectionString = "Server=KSK;Database=KSK;User Id=myUsername;Password=myPassword;";
//       // SqlConnection connection = new SqlConnection(connectionString);
//
//         // Create a sample DataSet with a DataTable containing a column named "Rank"
//         DataSet dataset = new DataSet();
//         DataTable table = new DataTable("Data");
//         table.Columns.Add("Rank", typeof(double));
//         table.Rows.Add(-86);
//         table.Rows.Add(-58);
//         // Add more rows with the data you need
//         dataset.Tables.Add(table);
//
//         // Extract data from the DataSet
//         double[] ranks = dataset.Tables["Data"].AsEnumerable().Select(row => row.Field<double>("Rank")).ToArray();
//
//         int binSize = 5;
//         double minRank = -90;
//         double maxRank = -30;
//
//         var histogram = new Histogram(ranks, binSize, minRank, maxRank);
//
//         Console.WriteLine("Histogram:");
//         for (int i = 0; i < histogram.BucketCount; i++)
//         {
//             Console.WriteLine($"Bin {histogram[i].LowerBound} to {histogram[i].UpperBound}: {histogram[i].Count}");
//         }
//     }
// }