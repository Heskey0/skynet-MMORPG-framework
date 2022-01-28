//code by 赫斯基皇
//https://space.bilibili.com/455965619
//https://github.com/Heskey0

using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using XLua;

#if UNITY_EDITOR
using UnityEditor;
#endif


    [LuaCallCSharp]
    public class Util {
        
        public static bool FloatEqual(float a,float b)
        {
            return Mathf.Abs(a - b) < 0.001f;
        }

        public static float Distance2_5D(Vector3 a,Vector3 b)
        {
            return Vector3.Distance(
                new Vector3(a.x, 0, a.z)
                , new Vector3(b.x, 0, b.z));
        }
        
        private static List<string> luaPaths = new List<string>();

        public static int Int(object o) {
            return Convert.ToInt32(o);
        }

        public static float Float(object o) {
            return (float)Math.Round(Convert.ToSingle(o), 2);
        }

        public static long Long(object o) {
            return Convert.ToInt64(o);
        }

        public static int Random(int min, int max) {
            return UnityEngine.Random.Range(min, max);
        }

        public static float Random(float min, float max) {
            return UnityEngine.Random.Range(min, max);
        }

        public static string Uid(string uid) {
            int position = uid.LastIndexOf('_');
            return uid.Remove(0, position + 1);
        }

        public static long GetTime() {
            TimeSpan ts = new TimeSpan(DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks);
            return (long)ts.TotalMilliseconds;
        }

        /// <summary>
        /// 搜索子物体组件-GameObject版
        /// </summary>
        public static T Get<T>(GameObject go, string subnode) where T : Component {
            if (go != null) {
                Transform sub = go.transform.Find(subnode);
                if (sub != null) return sub.GetComponent<T>();
            }
            return null;
        }

        /// <summary>
        /// 搜索子物体组件-Transform版
        /// </summary>
        public static T Get<T>(Transform go, string subnode) where T : Component {
            if (go != null) {
                Transform sub = go.Find(subnode);
                if (sub != null) return sub.GetComponent<T>();
            }
            return null;
        }

        /// <summary>
        /// 搜索子物体组件-Component版
        /// </summary>
        public static T Get<T>(Component go, string subnode) where T : Component {
            return go.transform.Find(subnode).GetComponent<T>();
        }

        /// <summary>
        /// 添加组件
        /// </summary>
        public static T Add<T>(GameObject go) where T : Component {
            if (go != null) {
                T[] ts = go.GetComponents<T>();
                for (int i = 0; i < ts.Length; i++) {
                    if (ts[i] != null) GameObject.Destroy(ts[i]);
                }
                return go.gameObject.AddComponent<T>();
            }
            return null;
        }

        /// <summary>
        /// 添加组件
        /// </summary>
        public static T Add<T>(Transform go) where T : Component {
            return Add<T>(go.gameObject);
        }

        /// <summary>
        /// 查找子对象
        /// </summary>
        public static GameObject Child(GameObject go, string subnode) {
            return Child(go.transform, subnode);
        }

        /// <summary>
        /// 查找子对象
        /// </summary>
        public static GameObject Child(Transform go, string subnode) {
            Transform tran = go.Find(subnode);
            if (tran == null) return null;
            return tran.gameObject;
        }

        /// <summary>
        /// 取平级对象
        /// </summary>
        public static GameObject Peer(GameObject go, string subnode) {
            return Peer(go.transform, subnode);
        }

        /// <summary>
        /// 取平级对象
        /// </summary>
        public static GameObject Peer(Transform go, string subnode) {
            Transform tran = go.parent.Find(subnode);
            if (tran == null) return null;
            return tran.gameObject;
        }

        /// <summary>
        /// 计算字符串的MD5值
        /// </summary>
        public static string md5(string source) {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(source);
            byte[] md5Data = md5.ComputeHash(data, 0, data.Length);
            md5.Clear();

            string destString = "";
            for (int i = 0; i < md5Data.Length; i++) {
                destString += System.Convert.ToString(md5Data[i], 16).PadLeft(2, '0');
            }
            destString = destString.PadLeft(32, '0');
            return destString;
        }

        /// <summary>
        /// 计算文件的MD5值
        /// </summary>
        public static string md5file(string file) {
            try {
                FileStream fs = new FileStream(file, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(fs);
                fs.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++) {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            } catch (Exception ex) {
                throw new Exception("md5file() fail, error:" + ex.Message);
            }
        }

        /// <summary>
        /// 清除所有子节点
        /// </summary>
        public static void ClearChild(Transform go) {
            if (go == null) return;
            for (int i = go.childCount - 1; i >= 0; i--) {
                GameObject.Destroy(go.GetChild(i).gameObject);
            }
        }

        /// <summary>
        /// 清理内存
        /// </summary>
        // public static void ClearMemory() {
        //     GC.Collect(); Resources.UnloadUnusedAssets();
        //     LuaManager mgr = AppFacade.Instance.GetManager<LuaManager>(ManagerName.Lua);
        //     if (mgr != null) mgr.LuaGC();
        // }


        /// <summary>
        /// 取得行文本
        /// </summary>
        public static string GetFileText(string path) {
            return File.ReadAllText(path);
        }

        public static byte[] GetFileBytes(string inFile)
        {
            try
            {
                if (string.IsNullOrEmpty(inFile))
                {
                    return null;
                }

                if (!File.Exists(inFile))
                {
                    return null;
                }
                // File.SetAttributes(inFile, FileAttributes.Normal);
                return File.ReadAllBytes(inFile);
            }
            catch (System.Exception ex)
            {
                Debug.LogError(string.Format("SafeReadAllBytes failed! path = {0} with err = {1}", inFile, ex.Message));
                return null;
            }
        }

        public static string GetFileNamesInFolder(string path, string separate=",")
        {
            System.IO.DirectoryInfo direction = new System.IO.DirectoryInfo(path);
            FileInfo[] files = direction.GetFiles("*.lua", SearchOption.AllDirectories);
            StringBuilder strbuf = new StringBuilder();
            foreach (var file in files)
            {
                strbuf.Append(Path.GetFileNameWithoutExtension(file.Name) + separate);
            }
            strbuf.Remove(strbuf.Length - 1, 1);
            return strbuf.ToString();
        }

        /// <summary>
        /// 网络可用
        /// </summary>
        public static bool NetAvailable {
            get {
                return Application.internetReachability != NetworkReachability.NotReachable;
            }
        }

        /// <summary>
        /// 是否是无线
        /// </summary>
        public static bool IsWifi {
            get {
                return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
            }
        }




        public static void Log(string str) {
            Debug.Log(str);
        }

        public static void LogWarning(string str) {
            Debug.LogWarning(str);
        }

        public static void LogError(string str) {
            Debug.LogError(str);
        }



        public static void ThrowLuaException(string error, Exception exception = null, int skip = 1)
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
            {
                Debug.LogWarning(error);
            }
            else
            {
                Debug.LogError(error);
            }
        }

        public static GameObject InstantiateObject(GameObject prefab)
        {
            try
            {
                return GameObject.Instantiate(prefab);
            }
            catch (Exception e)
            {
                Util.ThrowLuaException(e.Message);
                return null;
            }
        }
        
        public static ushort SwapUInt16(ushort n)
        {
            //Debug.Log("SwapUInt16() BitConverter.IsLittleEndian : " + BitConverter.IsLittleEndian.ToCString());
            if (BitConverter.IsLittleEndian)
                return (ushort)(((n & 0xff) << 8) | ((n >> 8) & 0xff));
            else
                return n;
        }
        public static short SwapInt16(short n)  
        {  
            if (BitConverter.IsLittleEndian)
                return (short)(((n & 0xff) << 8) | ((n >> 8) & 0xff));  
            else
                return n;
        }  
        /// <summary>
        /// 大端字序与小端字序切换
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int SwapInt32(int n)  
        {  
            if (BitConverter.IsLittleEndian)
                return (int)(((SwapInt16((short)n) & 0xffff) << 0x10) |  
                    (SwapInt16((short)(n >> 0x10)) & 0xffff));  
            else
                return n;
        }  

        public static uint SwapUInt32(uint n)  
        {  
            return (uint)(((SwapUInt16((ushort)n) & 0xffff) << 0x10) |  
                (SwapUInt16((ushort)(n >> 0x10)) & 0xffff));  
        }  

        public static long SwapInt64(long n)  
        {  
            return (long)(((SwapInt32((int)n) & 0xffffffffL) << 0x20) |  
                (SwapInt32((int)(n >> 0x20)) & 0xffffffffL));  
        }  

        public static ulong SwapUInt64(ulong n)  
        {  
            return (ulong)(((SwapUInt32((uint)n) & 0xffffffffL) << 0x20) |  
                (SwapUInt32((uint)(n >> 0x20)) & 0xffffffffL));  
        }

        public static int FindMaterial(Material[] materials, string materialName)
        {
            // var materials = renderer.materials;
            int materialIndex = -1;
            for (int i = 0; i < materials.Length; i++)
            {
                // Debug.Log("material name :"+materials[i].name+"  "+materials[i].name.IndexOf(materialName));
                if (-1 != materials[i].name.IndexOf(materialName))
                {
                    materialIndex = i;
                    break;
                }
            }
            return materialIndex;
        }
    }
