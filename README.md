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
Order | 表示順 | 
Pitch | 入力ノート | 
Note | 入力ノート名 | 
Out Pitch | 出力ノート | 
Out Note | 出力ノート名 | 
Name | インストルメント名 | 
Duplicate | 出力ノート重複 | 
Check | 強制出力 | 

