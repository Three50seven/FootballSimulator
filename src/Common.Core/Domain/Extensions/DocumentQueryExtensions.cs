namespace Common.Core.Domain
{
    public static class DocumentQueryExtensions
    {
        /// <summary>
        /// Filter by provided <paramref name="fileName"/>.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="fileName">Valid filename on which to filter the query.</param>
        /// <param name="directoryId">Optional directory Id to filter.</param>
        /// <returns></returns>
        public static IQueryable<Document> ByFileName(this IQueryable<Document> query, string fileName, int? directoryId = null)
        {
            if (query == null)
                return query;

            fileName = fileName.Trim();
            query = query.Where(d => d.File.FileName == fileName);

            directoryId = directoryId.CleanForNull();
            if (directoryId != null)
                query = query.Where(d => d.DirectoryId == directoryId);

            return query;
        }

        /// <summary>
        /// Filter by provided full <paramref name="filePath"/>. 
        /// Filename of the path is queried first, then the remaining path is confirmed against <see cref="Document.SubPath"/>.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="filePath">Valid full path to a file.</param>
        /// <param name="directoryId">Optional directory Id to filter.</param>
        /// <returns></returns>
        public static IQueryable<Document> ByFilePath(this IQueryable<Document> query, string filePath, int? directoryId = null)
        {
            if (query == null)
                return query;

            filePath = filePath.Trim();

            query = ByFileName(query, Path.GetFileName(filePath), directoryId);

            string subPath = Path.GetDirectoryName(filePath);

            return query.Where(d => d.SubPath == subPath);
        }
    }
}
