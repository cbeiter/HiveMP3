﻿using System.Collections.Generic;
using System.IO;
using Mp3LibrarySorter;
using NUnit.Framework;
using Rhino.Mocks;

namespace Mp3LibrarySorterTests
{
    [TestFixture]
    public class Mp3LibrarySorterTests
    {
        private Mp3LibrarySorter.Mp3LibraryGenerator _mp3LibrarySorter;
        private FileManager _mockFileManager;
        const string SomeStartDirectory = "someDirectory";
        List<Mp3Node> _Mp3Nodes = new List<Mp3Node>();
        private Mp3Node _mockMp3Node;
        private Mp3TagLibrary _mockMp3TagLibrary;
        private Mp3FileReader _mockMp3FileReader;
        private readonly string _albumFolder = SomeStartDirectory + Path.DirectorySeparatorChar + SomeArtistName + Path.DirectorySeparatorChar + SomeAlbumName;
        const string SomeFileName = "SomeFileName";
        const string SomeAlbumName = "SomeAlbumName";
        const string SomeArtistName = "SomeArtistName";

        [SetUp]
        public void Setup()
        {
            _mockMp3Node = MockRepository.GenerateStub<Mp3Node>();
            _mockMp3Node.AlbumName = SomeAlbumName;
            _mockMp3Node.ArtistName = SomeArtistName;
            _mockMp3Node.FileName = SomeFileName;

            _mockFileManager = MockRepository.GenerateStub<FileManager>();
            _mockFileManager.Stub(system => system.GetMp3FilePaths(SomeStartDirectory, true)).Return(new List<string> { SomeFileName });

            _mockMp3TagLibrary = MockRepository.GenerateStub<Mp3TagLibrary>();
            _mockMp3TagLibrary.Stub(hierarchy => hierarchy.Artists).Return(new List<string> {SomeArtistName});
            _mockMp3TagLibrary.Stub(tagsHierarchy => tagsHierarchy.GetAlbumsForArtist(SomeArtistName)).Return(
                new List<string> {SomeAlbumName});

            _mockMp3TagLibrary.Stub(
                mp3TagsHierarchy => mp3TagsHierarchy.GetSongsForAlbumOfArtist(SomeAlbumName, SomeArtistName)).Return(
                    new List<string> {SomeFileName});
            _mockMp3FileReader = MockRepository.GenerateStub<Mp3FileReader>();
            _mockMp3FileReader.Stub(reader => reader.RetrieveTagsFromMp3Files(new List<string> {SomeFileName})).Return(
                new List<Mp3Node>
                    {
                        new Mp3Node
                            {AlbumName = SomeAlbumName, ArtistName = SomeArtistName, FileName = SomeFileName}
                    });

            _mp3LibrarySorter = new Mp3LibrarySorter.Mp3LibraryGenerator(_mockFileManager, SomeStartDirectory,
                SomeStartDirectory, _mockMp3TagLibrary, _mockMp3FileReader);
            _mp3LibrarySorter.CreateFoldersForArtists();
        }

        [Test]
        public void ShouldRetrieveAllMp3FilesFromStartingDirectory()
        {
            _mockFileManager.AssertWasCalled(system => system.GetMp3FilePaths(SomeStartDirectory, false));
        }

        [Test]
        public void ShouldCreateDirectorySomeArtistName()
        {
            _mockFileManager.AssertWasCalled(system => system.CreateDirectory(SomeStartDirectory + Path.DirectorySeparatorChar + SomeArtistName));
        }

        [Test]
        public void ShouldCreateDirectorySomeAlbumName()
        {
            _mockFileManager.AssertWasCalled(system => system.CreateDirectory(_albumFolder));
        }

        [Test]
        public void ShouldMoveValidMp3FilesToAlbumFolder()
        {
            _mockFileManager.AssertWasCalled(system => system.Move(SomeFileName, _albumFolder + Path.DirectorySeparatorChar + SomeFileName));
        }
    }
}
