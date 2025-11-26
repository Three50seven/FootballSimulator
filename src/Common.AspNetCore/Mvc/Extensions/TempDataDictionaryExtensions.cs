using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Common.AspNetCore.Mvc
{
    public static class TempDataDictionaryExtensions
    {
        public const string RollbackTransactionKey = "RollbackTransactionKey";

        public static bool ShouldRollbackTransaction(this ITempDataDictionary tempData)
        {
            if (tempData == null)
                return false;

            var rollBack = tempData[RollbackTransactionKey];
            if (rollBack == null || rollBack is not bool) // if not set or properly not set, no rollback
                return false;
            else
                return (bool)rollBack;
        }

        public static void SetTransactionStatus(this ITempDataDictionary tempData, bool rollback)
        {
            ArgumentNullException.ThrowIfNull(tempData);

            tempData[RollbackTransactionKey] = rollback;
        }

        /// <summary>
        /// Extension lookup to allow for removing key from dictionary after getting it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tempData"></param>
        /// <param name="key"></param>
        /// <param name="removeAfter"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryGetValue<T>(this ITempDataDictionary tempData, string key, bool removeAfter, out T value)
        {
            if (tempData == null)
            {
                value = default;
                return false;
            }

            var data = tempData[key];
            if (data == null)
            {
                value = default;

                if (removeAfter)
                    tempData.Remove(key);

                return false;
            }

            value = (T)data;

            if (removeAfter)
                tempData.Remove(key);

            return true;
        }
    }
}
