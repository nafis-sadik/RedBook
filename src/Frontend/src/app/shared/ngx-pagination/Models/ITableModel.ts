export declare interface ITableModel {
  /**
   * Labels for collumn of HTML table
   * @type {Array<string>}
   */
  columnNames: Array<string>;
  /**
   * Source data object array
   * @type {Array<any>}
   */
  sourceData: Array<any>;
  /**
   * Mapped data to render on table UI
   * @type {Array<Array<any>>}
   */
  mappedData: Array<Array<any>>;
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
     * Description: width of column to be displayed in current page
     * @type {string}
     */
  actionColWidth: string;
}
