using UnityEngine;

namespace UIFlow
{
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        /// 保存实际的对象
        protected static T _instance;

        /// 对外提供访问接口
        /// 即我们可以使用Singleton<T>.Instance来访问_instance，而当_instance不存在时，我们会对其初始化
        /// 注意，这不会在程序运行的时候初始化这个类，只有我们访问时，才可能进行初始化
        public static T Instance
        {
            get
            {
                // 如果不存在实例，尝试进行初始化
                if (_instance == null)
                {
                    //从场景中找T脚本的对象，这会找到所有T对象，并返回最后一个加入场景中的那个
                    _instance = FindObjectOfType<T>();

                    // 如果T对象的数量比1大，那很明显场景里出现了多个单例类，必然是有错误的
                    if (FindObjectsOfType<T>().Length > 1)
                    {
                        Debug.LogError("场景中的单例脚本数量 > 1:" + _instance.GetType().ToString());
                        return _instance;
                    }

                    // 场景中找不到的情况，那就要进行初始化了
                    if (_instance == null)
                    {

                        string instanceName = typeof(T).Name;
                        GameObject instanceGO = GameObject.Find(instanceName);
                        // 我们会生成一个空的gameobject并挂在对应脚本，如果场景里有同名的物体，自然就会有问题 
                        if (instanceGO == null)
                        {
                            instanceGO = new GameObject(instanceName);
                            DontDestroyOnLoad(instanceGO);
                            _instance = instanceGO.AddComponent<T>();
                            // 删除这一行代码并运行游戏可以很明显的发现变化，他的作用是切换场景的时候不销毁指定对象
                            DontDestroyOnLoad(_instance);
                        }
                        else
                        {
                            //场景中已存在同名游戏物体时就打印提示
                            Debug.LogError("场景中已存在单例脚本所挂载的游戏物体:" + instanceGO.name);
                        }
                    }
                }

                return _instance;
            }
        }

        void OnDestroy()
        {
            _instance = null;
        }
    }
}