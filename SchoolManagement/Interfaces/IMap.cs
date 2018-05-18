namespace SchoolManagement.Interfaces
{
    public interface IMap<T, K>
    {
        K Map(T target);
    }
}
