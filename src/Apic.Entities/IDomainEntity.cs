namespace Apic.Entities
{
    public interface IDomainEntity<TKey>
    {
        TKey Id { get; set; }
    }
}