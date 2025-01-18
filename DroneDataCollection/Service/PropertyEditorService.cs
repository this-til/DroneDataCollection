namespace DroneDataCollection;

public class PropertyEditorService {

    protected Dictionary<Type, PropertyEditorBase?> propertyEditorBaseMap { get; set; } = new Dictionary<Type, PropertyEditorBase?>();

    public PropertyEditorBase? getPropertyEditor(Type type) {
        if (!typeof(PropertyEditorBase).IsAssignableFrom(type)) {
            return null;
        }
        if (propertyEditorBaseMap.TryGetValue(type, out PropertyEditorBase? editor)) {
            return editor;
        }
        PropertyEditorBase? editorBase = Activator.CreateInstance(type) as PropertyEditorBase;
        propertyEditorBaseMap.Add(type, editorBase);
        return editorBase;
    }

}
