# QB Converter
A tool to convert Cubase and Studio One drum maps to each other. 
CubaseおよびStudio Oneのドラムマップを相互変換するツールです。Windowsで利用可能.  
![QB Converter Image](https://github.com/user-attachments/assets/70f7498e-fbd2-4669-a9be-c2c4a7801250)
### 特徴
- ７つの変換タイプ搭載
- ドラムマップ以外にCSVファイルへの変換も対応
### 使い方
変換タイプを選択し、"Drop File Here!"と表示されている部分にファイルをドラッグ＆ドロップするだけです。
変換されたファイルは元ファイルと同じフォルダに出力されます。
同名のファイルがある場合は上書きされます。
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
Order | 内容B1 | 内容C1
Pitch | 内容B2 | 内容C2
Note | 内容B3 | 内容C3
Out Pitch | 内容B3 | 内容C3
Out Note | 内容B3 | 内容C3
Name | 内容B3 | 内容C3
Duplicate | 内容B3 | 内容C3
Check | 内容B3 | 内容C3

