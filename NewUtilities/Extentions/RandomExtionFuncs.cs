namespace System
{
    public static class RandomExtionFuncs
    {
        public static double NextDouble(this Random r, double min, double max)
        {
            if (min >= max)
                throw new ArgumentException("最小值不能大于或等于最大值");
            return (max - min) * r.NextDouble() + min;
        }
    }
}
