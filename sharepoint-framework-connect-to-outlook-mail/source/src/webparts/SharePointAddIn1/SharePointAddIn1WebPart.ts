import { Version } from '@microsoft/sp-core-library';
import {
    BaseClientSideWebPart,
    IPropertyPaneConfiguration,
    PropertyPaneTextField
} from '@microsoft/sp-webpart-base';
import { escape } from '@microsoft/sp-lodash-subset';

import styles from './SharePointAddIn1.module.scss';
import * as strings from 'sharePointAddIn1Strings';
import { ISharePointAddIn1WebPartProps } from './ISharePointAddIn1WebPartProps';

export default class SharePointAddIn1WebPart extends BaseClientSideWebPart<ISharePointAddIn1WebPartProps> {

    public render(): void {
        this.domElement.innerHTML = `
            <div class="${styles.container}">
            </div>`;
    }

    protected renderContent(): void {
        var container = this.domElement.getElementsByClassName(styles.container);
        var requestUrl = this.properties.resourceUrl + "/api/v2.0/me/mailfolders";  
        fetch(requestUrl, {
            method: "GET",
            headers: new Headers({
                "Accept": "application/json",
                "Authorization": `Bearer ${this.properties.accessToken}`
            })
        })
            .then(response => response.json())
            .then(data => {
                var items = data.value;
                for (var index = 0; index < items.length; index++) {
                    var displayName = items[index].DisplayName;
                    var unreadItemCount = items[index].UnreadItemCount;
                    container[0].innerHTML += `<div>${displayName}: ${unreadItemCount}</div>`;
                }
            });
    }

    protected onInit(): Promise<void> {
        if (window.location.hash == "") {
            if (this.properties.accessToken == null) {
                var redirectUrl = window.location.href.split("?")[0];
                var requestUrl = this.properties.authUrl + "?" +
                    "response_type=token" + "&" +
                    "client_id=" + encodeURI(this.properties.appId) + "&" +
                    "resource=" + encodeURI(this.properties.resourceUrl) + "&" +
                    "redirect_uri=" + encodeURI(redirectUrl);
                var popupWindow = window.open(requestUrl);
                var handle = setInterval((self) => {
                    if (popupWindow == null || popupWindow.closed == null || popupWindow.closed == true) {
                        clearInterval(handle);
                    }
                    try {
                        if (popupWindow.location.href.indexOf(redirectUrl) != -1) {
                            var hash = popupWindow.location.hash;
                            clearInterval(handle);
                            popupWindow.close();
                            var query = {};
                            var elements = hash.slice(1).split("&");
                            for (var index = 0; index < elements.length; index++) {
                                var pair = elements[index].split("=");
                                var key = decodeURIComponent(pair[0]);
                                var value = decodeURIComponent(pair[1]);
                                query[key] = value;
                            }
                            self.properties.accessToken = query["access_token"];
                            self.properties.refreshToken = query["refresh_token"];
                            self.renderContent();
                        }
                    } catch (e) { }
                }, 1, this);
            } else {
                this.renderContent();
            }
        }
        return super.onInit();
    }

    protected get dataVersion(): Version {
        return Version.parse('1.0');
    }

    protected getPropertyPaneConfiguration(): IPropertyPaneConfiguration {
        return {
            pages: [
                {
                    groups: [
                        {
                            groupName: strings.GeneralGroupName,
                            groupFields: [
                                PropertyPaneTextField('appId', {
                                    label: strings.AppIdFieldLabel
                                }),
                                PropertyPaneTextField('authUrl', {
                                    label: strings.AuthUrlFieldLabel
                                }),
                                PropertyPaneTextField('resourceUrl', {
                                    label: strings.ResourceUrlFieldLabel
                                })
                            ]
                        }
                    ]
                }
            ]
        };
    }
}
