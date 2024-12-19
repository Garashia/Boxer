public class Commander<T>
{
    private ICommand<T> m_command = null;
    private T m_parameter;

    // コマンドを取得する
    public ICommand<T> GetCommand()
    {
        return m_command;
    }

    // コマンドを設定する
    public void SetCommand(ICommand<T> command)
    {
        m_command = command;
    }

    // パラメータを取得する
    public T GetParameter()
    { return m_parameter; }

    // パラメータを設定する
    public void SetParameter(T parameter)
    { m_parameter = parameter; }

    // 実行中か確認
    public virtual bool Execute()
    {
        if (GetCommand() == null) return false;
        m_command.Execute();
        if (m_command.IsCompleted())
        {
            m_command = null;
        }
        return true;
    }
}