public interface ICommand
{
    // ���s����
    abstract public void Execute();

    // ���s�����m�F
    abstract public bool IsCompleted();
}