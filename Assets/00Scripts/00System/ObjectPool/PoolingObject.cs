public interface PoolingObject
{
    // ���� 1ȸ ����
    public abstract void Initialize(OBJPoolParent poolManager);
    public abstract void ActiveObj();
    public abstract void DeactiveObj();
}

