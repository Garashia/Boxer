public static class TestScript
{
    public static void Action<T>(this T obj)
    {
        obj.Equals(null);
    }
}
