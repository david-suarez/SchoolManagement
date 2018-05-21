
namespace SchoolManagement.Interfaces
{
    public interface IValidator<in T>
    {
        bool Validate(T target);
    }
}
