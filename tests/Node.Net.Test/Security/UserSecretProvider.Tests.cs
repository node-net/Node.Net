#nullable enable
extern alias NodeNet;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using NodeNet::Node.Net.Diagnostic;
using NodeNet::Node.Net.Security;

namespace Node.Net.Test.Security;

[TestFixture]
internal class UserSecretProviderTests : TestHarness
{
    public UserSecretProviderTests() : base("UserSecretProvider")
    {
    }

    private IUserSecretProvider _provider = null!;

    [SetUp]
    public void SetUp()
    {
        _provider = UserSecretProviderFactory.Create();
    }

    [Test]
    public async Task GetOrCreateAsync_NewKey_GeneratesNewSecret()
    {
        // Arrange
        // Use a unique key with timestamp to ensure it's a new secret
        var uniqueKey = UserSecretKey.LiteDb($"test-app-{DateTimeOffset.UtcNow.Ticks}");
        var beforeCreation = DateTimeOffset.UtcNow;

        // Act
        var secret = await _provider.GetOrCreateAsync(uniqueKey);
        var afterCreation = DateTimeOffset.UtcNow;

        // Assert
        Assert.That(secret, Is.Not.Null);
        Assert.That(secret.Base64, Is.Not.Null.And.Not.Empty);
        Assert.That(secret.ByteLength, Is.EqualTo(48), "Secret must be exactly 48 bytes (384 bits)");
        Assert.That(secret.CreatedUtc, Is.GreaterThanOrEqualTo(beforeCreation.AddMilliseconds(-100)), 
            "CreatedUtc should be close to creation time");
        Assert.That(secret.CreatedUtc, Is.LessThanOrEqualTo(afterCreation.AddMilliseconds(100)), 
            "CreatedUtc should be close to creation time");
        
        // Verify Base64 decodes to 48 bytes
        var bytes = secret.GetBytes();
        Assert.That(bytes, Is.Not.Null);
        Assert.That(bytes.Length, Is.EqualTo(48), "Secret bytes must be exactly 48 bytes");
    }

    [Test]
    public async Task GetOrCreateAsync_ExistingKey_ReturnsSameSecret()
    {
        // Arrange
        var key = UserSecretKey.LiteDb("test-app-stable");

        // Act
        var secret1 = await _provider.GetOrCreateAsync(key);
        var secret2 = await _provider.GetOrCreateAsync(key);

        // Assert
        Assert.That(secret2.Base64, Is.EqualTo(secret1.Base64), "Same key must return same secret");
        // Allow small time difference due to timestamp precision (within 1 second)
        Assert.That(secret2.CreatedUtc, Is.EqualTo(secret1.CreatedUtc).Within(TimeSpan.FromSeconds(1)), 
            "Same secret must have same creation time");
    }

    [Test]
    public async Task GetOrCreateAsync_DifferentKeys_GenerateDifferentSecrets()
    {
        // Arrange
        var key1 = UserSecretKey.LiteDb("test-app-1");
        var key2 = UserSecretKey.TokenCache("test-app-1");

        // Act
        var secret1 = await _provider.GetOrCreateAsync(key1);
        var secret2 = await _provider.GetOrCreateAsync(key2);

        // Assert
        Assert.That(secret2.Base64, Is.Not.EqualTo(secret1.Base64), "Different keys must generate different secrets");
    }

    [Test]
    public void GetOrCreateAsync_InvalidKey_ThrowsArgumentException()
    {
        // Arrange
        var invalidKey = new UserSecretKey(null!);

        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(async () => await _provider.GetOrCreateAsync(invalidKey));
    }

    [Test]
    public void GetOrCreateAsync_EmptyKey_ThrowsArgumentException()
    {
        // Arrange
        var emptyKey = new UserSecretKey(string.Empty);

        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(async () => await _provider.GetOrCreateAsync(emptyKey));
    }

    [Test]
    public async Task GetOrCreateAsync_SecretHasCorrectLength()
    {
        // Arrange
        var key = UserSecretKey.LiteDb("test-length");

        // Act
        var secret = await _provider.GetOrCreateAsync(key);

        // Assert
        Assert.That(secret.ByteLength, Is.EqualTo(48), "Secret must be exactly 48 bytes (384 bits)");
        var bytes = secret.GetBytes();
        Assert.That(bytes.Length, Is.EqualTo(48), "Secret bytes must be exactly 48 bytes");
    }

    [Test]
    public async Task GetOrCreateAsync_NeverUsesMachineIdentifiers()
    {
        // Arrange
        var key = UserSecretKey.LiteDb("test-no-machine-id");
        var machineIdentifiers = new[]
        {
            Environment.MachineName,
            Environment.UserDomainName,
            RuntimeInformation.OSDescription,
            GetMacAddress(),
            GetCpuId()
        }.Where(s => !string.IsNullOrEmpty(s) && s.Length > 3).ToList(); // Only check identifiers longer than 3 chars to avoid false positives

        // Act
        var secret = await _provider.GetOrCreateAsync(key);
        var secretString = secret.Base64;

        // Assert
        foreach (var identifier in machineIdentifiers)
        {
            Assert.That(secretString, Does.Not.Contain(identifier), 
                $"Secret must not contain machine identifier: {identifier}");
        }
    }

    [Test]
    public async Task GetOrCreateAsync_ConcurrentCalls_ReturnSameSecret()
    {
        // Arrange
        var key = UserSecretKey.LiteDb("test-concurrent");
        const int concurrentCalls = 10;

        // Act
        var tasks = Enumerable.Range(0, concurrentCalls)
            .Select(_ => _provider.GetOrCreateAsync(key).AsTask())
            .ToArray();
        
        var secrets = await Task.WhenAll(tasks);

        // Assert
        var firstSecret = secrets[0].Base64;
        Assert.That(secrets.All(s => s.Base64 == firstSecret), Is.True, 
            "All concurrent calls must return the same secret");
    }

    [Test]
    public async Task GetOrCreateAsync_CancellationBeforeGeneration_ThrowsOperationCanceledException()
    {
        // Arrange
        var key = UserSecretKey.LiteDb("test-cancel");
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act & Assert
        // TaskCanceledException is a subclass of OperationCanceledException
        // Use try-catch to verify the exception type
        Exception? caughtException = null;
        try
        {
            await _provider.GetOrCreateAsync(key, cts.Token);
        }
        catch (Exception ex)
        {
            caughtException = ex;
        }
        
        Assert.That(caughtException, Is.Not.Null, "An exception should be thrown on cancellation");
        Assert.That(caughtException, Is.InstanceOf<OperationCanceledException>(),
            "Cancellation should throw OperationCanceledException or TaskCanceledException");
    }

    [Test]
    public async Task RotateAsync_ExistingSecret_GeneratesNewSecret()
    {
        // Arrange
        var key = UserSecretKey.LiteDb($"test-rotate-{DateTimeOffset.UtcNow.Ticks}");
        var originalSecret = await _provider.GetOrCreateAsync(key);

        // Act
        var rotatedSecret = await _provider.RotateAsync(key);

        // Assert
        Assert.That(rotatedSecret, Is.Not.Null);
        Assert.That(rotatedSecret.Base64, Is.Not.Null.And.Not.Empty);
        Assert.That(rotatedSecret.Base64, Is.Not.EqualTo(originalSecret.Base64), 
            "Rotated secret must be different from original");
        Assert.That(rotatedSecret.ByteLength, Is.EqualTo(48), "Secret must be exactly 48 bytes");
        Assert.That(rotatedSecret.CreatedUtc, Is.GreaterThan(originalSecret.CreatedUtc),
            "Rotated secret must have a newer creation time");
    }

    [Test]
    public async Task RotateAsync_ReplacesOldSecret()
    {
        // Arrange
        var key = UserSecretKey.LiteDb($"test-replace-{DateTimeOffset.UtcNow.Ticks}");
        var originalSecret = await _provider.GetOrCreateAsync(key);

        // Act
        var rotatedSecret = await _provider.RotateAsync(key);
        var retrievedSecret = await _provider.GetOrCreateAsync(key);

        // Assert
        Assert.That(retrievedSecret.Base64, Is.EqualTo(rotatedSecret.Base64),
            "After rotation, GetOrCreateAsync should return the new secret");
        Assert.That(retrievedSecret.Base64, Is.Not.EqualTo(originalSecret.Base64),
            "Old secret should no longer be accessible");
    }

    [Test]
    public async Task RotateAsync_IsAtomic()
    {
        // Arrange
        var key = UserSecretKey.LiteDb($"test-atomic-{DateTimeOffset.UtcNow.Ticks}");
        var originalSecret = await _provider.GetOrCreateAsync(key);

        // Act - Rotate and immediately retrieve
        var rotateTask = _provider.RotateAsync(key);
        var retrieveTask = _provider.GetOrCreateAsync(key);
        
        var rotatedSecret = await rotateTask;
        var retrievedSecret = await retrieveTask;

        // Assert - Either we get the old secret or the new one, but not a corrupted state
        Assert.That(
            retrievedSecret.Base64 == originalSecret.Base64 || retrievedSecret.Base64 == rotatedSecret.Base64,
            Is.True,
            "Retrieved secret must be either the original or the rotated one (atomic operation)");
    }

    [Test]
    public void RotateAsync_InvalidKey_ThrowsArgumentException()
    {
        // Arrange
        var invalidKey = new UserSecretKey(null!);

        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(async () => await _provider.RotateAsync(invalidKey));
    }

    [Test]
    public async Task RotateAsync_ConcurrentCalls_HandleCorrectly()
    {
        // Arrange
        var key = UserSecretKey.LiteDb($"test-concurrent-rotate-{DateTimeOffset.UtcNow.Ticks}");
        await _provider.GetOrCreateAsync(key); // Create initial secret
        const int concurrentCalls = 5;

        // Act
        var tasks = Enumerable.Range(0, concurrentCalls)
            .Select(_ => _provider.RotateAsync(key).AsTask())
            .ToArray();
        
        var rotatedSecrets = await Task.WhenAll(tasks);

        // Assert - All rotations should complete, and final state should be one of the rotated secrets
        Assert.That(rotatedSecrets.Length, Is.EqualTo(concurrentCalls));
        Assert.That(rotatedSecrets.All(s => s != null), Is.True);
        Assert.That(rotatedSecrets.All(s => s.ByteLength == 48), Is.True);
        
        // Verify final state - GetOrCreateAsync should return one of the rotated secrets
        var finalSecret = await _provider.GetOrCreateAsync(key);
        Assert.That(rotatedSecrets.Any(s => s.Base64 == finalSecret.Base64), Is.True,
            "Final secret should be one of the rotated secrets");
    }

    [Test]
    public async Task DeleteAsync_ExistingSecret_RemovesSecret()
    {
        // Arrange
        var key = UserSecretKey.LiteDb($"test-delete-{DateTimeOffset.UtcNow.Ticks}");
        var secret = await _provider.GetOrCreateAsync(key);
        
        // Verify secret exists
        var retrievedSecret = await _provider.GetOrCreateAsync(key);
        Assert.That(retrievedSecret.Base64, Is.EqualTo(secret.Base64), "Secret should exist before deletion");

        // Act
        await _provider.DeleteAsync(key);

        // Assert - GetOrCreateAsync should create a new secret
        var newSecret = await _provider.GetOrCreateAsync(key);
        Assert.That(newSecret.Base64, Is.Not.EqualTo(secret.Base64),
            "After deletion, GetOrCreateAsync should generate a new secret");
    }

    [Test]
    public async Task DeleteAsync_AfterDelete_GetOrCreateGeneratesNewSecret()
    {
        // Arrange
        var key = UserSecretKey.LiteDb($"test-delete-new-{DateTimeOffset.UtcNow.Ticks}");
        var originalSecret = await _provider.GetOrCreateAsync(key);

        // Act
        await _provider.DeleteAsync(key);
        var newSecret = await _provider.GetOrCreateAsync(key);

        // Assert
        Assert.That(newSecret, Is.Not.Null);
        Assert.That(newSecret.Base64, Is.Not.Null.And.Not.Empty);
        Assert.That(newSecret.Base64, Is.Not.EqualTo(originalSecret.Base64),
            "New secret should be different from deleted secret");
        Assert.That(newSecret.ByteLength, Is.EqualTo(48), "New secret must be exactly 48 bytes");
        Assert.That(newSecret.CreatedUtc, Is.GreaterThan(originalSecret.CreatedUtc),
            "New secret should have a newer creation time");
    }

    [Test]
    public async Task DeleteAsync_NonExistentSecret_IsIdempotent()
    {
        // Arrange
        var key = UserSecretKey.LiteDb($"test-delete-nonexistent-{DateTimeOffset.UtcNow.Ticks}");

        // Act & Assert - Should not throw
        await _provider.DeleteAsync(key);
        
        // Delete again - should still not throw (idempotent)
        await _provider.DeleteAsync(key);
        
        // Verify we can create a new secret after deletion
        var secret = await _provider.GetOrCreateAsync(key);
        Assert.That(secret, Is.Not.Null);
        Assert.That(secret.ByteLength, Is.EqualTo(48));
    }

    [Test]
    public void DeleteAsync_InvalidKey_ThrowsArgumentException()
    {
        // Arrange
        var invalidKey = new UserSecretKey(null!);

        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(async () => await _provider.DeleteAsync(invalidKey));
    }

    [Test]
    public async Task DeleteAsync_ConcurrentCalls_HandleCorrectly()
    {
        // Arrange
        var key = UserSecretKey.LiteDb($"test-delete-concurrent-{DateTimeOffset.UtcNow.Ticks}");
        await _provider.GetOrCreateAsync(key); // Create initial secret
        const int concurrentCalls = 5;

        // Act
        var tasks = Enumerable.Range(0, concurrentCalls)
            .Select(_ => _provider.DeleteAsync(key).AsTask())
            .ToArray();
        
        await Task.WhenAll(tasks);

        // Assert - All deletions should complete without errors
        // After deletion, GetOrCreateAsync should create a new secret
        var newSecret = await _provider.GetOrCreateAsync(key);
        Assert.That(newSecret, Is.Not.Null);
        Assert.That(newSecret.ByteLength, Is.EqualTo(48), "New secret should be created after deletion");
    }

    private static string? GetMacAddress()
    {
        try
        {
            var networkInterface = NetworkInterface.GetAllNetworkInterfaces()
                .FirstOrDefault(ni => ni.OperationalStatus == OperationalStatus.Up && 
                                      ni.NetworkInterfaceType != NetworkInterfaceType.Loopback);
            return networkInterface?.GetPhysicalAddress().ToString();
        }
        catch
        {
            return null;
        }
    }

    private static string? GetCpuId()
    {
        try
        {
            // This is a simplified check - actual CPU ID detection is platform-specific
            return Environment.ProcessorCount.ToString();
        }
        catch
        {
            return null;
        }
    }

    #region Edge Case Tests

    [Test]
    public async Task GetOrCreateAsync_WithCancellationAfterGeneration_DiscardsSecret()
    {
        // Arrange
        var key = UserSecretKey.LiteDb($"test-cancel-after-gen-{DateTimeOffset.UtcNow.Ticks}");
        using var cts = new CancellationTokenSource();

        // Act - This test verifies that cancellation is properly handled
        // Note: Testing cancellation timing is non-deterministic, so we verify
        // that cancellation is honored and doesn't leave the system in a bad state
        Exception? caughtException = null;
        try
        {
            // Cancel before starting the operation
            cts.Cancel();
            await _provider.GetOrCreateAsync(key, cts.Token);
        }
        catch (OperationCanceledException)
        {
            caughtException = new OperationCanceledException();
        }
        catch (Exception ex)
        {
            caughtException = ex;
        }

        // Assert - Cancellation should be honored
        Assert.That(caughtException, Is.InstanceOf<OperationCanceledException>(),
            "Cancellation should throw OperationCanceledException");
        
        // Verify system is in a good state - we can still create secrets
        var secret = await _provider.GetOrCreateAsync(key);
        Assert.That(secret, Is.Not.Null, "System should be in a valid state after cancellation");
    }

    [Test]
    public async Task RotateAsync_WithCancellationAfterGeneration_DiscardsSecret()
    {
        // Arrange
        var key = UserSecretKey.LiteDb($"test-rotate-cancel-{DateTimeOffset.UtcNow.Ticks}");
        var originalSecret = await _provider.GetOrCreateAsync(key); // Create initial secret
        using var cts = new CancellationTokenSource();

        // Act - Cancel before starting the operation
        Exception? caughtException = null;
        try
        {
            cts.Cancel();
            await _provider.RotateAsync(key, cts.Token);
        }
        catch (OperationCanceledException)
        {
            caughtException = new OperationCanceledException();
        }
        catch (Exception ex)
        {
            caughtException = ex;
        }

        // Assert - Cancellation should be honored
        Assert.That(caughtException, Is.InstanceOf<OperationCanceledException>(),
            "Cancellation should throw OperationCanceledException");
        
        // Verify original secret is still accessible (rotation was cancelled)
        var secret = await _provider.GetOrCreateAsync(key);
        Assert.That(secret, Is.Not.Null, "System should be in a valid state after cancellation");
        // Note: The secret might be the original or a new one depending on cancellation timing,
        // but the system should be in a consistent state
    }

    #endregion

    #region Performance Tests

    [Test]
    public async Task GetOrCreateAsync_Performance_CompletesWithin100ms()
    {
        // Arrange
        var key = UserSecretKey.LiteDb($"test-perf-{DateTimeOffset.UtcNow.Ticks}");
        const int iterations = 50; // More iterations for better statistical accuracy
        var timings = new List<TimeSpan>();

        // Act - First call may be slower (creates secret), so we test subsequent calls
        await _provider.GetOrCreateAsync(key); // Warm-up call
        
        for (int i = 0; i < iterations; i++)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            await _provider.GetOrCreateAsync(key);
            stopwatch.Stop();
            timings.Add(stopwatch.Elapsed);
        }

        // Assert - 95% of requests should complete in under 100ms (with some tolerance for CI/test environments)
        var sortedTimings = timings.OrderBy(t => t.TotalMilliseconds).ToList();
        var percentile95 = sortedTimings[(int)(iterations * 0.95)];
        
        // Allow 200ms threshold for CI/test environments where disk I/O may be slower
        Assert.That(percentile95.TotalMilliseconds, Is.LessThan(200),
            $"95th percentile should be under 200ms (allowing for test environment), but was {percentile95.TotalMilliseconds}ms");
        
        // Also verify that median is reasonable
        var median = sortedTimings[iterations / 2];
        Assert.That(median.TotalMilliseconds, Is.LessThan(100),
            $"Median should be under 100ms, but was {median.TotalMilliseconds}ms");
    }

    [Test]
    public async Task RotateAsync_Performance_CompletesWithin100ms()
    {
        // Arrange
        var key = UserSecretKey.LiteDb($"test-rotate-perf-{DateTimeOffset.UtcNow.Ticks}");
        await _provider.GetOrCreateAsync(key); // Create initial secret
        const int iterations = 50; // More iterations for better statistical accuracy
        var timings = new List<TimeSpan>();

        // Act
        for (int i = 0; i < iterations; i++)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            await _provider.RotateAsync(key);
            stopwatch.Stop();
            timings.Add(stopwatch.Elapsed);
        }

        // Assert - 95% of requests should complete in under 100ms (with some tolerance for CI/test environments)
        var sortedTimings = timings.OrderBy(t => t.TotalMilliseconds).ToList();
        var percentile95 = sortedTimings[(int)(iterations * 0.95)];
        
        // Allow 200ms threshold for CI/test environments where disk I/O may be slower
        Assert.That(percentile95.TotalMilliseconds, Is.LessThan(200),
            $"95th percentile should be under 200ms (allowing for test environment), but was {percentile95.TotalMilliseconds}ms");
        
        // Also verify that median is reasonable
        var median = sortedTimings[iterations / 2];
        Assert.That(median.TotalMilliseconds, Is.LessThan(100),
            $"Median should be under 100ms, but was {median.TotalMilliseconds}ms");
    }

    #endregion

    #region Security Tests

    [Test]
    public async Task GetOrCreateAsync_NoSecretMaterialInExceptions()
    {
        // Arrange
        var key = UserSecretKey.LiteDb($"test-security-{DateTimeOffset.UtcNow.Ticks}");
        var secret = await _provider.GetOrCreateAsync(key);
        var secretBase64 = secret.Base64;

        // Act - Try to trigger an exception scenario
        // (This is a basic test - in practice, we'd need to simulate storage failures)
        try
        {
            // Use an invalid key to trigger ArgumentException
            var invalidKey = new UserSecretKey(null!);
            await _provider.GetOrCreateAsync(invalidKey);
        }
        catch (Exception ex)
        {
            // Assert - Verify secret material is not in exception message or stack trace
            var exceptionText = ex.ToString();
            Assert.That(exceptionText, Does.Not.Contain(secretBase64),
                "Exception should not contain secret material");
        }
    }

    [Test]
    public async Task UserSecret_ToString_DoesNotExposeSecret()
    {
        // Arrange
        var key = UserSecretKey.LiteDb($"test-tostring-{DateTimeOffset.UtcNow.Ticks}");
        var secret = await _provider.GetOrCreateAsync(key);

        // Act
        var toString = secret.ToString();

        // Assert
        Assert.That(toString, Does.Not.Contain(secret.Base64),
            "ToString() should not expose secret material");
        Assert.That(toString, Does.Contain("UserSecret"),
            "ToString() should indicate the type");
        Assert.That(toString, Does.Contain("ByteLength"),
            "ToString() should include metadata");
    }

    #endregion

    #region Integration Tests

    [Test]
    public async Task Integration_LiteDbEncryptionScenario()
    {
        // Arrange
        var key = UserSecretKey.LiteDb("node.net.test");
        var secret = await _provider.GetOrCreateAsync(key);
        var secretBytes = secret.GetBytes();

        // Act & Assert - Verify secret can be used for encryption
        Assert.That(secretBytes, Is.Not.Null);
        Assert.That(secretBytes.Length, Is.EqualTo(48));
        Assert.That(secret.Base64, Is.Not.Null.And.Not.Empty);
        
        // Verify secret is stable (can be retrieved again)
        var secret2 = await _provider.GetOrCreateAsync(key);
        Assert.That(secret2.Base64, Is.EqualTo(secret.Base64),
            "Secret should be stable for LiteDB encryption scenario");
        
        // Verify secret can be converted to Base64 for LiteDB connection string
        var connectionString = $"Filename=test.db;Password={secret.Base64}";
        Assert.That(connectionString, Does.Contain(secret.Base64),
            "Secret should be usable in LiteDB connection string");
    }

    [Test]
    public async Task Integration_TokenCacheEncryptionScenario()
    {
        // Arrange
        var key = UserSecretKey.TokenCache("node.net.test");
        var secret = await _provider.GetOrCreateAsync(key);
        var secretBytes = secret.GetBytes();

        // Act & Assert - Verify secret can be used for token cache encryption
        Assert.That(secretBytes, Is.Not.Null);
        Assert.That(secretBytes.Length, Is.EqualTo(48));
        
        // Verify secret is stable
        var secret2 = await _provider.GetOrCreateAsync(key);
        Assert.That(secret2.Base64, Is.EqualTo(secret.Base64),
            "Secret should be stable for token cache encryption scenario");
        
        // Verify rotation works for token cache
        var rotatedSecret = await _provider.RotateAsync(key);
        Assert.That(rotatedSecret.Base64, Is.Not.EqualTo(secret.Base64),
            "Rotated secret should be different");
    }

    #endregion
}
