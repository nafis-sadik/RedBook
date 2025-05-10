export class RouteModel {
    /**
     * Route Unique Identifier
     * @type {Number}
     */
    routeId: number = 0;

    /**
     * Display Name of route
     * @type {string}
     */
    routeName: string = '';

    /**
     * Routing path
     * @type {string}
     */
    routeValue: string = '';

    /**
     * Description on the route path or page that the route redirects to
     * @type {string}
     */
    description: string = '';

    /**
     * Unique Identifier of the application that the route belongs to
     * @type {string}
     */
    applicationId: number = 0;

    /**
     * Name of the application that this route belongs to
     * @type {string}
     */
    applicationName: string = '';

    /**
     * Parent route Id
     * @type {number | null}
     */
    parentRouteId: number | null = null;

    /**
     * Route Type Id
     * @type {number}
     */
    routeTypeId: number = 0;

    /**
     * Toggle menu item visibility
     * @type {boolean}
     */
    isMenuRoute: boolean = true;

    /**
     * Menu item display sequence
     * @type {number}
     */
    sequence: number = 0;
}
