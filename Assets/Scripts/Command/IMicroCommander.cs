using System.Collections.Generic;

public class IMicroCommander<T, Com> : ICommander<T>
    where Com : IMicroCommander<T, Com>.Command
{
    public class Command : ICommand<T>
    {
        private T m_parameter;

        public T Parameter
        {
            get { return m_parameter; }
            set { m_parameter = value; }
        }

        public virtual void Execute()
        { }

        public virtual bool Enable()
        { return true; }
    };

    private Queue<Com> m_microCommand = new Queue<Com>();

    // マクロコマンド数を取得する
    private int GetMacroCommandNumber()
    { return m_microCommand.Count; }

    public void AddCommand(Com command, T parameter)
    {
        command.Parameter = (parameter);
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
}