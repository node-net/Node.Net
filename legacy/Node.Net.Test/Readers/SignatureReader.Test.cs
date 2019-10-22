using NUnit.Framework;

namespace Node.Net.Readers.Test
{
    [TestFixture]
    class SignatureReaderTest
    {
        [Test]
        [TestCase("image.bmp", false, "42 4D 46 0E")]
        [TestCase("image.gif", false, "47 49 46 38")]
        [TestCase("image.jpg", false, "FF D8 FF E0")]
        [TestCase("image.png", false, "89 50 4E 47")]
        [TestCase("image.tif", false, "49 49 2A 00")]
        [TestCase("mesh.xaml", true, "<MeshGeometry")]
        [TestCase("simple.html", true, "<html><bod")]
        [TestCase("simple.json", true, "{\"string_0")]
        [TestCase("array.json", true, "[0,1,2,3]")]
        [TestCase("hash.json", true, "{}")]
        public void SignatureReader_Read(string name, bool is_text, string text_or_hex_signature)
        {
            var stream = GlobalFixture.GetStream(name);
            Assert.NotNull(stream, nameof(stream));

            var reader = new SignatureReader();
            var signature = reader.Read(stream) as Signature;

            Assert.NotNull(signature, nameof(signature));
            Assert.AreEqual(is_text, signature.IsText, $"{name} signature.IsText");

            if (is_text)
            {
                Assert.True(signature.Text.Contains(text_or_hex_signature));
                //Assert.AreEqual(text_or_hex_signature, signature.Text);
            }
            else
            {
                Assert.True(signature.HexString.Contains(text_or_hex_signature), signature.HexString);
            }
        }
    }
}
