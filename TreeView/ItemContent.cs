namespace Excubo.Blazor.TreeViews
{
    public struct ItemContent<T>
    {
        public int Level { get; set; }
        public T Item { get; set; }
        public ItemContent(int level, T item)
        {
            Level = level;
            Item = item;
        }
        public static implicit operator ItemContent<T>((int Level, T Item) value)
        {
            return new ItemContent<T>(value.Level, value.Item);
        }
    }
}
