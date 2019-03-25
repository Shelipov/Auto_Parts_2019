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
        void Create(_Order Part);
        void Delete(string id);
        IEnumerable<Part> GetParts(string number);
        IEnumerable<Part> GetParts();
        void Update(Part part);
        string GetName(string email);
        double GetCourseEuro();
        Part GetParts(int number);
        Address GetUser(string UserID);
        string GetUserID(string addressid);
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
                var result = db.Query<Part>("select * from Parts where Parts.Quantity > 0 order by Parts.Quantity desc ").ToList();
                return result;
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
                string[] warn = { @"'", "--", " Select ", " From ", " Create ", " Update ", " Delete ", " Where ", " and ", " or", "+", "=", " database ", " insert " };
                foreach (var i in warn)
                {
                    var str = i.ToUpper();
                    email = email.Replace(str, "").ToUpper();
                }
                var result= db.Query<string>("SELECT UserName FROM AspNetUsers Where NormalizedEmail= @NormalizedEmail", new { @NormalizedEmail = email}).FirstOrDefault();
                return result;
            }
        }

        public IEnumerable<Part> GetParts(string number)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var numbers= db.Query<Cros>("select [Number] from [Cross] where [SearchNumber]=@number or [Number]=@number", new { number }).FirstOrDefault();
                string num = numbers.Number.ToString();
                var analog = db.Query<Part>("select [Analogues] from [Parts] where [Number]= @num", new { num }).FirstOrDefault();
                string an = analog.Analogues.ToString();
                var parts = db.Query<Part>("select * from [Parts] where [Analogues] = @an AND Quantity > 0 ", new { an });
                
                if (parts != null)
                {
                    foreach(var i in parts)
                    {
                        if (i.Quantity > 4)
                            i.Quantity = 777;
                    }
                    return parts;
                }

                else return null;
            }
        }
        public Part GetParts(int number)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return  db.Query<Part>("select * from [Parts] where [ID] = @number", new { number }).FirstOrDefault();
            }
        }
        public Address GetUser(string UserID)
        {

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<Address>("Select a.Country,a.Sity,a.Avenue,a.Email,a.PhoneNumber,a.UserName,a.Discount FROM [AspNetUsers] a where [AddressID]=@UserID", new { UserID}).FirstOrDefault();
            }
        }

        public void Create(_Order Part)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = @"INSERT INTO _Orders ([SessionID],OrderID,PartID,Number,Brand,Price,Group_Parts,Quantity
                                 ,AddressID, Country, Sity, Avenue, IP, Comment, OneCCreate,UserID) 
                                 VALUES( @SessionID, @OrderID, @PartID, @Number, @Brand, @Price, @Group_Parts, @Quantity,
                                 @AddressID, @Country, @Sity, @Avenue, @IP, @Comment, @OneCCreate, @UserID)";
                db.Execute(sqlQuery, new { @SessionID = Part.SessionID, @OrderID=Part.OrderID,
                    @PartID=Part.PartID,@Number=Part.Number,@Brand=Part.Brand,@Price=Part.Price,
                    @Group_Parts=Part.Group_Parts,@Quantity=Part.Quantity,@AddressID=Part.AddressID,@Country=Part.Country,
                    @Sity=Part.Sity,@Avenue=Part.Avenue,@IP=Part.IP,@Comment=Part.Comment,@OneCCreate=Part.OneCCreate,@UserID=Part.UserID});

            }
        }

        public void Update(Part Part)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "UPDATE Parts SET Name = @Name, Quantity = @Quantity WHERE Id = @Id";
                db.Execute(sqlQuery, new { Part });
            }
        }

        public void Delete(string OrderID)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "DELETE _OrdersDTO Where OrderID = @OrderID";
                db.Execute(sqlQuery, new { OrderID = OrderID });
            }
        }
        public string GetUserID(string addressid)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = @"SELECT Id From AspNetUsers Where AddressID = @addressid ";
                var result = db.Query<string>(sqlQuery, new { addressid }).FirstOrDefault();
                return result;
            }
        }
    }
}
