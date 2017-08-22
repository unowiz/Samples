# oauth-aad-flow

Azure Active Directory の OAuth (v1.0/v2.0) のサンプルです。

- Authorization Code Grant (v1 Endopoint)
- Authorization Code Grant (v2 Endopoint)
- Implicit Code Grant (v1 Endopoint)
- Implicit Code Grant (v2 Endopoint)
- Client Credentials Grant (v1 Endopoint)
- Client Credentials Grant (v2 Endopoint)

## 実行方法

### アプリケーションの登録

[Application Registration Portal](https://apps.dev.microsoft.com) を表示します。

\[アプリの追加\] からアプリケーションを登録します。

|項目|値|
|---|---|
|名前|WebApplication1|

\[新しいパスワードを作成\] から共有シークレットを作成します。

\[プラットフォームの追加\] からプラットフォームを追加します。

|項目|値|
|---|---|
|プラットフォーム|Web|
|暗黙的フローを許可する|はい|
|リダイレクト URL|http://localhost:52238/AuthorizationCode/GetTokenV1|
||http://localhost:52238/AuthorizationCode/GetTokenV2|
||http://localhost:52238/Home/Index|
||http://localhost:52238|

\[Microsoft Graph のアクセス許可\] のアクセス許可を追加します。

|項目|値|
|---|---|
|委任されたアクセス許可|Files.Read|
||User.Read|
||offline_access|
|アプリケーションのアクセス許可|User.Read.Add|

### プロジェクトの設定

プロジェクトの Properties/Settings.settings を開き、以下を変更します。

|項目|値|
|---|---|
|TenantName|テナント名|
|ClientId|アプリケーション ID|
|ClientSecret|共有シークレット|
