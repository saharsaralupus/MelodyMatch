﻿using System.Threading.Tasks;

namespace Orquesta.WEB.Repositories
{
    public interface IRepository
    {
        Task<HttpResponseWrapper<T>> GetAsync<T>(string url);

        Task<HttpResponseWrapper<object>> PostAsync<T>(string url, T model);
        Task<HttpResponseWrapper<TResponse>> PostAsync<T, TResponse>(string url, T model);
        //Task<HttpResponseWrapper<TActionResponse>> PostAsync<T, TActionResponse>(string url, T model);
        Task<HttpResponseWrapper<object>> DeleteAsync<T>(string url);
        Task<HttpResponseWrapper<object>> DeleteAsync(string url);
        Task<HttpResponseWrapper<object>> PutAsync<T>(string url, T model);

        Task<HttpResponseWrapper<TActionResponse>> PutAsync<T, TActionResponse>(string url, T model);

        Task<HttpResponseWrapper<object>> Get(string url);

    }
}