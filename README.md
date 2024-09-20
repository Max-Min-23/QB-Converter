# QB Converter
CubaseおよびStudio Oneのドラムマップを相互変換するツールです。Windowsでのみ利用可能です。  
![qb converter](https://github.com/user-attachments/assets/878b6527-3936-4053-9793-324ca711f110)

### 特徴
- ドラッグ＆ドロップの簡単操作
- drm,pitchlist,CSV,Textの４形式を変換、逆変換

### インストール
https://github.com/Max-Min-23/QB-Converter/releases/tag/v1.0.0

上記のリンクをクリックし'QB.Converter.1.0.0.zip'をクリックするとダウンロードされます。
"QB Converter.exe"のプロパティにセキュリティチェックがある場合は「許可」を行ってください。
レジストりは使用していませんので。不要になった場合はフォルダ毎削除してください。

### 使い方
"Select Convert Type"よりFrom（変換元）,To(変換先)を選択しウィンドウ内にファイルをドラッグ＆ドロップしてください。
複数まとめてドロップ可能です。
変換されたファイルは元ファイルと同じフォルダに出力されます。
同名のファイルがある場合は上書きされます。

### 変換タイプ
タイプ名 | 内容
--- | --- 
drm | Cubaseのドラムマップファイル
pitchlist | Studio Oneのピッチリストファイル
csv | CSVファイル
txt | テキストファイル

### "CSV Pitch Order"
チェックがない場合は元データの並び順で出力されます。  
チェックを入れると、CSV出力の際、並び順が0～127の音程順になります。
データの並び順が統一されるため、マップを比較する場合などに使用します。

### Cubase->Studio Oneへの変換について
Cubaseのドラムマップでは各音源の違いに対応できるよう、入力ノートと出力ノートに別々の音程を設定しマッピングすることができますが、Studio Oneにはマッピング機能がありません。
Studio Oneのドラムマップは出力ノートのみの指定となっています。
マッピングが設定されている（入力ノートと出力ノートが異なる）Cubaseのドラムマップを変換する際、重複する出力ノートがある場合はエラー"Dupulicate ONote"が表示されます。
その際は、一度CSVへ変換、CSVファイルを編集※必要以外の重複する行を削除、CSVファイルから変換してください。

### CSVファイルフォーマット
列名 | 内容 | 備考
--- | --- | ---
Order | 表示順 | 
InPitch | 入力ピッチ番号 | 0～127
InNote | 入力ノート名 | 
OutPitch | 出力ピッチ番号 | 0～127
OutNote | 出力ノート名 | 
PitchName | ピッチ名 | 
Duplicate | 出力ノート重複 | 重複した出力ノートがある場合に＊がセットされます。
Check | 予備 | 
Tag | 予備 | 

### テキストファイルフォーマット
一行にタブ区切りでピッチ番号、ピッチ名を入力してください。
行頭の'#'はコメント行となります。


