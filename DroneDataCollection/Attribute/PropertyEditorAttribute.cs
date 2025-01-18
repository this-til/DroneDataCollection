namespace DroneDataCollection;

public class PropertyEditorAttribute(Type editorType) : Attribute {

    public Type editorType { get; private set; } = editorType;

}
