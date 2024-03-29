# vs_file_encoding

中文 | [English](./README.en-US.md)

#### 介绍
一个 Visual Studio 扩展, 在VS的文本编辑器右下角显示文档的编码, 点击按钮可修改编码.

建议使用.editorconfig设置编码.

#### 补充说明
有几个问题是说编码显示不准确, 这里做一下补充说明. 
这个插件只是把VS内部编码显示出来, 除非用户手动修改, 否则不会改变用户文件. 
由于历史原因, Windows默认是使用多字节字符集, 所以纯ASCII编码文件会被识别成本地编码(GB2312). 而很多年轻的编辑器没有历史包袱, 会直接把纯ASCII编码的文件识别成UTF-8编码, 这就造成了很多人所说的编码显示不准确. 
而我没有把纯ASCII编码文件显示成UTF-8是有如下原因:
1. VS没有标志纯ASCII编码文件, 需要手动识别, 会增加负荷;
2. 就算识别出纯ASCII编码的文件, 当用户手动输入汉字时, 由于内部编码是多字节编码, VS还是会用多字节编码来编码汉字;
3. 为了避免第二个问题, 需要在打开文件时手动识别并更改文件编码, 这就意味着要在用户不知情的情况下修改文件, 与插件的功能相悖.
4. 综合第二个和第三个问题, 需要只对纯ASCII编码文件更改编码, 这没有什么必要, 因为对于这种文件无论UTF-8还是多字节编码都没有区别.

下面对文件该使用什么编码给出一些建议:
1. 纯Windows环境, 使用 C/C++ 等语言, 无需关心编码, Windows对国际化做的非常好, 所有编码都可以正确识别和编辑.
2. 跨平台环境, 使用 C/C++ 等语言, 建议使用UTF-8(无BOM)编码, 并且如果文件不是纯ASCII编码, 建议在文件开头的注释中写一些汉字(比如文件用途, 作者名字, 版权信息等), 让编辑器更快识别出文件编码, 也会提高识别准确率. 这是因为Linux上的很多工具对多语言处理的并不好, 所以只能向它们妥协.
3. 使用 HTML/javascript/CSS 等语言, 这实际上是跨平台应用, 并且由于网络传输等原因, UTF-8更具优势, 建议同第二条.
