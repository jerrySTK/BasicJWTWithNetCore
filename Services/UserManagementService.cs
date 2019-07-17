namespace JWTApi.Services
{
    public interface IUserManagementService
    {
        bool IsValidUser(string username, string password);
    }
    public class UserManagementService:IUserManagementService
    {
        public bool IsValidUser(string userName , string password)
        {
            return true;
        }
    }
}