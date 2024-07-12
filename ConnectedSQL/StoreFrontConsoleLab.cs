using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Text.Json;

namespace ConnectedSQL;
internal class StoreFrontConsoleLab {
	static void Main(string[] args) {
		
		string conString = @"Data Source=.\sqlexpress;Initial Catalog=Northwind;Integrated Security=true;Encrypt=false;";

		SqlConnection conn = new SqlConnection(conString);
		conn.Open();

		SqlCommand cmd = new SqlCommand(@"SELECT * FROM Products", conn);

		SqlDataReader reader = cmd.ExecuteReader();

		List<Product> products = new List<Product>();

		while (reader.Read()) {
			Product p = new Product() {
				ProductID		= Convert.ToInt32(reader["ProductID"]),
				ProductName		= reader["ProductName"].ToString(),
				SupplierID		= reader["SupplierID"] is DBNull ? null : Convert.ToInt32(reader["SupplierID"]),
				CategoryID		= reader["CategoryID"] is DBNull ? null : Convert.ToInt32(reader["CategoryID"]),
				QuantityPerUnit = reader["QuantityPerUnit"] is DBNull ? null : reader["QuantityPerUnit"].ToString(),
				UnitPrice		= reader["UnitPrice"] is DBNull ? null : Convert.ToDecimal(reader["UnitPrice"]),
				UnitsInStock	= reader["UnitsInStock"] is DBNull ? null : Convert.ToInt32(reader["UnitsInStock"]),
				UnitsOnOrder	= reader["UnitsOnOrder"] is DBNull ? null : Convert.ToInt32(reader["UnitsOnOrder"]),
				ReorderLevel	= reader["ReorderLevel"].ToString(),
				Discontinued	= Convert.ToBoolean(reader["Discontinued"]),
			};
			products.Add(p);
		}

		Console.WriteLine(JsonSerializer.Serialize(products, new JsonSerializerOptions { WriteIndented = true }));

		reader.Close();
		conn.Close();

	}

	internal class Product {
		public int		ProductID { get; set; }
		public string	ProductName { get; set; }
		public int?		SupplierID { get; set; }
		public int?		CategoryID { get; set; }
		public string?	QuantityPerUnit { get; set; }
		public decimal?	UnitPrice { get; set; }
		public int?		UnitsInStock { get; set; }
		public int?		UnitsOnOrder { get; set; }
		public string?	ReorderLevel { get; set; }
		public bool		Discontinued { get; set; }
	}

}
