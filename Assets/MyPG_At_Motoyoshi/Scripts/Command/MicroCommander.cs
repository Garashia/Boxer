using System.Collections.Generic;

public class MicroCommander : Commander
{
    public class Command : ICommand
    {

        public virtual void Execute()
        { }

        public virtual bool IsCompleted()
        { return true; }
    };

    private Queue<Command> m_microCommand = new Queue<Command>();

    // マクロコマンド数を取得する
    private int GetMacroCommandNumber()
    { return m_microCommand.Count; }

    public void AddCommand(Command command)
    {
        m_microCommand.Enqueue(command);
    }

    public override bool Execute()
    {
        int index = GetMacroCommandNumber();
        if (GetCommand() == null && index != 0)
        {
            SetCommand(m_microCommand.Dequeue());
        }
        return base.Execute();
    }

    public void Clear()
    {
        m_microCommand.Clear();
    }

}