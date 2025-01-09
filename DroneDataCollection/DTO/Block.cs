namespace DroneDataCollection;

public class Block {

    public string[] head { get; set; }
    
    public float[,] body { get; set; }

    public Block(string[] head, float[,] body) {
        this.head = head;
        this.body = body;
    }

    
    
}
