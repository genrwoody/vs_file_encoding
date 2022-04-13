# FileEncoding

[中文](./README.zh.md) | English

## Introduction
It's a Visual Studio Extension, show text file encoding at editor right bottom corner, click button to convert encoding.

Recommend set charset with .editorconfig.

## Screenshots

![Preview](docs/screenshots/Preview.png?raw=true "Preview")

NOTES:
- Line 1: The **Default** character set that depends on your PC;
- Line 2: **UTF-8** (without BOM) is suitable for files that
    - will be processed by an application that can NOT recongnize the BOM (eg. old `PHP` files), or 
    - is NOT necessary to use a BOM since they have already included a meta tag to specific the charset(`charset=utf-8`) such as `html` files, or 
    - will never contain any multi-byte characters;
- Line 3: **UTF-8 BOM** is recommended for documents contain multi-byte characters.

## License
[MIT](LICENSE.txt)