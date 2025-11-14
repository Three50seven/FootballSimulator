namespace Common.Core.Domain
{
    public sealed class DefaultUser : IUser, IUserId, IUserId<int>
    {
        //
        // Summary:
        //     Static value to represent the "system" User ID for the application.
        public static int DefaultId = 1;

        public int Id => DefaultId;
    }
}
