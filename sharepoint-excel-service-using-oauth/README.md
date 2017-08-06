# sharepoint-excel-service-using-oauth

OAuth を使用して SharePoint Online の Excel Services (SOAP API) を実行するサンプルです。

[SharePoint Online の Excel Services (SOAP API) を叩いてみる](http://blog.karamem0.jp/entry/2017/06/13/190000)

## 実行方法

* Azure Active Directory にアプリケーションを登録します。
* Program.cs の TenantName, ClientId の値を書き換えます。
* アプリケーションのアクセス許可に SharePoint Online を追加します。
* Visual Studio の [サービス参照の追加] から「<SharePoint サイトの URL>/_vti_bin/ExcelService.asmx」を追加します。
* プロジェクトを実行するとデバイス コードの入力を要求されるので、ブラウザーで表示されたデバイス コードを入力します。
