# FileEncoding

[中文](./README.zh.md) | English

## Introduction
This Visual Studio Extensions (vsix) will display the charset of current document at the right bottom margin of the editor; Click to convert to your choice.
It's a Visual Studio Extension, show text file encoding at editor right bottom corner, click button to convert encoding.

**NOTES**: The file's modification status will be keep as the same as before. That means: if you select a different encoding to a dirty file, the file will be keep dirty; otherwise it will be automatically saved after changing the encoding.

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

```
Copyright (c) 2021 genrwoody
Copyright (c) 2022 Myvas Foundation
```