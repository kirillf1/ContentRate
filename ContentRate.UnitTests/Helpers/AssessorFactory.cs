using ContentRate.Domain.Notifications;

namespace ContentRate.UnitTests.Helpers
{
    public static class AssessorFactory
    {
        public static Assessor CreateAssessor(Guid? id = null)
        {
            id ??= Guid.NewGuid();
            return new Assessor(id.Value, id.Value.ToString());
        }
    }
}
