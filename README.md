# VillageSoftware.PathMatcher

This is a .Net Standard 1.3 library to do some things that `System.IO.Path` doesn't do. Its design goals include being thoroughly tested and easily integrated. We aim to make this a trustworthy library for working with mixed URL and Local-style file paths and in particular, resituating files from a local context into the cloud.

In principle, importing this module should keep you from doing work that's already been done by us in a proven, well-tested way.

## Setup

In NuGet Package Manager:

	Install-Package VillageSoftware.PathMatcher
	
Or see [VillageSoftware.PathMatcher on nuget.org][nuget]

[nuget]: https://www.nuget.org/packages/VillageSoftware.PathMatcher/

## Features

### `PathInfo` class

The PathInfo class is instantiated on a path string and exposes a load of helpful properties and methods. It is needed as the basis of the more complex tools in the library.

**Properties:**

 * `pathInfo.FilePath` - The path
 * `pathInfo.FileDirectoryOnly` - The directory (without the file)
 * `pathInfo.FileNameOnly` - The file name (without the directory)
 * `pathInfo.Chunks` - FilePath as a List of chunks (segments)
 * `pathInfo.Separator` - The (calculated) predominant separator in the path
 * `pathInfo.IsUrl` - True if FilePath is considered to be a URL
 * `pathInfo.HasTerminatingSeparator` - True if FilePath currently has a terminating separator
 
**Methods:**

 * `GetPathWithFinalSeparatorOnOff(string path, bool showFinalSeparator)` - Ensure that the passed path either has or does not have a trailing separator (using the predominant Separator)
 * `PathBeforeChunk(string chunk)` and `PathAfterChunk` - Return the reamining section of FilePath which appears before/after the specified chunk (segment)
 * `ConformSeparatorTo(char separator)` - Change the separators on this PathInfo object to the specified separator and rebuild all of the PathInfo fields (this is useful for merging together local `\` paths and remote `/` paths)
 * `AddChunks(bool addTerminatingSeparator, params string[] chunks)` - Lengthen the path safely, automatically inserting the predominant separator between your new specified chunks. Optionally add a terminating separator.
 * `SetFileName(string FileName)` - Set the file name of this PathInfo to the filename found in the supplied parameter (or pass `""` to remove the filename from FilePath altogether)
  
**Example Usage:**

	string myPath = @"C:\Users\Coder\Documents\Code\Project\File.cs";
	var pathInfo = new PathInfo(myPath);
	
	//We can flip all the separators if we want
	pathInfo.ConformSeparatorTo('/');

	//Remove the filename (strip it back to directory only)
	pathInfo.SetFileName("");

	pathInfo.AddChunks(true, "src", "Controllers");
	pathInfo.SetFileName("FileNameController.cs");

	// Value of pathInfo.FilePath is now 
	// "C:\Users\Coder\Documents\Code\Project\src\Controllers\FileNameController.cs"

	string remotePath = "https://files.example/Coder/Documents/";
	string newPath = PathMatcher.Resituate(pathInfo.FilePath, remotePath);

	// After "resituating" into remotePath, value of newPath is
	// "https://files.example/Coder/Documents/Code/Project/src/Controllers/FileNameController.cs";

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
