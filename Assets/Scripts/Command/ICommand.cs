public interface ICommand<T>
{
    //// パラメータを取得する
    //abstract public T GetParameter();
    //// パラメータを設定する
    //abstract public void SetParameter(T parameter);
    // 実行する
    abstract public void Execute();

    // abstract public IEnumerator Enumerator();

    abstract public bool Enable();
}