export class OrganizationModel{
    /**
     * Item Title
     * @type {Number}
     */
    organizationId: number = 0;
    /**
     * Business Title
     * @type {string}
     */
    organizationName: string = '';
    /**
     * Business Title
     * @type {string}
     */
    organizationAddress: string = '';
    /**
     * Business Logo
     * @type {string}
     */
    businessLogo: string | null = null;
    /**
     * Subscription Monthly Fee
     * @type {number}
     */
    subscriptionFee: number = 0;
}
