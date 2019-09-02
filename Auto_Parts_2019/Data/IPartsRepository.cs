using Auto_Parts_2019.Models.Parts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using Auto_Parts_2019.Areas.Identity.Pages.Account.Manage;

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
        IEnumerable<string> AutocompleteSearch(string number);
        int GetPackCount(int PartID);
        void CreateDebit_BN(Debit debit);
        List<MutualSettlementModelDTO> GetMutualSettlemenList(string UserID);
        void EmailMutualSettlemenList(int MutualSettlementID, string UserID, string Email);
        string GetUserEmail(string UserID);


    }
    public class PartsRepository : IPartsRepository
    {
        string connectionString = null;
        public PartsRepository(string conn)
        {
            connectionString = conn;
        }
        public IEnumerable<string> AutocompleteSearch(string number)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<string>("SELECT c.Number FROM [shelipov].[Parts] c Where c.Number LIKE Concat('%',@number,'%')", new { number });
            }
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
                
                var parts = db.Query<Part>(@"Declare @num nvarchar(max);
                                            Set @num=(select distinct p.Analogues
                                            from Parts as p
                                            left join [shelipov].[Cross] as c on c.Number=p.Number
                                            where c.SearchNumber = @number or p.Number = @number)
                                            Select *
                                            From Parts as p
                                            Where p.Analogues = @num and p.Brand != 'Fremax'", new { number }); //AND Quantity > 0

                if (parts != null)
                {
                    foreach (var i in parts)
                    {
                        if (i.Quantity > 4)
                            i.Quantity = 777;
                    }
                    return parts;
                }

                else {
                    
                    return null; }
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
        public string GetUserEmail(string UserID)
        {

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<string>("Select a.Email FROM AspNetUsers a where AddressID=@UserID", new { UserID }).FirstOrDefault();
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
        public  void CreateDebit_BN(Debit Part)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = @"INSERT INTO _Orders (AdressID,UserID) 
                                 VALUES( 
                                 @AdressID, @UserID)";
                db.Execute(sqlQuery, new
                {
                    @UserID = Part.UserID,
                    @AdressID = Part.AdressID
                });

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
        public int GetPackCount(int PartID)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = @"SELECT Packaging From Packaging Where PartID = @PartID ";
                var result = db.Query<int>(sqlQuery, new { PartID }).FirstOrDefault();
                return result;
            }
        }
        public List<MutualSettlementModelDTO> GetMutualSettlemenList(string UserID)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                try
                {
                    var sqlQuery = @"SELECT m.MutualSettlementID
                                          ,m.UserID
										  ,u.UserName
                                          ,m.InvoiceType
                                          ,m.InvoceNumber
                                          ,m.EU
                                          ,m.UA
                                          ,(select c.CourseEuro from Courses as c) as Course
                                          ,m.LastСhange
                                          ,m.Del
	                                      ,d.debit as Debit
	                                      ,b.Debit_BN as DebitBN
                                          ,m.DateCreate
                                      from  Debits as d 
                                      left join Debit_BN as b on b.UserID=d.UserID
                                      left join MutualSettlement as m on m.UserID = d.UserID
									  left join AspNetUsers as u on u.AddressID = @UserID
                                      Where d.AdressID =@UserID or d.UserID=@UserID
                                      Order by d.DebitID DESC";
                    var result = db.Query<MutualSettlementModelDTO>(sqlQuery, new { UserID }).ToList();
                    if (result != null)
                        return result;
                    else
                        return new List<MutualSettlementModelDTO>();
                }
                catch(Exception ex)
                {
                    return new List<MutualSettlementModelDTO>();
                }
            }
        }
        public void EmailMutualSettlemenList(int MutualSettlementID,string UserID,string Email)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = @"Insert EmailMutualSettlement (MutualSettlementID,UserID,Email)
                                 Values(@MutualSettlementID,@UserID,@Email)";
                db.Execute(sqlQuery, new
                {
                    @MutualSettlementID=MutualSettlementID,
                    @UserID=UserID,
                    @Email=Email
                });
            }
        }
    }
}
