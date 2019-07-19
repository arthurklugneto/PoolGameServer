namespace PoolGameServer.Populators
{
    public interface IPopulator<T>
    {
        T populate(T entity);
    }
}