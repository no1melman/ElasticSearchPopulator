namespace ElasticSearchPopulator.Core.Data
{
    public interface IDataItemGenerator<out T>
    {
        T Generate();
    }
}