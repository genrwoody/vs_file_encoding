# FileEncoding

中文 | [English](./README.md)

## 介绍
本 Visual Studio 扩展（vsix）在Visual Studio的编辑器边栏的右下角显示文档的编码, 点击后可修改为您指定的编码。

- **保持切换前文件的状态**：若切换到另一编码前，文件状态是未保存状态，则切换后也是未保存状态；若切换前文件不是修改状态，则切换后自动保存。

## Screenshots

![Preview](docs/screenshots/Preview.png?raw=true "Preview")

关于上图中三个选项的说明：
- 第1行：`默认`编码，与您的计算机操作系统当前配置有关。
- 第2行：`UTF-8` (不带BOM)
    - 有一些旧的程序没有判断处理那些以BOM开头的文件，那些BOM字节会造成错误的字节流输出，例如早期的PHP文件不能带BOM。
    - 那些已经指定了"*charset=utf-8*"的文件（例如html布局文件），没有必要带BOM。
    - 那些**不可能**有多字节字符的文件，当然也没有必要带BOM。
- 第3行：`UTF-8 BOM`
    - 含有多字节字符的文件，如果处理程序支持带BOM，一般推荐用这个选项，既可以加快字节流处理速度，又可以避免一些*莫名其妙*乱码的问题。

## License
[MIT](LICENSE.txt)

简而言之，您可以针对本程序做任何修改（包括且不限于命名、徽标、源代码），也可以自由分发或再发布。我们只希望您保留以下授权声明：
```
Copyright (c) 2021 genrwoody
Copyright (c) 2022 Myvas Foundation
```
（您的声明可以附加在上述声明之后。）