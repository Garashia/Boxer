public interface ICommand<T>
{
    // ���s����
    abstract public void Execute();


    // ���s�����m�F
    abstract public bool IsCompleted();
}