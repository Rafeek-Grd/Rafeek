using Rafeek.Domain.Common;

namespace Rafeek.Domain.Entities
{
    public class RolePermission : BaseEntity
    {
        public string ModuleKey { get; set; } = null!;
        public string ModuleNameAr { get; set; } = null!;
        public bool CanView { get; set; }
        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanCreate { get; set; }
    }
}
