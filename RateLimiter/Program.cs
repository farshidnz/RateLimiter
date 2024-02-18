// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");



public class RateLimit
{

    private long _lastCheck;
    private readonly int _numberOfRequest;
    private double _allowedRequests;
    private readonly double _ratePer;

    public RateLimit(int numberOfRequest)
    {
        _numberOfRequest = numberOfRequest;
        _ratePer = numberOfRequest * (1/10);
        _allowedRequests = _numberOfRequest;
        _lastCheck = DateTime.Now.Millisecond;
    }
    public bool ShouldAdd()
    {
        var now = DateTime.Now.Millisecond;
        var rateDifference = now - _lastCheck;
        _lastCheck = now;
        
        _allowedRequests += rateDifference * _ratePer;

        if (_allowedRequests > _numberOfRequest)
        {
            _allowedRequests = _numberOfRequest;
        }

        if (_allowedRequests < 1)
        {
            return false;
        }
        else
        {
            _allowedRequests--;
            return true;
        }
        
    }
}

public class RateLimiter2
{
    private readonly int _maxRequests;
    private readonly int _intervalMs;
    private int _tokens;
    private DateTime _lastRefillTime;

    public RateLimiter2(int maxRequests, TimeSpan interval)
    {
        _maxRequests = maxRequests;
        _intervalMs = (int)interval.TotalMilliseconds;
        _tokens = maxRequests;
        _lastRefillTime = DateTime.UtcNow;
    }

    public async Task WaitAsync(CancellationToken cancellationToken = default)
    {
        while (true)
        {
            int tokens = Interlocked.CompareExchange(ref _tokens, 0, 0);
            DateTime now = DateTime.UtcNow;
            int refillAmount = (int)((now - _lastRefillTime).TotalMilliseconds / _intervalMs) * _maxRequests;
            int newTokens = Math.Min(tokens + refillAmount, _maxRequests);

            if (newTokens == 0)
            {
                int delayMs = (int)((_lastRefillTime.AddMilliseconds(_intervalMs) - now).TotalMilliseconds);
                await Task.Delay(delayMs, cancellationToken);
            }
            else if (Interlocked.CompareExchange(ref _tokens, newTokens - 1, tokens) == tokens)
            {
                _lastRefillTime = now;
                break;
            }
        }
    }
}


public class RateLimiter3
{
    private readonly int _maxRequests;
    private readonly int _intervalMs;
    private int _tokens;
    private DateTime _lastRefillTime;
    private readonly object _lock = new object();

    public RateLimiter3(int maxRequests, TimeSpan interval)
    {
        _maxRequests = maxRequests;
        _intervalMs = (int)interval.TotalMilliseconds;
        _tokens = maxRequests;
        _lastRefillTime = DateTime.UtcNow;
    }

    public async Task WaitAsync(CancellationToken cancellationToken = default)
    {
        while (true)
        {
            int tokens;
            lock (_lock)
            {
                tokens = _tokens;
                DateTime now = DateTime.UtcNow;
                int refillAmount = (int)((now - _lastRefillTime).TotalMilliseconds / _intervalMs) * _maxRequests;
                int newTokens = Math.Min(tokens + refillAmount, _maxRequests);
                _tokens = newTokens;
                _lastRefillTime = now;
            }

            if (tokens == 0)
            {
                int delayMs = (int)(_lastRefillTime.AddMilliseconds(_intervalMs) - DateTime.UtcNow).TotalMilliseconds;
                await Task.Delay(delayMs, cancellationToken);
            }
            else
            {
                lock (_lock)
                {
                    if (_tokens > 0)
                    {
                        _tokens--;
                        break;
                    }
                }
            }
        }
    }
}


public class RateLimiter4
{
    private readonly int _maxRequests;
    private readonly int _intervalMs;
    private int _tokens;
    private DateTime _lastRefillTime;
    private readonly object _lock = new object();

    //interval refers to the interval between refills of the token bucket. It is specified in milliseconds and determines how often the token bucket is refilled with tokens.
    public RateLimiter4(int maxRequests, TimeSpan interval)
    {
        _maxRequests = maxRequests;
        _intervalMs = (int)interval.TotalMilliseconds;
        _tokens = maxRequests;
        _lastRefillTime = DateTime.UtcNow;
    }

    // refillAmount is the number of tokens that should be refilled since the last refill time.
    // delayMs is the number of milliseconds we need to wait before attempting to consume another token.
    public void Wait()
    {
        while (true)
        {
            int tokens;
            lock (_lock)
            {
                tokens = _tokens;
                DateTime now = DateTime.UtcNow;
                int refillAmount = (int)((now - _lastRefillTime).TotalMilliseconds / _intervalMs) * _maxRequests;
                int newTokens = Math.Min(tokens + refillAmount, _maxRequests);
                _tokens = newTokens;
                _lastRefillTime = now;
            }

            if (tokens == 0)
            {
                int delayMs = (int)(_lastRefillTime.AddMilliseconds(_intervalMs) - DateTime.UtcNow).TotalMilliseconds;
                if (delayMs > 0)
                {
                    Thread.Sleep(delayMs);
                }
            }
            else
            {
                lock (_lock)
                {
                    if (_tokens > 0)
                    {
                        _tokens--;
                        break;
                    }
                }
            }
        }
    }
}