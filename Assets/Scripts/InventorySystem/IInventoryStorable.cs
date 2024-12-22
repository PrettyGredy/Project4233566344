namespace DemiInventory
{
    public interface IInventoryStorable
    {
        public void PutInInventory<T>(T par);
        public void ExtractOfInventory<T>(T par);
    }
}