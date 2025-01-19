namespace DroneDataCollection;

public class VisibilityAttribute(string visibilityBinding = "") : Attribute {

    public string visibilityBinding { get; set; } = visibilityBinding;

}
