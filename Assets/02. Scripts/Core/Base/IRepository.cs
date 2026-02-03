public interface IRepository<T>
{
    T Load();
    void Save(T data);
    void Delete();
    bool Exists();
}
