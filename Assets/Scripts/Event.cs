public enum EEventType { NONE, GENERIC, CONDITION, LOAD_LEVEL, MOVE, AUDIO, SPAWN, AREA_CLEAR, WAIT, SAFETY_ON, SAFETY_OFF }

public class GameEvent
{
    public EEventType EventType { get; private set; }

    public bool Async { get; private set; }

    public object[] Parameters { get; private set; }

    public GameEvent(EEventType eventtype, bool async, object[] parameters)
    {
        EventType = eventtype;
        Async = async;
        Parameters = parameters;
    }
}
