# FileEncoding

[中文](./README.zh.md) | English

## What's this?
This Visual Studio Extension will display the charset of current document at the right bottom margin of the editor; Click to popup a contextual menu with a list of encodings and choose the encoding that you want to convert the document to.
It's a Visual Studio Extension, show text file encoding at editor right bottom corner, click button to convert encoding.

**NOTES**:
1. The file's modification status will be keep as the same as before. That means: if you select a different encoding to a dirty file, the file will be keep dirty; otherwise it will be automatically saved after changing the encoding.
2. UTF-8 as default: When the encoding could be regarded as both utf8 and the default locale encoding (e.g. `GB2312`), but we would like to display it as `UTF-8` instead of the default. So we always change the encoding from default to `UTF-8`.

## Screenshots

![Preview](docs/screenshots/Preview.png?raw=true "Preview")

NOTES:
- **UTF-8** (without [BOM](http://en.wikipedia.org/wiki/Byte_order_mark))
    - `html`|`xhtml`|`_Layout.cshtml`: It is not necessary to use a BOM if a output template has already pointed out its `charset=utf-8`.
    - `PHP`: In case of PHP files are usually as a template to output, it is not a good idea to save PHP files with the BOM at the beginning. Do not add BOM to a script file that cannot be correctly processed by its interpretor. 
    - `JSON`: Implementations must not add a **byte order mark** to the beginning of a JSON text. [[RFC 7159, Section 8.1]](https://www.rfc-editor.org/rfc/rfc7159#section-8.1) 
    - `sh`: POSIX (Unix-like) scripts are required to start with '#!' (e.g. `#!/bin/sh`, `#!/bin/bash`), so you MUST NOT add a BOM to the beginning.
- **UTF-8 BOM**
    - IMHO, it is recommended for a document without an indicator of charset or reader, and it posiblbly contains multi-byte characters, for two reasons:
        1. BOM is actually the most efficient way of identifying an UTF-8 file.
        2. Most modern applications and standards support and encourage the use of BOM.
        
**DISCUSS**: There are more and more new characters (e.g. emoji) appear in the comments, resources and elsewhere of soruce codes; An efficient and safe convention of interpreting a file into a human friendly visible artifact is required and more and more important in the future; The Unicode BOM is already an accept standard, so why people 'refuse' to use it? 
 
## License
[MIT License](LICENSE.txt)

```
Copyright (c) 2021 genrwoody
Copyright (c) 2022 Myvas Foundation
```
