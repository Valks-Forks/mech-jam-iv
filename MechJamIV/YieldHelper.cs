namespace MechJamIV;

public static class YieldHelper
{
    public static IEnumerable<T> Yield<T>(this T obj)
    {
        yield return obj;
    }
}