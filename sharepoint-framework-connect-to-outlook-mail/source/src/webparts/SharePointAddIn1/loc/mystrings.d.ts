declare interface ISharePointAddIn1Strings {
    GeneralGroupName: string;
    AppIdFieldLabel: string;
    AuthUrlFieldLabel: string;
    ResourceUrlFieldLabel: string;
}

declare module 'sharePointAddIn1Strings' {
    const strings: ISharePointAddIn1Strings;
    export = strings;
}
