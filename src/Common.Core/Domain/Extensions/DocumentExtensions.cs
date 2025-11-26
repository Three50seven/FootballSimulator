using Common.Core.DTOs;

namespace Common.Core.Domain
{
    public static class DocumentExtensions
    {
        /// <summary>
        /// Returns simplified document information for display purposes.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static DocumentDisplayItem ToDisplayItem(this Document document)
        {
            if (document == null!)
                return null!;

            return new DocumentDisplayItem()
            {
                Title = document.File?.FileName!,
                FullPath = document.FullPath,
                IsImage = document.File?.IsImage ?? false
            };
        }
    }
}
