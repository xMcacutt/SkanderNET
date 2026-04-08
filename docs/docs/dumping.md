# Dumping & Backing Up Figures

The <xref:SkanderNET.Figures.Figure> base class provides methods for dumping the data of a figure.

The data on a figure is encrypted with AES. The games are only capable of loading encrypted data. This data is illegible
and for debugging purposes, it is useful to be able to see the unencrypted data.

Therefore, the <xref:SkanderNET.Figures.Figure.DumpFullFigure(System.String,System.Boolean)> method accepts a boolean value for whether the data
should be dumped as encrypted or not.

It's also often easier to only view the data area without the access control blocks or header.
This can be dumped with <xref:SkanderNET.Figures.Figure.DumpDataArea(System.String)> which always dumps decrypted data.
