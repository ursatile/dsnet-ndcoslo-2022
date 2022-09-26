namespace Messages;
public class Greeting {

    public int Number { get; set; }
    public override string ToString() {
        return $"Greeting #{Number}";
    }
}
