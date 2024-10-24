namespace ProjProcessOrders.Domain.Bases
{
    public abstract class BaseEntity<TKeyType>
    {
        protected BaseEntity(TKeyType id = default)
        {
            Id = id;
            CreatedAt = DateTime.UtcNow;
        }

        public virtual TKeyType Id { get; }
        public virtual string? CreatedBy { get; set; }
        public virtual DateTime CreatedAt { get; set; }
    }
}
