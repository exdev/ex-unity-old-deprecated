// ======================================================================================
// File         : exPool.cs
// Author       : Wu Jie 
// Last Change  : 02/19/2012 | 21:21:21 PM | Sunday,February
// Description  : 
// ======================================================================================

///////////////////////////////////////////////////////////////////////////////
// usings
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

///////////////////////////////////////////////////////////////////////////////
// exPool
///////////////////////////////////////////////////////////////////////////////

[System.Serializable]
public class exPool<T> where T : class, new() {
 
    [System.NonSerialized] public int size = 0;
    [System.NonSerialized] public int idx = 0;
    [System.NonSerialized] public T[] data;
    [System.NonSerialized] public T[] initData;
 
    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 
 
    public void Init ( int _size ) {
        size = _size;
        initData = new T[size]; 
        data = new T[size];
        for ( int i = 0; i < size; ++i ) {
            T obj = new T();
            initData[i] = obj;
            data[i] = initData[i];
        }
        idx = size - 1;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Reset () {
        for ( int i = 0; i < size; ++i ) {
            data[i] = initData[i];
        }
        idx = size - 1;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public T Request ()  {
        if ( idx < 0 ) {
            Debug.LogError ("Error: the pool do not have enough free item.");
            return null;
        }
 
        T result = data[idx];
        --idx; 
        return result;
    }
 
    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 
 
    public void Return ( T _item ) {
        ++idx;
        data[idx] = _item;
    }
}

///////////////////////////////////////////////////////////////////////////////
// exComponentPool
///////////////////////////////////////////////////////////////////////////////

[System.Serializable]
public class exComponentPool<T> where T : Component {
 
    [System.NonSerialized] public GameObject prefab;
    [System.NonSerialized] public int size = 0;
    [System.NonSerialized] public int idx = 0;
    [System.NonSerialized] public T[] data;
    [System.NonSerialized] public T[] initData;
 
    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 
 
    public void Init ( GameObject _prefab, int _size ) {
        prefab = _prefab;
        size = _size;
        initData = new T[size]; 
        data = new T[size];
        if ( prefab != null ) {
            for ( int i = 0; i < size; ++i ) {
                GameObject obj = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
                initData[i] = obj.GetComponent<T>();
                data[i] = initData[i];
            }
        }
        idx = size - 1;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Reset () {
        for ( int i = 0; i < size; ++i ) {
            data[i] = initData[i];
        }
        idx = size - 1;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public T Request ()  {
        if ( idx < 0 ) {
            Debug.LogError ("Error: the pool do not have enough free item.");
            return null;
        }
 
        T result = data[idx];
        --idx; 
        return result;
    }
 
    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 
 
    public T Request ( Vector2 _pos )  {
        T result = Request();
        result.transform.position = new Vector3( _pos.x, _pos.y, result.transform.position.z );
        return result;
    }
 
    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 
 
    public T Request ( Vector3 _pos, Quaternion _rot )  {
        T result = Request();
        result.transform.position = _pos;
        result.transform.rotation = _rot;
        return result;
    }
 
    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 
 
    public void Return ( T _item ) {
        ++idx;
        data[idx] = _item;
    }
}

///////////////////////////////////////////////////////////////////////////////
// exGameObjectPool
///////////////////////////////////////////////////////////////////////////////

[System.Serializable]
public class exGameObjectPool {
 
    public GameObject prefab;
    public int size = 0;
    [System.NonSerialized] public int idx = 0;
    [System.NonSerialized] public GameObject[] data;
    [System.NonSerialized] public GameObject[] initData;

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 
 
    public void Init () {
        Init ( prefab, size );
    }
 
    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 
 
    public void Init ( GameObject _prefab, int _size ) {
        prefab = _prefab;
        size = _size;
        initData = new GameObject[size]; 
        data = new GameObject[size];
        if ( prefab != null ) {
            for ( int i = 0; i < size; ++i ) {
                GameObject obj = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
                initData[i] = obj;
                data[i] = initData[i];
            }
        }
        idx = size - 1;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Reset () {
        for ( int i = 0; i < size; ++i ) {
            data[i] = initData[i];
        }
        idx = size - 1;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public GameObject Request ()  {
        if ( idx < 0 ) {
            Debug.LogError ("Error: the pool do not have enough free item.");
            return null;
        }
 
        GameObject result = data[idx];
        --idx; 
        return result;
    }
 
    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 
 
    public GameObject Request ( Vector2 _pos )  {
        GameObject result = Request();
        result.transform.position = new Vector3( _pos.x, _pos.y, result.transform.position.z );
        return result;
    }
 
    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 
 
    public GameObject Request ( Vector3 _pos, Quaternion _rot )  {
        GameObject result = Request();
        result.transform.position = _pos;
        result.transform.rotation = _rot;
        return result;
    }
 
    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 
 
    public T Request<T> () where T : MonoBehaviour  {
        GameObject go = Request ();
        if ( go )
            return go.GetComponent<T>();
        return null;
    }
 
    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 
 
    public T Request<T> ( Vector2 _pos ) where T : MonoBehaviour  {
        GameObject go = Request (_pos);
        if ( go )
            return go.GetComponent<T>();
        return null;
    }
 
    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 
 
    public T Request<T> ( Vector3 _pos, Quaternion _rot ) where T : MonoBehaviour  {
        GameObject go = Request ( _pos, _rot );
        if ( go )
            return go.GetComponent<T>();
        return null;
    }
 
    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 
 
    public void Return ( GameObject _item ) {
        ++idx;
        // _item.gameObject.SetActiveRecursively(false);
        data[idx] = _item;
    }
}
