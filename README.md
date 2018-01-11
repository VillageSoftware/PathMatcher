# VillageSoftware.PathMatcher

This is a .Net Standard 1.3 library to do some things that `System.IO.Path` doesn't do. Its design goals include being thoroughly tested and easily integrated. We aim to make this a trustworthy library for working with mixed URL and Local-style file paths and in particular, resituating files from a local context into the cloud.

In principle, importing this module should keep you from doing work that's already been done by us in a proven, well-tested way.

## Setup

This will be a NuGet package but it's not available yet. To try out PathMatcher, please clone and build the repo in Visual Studio.

## Features

### `PathInfo` class

The PathInfo class is instantiated on a path string and exposes a load of helpful properties and methods. It is needed as the basis of the more complex tools in the library.

**Properties:**

 * `pathInfo.FileDirectoryOnly` - Exposes the directory (without the file)
 * `pathInfo.FileNameOnly` - Exposes the file (without the directory)
 * `pathInfo.Chunks` - Exposes the path as a List of chunks
 * `pathInfo.Separator` - Exposes what was deemed to be the predominant separator in the path
 * `pathInfo.IsUrl` - Exposes a calculated result of whether the path was a URL

**Methods:**

 * `GetPathWithFinalSeparatorOnOff(string path, bool showFinalSeparator)` - Ensure that the passed path either has or does not have a trailing separator
 * `PathAfterChunk(string chunk)` and `PathBeforeChunk` - Return the reamining section of this path which appears after/before the specified chunk (a.k.a segment)
 * `ConformSeparatorTo(char separator)` - Change the separators on this PathInfo object to the specified separator and rebuild all of the PathInfo fields (this is useful for merging together local `\` paths and remote `/` paths)

**Example Usage:**

	string myPath = @"C:\Users\Coder\Documents\Code\Project\File.cs";
	var pathInfo = new PathInfo(myPath);
	pathInfo.ConformSeparatorTo('/');
	var finalPath = pathInfo.GetPathWithFinalSeparatorOnOff(pathInfo.FilePath, true);

### PathMatcher

PathMatcher is a static class with utilites which operate on string paths to do special things.

**PathMatcher.Resituate**

This merges together two paths with some degree of common ancestry to let you, for example, move a local file into the appropriate path on a remote server:

	string localPath = @"C:\Users\Alice\Documents\Codes\OpenSource\Project\README.md";
	string destinationPath = @"https://filestore.cloud.example/Alice/";
	string newRemotePath = PathMatcher.Resituate(localPath, destinationPath);
	
	-> https://filestore.cloud.example/Alice/Documents/Codes/OpenSource/Project/README.md

See [PathMatcherTests][tests] for more info.	

See also [design-notes.md](/design-notes.md)

[tests]: /VillageSoftware.PathMatcher/VillageSoftware.PathMatcher.Tests/PathMatcherTests.cs

## Contributing

To contribute, please open an issue first, where we'd love to discuss the library with you!

## License (MIT)

See License.txt
