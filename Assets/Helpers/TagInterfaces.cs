namespace TagInterfacesSpace
{
    public interface ITag {}
    public interface IPlayer: ITag {}
    public interface IEnemy: ITag {}
    public interface IWeapon: ITag {}
    public interface IBox: ITag {}
    public interface IFruit: ITag {}
    public interface ICheckpoint: ITag {}
    public interface ITrap: ITag {}
    public interface ILevelPoint: ITag {}
    public interface IDestructible : ITag
    {
        public void DestroyObject();
    }
}