using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T>
{
    public delegate T FactoryMethod();
    public delegate void TCallBack(T poolObj);

    public List<T> currentStock;
    int _initialStock;
    FactoryMethod _factoryMethod;
    bool _isDynamic;
    TCallBack _turnOnCallback;
    TCallBack _turnOffCallback;

    public ObjectPool(FactoryMethod factory, TCallBack turnOnCallback, TCallBack turnOffCallback, int initialStock = 0, bool isDynamic = true)
    {
        _factoryMethod = factory;
        _turnOnCallback = turnOnCallback;
        _turnOffCallback = turnOffCallback;
        _initialStock = initialStock;
        _isDynamic = isDynamic;

        currentStock = new List<T>();

        for (int i = 0; i < _initialStock; i++)
        {
            var objPool = _factoryMethod();
            _turnOffCallback(objPool);
            _turnOnCallback(objPool);
        }
    }

    public T GetObject()
    {
        var result = default(T);
        if(currentStock.Count > 0)
        {
            result = currentStock[0];
            currentStock.Remove(result);
        }
        else if(_isDynamic)
            result = _factoryMethod();

        _turnOnCallback(result);
        return result;
    }

    public void ReturnObject(T objPool)
    {
        _turnOffCallback(objPool);
        currentStock.Add(objPool);
    }
}
