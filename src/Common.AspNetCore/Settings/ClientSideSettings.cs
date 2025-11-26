namespace Common.AspNetCore
{
    public class ClientSideSettings
    {
        public bool Debug { get; set; } = false;
        public bool AlertErrors { get; set; } = true;
        public int UserSessionTimeout { get; set; } 
    }
}
