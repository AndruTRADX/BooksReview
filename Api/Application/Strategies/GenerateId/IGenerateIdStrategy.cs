using System;

namespace Application.Strategies.GenerateId;

public interface IGenerateIdStrategy<T> where T : class
{
    string GenerateId(T entity);
}
