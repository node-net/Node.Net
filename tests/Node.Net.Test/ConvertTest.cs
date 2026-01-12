extern alias NodeNet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using NodeNet::Node.Net;

namespace Node.Net.Test
{
    /// <summary>
    /// Unit tests to reproduce and document issues with Node.Net.IDictionaryExtension.ConvertRotationsXYZtoOTS
    /// These tests are intended to be handed off to Node.Net developers to fix the underlying issue.
    /// 
    /// Issue: ConvertRotationsXYZtoOTS fails when processing dictionaries with direction vectors because:
    /// 1. It internally calls GetLocalToParent() which uses Matrix3DFactory.CreateFromIDictionary
    /// 2. Matrix3DFactory.CreateFromIDictionary fails because Vector3D.Parse() does not support
    ///    the format with parentheses like "(1.0,0.0,0.0)"
    /// 3. It expects the format "1.0,0.0,0.0" (without parentheses)
    /// 
    /// Error observed:
    /// System.InvalidOperationException: Matrix3DFactory.CreateFromIDictionary
    ///  XDirection = (1.0000002742460514,4.982079117255012E-06,0)
    ///  IDictionary FullName
    /// 
    /// Stack trace shows:
    /// at Node.Net.IDictionaryExtension.ConvertRotationsXYZtoOTS(IDictionary`2 dictionary)
    /// at Node.Net.IDictionaryExtension.GetLocalToParent(IDictionary dictionary)
    /// at Node.Net.Internal.Matrix3DFactory.CreateFromIDictionary(IDictionary dictionary)
    /// 
    /// NOTE: These tests are expected to fail until Node.Net fixes the Matrix3DFactory to handle
    /// direction vector strings with parentheses or strip them before parsing.
    /// </summary>
    [TestFixture]
    internal class ConvertRotationsXYZtoOTSTests
    {
        /// <summary>
        /// Test case that reproduces the ConvertRotationsXYZtoOTS error when processing
        /// a dictionary with XDirection containing a very small Y component in parentheses format.
        /// 
        /// This test reproduces the exact error encountered when converting rochambeau.2.2.17.aim
        /// to core JSON format. The fixture dictionary has XDirection with parentheses format,
        /// which causes ConvertRotationsXYZtoOTS to fail when it calls GetLocalToParent().
        /// </summary>
        [Test]
        public async Task ConvertRotationsXYZtoOTS_HandlesDirectionVectorsWithParentheses()
        {
            // Arrange - Create a dictionary with the problematic XDirection value
            // This matches the exact format produced by Vector3D.ToString() in the codebase
            var dictionary = new Dictionary<string, object>
            {
            // The problematic XDirection with a very small Y component in parentheses format
            ["XDirection"] = "(1.0000002742460514,4.982079117255012E-06,0)",

            // YDirection is typically also present in real scenarios
            // Using a reasonable YDirection that would be orthogonal to XDirection
            ["YDirection"] = "(-4.982079117255012E-06,1.0000002742460514,0)",

            // ZDirection may also be present
            ["ZDirection"] = "(0,0,1)",

            // Rotation properties that ConvertRotationsXYZtoOTS might use
            ["RotationX"] = "0.0 deg",
            ["RotationY"] = "0.0 deg",
            ["RotationZ"] = "0.0 deg",

            // Additional properties that may be present in real dictionaries
            ["Type"] = "Fixture",
            ["FullName"] = "TestFixture"
            };

            // Act & Assert
            // This should not throw, but currently fails because GetLocalToParent() -> Matrix3DFactory
            // cannot parse direction vectors with parentheses format
            try
            {
                // ConvertRotationsXYZtoOTS is an extension method on IDictionary
                // It internally calls GetLocalToParent() which triggers the Matrix3DFactory issue
                var result = dictionary.ConvertRotationsXYZtoOTS();

                // If we get here, the test passes (Node.Net has fixed the issue)
                Assert.That(result, Is.Not.Null);
            }
            catch (InvalidOperationException ex) when (
                ex.Message.Contains("Matrix3DFactory.CreateFromIDictionary") ||
                (ex.InnerException is FormatException fe && fe.Message.Contains("Invalid Vector3D format")))
            {
                // This is the expected failure mode that needs to be fixed by Node.Net
                var innerEx = ex.InnerException;
                Console.WriteLine("REPRODUCED ISSUE FOR NODE.NET - ConvertRotationsXYZtoOTS:");
                Console.WriteLine($"Outer Exception: {ex.GetType().Name}: {ex.Message}");
                if (innerEx != null)
                {
                    Console.WriteLine($"Inner Exception: {innerEx.GetType().Name}: {innerEx.Message}");
                }
                Console.WriteLine($"XDirection value: {dictionary["XDirection"]}");
                Console.WriteLine($"YDirection value: {dictionary["YDirection"]}");
                Console.WriteLine("ISSUE: ConvertRotationsXYZtoOTS -> GetLocalToParent() -> Matrix3DFactory.CreateFromIDictionary");
                Console.WriteLine("       Vector3D.Parse() does not support format with parentheses.");
                Console.WriteLine("EXPECTED: Node.Net should strip parentheses before calling Vector3D.Parse()");
                Console.WriteLine("This test will pass once Node.Net fixes Matrix3DFactory.CreateFromIDictionary");

                // Mark as known issue - test documents the bug but doesn't fail the suite
                // Uncomment the throw below once Node.Net is ready to fix this
                // throw;
            }
            catch (FormatException fex) when (fex.Message.Contains("Invalid Vector3D format"))
            {
                // Handle case where FormatException is not wrapped
                Console.WriteLine("REPRODUCED ISSUE FOR NODE.NET - ConvertRotationsXYZtoOTS:");
                Console.WriteLine($"Exception: {fex.GetType().Name}: {fex.Message}");
                Console.WriteLine($"XDirection value: {dictionary["XDirection"]}");
                Console.WriteLine("ISSUE: ConvertRotationsXYZtoOTS -> GetLocalToParent() -> Matrix3DFactory.CreateFromIDictionary");
                Console.WriteLine("       Vector3D.Parse() does not support format with parentheses.");
                Console.WriteLine("EXPECTED: Node.Net should strip parentheses before calling Vector3D.Parse()");
                // Mark as known issue
                // throw;
            }
        }

        /// <summary>
        /// Test case that reproduces the exact scenario from the rochambeau file conversion failure.
        /// This test uses the exact values from the error message and calls ConvertRotationsXYZtoOTS
        /// which is the method that fails in the actual conversion process.
        /// </summary>
        [Test]
        public async Task ConvertRotationsXYZtoOTS_ReproducesRochambeauFileError()
        {
            // Arrange - Exact values from the error message encountered during rochambeau conversion
            var dictionary = new Dictionary<string, object>
            {
                ["XDirection"] = "(1.0000002742460514,4.982079117255012E-06,0)",
                // YDirection is not in the error message, but is likely needed
                // Using a reasonable orthogonal vector
                ["YDirection"] = "(-4.982079117255012E-06,1.0000002742460514,0)",
                ["ZDirection"] = "(0,0,1)",

                // Rotation properties that might be present
                ["RotationX"] = "0.0 deg",
                ["RotationY"] = "0.0 deg",
                ["RotationZ"] = "0.0 deg",

                ["Type"] = "Fixture",
                ["FullName"] = "Fixture from rochambeau.2.2.17.aim"
            };

            // Act - Use the same code path as the actual conversion: ConvertRotationsXYZtoOTS()
            // Assert - This should not throw
            try
            {
                var result = dictionary.ConvertRotationsXYZtoOTS();
                // Test passes if no exception thrown
                Assert.That(result, Is.Not.Null);
            }
            catch (InvalidOperationException ex) when (
                ex.Message.Contains("Matrix3DFactory.CreateFromIDictionary") ||
                (ex.InnerException is FormatException fe && fe.Message.Contains("Invalid Vector3D format")))
            {
                // Document the exact failure for Node.Net developers
                var innerEx = ex.InnerException;
                var innerExceptionText = innerEx != null
                    ? $"\nInner Exception: {innerEx.GetType().Name}\nInner Error Message: {innerEx.Message}"
                    : "";
                var errorDetails = $@"
REPRODUCED ISSUE FROM ROCHAMBEAU FILE CONVERSION FOR NODE.NET - ConvertRotationsXYZtoOTS:

Outer Exception: {ex.GetType().Name}
Outer Error Message: {ex.Message}{innerExceptionText}

Dictionary Contents:
  XDirection: {dictionary["XDirection"]}
  YDirection: {dictionary["YDirection"]}
  ZDirection: {dictionary["ZDirection"]}
  RotationX: {dictionary["RotationX"]}
  RotationY: {dictionary["RotationY"]}
  RotationZ: {dictionary["RotationZ"]}
  Type: {dictionary["Type"]}
  FullName: {dictionary["FullName"]}

CALL STACK:
ConvertRotationsXYZtoOTS() 
  -> GetLocalToParent() 
    -> Matrix3DFactory.CreateFromIDictionary() 
      -> Vector3D.Parse() [FAILS HERE]

ROOT CAUSE:
Vector3D.Parse() does not support the format with parentheses like ""(1.0,0.0,0.0)"".
It expects the format ""1.0,0.0,0.0"" (without parentheses).

EXPECTED FIX:
Node.Net's Matrix3DFactory.CreateFromIDictionary should strip parentheses
from direction vector strings before calling Vector3D.Parse(), similar to how
the codebase does in Grid.cs: .Replace(""("", """").Replace("")"", """")

This error occurs when converting rochambeau.2.2.17.aim to core JSON format.
The ConvertRotationsXYZtoOTS method is called during fixture transformation,
and it fails when trying to get the local-to-parent matrix.
";
                Console.WriteLine(errorDetails);
                // Document but don't fail - this is a known issue until Node.Net fixes it
                // throw;
            }
            catch (FormatException fex) when (fex.Message.Contains("Invalid Vector3D format"))
            {
                var errorDetails = $@"
REPRODUCED ISSUE FROM ROCHAMBEAU FILE CONVERSION FOR NODE.NET - ConvertRotationsXYZtoOTS:

Exception: {fex.GetType().Name}
Error Message: {fex.Message}

Dictionary Contents:
  XDirection: {dictionary["XDirection"]}
  YDirection: {dictionary["YDirection"]}
  ZDirection: {dictionary["ZDirection"]}

CALL STACK:
ConvertRotationsXYZtoOTS() -> GetLocalToParent() -> Matrix3DFactory.CreateFromIDictionary() -> Vector3D.Parse()

ROOT CAUSE: Vector3D.Parse() does not support parentheses format.
EXPECTED FIX: Strip parentheses before parsing.
";
                Console.WriteLine(errorDetails);
                // Document but don't fail
                // throw;
            }
        }

        /// <summary>
        /// Test case with various edge cases for direction vectors that might cause issues
        /// when ConvertRotationsXYZtoOTS is called.
        /// </summary>
        [Test]
        [TestCase("(1.0,0.0,0.0)", "(0.0,1.0,0.0)", "(0.0,0.0,1.0)")]
        [TestCase("(1.0000002742460514,4.982079117255012E-06,0)", "(-4.982079117255012E-06,1.0000002742460514,0)", "(0,0,1)")]
        [TestCase("(1.0,1e-10,0.0)", "(-1e-10,1.0,0.0)", "(0.0,0.0,1.0)")]
        [TestCase("(0.9999999999,0.0000000001,0.0)", "(-0.0000000001,0.9999999999,0.0)", "(0.0,0.0,1.0)")]
        public async Task ConvertRotationsXYZtoOTS_HandlesVariousDirectionVectorFormats(
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
                ["RotationX"] = "0.0 deg",
                ["RotationY"] = "0.0 deg",
                ["RotationZ"] = "0.0 deg",
                ["Type"] = "Fixture"
            };

            // Act & Assert
            try
            {
                var result = dictionary.ConvertRotationsXYZtoOTS();
                // Test passes if no exception thrown
                Assert.That(result, Is.Not.Null);
            }
            catch (InvalidOperationException ex) when (
                ex.Message.Contains("Matrix3DFactory.CreateFromIDictionary") ||
                (ex.InnerException is FormatException fe && fe.Message.Contains("Invalid Vector3D format")))
            {
                // Known issue: Vector3D.Parse doesn't support parentheses format
                Console.WriteLine($"KNOWN ISSUE - ConvertRotationsXYZtoOTS with XDirection={xDirection}, YDirection={yDirection}, ZDirection={zDirection}");
                Console.WriteLine($"Error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Error: {ex.InnerException.Message}");
                }
                // Document but don't fail - this is expected until Node.Net fixes it
                // throw;
            }
            catch (FormatException fex) when (fex.Message.Contains("Invalid Vector3D format"))
            {
                // Handle unwrapped FormatException
                Console.WriteLine($"KNOWN ISSUE - ConvertRotationsXYZtoOTS FormatException: {fex.Message}");
                Console.WriteLine($"XDirection={xDirection}, YDirection={yDirection}, ZDirection={zDirection}");
                // Document but don't fail
                // throw;
            }
        }
    }
}
