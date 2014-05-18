using System.Collections.Generic;
using System.IO;
using Mp3LibrarySorter;
using NUnit.Framework;

namespace Mp3LibrarySorterTests 
{
    [TestFixture]
    public class Mp3FileReaderFixture 
    {
        private IList<Mp3Node> _tags;
        private IList<string> _filesWithMissingTags;

        [SetUp]
        public void Setup()
        {
            var reader = new Mp3FileReader();
            var files = Directory.GetFiles(@"C:\Development\Mp3LibrarySorter\Data", "*.*", SearchOption.AllDirectories);
            _tags = reader.RetrieveTagsFromMp3Files(files);
            _filesWithMissingTags = reader.FilesWithMissingTags;
        }

        [Test]
        public void ShouldReadMp3FileCorrectly()
        {
            Assert.That(_tags[0].ArtistName, Contains.Substring("Coldplay"));
        }

        [Test]
        public void ShouldContain1FileWithNoTag()
        {
            Assert.That(_filesWithMissingTags.Count, Is.EqualTo(1));
        }
    }
}
