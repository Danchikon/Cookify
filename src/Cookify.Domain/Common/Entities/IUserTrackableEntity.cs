namespace Cookify.Domain.Common.Entities;

public interface IUserTrackableEntity : IEntity<Guid>
{   
    Guid? CreatedBy { get; init;  }
    Guid? UpdatedBy { get; set; }
}