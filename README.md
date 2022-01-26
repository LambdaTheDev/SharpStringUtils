# SharpStringUtils
SharpStringUtils is a library that lets you perform high performance or/and non-alloc operations on C# strings.

# Current features:
- Available on NuGet: https://www.nuget.org/packages/SharpStringUtils/
- Non-alloc base64 encoding/decoding
- Non-alloc string encoding operations
- Small-alloc string splitter (right now, only a char separator is supported)
- Possibility to validate string characters while splitting them, by providing custom validator method
- Possibility to validate StringSegment by providing custom validator method
- (For .NET Standard 2.1) Extension method to convert StringSegment into ReadOnlySpan<char>
- (For people who like taking big risks) Access to string.FastAllocateString(int) method
- Commented code. Whenever you get stuck, go to source & read comments
- Methods that don't allocate memory are suffixed with "NonAlloc"
- .NET Standard 2.0 (Unity) is fully supported

# Unit & Performance tests
I have conducted unit & performance tests (you can see them in 2 other projects) to ensure that everything
 is working as it should and if it doesn't allocate, but currently they are not clear/written chaotically. 
 All of them will re-coded in nearest future, and performance test results will be shown here
 
 # Important note about using this library
 This lib re-uses managed resources, like arrays, to reduce runtime allocations to minimum. And, due to this,
  every usage of this library functionality overrides previously generated result. Remember to ensure thread-safety
  in your application, and to use results immediately after you got them!  
 
 # Third-party content
 I have used third-party content in this library. Check THIRD-PARTY-NOTICE.txt file!
 
 # For contributors
 If you want to contribute to this project, please make 1 PR per new feature/fix, so it's easy for me to review them!