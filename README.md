# SkanderNET
![](./SocialImage.png)
A multiframework compatible library for connecting to Skylanders portals.

---

## Usage & Documentation

This library depends on libusb. You must manually provide this library. Be aware that bundling this library with libusb as
a single library is not permitted as discussed in [Licensing](#licensing).

Use the correct libusb dll for your build system and architecture and provide it as a dll with your library or application.
Avoid statically linking against libusb when using SkanderNET.
[The libusb project can be found here](https://github.com/libusb/libusb/releases)

**[Full documentation and guidance on using this library can be found here](https://xmcacutt.github.io/SkanderNET/docs/introduction.html)**

---

## Licensing

The full license for this project can be found in the License file provided but please note the following:
This project is licensed under Polyform Non-Commercial 1.0.0. 
Any modification of this library is permitted but must be redistributed under Polyform Non-Commercial 1.0.0.

This library also interfaces with libusb, which is licensed under LGPL3.
As a result, any distribution of this library must maintain all libusb as a separate library.
libusb therefore remains licensed under LGPL and SkanderNET under Polyform.
Statically linking both libraries into a single library or binary is prohibited as it forms a license conflict.

Any work distributed with this library may not be used for any commercial gains as expressed in the full Polyform license.
