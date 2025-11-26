using Common.Core;

namespace Common.AspNetCore
{
    public abstract class EditViewModelBase : EditViewModelBase<int?>
    {
        public override bool IsNew => Id.CleanForNull() == null;
    }

    public abstract class EditViewModelBase<TKey>
    {
        public TKey Id { get; set; }
        public virtual bool IsNew => Id == null || Id.Equals(default(TKey));
    }
}
