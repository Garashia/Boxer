public class Commander
{
    private ICommand m_command = null;

    // �R�}���h���擾����
    public ICommand GetCommand()
    {
        return m_command;
    }

    // �R�}���h��ݒ肷��
    public void SetCommand(ICommand command)
    {
        m_command = command;
    }

    // ���s�����m�F
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