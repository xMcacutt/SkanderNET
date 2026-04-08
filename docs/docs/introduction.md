# Introduction

SkanderNET is a C#, multiframework library for communicating with the Skylanders Portal of Power
to read and modify figures placed onto the portal. It also provides support for dumping and modifying the dumps of Skylanders.

The library has pre-built targets for
- Net Framework 3.5 - 4.5
- NetStandard 2.0, 2.1
- NetCore 6.0, 7.0, 8.0, 9.0

The code is designed to support anything post net35 so manual builds of later frameworks should be compatible.

Much of the code throughout this documentation will be designed around net35 patterns.
Many optimizations can be made using more mordern dotnet features.

This library was created primarily for integration with the game Keep Talking & Nobody Explodes.
The game is built on net35 hence the compatibility requirement. However, it is (at time of writing) the most comprehensive
library that exists for modifying Skylanders without significant setup.

# Legal
The license requirements for using this library are a little unorthodox.
The library is licensed under PolyForm Non-Commercial, motivated by a desire to prevent 
any bad actors damaging the community or markets using this library to create commercial products.

This library depends on libusb, which is licensed under the Lesser GPL (LGPL).
Because of the LGPL, this library can **dynamically link** to libusb without requiring the entire project to be LGPL-licensed.
It is important that libusb is never compiled into the same binary as your own code,
as doing so could create a derivative work subject to LGPL obligations.
Therefore, it is vital that all works using this library must dynamically link against libusb.

PolyForm NC requires that derivative works of this library inherit the same non-commercial conditions,
but allows sublicensing under a different license as long as the non-commercial requirement is maintained.

**TL;DR**
- Always **dynamically link** against libusb.
- Ensure your use is **non-commercial** according to PolyForm NC.
- Include the **PolyForm notice** in your distribution.

For full details, please consult the licenses themselves.

# Acknowledgements

- [NefariousTechSupport](https://github.com/NefariousTechSupport/Runes/)
- Brandon Wilson
- WinnerNombre
- Texthead
- Mandar1jn
- Maff
- CallMeZero

Thank you to everyone who has previously done any work to document the formats.