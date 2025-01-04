using System.Reflection;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.TypeInspectors;
using YamlDotNet.Serialization.TypeResolvers;

namespace DroneDataCollection;

public class YamlService {

    public IDeserializer deserializer { get; private set; }

    public ISerializer serializer { get; private set; }

    public YamlService() {

        DeserializerBuilder deserializerBuilder = new DeserializerBuilder();
        deserializerBuilder.WithTypeInspector
        (
            inspector => new CompositeTypeInspector
            (
                new YamlMemberReadableFieldsTypeInspector(new StaticTypeResolver()),
                inspector
            )
        );
        deserializer = deserializerBuilder.Build();

        SerializerBuilder serializerBuilder = new SerializerBuilder();
        serializerBuilder.WithTypeInspector
        (
            inspector => new CompositeTypeInspector
            (
                new YamlMemberReadableFieldsTypeInspector(new StaticTypeResolver()),
                inspector
            )
        );
        serializer = serializerBuilder.Build();
    }

}

public class YamlMemberReadableFieldsTypeInspector : ReflectionTypeInspector {

    private readonly ITypeResolver typeResolver;

    public YamlMemberReadableFieldsTypeInspector(ITypeResolver typeResolver) {
        this.typeResolver = typeResolver ?? throw new ArgumentNullException(nameof(typeResolver));
    }

    public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, object? container) {
        return type
            .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .Select(p => new ReflectionFieldDescriptor(p, typeResolver));
    }

    protected class ReflectionFieldDescriptor : IPropertyDescriptor {

        private readonly FieldInfo fieldInfo;

        private readonly ITypeResolver typeResolver;

        public ReflectionFieldDescriptor(FieldInfo fieldInfo, ITypeResolver typeResolver) {
            this.fieldInfo = fieldInfo;
            this.typeResolver = typeResolver;

            YamlConverterAttribute? converterAttribute = fieldInfo.GetCustomAttribute<YamlConverterAttribute>();
            if (converterAttribute != null) {
                ConverterType = converterAttribute.ConverterType;
            }

            ScalarStyle = ScalarStyle.Any;
        }

        public string Name { get { return fieldInfo.Name; } }

        public bool Required => false;

        public Type Type { get { return fieldInfo.FieldType; } }

        public Type? ConverterType { get; }

        public Type? TypeOverride { get; set; }

        public bool AllowNulls => true;

        public int Order { get; set; }

        public bool CanWrite { get { return !fieldInfo.IsInitOnly; } }

        public ScalarStyle ScalarStyle { get; set; }

        public void Write(object target, object? value) {
            fieldInfo.SetValue(target, value);
        }

        public T? GetCustomAttribute<T>() where T : System.Attribute {
            object[] attributes = fieldInfo.GetCustomAttributes(typeof(T), true);
            return (T?)attributes.FirstOrDefault();
        }

        public IObjectDescriptor Read(object target) {
            object? propertyValue = fieldInfo.GetValue(target);
            Type actualType = TypeOverride ?? typeResolver.Resolve(Type, propertyValue);
            return new ObjectDescriptor(propertyValue, actualType, Type, ScalarStyle);
        }

    }

}
