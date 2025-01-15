public class Commander
{
    private ICommand m_command = null;

    // コマンドを取得する
    public ICommand GetCommand()
    {
        return m_command;
    }

    // コマンドを設定する
    public void SetCommand(ICommand command)
    {
        m_command = command;
    }

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