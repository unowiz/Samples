import { Version } from '@microsoft/sp-core-library';
import {
    BaseClientSideWebPart,
    IPropertyPaneConfiguration,
    PropertyPaneTextField
} from '@microsoft/sp-webpart-base';
import { escape } from '@microsoft/sp-lodash-subset';
import {
    GraphHttpClient,
    GraphHttpClientResponse
} from '@microsoft/sp-http';

import styles from './SharePointFrameworkUsingGraphHttpClientWebPart.module.scss';
import * as strings from 'SharePointFrameworkUsingGraphHttpClientWebPartStrings';

export interface ISharePointFrameworkUsingGraphHttpClientWebPartProps {
    description: string;
}

export default class SharePointFrameworkUsingGraphHttpClientWebPart extends BaseClientSideWebPart<ISharePointFrameworkUsingGraphHttpClientWebPartProps> {

    public render(): void {
        this.context.graphHttpClient.get(`v1.0/me`, GraphHttpClient.configurations.v1)
            .then((response: GraphHttpClientResponse) => {
                if (response.ok) {
                    return response.json();
                } else {
                    console.warn(response.statusText);
                }
            })
            .then((data: any) => {
                this.domElement.innerHTML = `
                    <div class="${styles.sharepointframeworkusinggraphhttpclient}">
                        <div class="${styles.container}">
                            <div class="${styles.row}">
                                <div class="${styles.column}">
                                    <div>${strings.welcomeMessage.replace(/{displayName}/, data.displayName)}</div>
                                </div>
                            </div>
                        </div>
                    </div>`;
            });
    }

    protected get dataVersion(): Version {
        return Version.parse('1.0');
    }

    protected getPropertyPaneConfiguration(): IPropertyPaneConfiguration {
        return null;
    }

}
