namespace Orange.Web.Utility;

public static class SharedDetail
{
    public static string CouponApiBase { get; set; } = string.Empty;
    public static string AuthApiBase { get; set; } = string.Empty;
    public static string ProductApiBase { get; set; } = string.Empty;
    public static string CartApiBase { get; set; } = string.Empty;
    public static string OrderApiBase { get; set; } = string.Empty;

    public const string RoleAdmin = "ADMIN";
    public const string RoleCustomer = "CUSTOMER";
    public const string RoleEmployee = "EMPLOYEE";
    
    public const string TokenCookie = "JWTToken";
    public enum ApiType
    {
        Get,
        Post,
        Put,
        Delete,
    }
    
    
}