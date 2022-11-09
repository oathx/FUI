using System.Threading.Tasks;

using UnityEngine;

namespace FUI
{
    public interface IAssetLoader
    {
        T Load<T>(string path) where T : Object;
        Task<T> LoadAsync<T>(string path) where T : Object;
    }
}