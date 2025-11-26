namespace Common.Core.Domain
{
    public sealed class DefaultUser : IUser
    {
        /// <summary>
        /// Static value to represent the "system" User ID for the application.
        /// </summary>
        public static int DefaultId = 1;

        public int Id => DefaultId;
    }
}
