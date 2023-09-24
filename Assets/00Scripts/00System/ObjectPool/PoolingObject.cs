public interface PoolingObject
{
    // 최초 1회 실행
    public abstract void Initialize(OBJPoolParent poolManager);
    public abstract void ActiveObj();
    public abstract void DeactiveObj();
}

