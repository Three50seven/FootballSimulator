using Common.Core;

namespace Common.AspNetCore
{
    public class FriendlyErrorInfo
    {
        public FriendlyErrorInfo(string message, string errorCode = null)
        {
            Message = message.SetNullToEmpty();
            ErrorCode = errorCode.SetNullToEmpty();
        }

        public string Message { get; private set; }
        public string ErrorCode { get; private set; }
        public virtual bool DisplayCustomMessage => !string.IsNullOrWhiteSpace(Message);
    }
}
