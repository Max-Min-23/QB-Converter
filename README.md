# QB Converter
CubaseおよびStudio Oneのドラムマップを相互変換するツールです。Windowsで利用可能.  
![QB Converter Image](https://github.com/user-attachments/assets/70f7498e-fbd2-4669-a9be-c2c4a7801250)
### 特徴
- ７つの変換タイプ搭載
- ドラムマップ以外にCSVファイルへの変換も対応

### インストール
https://github.com/Max-Min-23/QB-Converter/blob/master/QB%20Converter%201.0.0.zip
上記のリンクをクリックし、右側にあるダウンロードボタンでダウンロードしてください。
"QB Converter.exe"のプロパティにセキュリティチェックがある場合は「許可」を行ってください。
レジストりは使用していませんので。不要になった場合はフォルダ毎削除してください。

### 使い方
"Select Convert Type"より変換タイプを選択し、"Drop File Here!"と表示されている部分にファイルをドラッグ＆ドロップするだけです。
複数まとめてドロップ可能です。
変換されたファイルは元ファイルと同じフォルダに出力されます。
同名のファイルがある場合は上書きされます。

### 変換タイプ
タイプ名 | 変換内容
--- | --- 
*.drm -> *.pitchlist Based On In Pitch | Cubaseの*.drmファイルを、入力ノートを基準にStudio Oneの*.pitchlistへ変換します
*.drm -> *.pitchlist Based On Out Pitch | Cubaseの*.drmファイルを、出力ノートを基準にStudio Oneの*.pitchlistへ変換します
*.pitchlist -> *.drm | Studio Oneの*.pitchlistをCubasの*.drmファイルへ変換します
*.drm -> .csv | Cubaseの*.drmファイルを、CSVファイルへ変換します
*.Pitchlist -> .csv | Studio Oneの*.pitchlistファイルを、CSVファイルへ変換します
*.csv -> *.pitchlist | CSVファイルをCubasの*.drmファイルへ変換します
*.csv -> *.drm | Studio Oneの*.pitchlistへ変換します

### "CSV Pitch Order"
チェックを入れると、CSV出力の際、並び順が0～127の音程順になります。
チェックがない場合は元データの並び順で出力されます。

### Cubase->Studio Oneへの変換について
Cubaseのドラムマップでは各音源の違いに対応できるよう、入力ノートと出力ノートに別々の音程を設定しマッピングすることができますが、Studio Oneにはマッピング機能がありません。
Studio Oneのドラムマップは出力ノートのみの指定となっています。
マッピングが設定されている（入力ノートと出力ノートが異なる）Cubaseのドラムマップを変換する際、重複する出力ノートがある場合はエラー"Dupulicate ONote"が表示されます。
その際は、一度CSVへ変換、CSVファイルを編集、CSVファイルから変換してください。

### CSVファイルフォーマット
列名 | 内容 | 備考
--- | --- | ---
Order | 表示順 | 
Pitch | 入力ノート | 
Note | 入力ノート名 | 
Out Pitch | 出力ノート | 
Out Note | 出力ノート名 | 
Name | インストルメント名 | 
Duplicate | 出力ノート重複 | 重複した出力ノートがある場合に＊がセットされます。  CSVからの変換の際、空白以外が設定された行は無視されます。
Check | 強制出力 | CSVからの変換する際、空白以外を設定しておくと強制的に取り込みされます

