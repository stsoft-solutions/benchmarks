namespace Core;

[AttributeUsage(AttributeTargets.Parameter)]
public class FieldMapAttribute : Attribute
{
    public string Name { get; }

    public FieldMapAttribute(string name)
    {
        Name = name;
    }
}