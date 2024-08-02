# vs_file_encoding

[中文](./README.md) | English

#### Introduction
It's a Visual Studio Extension, show text file encoding at editor right bottom corner, click button to convert encoding.

Recommend set charset with .editorconfig.

#### Supplementary explanation
There are several issues regarding inaccurate encoding display. Here is a supplementary explanation. 
This plugin only displays the internal code of VS and will not change the user file unless manually modified by the user. 
Due to historical reasons, Windows defaults to using a multi byte character set, so pure ASCII encoded files will be recognized as local encoding (eg: GB2312) However, many young editors do not have historical baggage and directly recognize pure ASCII encoded files as UTF-8 encoding, which leads to what many people call inaccurate encoding display. 
And I didn't display the pure ASCII encoded file as UTF-8 for the following reasons:
1. VS does not have a flag for pure ASCII encoded files and requires manual recognition, which will increase the load;
2. Even if a pure ASCII encoded file is recognized, when the user manually inputs Chinese characters, VS will still use multi byte encoding to encode Chinese characters due to the internal encoding being multi byte encoding;
3. To avoid the second issue, it is necessary to manually identify and change the file encoding when opening the file, which means modifying the file without the user's knowledge, which contradicts the functionality of the plugin;
4. Taking into account the second and third questions, it is not necessary to change the encoding only for pure ASCII encoded files, as there is no difference between UTF-8 and multi byte encoding for such files.

Here are some suggestions on what encoding to use for files:
1. In a pure Windows environment, using languages such as C/C++, there is no need to worry about encoding. Windows is very good at internationalization, and all encoding can be correctly recognized and edited;
2. Cross platform environment, using languages such as C/C++, it is recommended to use UTF-8 (no BOM) encoding. If the file is not pure ASCII encoding, it is recommended to write some Chinese characters (such as file purpose, author name, copyright information, etc.) in the comments at the beginning of the file to help the editor recognize the file encoding faster and improve recognition accuracy. This is because many tools on Linux are not good at handling multiple languages, so we can only compromise with them;
3. Using languages such as HTML/JavaScript/CSS is actually a cross platform application, and due to network transmission and other reasons, UTF-8 has more advantages. It is recommended to follow the second option.
