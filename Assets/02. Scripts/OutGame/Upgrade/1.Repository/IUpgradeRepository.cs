using Cysharp.Threading.Tasks;

public interface IUpgradeRepository
{
    UniTask<PlayerUpgradeData> Load();
    UniTaskVoid Save(PlayerUpgradeData data);
}