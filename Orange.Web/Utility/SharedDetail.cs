namespace Orange.Web.Utility;

public class SharedDetail
{
    public static string CouponApiBase { get; set; }
    public static string AuthApiBase { get; set; }

    public static string RoleAdmin = "ADMIN";
    public static string RoleCustomer = "CUSTOMER";
    public static string RoleEmployee = "EMPLOYEE";
    public enum ApiType
    {
        Get,
        Post,
        Put,
        Delete,
    }
    
    
}