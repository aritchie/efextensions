using System;


namespace Acr.EfExtensions
{
    public interface ISoftDeleteEntity
    {
        bool IsDeleted { get; set; }
    }
}
