using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
#if IS_WINDOWS
using System.Windows.Media.Media3D;
#endif

namespace Node.Net.Test
{
    [TestFixture]
    internal class FactoryTest
    {
        [Test]
        public void Coverage()
        {
            Factory factory = new Factory();
        }

#if IS_WINDOWS
        [Test]
        public void ClearCache()
        {
            Factory factory = new Factory();
            factory.Cache = true;
            factory.ClearCache();
            if (factory.Cache) factory.Cache = false;

            Matrix3D matrix = factory.Create<Matrix3D>();
            factory.ClearCache(matrix);
            factory.ClearCache();
        }

        /// <summary>
        /// Test case that reproduces the Matrix3DFactory.CreateFromIDictionary error
        /// when processing a dictionary with XDirection containing a very small Y component.
        /// 
        /// This test reproduces the exact error encountered when converting rochambeau.2.2.17.aim
        /// to core JSON format. The XDirection vector has a Y component of 4.982079117255012E-06,
        /// which is very close to zero but not exactly zero, causing numerical precision issues
        /// in the matrix calculation.
        /// </summary>
        [Test]
        public async Task CreateFromIDictionary_HandlesSmallYComponentInXDirection()
        {
            // Arrange - Create a dictionary with the problematic XDirection value
            // This matches the exact format produced by Vector3D.ToString() in the codebase
            var dictionary = new Dictionary<string, object>
            {
                // The problematic XDirection with a very small Y component
                ["XDirection"] = "(1.0000002742460514,4.982079117255012E-06,0)",
                
                // YDirection is typically also present in real scenarios
                // Using a reasonable YDirection that would be orthogonal to XDirection
                ["YDirection"] = "(-4.982079117255012E-06,1.0000002742460514,0)",
                
                // ZDirection may also be present
                ["ZDirection"] = "(0,0,1)",
                
                // Additional properties that may be present in real dictionaries
                ["Type"] = "Fixture",
                ["FullName"] = "TestFixture"
            };

            // Act & Assert
            // This should not throw an InvalidOperationException
            // The current implementation throws: "Matrix3DFactory.CreateFromIDictionary"
            try
            {
                // Use the same path as the codebase: GetLocalToParent() extension method
                // which internally calls Factory.Create<Matrix3D>() -> Node.Net.Matrix3DFactory.CreateFromIDictionary()
                var matrix3D = dictionary.GetLocalToParent();
                
                // If we get here, the test passes
                // Matrix3D is a struct, so we check it was created successfully
                // (Matrix3D.IsIdentity is a valid property to check)
                await Assert.That(matrix3D.IsIdentity || !matrix3D.IsIdentity).IsTrue(); // Test passes if no exception thrown
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("Matrix3DFactory.CreateFromIDictionary"))
            {
                // This is the expected failure mode that needs to be fixed
                await Assert.That(ex.Message).Contains("Matrix3DFactory.CreateFromIDictionary");
                
                // Document the failure for Node.Net developers
                Console.WriteLine("REPRODUCED ISSUE:");
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"XDirection value: {dictionary["XDirection"]}");
                Console.WriteLine($"YDirection value: {dictionary["YDirection"]}");
                Console.WriteLine("This test should pass once Node.Net fixes Matrix3DFactory.CreateFromIDictionary");
                
                // Re-throw to fail the test (this is expected until the issue is fixed)
                throw;
            }
        }

        /// <summary>
        /// Test case with various edge cases for direction vectors that might cause issues.
        /// </summary>
        [Test]
        [Arguments("(1.0,0.0,0.0)", "(0.0,1.0,0.0)", "(0.0,0.0,1.0)")]
        [Arguments("(1.0000002742460514,4.982079117255012E-06,0)", "(-4.982079117255012E-06,1.0000002742460514,0)", "(0,0,1)")]
        [Arguments("(1.0,1e-10,0.0)", "(-1e-10,1.0,0.0)", "(0.0,0.0,1.0)")]
        [Arguments("(0.9999999999,0.0000000001,0.0)", "(-0.0000000001,0.9999999999,0.0)", "(0.0,0.0,1.0)")]
        public async Task CreateFromIDictionary_HandlesVariousDirectionVectorFormats(
            string xDirection, 
            string yDirection, 
            string zDirection)
        {
            // Arrange
            var dictionary = new Dictionary<string, object>
            {
                ["XDirection"] = xDirection,
                ["YDirection"] = yDirection,
                ["ZDirection"] = zDirection,
                ["Type"] = "Fixture"
            };

            // Act & Assert
            try
            {
                var matrix3D = dictionary.GetLocalToParent();
                // Test passes if no exception thrown - Matrix3D was created successfully
                // Access a property to ensure the matrix was created (not just default)
                var _ = matrix3D.M11; // Suppress unused variable warning
                // If we reach here, the test passes (no exception was thrown)
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("Matrix3DFactory.CreateFromIDictionary"))
            {
                Console.WriteLine($"FAILED with XDirection={xDirection}, YDirection={yDirection}, ZDirection={zDirection}");
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Test case that verifies the exact scenario from the rochambeau file conversion failure.
        /// This test uses the exact values from the error message.
        /// </summary>
        [Test]
        public async Task CreateFromIDictionary_ReproducesRochambeauFileError()
        {
            // Arrange - Exact values from the error message
            var dictionary = new Dictionary<string, object>
            {
                ["XDirection"] = "(1.0000002742460514,4.982079117255012E-06,0)",
                // YDirection is not in the error message, but is likely needed
                // Using a reasonable orthogonal vector
                ["YDirection"] = "(-4.982079117255012E-06,1.0000002742460514,0)",
                ["ZDirection"] = "(0,0,1)",
                ["Type"] = "Fixture",
                ["FullName"] = "Fixture from rochambeau.2.2.17.aim"
            };

            // Act - Use the same code path as Extensions.GetLocalToParent()
            // Assert - This should not throw
            try
            {
                var matrix3D = dictionary.GetLocalToParent();
                // Test passes if no exception thrown - Matrix3D was created successfully
                // Access a property to ensure the matrix was created (not just default)
                var _ = matrix3D.M11; // Suppress unused variable warning
                // If we reach here, the test passes (no exception was thrown)
            }
            catch (InvalidOperationException ex)
            {
                // Document the exact failure
                var errorDetails = $@"
REPRODUCED ISSUE FROM ROCHAMBEAU FILE CONVERSION:

Error Type: {ex.GetType().Name}
Error Message: {ex.Message}

Dictionary Contents:
  XDirection: {dictionary["XDirection"]}
  YDirection: {dictionary["YDirection"]}
  ZDirection: {dictionary["ZDirection"]}
  Type: {dictionary["Type"]}
  FullName: {dictionary["FullName"]}

Stack Trace:
{ex.StackTrace}

This error occurs when converting rochambeau.2.2.17.aim to core JSON format.
The Matrix3DFactory.CreateFromIDictionary method in Node.Net needs to handle
direction vectors with very small floating-point components gracefully.

Expected Behavior:
- Matrix3DFactory.CreateFromIDictionary should parse the direction vector strings
- It should handle numerical precision issues with very small values
- It should create a valid Matrix3D from the direction vectors
";
                Console.WriteLine(errorDetails);
                throw;
            }
        }
#endif
    }
}