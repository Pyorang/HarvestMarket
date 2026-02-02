public interface IAccountRepository
{
    AccountData FindByEmail(string email);
    void Save(AccountData data);
    void Delete(string email);
    bool Exists(string email);
}
