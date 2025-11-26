using Microsoft.AspNetCore.Http;
using Common.Core;
using Common.Core.Domain;
using Common.Core.Validation;
using System.ComponentModel.DataAnnotations;

namespace Common.AspNetCore
{
    public class DocumentUploadWebModel
    {
        [Required]
        public IFormFile File { get; set; }

        [GreaterThanZero]
        public int DirectoryId { get; set; }

        [MaxLength(255)]
        public string SubPath { get; set; }

        public virtual void UpdateValuesFromForm(IFormCollection form)
        {
            Guard.IsNotNull(form, nameof(form));

            DirectoryId = form["DirectoryId"].ToString().ParseInteger();
            SubPath = form["SubPath"].ToString().SetEmptyToNull();
        }

        public virtual DocumentUpload ToUploadModel()
        {
            if (File == null)
                return null;

            string fileName = File.FileName?.Trim();

            // be sure to only get the qualified filename - IFormFile.FileName can sometimes be the full path
            if (!string.IsNullOrWhiteSpace(fileName))
                fileName = System.IO.Path.GetFileName(fileName);

            return new DocumentUpload(File.OpenReadStream(), fileName, DirectoryId, File.Length, File.ContentType);
        }
    }
}
