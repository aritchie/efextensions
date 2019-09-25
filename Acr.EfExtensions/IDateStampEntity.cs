using System;


namespace Acr.EfExtensions
{
    public interface IDateStampEntity
    {
        DateTimeOffset DateUpdated { get; set; }
        DateTimeOffset DateCreated { get; set; }
    }
}
