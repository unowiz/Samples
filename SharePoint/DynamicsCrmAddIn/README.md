# DynamicsCrmAddIn

SharePoint ホスト型のアドインから Dynamics CRM Web API を実行するサンプルです。

## 実行方法

* Azure Active Directory にアプリケーションを登録します。
* プロジェクトの Scripts/WebPart.js を開き、クライアント ID および Dynamics CRM の接続先の URL を変更します。
* SharePoint Online に開発者向けサイトを作成します。Visual Studio 2015 でプロジェクトを SharePoint Online に配置します。出力ログに配置先の URL が出力されるので控えておきます。
* Azure Active Directory の 応答 URL を配置先の URL に変更します。
* アプリケーションのアクセス許可に Dyamics CRM Online を追加します。
* マニフェスト ファイルをダウンロードして oauth2AllowImplicitFlow を true に変更しアップロードします。
* 開発者サイトに新しいページを作成し、アプリ パーツを貼り付けます。
