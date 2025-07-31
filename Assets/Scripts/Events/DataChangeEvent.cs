public class DataChangeEvent<T> : IGameEvent
{
    public string FieldName { get; private set; }
    public T NewValue { get; private set; }

    public DataChangeEvent(string fieldName, T newValue)
    {
        FieldName = fieldName;
        NewValue = newValue;
    }
}