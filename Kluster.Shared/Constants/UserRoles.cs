namespace Kluster.Shared.Constants
{
    /// <summary>
    /// Use for Business Type
    /// </summary>
    public static class UserRoles
    {
        public const string Admin = nameof(Admin);
        public const string User = nameof(User);
        public const string Client = nameof(Client);
        public static string[] AllRoles = [Admin, User];
    }
}
