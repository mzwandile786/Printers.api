namespace Printers.api.Models
{
    public class UserLoginResult
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int DesignationID { get; set; }
        public string DesignationName { get; set; }
    }

    public class UserLoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
