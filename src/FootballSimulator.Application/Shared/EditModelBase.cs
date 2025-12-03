using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FootballSimulator.Application.Models
{
    public abstract class EditModelBase
    {
        public const string BindProperty = $"{nameof(Id)} {nameof(Guid)}";

        [DisplayName("ID")]
        public int? Id { get; set; }
        [DisplayName("GUID")]
        public Guid? Guid { get; set; }
        public bool IsNew => !Guid.HasValue;
        [AllowedValues(["Create", "Edit"])]
        public string CreateOrEdit => IsNew ? "Create" : "Edit";
    }
}
