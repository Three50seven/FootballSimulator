using Microsoft.AspNetCore.Http;
using Common.Core.Domain;

namespace Common.AspNetCore
{
    public static class FormFileExtensions
    {
        /// <summary>
        /// Copy form file values to FileUpload value object. Stream will be the open read stream.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static FileUpload ToFileUpload(this IFormFile file)
        {
            if (file == null)
                return null;

            return new FileUpload(
                file.OpenReadStream(), 
                System.IO.Path.GetFileName(file.FileName),  // be sure to only get the qualified filename - IFormFile.FileName can sometimes be the full path
                file.Length, 
                file.ContentType);
        }
    }
}
