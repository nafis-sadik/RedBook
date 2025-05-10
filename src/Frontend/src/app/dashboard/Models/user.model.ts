import { environment } from "src/environments/environment.development";
import { RoleModel } from "./role.model";

export class UserModel {
  /**
   * Unique Identifier of specific user
   * @type {number}
   */
  userId: number = 0;
  /**
   * Email address of user
   * @type {string}
   */
  email: string = '';
  /**
   * Phone number of user
   * @type {string}
   */
  phoneNumber: string = '';
  /**
   * User name of user
   * @type {string}
   */
  userName: string = '';
  /**
   * First name of user
   * @type {string}
   */
  firstName: string = '';
  /**
   * User name of user
   * @type {string}
   */
  lastName: string = '';
  /**
   * User name of user
   * @type {string}
   */
  password: string = '';
  /**
   * Current account balance of user
   * @type {number}
   */
  accountBalance: number = 0;
  /**
   * Role ids assigned to user from the user form (Only for form control)
   * @type {Array<number>}
   */
  userRoleIds: Array<number> = [];
  /**
   * User role names seperated by comma in a single string to be displayed on table
   * @type {string}
   */
  roleNames: string = '';
  /**
   * Array of user roles
   * @type {Array<RoleModel>}
   */
  userRoles: Array<RoleModel> = [];
  /**
   * Application id of the application this user is making the request from
   * @type {Array<string>}
   */
  applicationId: number = 1;
  /**
   * Id of the organization that the user is associated with and trying to run the operation upon
   * @type {Array<string>}
   */
  organizationId: number = 0;
}
