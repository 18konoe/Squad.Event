namespace SquadEvent.Shared.Utilities
{
    public interface ICustomConverter<in T1, out T2>
    {
        T2 Convert(T1 source);
    }
}