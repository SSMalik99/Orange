namespace Orange.Web.Utility;

public class SharedDetail
{
    public static string CouponApiBase { get; set; }
    public enum ApiType
    {
        Get,
        Post,
        Put,
        Delete,
    }
    
    
}