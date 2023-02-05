using ContentRate.Domain.Notifications;
using ContentRate.Domain.Rooms;

namespace ContentRate.Infrastructure.Repositories.EnitiesGenerator.Factories;

public static class AssessorFactory
{
    public static Assessor CreateAssessor(Guid? id = null)
    {
        id ??= Guid.NewGuid();
        return new Assessor(id.Value, id.Value.ToString());
    }
}
