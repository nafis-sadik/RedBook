export declare interface IParamModel {
    /**
     * Default value: null
     * Description: Text to search in database
     * @type {string}
     */
    searchString: string | null;
    /**
     * Default value:
     * Description: Selected page number
     * @type {Number}
     */
    pageNumber: number;
    /**
     * Nullable field: Yes
     * Default value: 0
     * Description: Default value for pagination options, Must be and index number of 'pageLengthOptions'.
     * @type {Number}
     */
    pageLength: number;
}
  