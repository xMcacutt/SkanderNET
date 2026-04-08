# Loading Figures From File

If you happen to be using digitized versions of your figures, you can also load these files into <xref:SkanderNET.Figures.Figure> objects even if a portal isn't connected.

<xref:SkanderNET.Figures.FigureSession> is a mostly internal class used as a middleman between the portal and the figure creation.

However, it exposes the method <xref:SkanderNET.Figures.FigureSession.LoadFromFile(System.IO.FileStream)>.

When calling this method, you should first open and provide a `FileStream`. You are responsible for cleaning up the filestream.

```csharp
var fileStream = File.Open($"./Path/To/File", FileMode.Open, FileAccess.ReadWrite);
var figure = FigureSession.LoadFromFile(_fileStream);
```

Provided that the file is valid, encrypted figure data, this will produce a valid figure that can be modified as if placed on the portal.
When the figure is saved, it will save directly back to the file.
Therefore, the `FileStream` should not be destroyed until modification is expected to be over for that figure.

> [!WARNING]
> The `FileStream` must be created with the correct `FileAccess` for the expected action.
> This means it should have write access if you expect to be modifying the figure.