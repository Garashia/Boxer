public interface ICommand<T>
{
    // 実行する
    abstract public void Execute();


    // 実行中か確認
    abstract public bool IsCompleted();
}