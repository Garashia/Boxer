using CommandList = EventGridMicroCommander.EventGridCommand;

public struct EventParameter
{

}
public class EventGridMicroCommander :
    MicroCommander<EventParameter, CommandList>
{
    public class EventGridCommand : Command { }
}

public class EventGridEncounterCommand : CommandList
{

}

