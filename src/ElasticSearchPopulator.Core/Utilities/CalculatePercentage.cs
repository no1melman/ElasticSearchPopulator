namespace ElasticSearchPopulator.Core.Utilities
{
    public interface ICalculatePercentage
    {
        double Calculate(int count, int current);
    }

    public class CalculatePercentage : ICalculatePercentage
    {
        public double Calculate(int count, int current)
        {
            return ((double)current / count) * 100;
        }
    }
}