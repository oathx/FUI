namespace FUI.UGUI
{
    /// <summary>
    /// 可以加载资源的值转换器
    /// </summary>
    /// <typeparam name="TValueType">值类型</typeparam>
    /// <typeparam name="TAssetType">资源类型</typeparam>
    public abstract class AssetLoadableValueConverter<TValueType, TAssetType> : ValueConverter<TValueType, TAssetType>, IAssetLoadable where TAssetType : UnityEngine.Object
    {
        protected IAssetLoader AssetLoader { get; private set; }
        public void SetAssetLoader(IAssetLoader assetLoader)
        {
            this.AssetLoader = assetLoader;
        }
    }
}
