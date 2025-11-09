namespace DoDone.Domain.Roles
{
    public class Role
    {
        public byte Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        override public string ToString()
            => $"{Name}";
    }
}
