# Design

**Useful paths for examples:**

	C:\Users\Alice\Documents\Codes\OpenSource\Project\README.md
	https://filestore.cloud.example/#!/Alice/Documents/Codes/OpenSource/Project/README.md

## 1. Resituate A into B

 A. `C:\Users\Alice\Documents\Codes\OpenSource\Project\README.md`
 B. `https://filestore.cloud.example/#!/Alice/`

Process: Searching backwards through both paths, find first common chunk, then take the remainder of the source path and integrate it into the destination path. All separators are made to conform to the destination path (B).

Result: `https://filestore.cloud.example/#!/Alice/Documents/Codes/OpenSource/Project/README.md`

Test examples:

