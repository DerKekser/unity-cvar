namespace Kekser.UnityCVar
{
    public struct CVarResult
    {
        public bool Success { get; }
        public string Message { get; }
        
        public CVarResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }
        
        public static implicit operator bool(CVarResult result)
        {
            return result.Success;
        }
    }
}