public interface ICommand<T>
{
    //// �p�����[�^���擾����
    //abstract public T GetParameter();
    //// �p�����[�^��ݒ肷��
    //abstract public void SetParameter(T parameter);
    // ���s����
    abstract public void Execute();

    // abstract public IEnumerator Enumerator();

    abstract public bool Enable();
}