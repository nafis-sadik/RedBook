export declare interface IOrganizationModel{
    /**
     * Item Title
     * @type {Number}
     */
    organizationId: number;
    /**
     * Business Title
     * @type {string}
     */
    organizationName: string;
    /**
     * Collection of addresses of a business with corresponding Address Ids
     * @type { { [ key: number ]: string }[] | null }
     */
    address: { [ key: number ]: string }[] | null;
}
