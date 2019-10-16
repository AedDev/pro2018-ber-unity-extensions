using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IService<T>
{
    bool Create(T model);
    T Get(int id);
    List<T> GetAll();
    List<T> Find(Predicate<T> predicate);
    bool Update(T model);
    bool Delete(T model);
}
