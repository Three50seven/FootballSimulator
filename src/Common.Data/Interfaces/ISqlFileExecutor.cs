namespace Common.Data
{
    public interface ISqlFileExecutor
    {
        void DropExecuteStoredProcedure(string storedProcName, string filePath, int timeout = 60);
    }
}
