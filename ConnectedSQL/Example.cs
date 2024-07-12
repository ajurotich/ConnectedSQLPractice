using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Text.Json;

namespace ConnectedSql {
	internal class Example {
		static void Main(string[] args) {
			Console.WriteLine("SQL CONNECTION PRACTICE");

			//Connected SQL is a part of ADO.NET (Active DataX Objects -> Microsoft's solution to incorporating data
			//into applications).  
			//With this implementation, we typically retrieve items from the database and convert those to 
			//C# objects.  Once that is done, we can display them.

			/*
			 *  In order to retrieve data from the database we must define the connection string
			 *  (roadmap the database)
			 *  ConnectionString 3 basic parts: Data Source (SERVER), Initial Catalog (Db Name)
			 *  Integrated Security (true/false/sspi). True indicates Windows (integrated) authentication.
			 *  False indicates Sql Server authentication which requires a user name and password combo.
			 *  SSPI basically works the same as false, but it rarely used.
			 *  
			 *  SqlConnection Object.  The connection object requires a connection string as a parameter
			 *  to build it.  To acces the data, call SqlConnection.Open() then articulate our query.
			 *  
			 *  SqlCommand Object.  The command requires a minimum of CommandText (sql query as a string)
			 *  and a SqlConnection object, to initialize the request.
			 *  If a parameterized query is written, you can use the Paramters property and the 
			 *  AddWithValue() to specify both the parameter AND its value.
			 *  Available Methods (SqlCommand)
			 *  ExecuteReader() - SELECT statements - ReturnType is SqlDataReader
			 *  ExecuteNonQuery() - INSERT, UPDATE, DELETE statments - ReturnType is an int.
			 *                    - Usually used to show RowsAffected by the statement
			 *  ExecuteScalar()   - Aggragate Functions: SUM, AVG, COUNT, etc. - ReturnType Object
			 *  
			 *  SqlDataReader - This object holds the results of the command object's ExecuteReader().
			 *  The reader will need to be looped through (if more than result is desired) or branched
			 *  (if or switch) if a single result is required.  To call ExecuteReader() the connection
			 *  object's Open() must have been called.  When the reader is done, you should call
			 *  SqlDataReader.Close().  You should also close the connection as soon as possible.
			 *  (SqlConnection.Close()).
			 */


			string connectionString = @"Data Source=.\sqlexpress;Initial Catalog=Northwind;Integrated Security=true;Encrypt=false;";

			SqlConnection connection = new SqlConnection(connectionString);

			connection.Open();

			SqlCommand command = new SqlCommand(@"
				select *
				from Categories
			", connection);

			SqlDataReader reader = command.ExecuteReader();

			List<Category> categories = new List<Category>();

			while(reader.Read()) {
				Category c = new Category() {
					CategoryID = Convert.ToInt32(reader["CategoryID"]),
					CategoryName = reader["CategoryName"].ToString(),
					Description = reader["Description"] is DBNull ? null : reader["Description"].ToString(),
					Picture = reader["Picture"] is DBNull ? null : reader["Picture"].ToString()
				};
				categories.Add(c);
			}

			Console.WriteLine(JsonSerializer.Serialize(categories, new JsonSerializerOptions {WriteIndented = true} ));


			connection.Close();
			reader.Close();
		}
	}

	internal class Category {
		public int CategoryID { get; set; }
		public string CategoryName { get; set; }
		public string? Description { get; set; }
		public string? Picture { get; set; }
	}

}
