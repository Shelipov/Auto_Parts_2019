using Auto_Parts_2019.Models.Parts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;

namespace Auto_Parts_2019.Data
{
    public interface IPartsRepository
    {
        void Create(Part Part);
        void Delete(int id);
        IEnumerable<Part> GetParts(string number);
        IEnumerable<Part> GetParts();
        void Update(Part part);
        string GetName(string email);
        double GetCourseEuro();
        Part GetParts(int number);
        Address GetUser(string UserID);
    }
    public class PartsRepository : IPartsRepository
    {
        string connectionString = null;
        public PartsRepository(string conn)
        {
            connectionString = conn;
        }
        public IEnumerable<Part> GetParts()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<Part>("select * from Parts order by Parts.Quantity desc").ToList();
            }
        }
        public double GetCourseEuro()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var course= db.Query<Course>("select * from Courses order by DateLastModified desc").FirstOrDefault();
                return course.CourseEuro;
            }
        }
        public string GetName(string email)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string[] warn = { @"'", "--", "Select", "From", "Create", "Update", "Delete", "Where", "and", "or", "+", "=", "database", "insert" };
                foreach (var i in warn)
                {
                    var str = i.ToUpper();
                    email = email.Replace(str, "").ToUpper();
                }
                var result= db.Query<Address>("SELECT [UserName] FROM [Auto_Parts_2019].[dbo].[AspNetUsers] Where [NormalizedEmail]=@NormalizedEmail",new { @NormalizedEmail = email}).FirstOrDefault();               
                return result.ToString();
            }
        }

        public IEnumerable<Part> GetParts(string number)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var numbers= db.Query<Cros>("select [Number] from [Auto_Parts_2019].[dbo].[Cross] where [SearchNumber]=@number or [Number]=@number", new { number }).FirstOrDefault();
                string num = numbers.Number.ToString();
                var analog = db.Query<Part>("select [Analogues] from [Auto_Parts_2019].[dbo].[Parts] where [Number]= @num", new { num }).FirstOrDefault();
                string an = analog.Analogues.ToString();
                var parts = db.Query<Part>("select * from [Auto_Parts_2019].[dbo].[Parts] where [Analogues] = @an", new { an });
                
                if (parts != null)
                    return parts;
                
                else return null;
            }
        }
        public Part GetParts(int number)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return  db.Query<Part>("select * from [Auto_Parts_2019].[dbo].[Parts] where [ID] = @number", new { number }).FirstOrDefault();
            }
        }
        public Address GetUser(string UserID)
        {

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<Address>("Select a.Country,a.Sity,a.Avenue,a.Email,a.PhoneNumber,a.UserName,a.Discount FROM [Auto_Parts_2019].[dbo].[AspNetUsers] a where [AddressID]=@UserID", new { UserID}).FirstOrDefault();
            }
        }

        public void Create(Part Part)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "INSERT INTO Parts (Name, Age) VALUES(@Name, @Age)";
                db.Execute(sqlQuery, Part);

                // если мы хотим получить id добавленного пользователя
                //var sqlQuery = "INSERT INTO Parts (Name, Age) VALUES(@Name, @Age); SELECT CAST(SCOPE_IDENTITY() as int)";
                //int? PartId = db.Query<int>(sqlQuery, Part).FirstOrDefault();
                //Part.Id = PartId.Value;
            }
        }

        public void Update(Part Part)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "UPDATE Parts SET Name = @Name, Age = @Age WHERE Id = @Id";
                db.Execute(sqlQuery, Part);
            }
        }

        public void Delete(int id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "DELETE FROM Parts WHERE Id = @id";
                db.Execute(sqlQuery, new { id });
            }
        }
    }
}
