# Writing Figures Directly

> [!WARNING]
> This functionality is intended for advanced users who understand the format only.
> If you are unsure of what you're doing, avoid this feature as it can lead to permanent figure corruption.

In some rare cases, it might be helpful to write data to the figures without the safeguards of the public API of this library.
The <xref:SkanderNET.Figures.Figure> base class provides two methods for this.

<xref:SkanderNET.Figures.Figure.ForceWriteBlock(System.UInt32,System.Byte[])> first encrypts the data provided, then writes it to the file or portal.
The portal will still refuse to write to readonly sectors but for loaded files, these blocks can be modified.
<xref:SkanderNET.Figures.Figure.ForceWriteBlockDirect(System.UInt32,System.Byte[])> writes the block without encryption. 
It is assumed that you have already encrypted the data when calling this function.

The length of the data block you provide to these functions must be `0x10` or an exception will be raised.
