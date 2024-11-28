public class Commander<T>
{
    private ICommand<T> m_command = null;
    private T m_parameter;

    // �R�}���h���擾����
    public ICommand<T> GetCommand()
    {
        return m_command;
    }

    // �R�}���h��ݒ肷��
    public void SetCommand(ICommand<T> command)
    {
        m_command = command;
    }

    // �p�����[�^���擾����
    public T GetParameter()
    { return m_parameter; }

    // �p�����[�^��ݒ肷��
    public void SetParameter(T parameter)
    { m_parameter = parameter; }

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