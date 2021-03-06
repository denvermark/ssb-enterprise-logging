﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using Moq;

namespace Ssb.Common.Logging.Utilities
{
    internal class MockFileProvider : IFileProvider
    {
        private readonly IEnumerable<IFileInfo> _files;
        private readonly Dictionary<string, IChangeToken> _changeTokens;

        public MockFileProvider()
        { }

        public MockFileProvider(params IFileInfo[] files)
        {
            _files = files;
        }

        public MockFileProvider(params KeyValuePair<string, IChangeToken>[] changeTokens)
        {
            _changeTokens = changeTokens.ToDictionary(
                changeToken => changeToken.Key,
                changeToken => changeToken.Value,
                StringComparer.Ordinal);
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            var contents = new Mock<IDirectoryContents>();
            contents.Setup(m => m.Exists).Returns(true);

            if (string.IsNullOrEmpty(subpath))
            {
                contents.Setup(m => m.GetEnumerator()).Returns(_files.GetEnumerator());
                return contents.Object;
            }

            var filesInFolder = _files.Where(f => f.Name.StartsWith(subpath, StringComparison.Ordinal));
            if (filesInFolder.Any())
            {
                contents.Setup(m => m.GetEnumerator()).Returns(filesInFolder.GetEnumerator());
                return contents.Object;
            }
            return NotFoundDirectoryContents.Singleton;
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            var file = _files.FirstOrDefault(f => f.Name == subpath);
            return file ?? new NotFoundFileInfo(subpath);
        }

        public IChangeToken Watch(string filter)
        {
            if (_changeTokens != null && _changeTokens.ContainsKey(filter))
            {
                return _changeTokens[filter];
            }
            return NullChangeToken.Singleton;
        }
    }
}