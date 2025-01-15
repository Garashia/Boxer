using CommandList = EventGridMicroCommander.EventGridCommand;

public struct EventParameter
{
}

public class EventGridMicroCommander :
    MicroCommander
{
    public class EventGridCommand : Command
    { }
}

public class EventGridEncounterCommand : CommandList
{
}