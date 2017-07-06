# OutlookAddIn

SharePoint ホスト型のアドインから Outlook Web API を実行するサンプルです。

## 実行方法

* Azure Active Directory にアプリケーションを登録します。
* プロジェクトの Scripts/WebPart.js を開き、クライアント ID を変更します。
* SharePoint Online に開発者向けサイトを作成します。Visual Studio 2015 でプロジェクトを SharePoint Online に配置します。出力ログに配置先の URL が出力されるので控えておきます。
* Azure Active Directory の 応答 URL を配置先の URL に変更します。
* アプリケーションのアクセス許可に Exchange Online を追加します。
* マニフェスト ファイルの oauth2AllowImplicitFlow を true に変更します。
* 開発者サイトに新しいページを作成し、アプリ パーツを貼り付けます。
