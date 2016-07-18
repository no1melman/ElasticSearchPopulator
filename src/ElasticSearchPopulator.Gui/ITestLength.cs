namespace ElasticSearchPopulator.Gui
{
    using System;

    using Core;

    public interface ITestLength
    {
        int GetSizeInBytes<T>(int recordCount, Func<T> generate) where T : IHasId;
    }
}