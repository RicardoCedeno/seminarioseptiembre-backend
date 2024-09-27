using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;


Seminario seminario = new();
await seminario.EjecutarMetodosAsync();
await seminario.EjecutarMetodosParaleloAsync();
await seminario.EjecutarMetodosAny();
seminario.GuardarTokenEnCache("clave123");
Console.WriteLine(seminario.ObtenerTokenDeCache());
public class Seminario
{
    private readonly IMemoryCache _memoryCache;
    public Seminario()
    {
        _memoryCache = new MemoryCache(new MemoryCacheOptions());
    }
    public async Task<int> ContarHasta10000()
    {
        await Task.Delay(100);
        for (int i = 0; i <= 10000; i++)
        {
            //Console.WriteLine(i);
        }
        return 1;
    }

    public async Task<int> EncontrarNumerosPares()
    {
        await Task.Delay(100);
        for (int i = 0; i <= 10000; i++)
        {
            bool esPar = false;
            if (i % 2 == 0) esPar = true;
            //Console.WriteLine($"{i} es par: {esPar}");
        }
        return 1;
    }

    public async Task EjecutarMetodosAsync()
    {
        Stopwatch stopWatch = Stopwatch.StartNew();
        int tarea1 = await ContarHasta10000();
        int tarea2 = await EncontrarNumerosPares();
        stopWatch.Stop();
        long requiredTime = stopWatch.ElapsedMilliseconds;
        Console.WriteLine($"el tiempo requerido fue de {requiredTime} ms");
    }

    public async Task EjecutarMetodosParaleloAsync()
    {
        Stopwatch stopWatch = Stopwatch.StartNew();
        await Task.WhenAll(ContarHasta10000(), EncontrarNumerosPares());
        stopWatch.Stop();
        long requiredTime = stopWatch.ElapsedMilliseconds;
        Console.WriteLine($"el tiempo requerido fue de {requiredTime} ms");
    }

    public async Task EjecutarMetodosAny()
    {
        Stopwatch stopWatch = Stopwatch.StartNew();
        await Task.WhenAny(ContarHasta10000(), EncontrarNumerosPares());
        stopWatch.Stop();
        long requiredTime = stopWatch.ElapsedMilliseconds;
        Console.WriteLine($"el tiempo requerido fue de {requiredTime} ms");
    }

    public void GuardarTokenEnCache(string token)
    {
        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        string cacheKey = "claveMuySegura";
        var cacheOptions = new MemoryCacheEntryOptions().
            SetSlidingExpiration(TimeSpan.FromMinutes(5));
        _memoryCache.Set(cacheKey, token, cacheOptions);
    }

    public string ObtenerTokenDeCache()
    {
        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        string cacheKey = "claveMuySegura";

        if (_memoryCache.TryGetValue(cacheKey, out string token))
        {
            return token;
        }
        else return "No se obtuvo token. Por favor vuelva a inicia sesión";
    }
}


//