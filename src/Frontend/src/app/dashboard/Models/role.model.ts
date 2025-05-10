export class RoleModel {
  /**
   * Role Unique Id
   * @type {Number}
   */
  roleId: number;
  /**
   * Name of Role
   * @type {string}
   */
  roleName: string;
  /**
   * Unique Id of the business that contains this role
   * @type {Number}
   */
  organizationId: number;
  /**
   * If the role is an admin role or not
   * @type {boolean}
   */
  isAdmin: boolean;
}
