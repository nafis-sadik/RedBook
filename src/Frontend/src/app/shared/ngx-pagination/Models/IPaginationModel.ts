export declare interface IPaginationModel<T> {
  /**
   * Default value:
   * Description: Heading for the card that shall contain the table
   * @type {string}
   */
  tableCardHeader: string;
  /**
   * Configuration for paging your table
   */
  tableConfig: {
    /**
     * Default value:
     * Description: Collumn names for table
     * @type {[key: string]: string}
     */
    tableMaping: { [key: string]: string };
    /**
     * Default value: null
     * Description: Callback method for onClick event on edit button
     * @type {Function}
     */
    onEdit: Function | null;
    /**
     * Default value: null
     * Description: Callback method for onClick event on delete button
     * @type {Function}
     */
    onDelete: Function | null;
    /**
     * Default value: null
     * Description: Callback method for onClick event on View Details button
     * @type {Function}
     */
    onView: Function | null;
    /**
     * Default value:
     * Description: Array of items to be displayed in current page
     * @type {T | null}
     */
    sourceData: T[]
    /**
     * Default value:
     * Description: width of column to be displayed in current page
     * @type {string}
     */
    actionColWidth: string | null;
  } | null;
  /**
   * Configuration for pages
   */
  pagingConfig: {
    /**
     * Default value:
     * Description: Selected page number
     * @type {Number}
     */
    pageNumber: number;
    /**
     * Default value: 0
     * Description: Default value for pagination options, Must be and index number of 'pageLengthOptions'.
     * @type {Number}
     */
    pageLength: number;
    /**
     * Default value:
     * Description: Number of total items in database
     * @type {Number}
     */
    totalItems: number;
    /**
     * Default value: null
     * Description: Callback method for onClick event
     * @type {Function}
     */
    onUpdate: Function;
  } | null;
  /**
   * Configuration for search field
   */
  searchingConfig: {
    /**
     * Default value: null
     * Description: Text to search in database
     * @type {string}
     */
    searchString: string;
    /**
     * Default value: 'Search Here'
     * Description: Place holder text for search input field
     * @type {string | null}
     */
    inputFieldPlaceholder: string | null;
    /**
     * Default value: 'Search Here'
     * Description: Place holder text for search input field
     * @type {string | null}
     */
    buttonLabel: string | null;
    /**
     * Default value: 'Search Here'
     * Description: Place holder text for search input field
     * @type {boolean}
     */
    showIcon: boolean;
    /**
     * Default value: null
     * Description: Callback method for onClick event
     * @type {Function}
     */
    onSearch: Function;
  } | null;
  /**
   * Configuration for add new button
   */
  addNewElementButtonConfig: {
    /**
     * Default value: true
     * Description: View add icon visibility
     * @type {boolean}
     */
    showIcon: boolean;
    /**
     * Default value: null
     * Description: Label of search button
     * @type {string}
     */
    buttonLabel: string | null;
    /**
     * Description: Callback method for onClick event
     * @type {Function}
     */
    onAdd: Function;
  } | null;

  organizationId: number;
}
