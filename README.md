# Results

The Result pattern to avoid the usage of exceptions in making decisions.

## Objectives

This library doesn't have the intent to replace the exceptions usage, but sometimes we need to implement a method that can return more than one type of response, and based on this response we need to modify the application flow to reflect system specifications.

It's very common to see codes using exceptions as control flow to solve that, but as we know it's an anti pattern and should be avoided.

Example:

```csharp
public class Repository
{
    private readonly ICache _cache;

    public Repository(ICache cache)
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    public ISomething GetSomething(int id, bool loadFromCache = true)
    {
        ISomething item;
        if (loadFromCache)
        {
            item = _cache.Load();
            if (item.NeedToBeReloaded())
            {
                throw new EntityMustBeLoadFromDatabaseException(id);
            }

            return item;
        }

        item = GetSomethingFromDatabase(id);

        _cache.Set(id, item);
        return item;
    }

    private ISomething GetSomethingFromDatabase(int id)
    {
        return $"SELECT * FROM Something WHERE Id = {id}".Query<Something>();
    }
}
    
public class Service
{
    private readonly Repository _repository;
    private readonly ILogger<Service> _logger;

    public Service(Repository repository, ILogger<Service> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public ISomething GetSomething(int id)
    {
        try
        {
            return _repository.GetSomething(id);
        }
        catch (EntityMustBeLoadFromDatabaseException _)
        {
            return _repository.GetSomething(id, false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            throw;
        }
    }
}
```

## How to use

```csharp
    public class EntityMustBeLoadFromDatabase : Fail
    {
        public EntityMustBeLoadFromDatabase(int id)
            : base($"Entity must be reload from database. Id: {id}")
        {
            Id = id;
        }

        public int Id { get; }
    }

    public class Repository
    {
        private readonly ICache _cache;

        public Repository(ICache cache)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public Result<ISomething> GetSomething(int id, bool loadFromCache = true)
        {
            Something item;
            if (loadFromCache)
            {
                item = _cache.Load();
                if (item.NeedToBeReloaded())
                {
                    return new EntityMustBeLoadFromDatabase(id);
                }

                return item;
            }

            item = GetSomethingFromDatabase(id);

            _cache.Set(id, item);
            return item;
        }

        private Something GetSomethingFromDatabase(int id)
        {
            return $"SELECT * FROM Something WHERE Id = {id}".Query<Something>();
        }
    }

    
    public class Service
    {
        private readonly Repository _repository;
        private readonly ILogger<Service> _logger;

        public Service(Repository repository, ILogger<Service> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public ISomething GetSomething(int id)
        {
            try
            {
                var result = _repository.GetSomething(id);
                return result
                    .Switch<ISomething>()
                    .Case<Success<ISomething>>(success => success.GetResult())
                    .Case<EntityMustBeLoadFromDatabase>(reload =>
                    {
                        var something = _repository.GetSomething(reload.Id, false);
                        if (something.IsFail())
                        {
                            throw new Exception($"Cannot get the entity with id: {id}.");
                        }

                        return something.GetResult();
                    })
                    .Default(another => throw new Exception("You can do a type check and process as you wish."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }
    }
```


