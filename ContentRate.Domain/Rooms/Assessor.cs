namespace ContentRate.Domain.Rooms
{
    public class Assessor
    {
        public Assessor(Guid id, string name,bool isMockAssessor = false) 
        {
            Id = id;
            Name = name;
            IsMockAssessor = isMockAssessor;
        }

        public Guid Id { get; }
        public string Name { get; }
        public bool IsMockAssessor { get; set; }
    }
}
