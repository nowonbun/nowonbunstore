namespace PTM.Httpd
{
    public enum Opcode : int
    {
        MESSAGE = 1,
        BINARY = 2,
        EXIT = 8,
        PING = 9,
        PONG = 10
    }
}
