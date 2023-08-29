namespace CongTDev.EventManagers
{
    public class ObjectHolder<T>
    {
        public ObjectHolder() { }
        public ObjectHolder(T obj) 
        { 
            value = obj;
        }
        public T value;
    }
}