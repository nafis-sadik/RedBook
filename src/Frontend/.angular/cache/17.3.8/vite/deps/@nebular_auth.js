import {
  NB_WINDOW,
  NbAlertComponent,
  NbAlertModule,
  NbButtonComponent,
  NbButtonModule,
  NbCardBodyComponent,
  NbCardComponent,
  NbCardHeaderComponent,
  NbCardModule,
  NbCheckboxComponent,
  NbCheckboxModule,
  NbIconComponent,
  NbIconModule,
  NbInputDirective,
  NbInputModule,
  NbLayoutColumnComponent,
  NbLayoutComponent,
  NbLayoutModule
} from "./chunk-3BBPLQMJ.js";
import "./chunk-IIRQSIIO.js";
import {
  DefaultValueAccessor,
  FormsModule,
  MaxLengthValidator,
  MinLengthValidator,
  NgControlStatus,
  NgControlStatusGroup,
  NgForm,
  NgModel,
  PatternValidator,
  RequiredValidator,
  ɵNgNoValidate
} from "./chunk-WZKEDKVK.js";
import {
  ActivatedRoute,
  Router,
  RouterLink,
  RouterModule,
  RouterOutlet
} from "./chunk-NN7MDTWB.js";
import "./chunk-2IJ3QWNU.js";
import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders,
  HttpResponse
} from "./chunk-OXWKBFTS.js";
import {
  CommonModule,
  Location,
  NgForOf,
  NgIf
} from "./chunk-H2OSWUDB.js";
import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  Inject,
  Injectable,
  InjectionToken,
  Injector,
  NgModule,
  setClassMetadata,
  ɵɵadvance,
  ɵɵattribute,
  ɵɵclassProp,
  ɵɵdefineComponent,
  ɵɵdefineInjectable,
  ɵɵdefineInjector,
  ɵɵdefineNgModule,
  ɵɵdirectiveInject,
  ɵɵelement,
  ɵɵelementContainerEnd,
  ɵɵelementContainerStart,
  ɵɵelementEnd,
  ɵɵelementStart,
  ɵɵgetCurrentView,
  ɵɵgetInheritedFactory,
  ɵɵinject,
  ɵɵlistener,
  ɵɵnextContext,
  ɵɵprojection,
  ɵɵprojectionDef,
  ɵɵproperty,
  ɵɵreference,
  ɵɵresetView,
  ɵɵrestoreView,
  ɵɵsanitizeUrl,
  ɵɵtemplate,
  ɵɵtemplateRefExtractor,
  ɵɵtext,
  ɵɵtextInterpolate,
  ɵɵtextInterpolate2,
  ɵɵtwoWayBindingSet,
  ɵɵtwoWayListener,
  ɵɵtwoWayProperty
} from "./chunk-TEAXYPYX.js";
import {
  BehaviorSubject,
  Subject,
  catchError,
  delay,
  filter,
  map,
  of,
  share,
  switchMap,
  takeUntil
} from "./chunk-4J25ECOH.js";
import {
  __spreadProps,
  __spreadValues
} from "./chunk-ASLTLD6L.js";

// node_modules/@nebular/auth/fesm2022/nebular-auth.mjs
var _c0 = ["*"];
function NbLoginComponent_nb_alert_4_li_5_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementStart(0, "li", 27);
    ɵɵtext(1);
    ɵɵelementEnd();
  }
  if (rf & 2) {
    const error_r2 = ctx.$implicit;
    ɵɵadvance();
    ɵɵtextInterpolate(error_r2);
  }
}
function NbLoginComponent_nb_alert_4_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementStart(0, "nb-alert", 23)(1, "p", 24)(2, "b");
    ɵɵtext(3, "Oh snap!");
    ɵɵelementEnd()();
    ɵɵelementStart(4, "ul", 25);
    ɵɵtemplate(5, NbLoginComponent_nb_alert_4_li_5_Template, 2, 1, "li", 26);
    ɵɵelementEnd()();
  }
  if (rf & 2) {
    const ctx_r2 = ɵɵnextContext();
    ɵɵadvance(5);
    ɵɵproperty("ngForOf", ctx_r2.errors);
  }
}
function NbLoginComponent_nb_alert_5_li_5_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementStart(0, "li", 27);
    ɵɵtext(1);
    ɵɵelementEnd();
  }
  if (rf & 2) {
    const message_r4 = ctx.$implicit;
    ɵɵadvance();
    ɵɵtextInterpolate(message_r4);
  }
}
function NbLoginComponent_nb_alert_5_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementStart(0, "nb-alert", 28)(1, "p", 24)(2, "b");
    ɵɵtext(3, "Hooray!");
    ɵɵelementEnd()();
    ɵɵelementStart(4, "ul", 25);
    ɵɵtemplate(5, NbLoginComponent_nb_alert_5_li_5_Template, 2, 1, "li", 26);
    ɵɵelementEnd()();
  }
  if (rf & 2) {
    const ctx_r2 = ɵɵnextContext();
    ɵɵadvance(5);
    ɵɵproperty("ngForOf", ctx_r2.messages);
  }
}
function NbLoginComponent_ng_container_13_p_1_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementStart(0, "p", 30);
    ɵɵtext(1, " Email is required! ");
    ɵɵelementEnd();
  }
}
function NbLoginComponent_ng_container_13_p_2_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementStart(0, "p", 30);
    ɵɵtext(1, " Email should be the real one! ");
    ɵɵelementEnd();
  }
}
function NbLoginComponent_ng_container_13_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementContainerStart(0);
    ɵɵtemplate(1, NbLoginComponent_ng_container_13_p_1_Template, 2, 0, "p", 29)(2, NbLoginComponent_ng_container_13_p_2_Template, 2, 0, "p", 29);
    ɵɵelementContainerEnd();
  }
  if (rf & 2) {
    ɵɵnextContext();
    const email_r5 = ɵɵreference(12);
    ɵɵadvance();
    ɵɵproperty("ngIf", email_r5.errors == null ? null : email_r5.errors.required);
    ɵɵadvance();
    ɵɵproperty("ngIf", email_r5.errors == null ? null : email_r5.errors.pattern);
  }
}
function NbLoginComponent_ng_container_22_p_1_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementStart(0, "p", 30);
    ɵɵtext(1, " Password is required! ");
    ɵɵelementEnd();
  }
}
function NbLoginComponent_ng_container_22_p_2_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementStart(0, "p", 30);
    ɵɵtext(1);
    ɵɵelementEnd();
  }
  if (rf & 2) {
    const ctx_r2 = ɵɵnextContext(2);
    ɵɵadvance();
    ɵɵtextInterpolate2(" Password should contain from ", ctx_r2.getConfigValue("forms.validation.password.minLength"), " to ", ctx_r2.getConfigValue("forms.validation.password.maxLength"), " characters ");
  }
}
function NbLoginComponent_ng_container_22_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementContainerStart(0);
    ɵɵtemplate(1, NbLoginComponent_ng_container_22_p_1_Template, 2, 0, "p", 29)(2, NbLoginComponent_ng_container_22_p_2_Template, 2, 2, "p", 29);
    ɵɵelementContainerEnd();
  }
  if (rf & 2) {
    ɵɵnextContext();
    const password_r6 = ɵɵreference(21);
    ɵɵadvance();
    ɵɵproperty("ngIf", password_r6.errors == null ? null : password_r6.errors.required);
    ɵɵadvance();
    ɵɵproperty("ngIf", (password_r6.errors == null ? null : password_r6.errors.minlength) || (password_r6.errors == null ? null : password_r6.errors.maxlength));
  }
}
function NbLoginComponent_nb_checkbox_24_Template(rf, ctx) {
  if (rf & 1) {
    const _r7 = ɵɵgetCurrentView();
    ɵɵelementStart(0, "nb-checkbox", 31);
    ɵɵtwoWayListener("ngModelChange", function NbLoginComponent_nb_checkbox_24_Template_nb_checkbox_ngModelChange_0_listener($event) {
      ɵɵrestoreView(_r7);
      const ctx_r2 = ɵɵnextContext();
      ɵɵtwoWayBindingSet(ctx_r2.user.rememberMe, $event) || (ctx_r2.user.rememberMe = $event);
      return ɵɵresetView($event);
    });
    ɵɵtext(1, "Remember me");
    ɵɵelementEnd();
  }
  if (rf & 2) {
    const ctx_r2 = ɵɵnextContext();
    ɵɵtwoWayProperty("ngModel", ctx_r2.user.rememberMe);
  }
}
function NbLoginComponent_section_27_ng_container_3_a_1_nb_icon_1_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelement(0, "nb-icon", 39);
  }
  if (rf & 2) {
    const socialLink_r8 = ɵɵnextContext(2).$implicit;
    ɵɵproperty("icon", socialLink_r8.icon);
  }
}
function NbLoginComponent_section_27_ng_container_3_a_1_ng_template_2_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵtext(0);
  }
  if (rf & 2) {
    const socialLink_r8 = ɵɵnextContext(2).$implicit;
    ɵɵtextInterpolate(socialLink_r8.title);
  }
}
function NbLoginComponent_section_27_ng_container_3_a_1_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementStart(0, "a", 37);
    ɵɵtemplate(1, NbLoginComponent_section_27_ng_container_3_a_1_nb_icon_1_Template, 1, 1, "nb-icon", 38)(2, NbLoginComponent_section_27_ng_container_3_a_1_ng_template_2_Template, 1, 1, "ng-template", null, 3, ɵɵtemplateRefExtractor);
    ɵɵelementEnd();
  }
  if (rf & 2) {
    const title_r9 = ɵɵreference(3);
    const socialLink_r8 = ɵɵnextContext().$implicit;
    ɵɵclassProp("with-icon", socialLink_r8.icon);
    ɵɵproperty("routerLink", socialLink_r8.link);
    ɵɵattribute("target", socialLink_r8.target)("class", socialLink_r8.icon);
    ɵɵadvance();
    ɵɵproperty("ngIf", socialLink_r8.icon)("ngIfElse", title_r9);
  }
}
function NbLoginComponent_section_27_ng_container_3_a_2_nb_icon_1_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelement(0, "nb-icon", 39);
  }
  if (rf & 2) {
    const socialLink_r8 = ɵɵnextContext(2).$implicit;
    ɵɵproperty("icon", socialLink_r8.icon);
  }
}
function NbLoginComponent_section_27_ng_container_3_a_2_ng_template_2_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵtext(0);
  }
  if (rf & 2) {
    const socialLink_r8 = ɵɵnextContext(2).$implicit;
    ɵɵtextInterpolate(socialLink_r8.title);
  }
}
function NbLoginComponent_section_27_ng_container_3_a_2_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementStart(0, "a");
    ɵɵtemplate(1, NbLoginComponent_section_27_ng_container_3_a_2_nb_icon_1_Template, 1, 1, "nb-icon", 38)(2, NbLoginComponent_section_27_ng_container_3_a_2_ng_template_2_Template, 1, 1, "ng-template", null, 3, ɵɵtemplateRefExtractor);
    ɵɵelementEnd();
  }
  if (rf & 2) {
    const title_r10 = ɵɵreference(3);
    const socialLink_r8 = ɵɵnextContext().$implicit;
    ɵɵclassProp("with-icon", socialLink_r8.icon);
    ɵɵattribute("href", socialLink_r8.url, ɵɵsanitizeUrl)("target", socialLink_r8.target)("class", socialLink_r8.icon);
    ɵɵadvance();
    ɵɵproperty("ngIf", socialLink_r8.icon)("ngIfElse", title_r10);
  }
}
function NbLoginComponent_section_27_ng_container_3_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementContainerStart(0);
    ɵɵtemplate(1, NbLoginComponent_section_27_ng_container_3_a_1_Template, 4, 7, "a", 35)(2, NbLoginComponent_section_27_ng_container_3_a_2_Template, 4, 7, "a", 36);
    ɵɵelementContainerEnd();
  }
  if (rf & 2) {
    const socialLink_r8 = ctx.$implicit;
    ɵɵadvance();
    ɵɵproperty("ngIf", socialLink_r8.link);
    ɵɵadvance();
    ɵɵproperty("ngIf", socialLink_r8.url);
  }
}
function NbLoginComponent_section_27_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementStart(0, "section", 32);
    ɵɵtext(1, " or enter with: ");
    ɵɵelementStart(2, "div", 33);
    ɵɵtemplate(3, NbLoginComponent_section_27_ng_container_3_Template, 3, 2, "ng-container", 34);
    ɵɵelementEnd()();
  }
  if (rf & 2) {
    const ctx_r2 = ɵɵnextContext();
    ɵɵadvance(3);
    ɵɵproperty("ngForOf", ctx_r2.socialLinks);
  }
}
function NbRegisterComponent_nb_alert_2_li_5_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementStart(0, "li", 29);
    ɵɵtext(1);
    ɵɵelementEnd();
  }
  if (rf & 2) {
    const error_r2 = ctx.$implicit;
    ɵɵadvance();
    ɵɵtextInterpolate(error_r2);
  }
}
function NbRegisterComponent_nb_alert_2_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementStart(0, "nb-alert", 25)(1, "p", 26)(2, "b");
    ɵɵtext(3, "Oh snap!");
    ɵɵelementEnd()();
    ɵɵelementStart(4, "ul", 27);
    ɵɵtemplate(5, NbRegisterComponent_nb_alert_2_li_5_Template, 2, 1, "li", 28);
    ɵɵelementEnd()();
  }
  if (rf & 2) {
    const ctx_r2 = ɵɵnextContext();
    ɵɵadvance(5);
    ɵɵproperty("ngForOf", ctx_r2.errors);
  }
}
function NbRegisterComponent_nb_alert_3_li_5_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementStart(0, "li", 29);
    ɵɵtext(1);
    ɵɵelementEnd();
  }
  if (rf & 2) {
    const message_r4 = ctx.$implicit;
    ɵɵadvance();
    ɵɵtextInterpolate(message_r4);
  }
}
function NbRegisterComponent_nb_alert_3_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementStart(0, "nb-alert", 30)(1, "p", 26)(2, "b");
    ɵɵtext(3, "Hooray!");
    ɵɵelementEnd()();
    ɵɵelementStart(4, "ul", 27);
    ɵɵtemplate(5, NbRegisterComponent_nb_alert_3_li_5_Template, 2, 1, "li", 28);
    ɵɵelementEnd()();
  }
  if (rf & 2) {
    const ctx_r2 = ɵɵnextContext();
    ɵɵadvance(5);
    ɵɵproperty("ngForOf", ctx_r2.messages);
  }
}
function NbRegisterComponent_ng_container_11_p_1_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementStart(0, "p", 32);
    ɵɵtext(1, " Full name is required! ");
    ɵɵelementEnd();
  }
}
function NbRegisterComponent_ng_container_11_p_2_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementStart(0, "p", 32);
    ɵɵtext(1);
    ɵɵelementEnd();
  }
  if (rf & 2) {
    const ctx_r2 = ɵɵnextContext(2);
    ɵɵadvance();
    ɵɵtextInterpolate2(" Full name should contains from ", ctx_r2.getConfigValue("forms.validation.fullName.minLength"), " to ", ctx_r2.getConfigValue("forms.validation.fullName.maxLength"), " characters ");
  }
}
function NbRegisterComponent_ng_container_11_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementContainerStart(0);
    ɵɵtemplate(1, NbRegisterComponent_ng_container_11_p_1_Template, 2, 0, "p", 31)(2, NbRegisterComponent_ng_container_11_p_2_Template, 2, 2, "p", 31);
    ɵɵelementContainerEnd();
  }
  if (rf & 2) {
    ɵɵnextContext();
    const fullName_r5 = ɵɵreference(10);
    ɵɵadvance();
    ɵɵproperty("ngIf", fullName_r5.errors == null ? null : fullName_r5.errors.required);
    ɵɵadvance();
    ɵɵproperty("ngIf", (fullName_r5.errors == null ? null : fullName_r5.errors.minlength) || (fullName_r5.errors == null ? null : fullName_r5.errors.maxlength));
  }
}
function NbRegisterComponent_ng_container_17_p_1_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementStart(0, "p", 32);
    ɵɵtext(1, " Email is required! ");
    ɵɵelementEnd();
  }
}
function NbRegisterComponent_ng_container_17_p_2_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementStart(0, "p", 32);
    ɵɵtext(1, " Email should be the real one! ");
    ɵɵelementEnd();
  }
}
function NbRegisterComponent_ng_container_17_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementContainerStart(0);
    ɵɵtemplate(1, NbRegisterComponent_ng_container_17_p_1_Template, 2, 0, "p", 31)(2, NbRegisterComponent_ng_container_17_p_2_Template, 2, 0, "p", 31);
    ɵɵelementContainerEnd();
  }
  if (rf & 2) {
    ɵɵnextContext();
    const email_r6 = ɵɵreference(16);
    ɵɵadvance();
    ɵɵproperty("ngIf", email_r6.errors == null ? null : email_r6.errors.required);
    ɵɵadvance();
    ɵɵproperty("ngIf", email_r6.errors == null ? null : email_r6.errors.pattern);
  }
}
function NbRegisterComponent_ng_container_23_p_1_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementStart(0, "p", 32);
    ɵɵtext(1, " Password is required! ");
    ɵɵelementEnd();
  }
}
function NbRegisterComponent_ng_container_23_p_2_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementStart(0, "p", 32);
    ɵɵtext(1);
    ɵɵelementEnd();
  }
  if (rf & 2) {
    const ctx_r2 = ɵɵnextContext(2);
    ɵɵadvance();
    ɵɵtextInterpolate2(" Password should contain from ", ctx_r2.getConfigValue("forms.validation.password.minLength"), " to ", ctx_r2.getConfigValue("forms.validation.password.maxLength"), " characters ");
  }
}
function NbRegisterComponent_ng_container_23_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementContainerStart(0);
    ɵɵtemplate(1, NbRegisterComponent_ng_container_23_p_1_Template, 2, 0, "p", 31)(2, NbRegisterComponent_ng_container_23_p_2_Template, 2, 2, "p", 31);
    ɵɵelementContainerEnd();
  }
  if (rf & 2) {
    ɵɵnextContext();
    const password_r7 = ɵɵreference(22);
    ɵɵadvance();
    ɵɵproperty("ngIf", password_r7.errors == null ? null : password_r7.errors.required);
    ɵɵadvance();
    ɵɵproperty("ngIf", (password_r7.errors == null ? null : password_r7.errors.minlength) || (password_r7.errors == null ? null : password_r7.errors.maxlength));
  }
}
function NbRegisterComponent_ng_container_29_p_1_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementStart(0, "p", 32);
    ɵɵtext(1, " Password confirmation is required! ");
    ɵɵelementEnd();
  }
}
function NbRegisterComponent_ng_container_29_p_2_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementStart(0, "p", 32);
    ɵɵtext(1, " Password does not match the confirm password. ");
    ɵɵelementEnd();
  }
}
function NbRegisterComponent_ng_container_29_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementContainerStart(0);
    ɵɵtemplate(1, NbRegisterComponent_ng_container_29_p_1_Template, 2, 0, "p", 31)(2, NbRegisterComponent_ng_container_29_p_2_Template, 2, 0, "p", 31);
    ɵɵelementContainerEnd();
  }
  if (rf & 2) {
    ɵɵnextContext();
    const password_r7 = ɵɵreference(22);
    const rePass_r8 = ɵɵreference(28);
    ɵɵadvance();
    ɵɵproperty("ngIf", rePass_r8.errors == null ? null : rePass_r8.errors.required);
    ɵɵadvance();
    ɵɵproperty("ngIf", password_r7.value != rePass_r8.value && !(rePass_r8.errors == null ? null : rePass_r8.errors.required));
  }
}
function NbRegisterComponent_div_30_Template(rf, ctx) {
  if (rf & 1) {
    const _r9 = ɵɵgetCurrentView();
    ɵɵelementStart(0, "div", 33)(1, "nb-checkbox", 34);
    ɵɵtwoWayListener("ngModelChange", function NbRegisterComponent_div_30_Template_nb_checkbox_ngModelChange_1_listener($event) {
      ɵɵrestoreView(_r9);
      const ctx_r2 = ɵɵnextContext();
      ɵɵtwoWayBindingSet(ctx_r2.user.terms, $event) || (ctx_r2.user.terms = $event);
      return ɵɵresetView($event);
    });
    ɵɵtext(2, " Agree to ");
    ɵɵelementStart(3, "a", 35)(4, "strong");
    ɵɵtext(5, "Terms & Conditions");
    ɵɵelementEnd()()()();
  }
  if (rf & 2) {
    const ctx_r2 = ɵɵnextContext();
    ɵɵadvance();
    ɵɵtwoWayProperty("ngModel", ctx_r2.user.terms);
    ɵɵproperty("required", ctx_r2.getConfigValue("forms.register.terms"));
  }
}
function NbRegisterComponent_section_33_ng_container_3_a_1_nb_icon_1_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelement(0, "nb-icon", 43);
  }
  if (rf & 2) {
    const socialLink_r10 = ɵɵnextContext(2).$implicit;
    ɵɵproperty("icon", socialLink_r10.icon);
  }
}
function NbRegisterComponent_section_33_ng_container_3_a_1_ng_template_2_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵtext(0);
  }
  if (rf & 2) {
    const socialLink_r10 = ɵɵnextContext(2).$implicit;
    ɵɵtextInterpolate(socialLink_r10.title);
  }
}
function NbRegisterComponent_section_33_ng_container_3_a_1_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementStart(0, "a", 41);
    ɵɵtemplate(1, NbRegisterComponent_section_33_ng_container_3_a_1_nb_icon_1_Template, 1, 1, "nb-icon", 42)(2, NbRegisterComponent_section_33_ng_container_3_a_1_ng_template_2_Template, 1, 1, "ng-template", null, 5, ɵɵtemplateRefExtractor);
    ɵɵelementEnd();
  }
  if (rf & 2) {
    const title_r11 = ɵɵreference(3);
    const socialLink_r10 = ɵɵnextContext().$implicit;
    ɵɵclassProp("with-icon", socialLink_r10.icon);
    ɵɵproperty("routerLink", socialLink_r10.link);
    ɵɵattribute("target", socialLink_r10.target)("class", socialLink_r10.icon);
    ɵɵadvance();
    ɵɵproperty("ngIf", socialLink_r10.icon)("ngIfElse", title_r11);
  }
}
function NbRegisterComponent_section_33_ng_container_3_a_2_nb_icon_1_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelement(0, "nb-icon", 43);
  }
  if (rf & 2) {
    const socialLink_r10 = ɵɵnextContext(2).$implicit;
    ɵɵproperty("icon", socialLink_r10.icon);
  }
}
function NbRegisterComponent_section_33_ng_container_3_a_2_ng_template_2_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵtext(0);
  }
  if (rf & 2) {
    const socialLink_r10 = ɵɵnextContext(2).$implicit;
    ɵɵtextInterpolate(socialLink_r10.title);
  }
}
function NbRegisterComponent_section_33_ng_container_3_a_2_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementStart(0, "a");
    ɵɵtemplate(1, NbRegisterComponent_section_33_ng_container_3_a_2_nb_icon_1_Template, 1, 1, "nb-icon", 42)(2, NbRegisterComponent_section_33_ng_container_3_a_2_ng_template_2_Template, 1, 1, "ng-template", null, 5, ɵɵtemplateRefExtractor);
    ɵɵelementEnd();
  }
  if (rf & 2) {
    const title_r12 = ɵɵreference(3);
    const socialLink_r10 = ɵɵnextContext().$implicit;
    ɵɵclassProp("with-icon", socialLink_r10.icon);
    ɵɵattribute("href", socialLink_r10.url, ɵɵsanitizeUrl)("target", socialLink_r10.target)("class", socialLink_r10.icon);
    ɵɵadvance();
    ɵɵproperty("ngIf", socialLink_r10.icon)("ngIfElse", title_r12);
  }
}
function NbRegisterComponent_section_33_ng_container_3_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementContainerStart(0);
    ɵɵtemplate(1, NbRegisterComponent_section_33_ng_container_3_a_1_Template, 4, 7, "a", 39)(2, NbRegisterComponent_section_33_ng_container_3_a_2_Template, 4, 7, "a", 40);
    ɵɵelementContainerEnd();
  }
  if (rf & 2) {
    const socialLink_r10 = ctx.$implicit;
    ɵɵadvance();
    ɵɵproperty("ngIf", socialLink_r10.link);
    ɵɵadvance();
    ɵɵproperty("ngIf", socialLink_r10.url);
  }
}
function NbRegisterComponent_section_33_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementStart(0, "section", 36);
    ɵɵtext(1, " or enter with: ");
    ɵɵelementStart(2, "div", 37);
    ɵɵtemplate(3, NbRegisterComponent_section_33_ng_container_3_Template, 3, 2, "ng-container", 38);
    ɵɵelementEnd()();
  }
  if (rf & 2) {
    const ctx_r2 = ɵɵnextContext();
    ɵɵadvance(3);
    ɵɵproperty("ngForOf", ctx_r2.socialLinks);
  }
}
function NbRequestPasswordComponent_nb_alert_4_li_5_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementStart(0, "li", 19);
    ɵɵtext(1);
    ɵɵelementEnd();
  }
  if (rf & 2) {
    const error_r2 = ctx.$implicit;
    ɵɵadvance();
    ɵɵtextInterpolate(error_r2);
  }
}
function NbRequestPasswordComponent_nb_alert_4_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementStart(0, "nb-alert", 15)(1, "p", 16)(2, "b");
    ɵɵtext(3, "Oh snap!");
    ɵɵelementEnd()();
    ɵɵelementStart(4, "ul", 17);
    ɵɵtemplate(5, NbRequestPasswordComponent_nb_alert_4_li_5_Template, 2, 1, "li", 18);
    ɵɵelementEnd()();
  }
  if (rf & 2) {
    const ctx_r2 = ɵɵnextContext();
    ɵɵadvance(5);
    ɵɵproperty("ngForOf", ctx_r2.errors);
  }
}
function NbRequestPasswordComponent_nb_alert_5_li_5_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementStart(0, "li", 19);
    ɵɵtext(1);
    ɵɵelementEnd();
  }
  if (rf & 2) {
    const message_r4 = ctx.$implicit;
    ɵɵadvance();
    ɵɵtextInterpolate(message_r4);
  }
}
function NbRequestPasswordComponent_nb_alert_5_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementStart(0, "nb-alert", 20)(1, "p", 16)(2, "b");
    ɵɵtext(3, "Hooray!");
    ɵɵelementEnd()();
    ɵɵelementStart(4, "ul", 17);
    ɵɵtemplate(5, NbRequestPasswordComponent_nb_alert_5_li_5_Template, 2, 1, "li", 18);
    ɵɵelementEnd()();
  }
  if (rf & 2) {
    const ctx_r2 = ɵɵnextContext();
    ɵɵadvance(5);
    ɵɵproperty("ngForOf", ctx_r2.messages);
  }
}
function NbRequestPasswordComponent_ng_container_13_p_1_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementStart(0, "p", 22);
    ɵɵtext(1, " Email is required! ");
    ɵɵelementEnd();
  }
}
function NbRequestPasswordComponent_ng_container_13_p_2_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementStart(0, "p", 22);
    ɵɵtext(1, " Email should be the real one! ");
    ɵɵelementEnd();
  }
}
function NbRequestPasswordComponent_ng_container_13_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementContainerStart(0);
    ɵɵtemplate(1, NbRequestPasswordComponent_ng_container_13_p_1_Template, 2, 0, "p", 21)(2, NbRequestPasswordComponent_ng_container_13_p_2_Template, 2, 0, "p", 21);
    ɵɵelementContainerEnd();
  }
  if (rf & 2) {
    ɵɵnextContext();
    const email_r5 = ɵɵreference(12);
    ɵɵadvance();
    ɵɵproperty("ngIf", email_r5.errors == null ? null : email_r5.errors.required);
    ɵɵadvance();
    ɵɵproperty("ngIf", email_r5.errors == null ? null : email_r5.errors.pattern);
  }
}
function NbResetPasswordComponent_nb_alert_4_li_5_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementStart(0, "li", 23);
    ɵɵtext(1);
    ɵɵelementEnd();
  }
  if (rf & 2) {
    const error_r2 = ctx.$implicit;
    ɵɵadvance();
    ɵɵtextInterpolate(error_r2);
  }
}
function NbResetPasswordComponent_nb_alert_4_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementStart(0, "nb-alert", 19)(1, "p", 20)(2, "b");
    ɵɵtext(3, "Oh snap!");
    ɵɵelementEnd()();
    ɵɵelementStart(4, "ul", 21);
    ɵɵtemplate(5, NbResetPasswordComponent_nb_alert_4_li_5_Template, 2, 1, "li", 22);
    ɵɵelementEnd()();
  }
  if (rf & 2) {
    const ctx_r2 = ɵɵnextContext();
    ɵɵadvance(5);
    ɵɵproperty("ngForOf", ctx_r2.errors);
  }
}
function NbResetPasswordComponent_nb_alert_5_li_5_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementStart(0, "li", 23);
    ɵɵtext(1);
    ɵɵelementEnd();
  }
  if (rf & 2) {
    const message_r4 = ctx.$implicit;
    ɵɵadvance();
    ɵɵtextInterpolate(message_r4);
  }
}
function NbResetPasswordComponent_nb_alert_5_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementStart(0, "nb-alert", 24)(1, "p", 20)(2, "b");
    ɵɵtext(3, "Hooray!");
    ɵɵelementEnd()();
    ɵɵelementStart(4, "ul", 21);
    ɵɵtemplate(5, NbResetPasswordComponent_nb_alert_5_li_5_Template, 2, 1, "li", 22);
    ɵɵelementEnd()();
  }
  if (rf & 2) {
    const ctx_r2 = ɵɵnextContext();
    ɵɵadvance(5);
    ɵɵproperty("ngForOf", ctx_r2.messages);
  }
}
function NbResetPasswordComponent_ng_container_13_p_1_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementStart(0, "p", 26);
    ɵɵtext(1, " Password is required! ");
    ɵɵelementEnd();
  }
}
function NbResetPasswordComponent_ng_container_13_p_2_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementStart(0, "p", 26);
    ɵɵtext(1);
    ɵɵelementEnd();
  }
  if (rf & 2) {
    const ctx_r2 = ɵɵnextContext(2);
    ɵɵadvance();
    ɵɵtextInterpolate2(" Password should contains from ", ctx_r2.getConfigValue("forms.validation.password.minLength"), " to ", ctx_r2.getConfigValue("forms.validation.password.maxLength"), " characters ");
  }
}
function NbResetPasswordComponent_ng_container_13_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementContainerStart(0);
    ɵɵtemplate(1, NbResetPasswordComponent_ng_container_13_p_1_Template, 2, 0, "p", 25)(2, NbResetPasswordComponent_ng_container_13_p_2_Template, 2, 2, "p", 25);
    ɵɵelementContainerEnd();
  }
  if (rf & 2) {
    ɵɵnextContext();
    const password_r5 = ɵɵreference(12);
    ɵɵadvance();
    ɵɵproperty("ngIf", password_r5.errors == null ? null : password_r5.errors.required);
    ɵɵadvance();
    ɵɵproperty("ngIf", (password_r5.errors == null ? null : password_r5.errors.minlength) || (password_r5.errors == null ? null : password_r5.errors.maxlength));
  }
}
function NbResetPasswordComponent_ng_container_19_p_1_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementStart(0, "p", 26);
    ɵɵtext(1, " Password confirmation is required! ");
    ɵɵelementEnd();
  }
}
function NbResetPasswordComponent_ng_container_19_p_2_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementStart(0, "p", 26);
    ɵɵtext(1, " Password does not match the confirm password. ");
    ɵɵelementEnd();
  }
}
function NbResetPasswordComponent_ng_container_19_Template(rf, ctx) {
  if (rf & 1) {
    ɵɵelementContainerStart(0);
    ɵɵtemplate(1, NbResetPasswordComponent_ng_container_19_p_1_Template, 2, 0, "p", 25)(2, NbResetPasswordComponent_ng_container_19_p_2_Template, 2, 0, "p", 25);
    ɵɵelementContainerEnd();
  }
  if (rf & 2) {
    ɵɵnextContext();
    const password_r5 = ɵɵreference(12);
    const rePass_r6 = ɵɵreference(18);
    ɵɵadvance();
    ɵɵproperty("ngIf", rePass_r6.invalid && (rePass_r6.errors == null ? null : rePass_r6.errors.required));
    ɵɵadvance();
    ɵɵproperty("ngIf", password_r5.value != rePass_r6.value && !(rePass_r6.errors == null ? null : rePass_r6.errors.required));
  }
}
var _c1 = "\n\n\n\n\n[_nghost-%COMP%]   .form-group[_ngcontent-%COMP%]:last-of-type{margin-bottom:3rem}";
var socialLinks = [];
var defaultAuthOptions = {
  strategies: [],
  forms: {
    login: {
      redirectDelay: 500,
      // delay before redirect after a successful login, while success message is shown to the user
      strategy: "email",
      // provider id key. If you have multiple strategies, or what to use your own
      rememberMe: true,
      // whether to show or not the `rememberMe` checkbox
      showMessages: {
        success: true,
        error: true
      },
      socialLinks
      // social links at the bottom of a page
    },
    register: {
      redirectDelay: 500,
      strategy: "email",
      showMessages: {
        success: true,
        error: true
      },
      terms: true,
      socialLinks
    },
    requestPassword: {
      redirectDelay: 500,
      strategy: "email",
      showMessages: {
        success: true,
        error: true
      },
      socialLinks
    },
    resetPassword: {
      redirectDelay: 500,
      strategy: "email",
      showMessages: {
        success: true,
        error: true
      },
      socialLinks
    },
    logout: {
      redirectDelay: 500,
      strategy: "email"
    },
    validation: {
      password: {
        required: true,
        minLength: 4,
        maxLength: 50
      },
      email: {
        required: true
      },
      fullName: {
        required: false,
        minLength: 4,
        maxLength: 50
      }
    }
  }
};
var NB_AUTH_OPTIONS = new InjectionToken("Nebular Auth Options");
var NB_AUTH_USER_OPTIONS = new InjectionToken("Nebular User Auth Options");
var NB_AUTH_STRATEGIES = new InjectionToken("Nebular Auth Strategies");
var NB_AUTH_TOKENS = new InjectionToken("Nebular Auth Tokens");
var NB_AUTH_INTERCEPTOR_HEADER = new InjectionToken("Nebular Simple Interceptor Header");
var NB_AUTH_TOKEN_INTERCEPTOR_FILTER = new InjectionToken("Nebular Interceptor Filter");
var deepExtend = function(...objects) {
  if (arguments.length < 1 || typeof arguments[0] !== "object") {
    return false;
  }
  if (arguments.length < 2) {
    return arguments[0];
  }
  const target = arguments[0];
  const args = Array.prototype.slice.call(arguments, 1);
  let val, src;
  args.forEach(function(obj) {
    if (typeof obj !== "object" || Array.isArray(obj)) {
      return;
    }
    Object.keys(obj).forEach(function(key) {
      src = target[key];
      val = obj[key];
      if (val === target) {
        return;
      } else if (typeof val !== "object" || val === null) {
        target[key] = val;
        return;
      } else if (Array.isArray(val)) {
        target[key] = deepCloneArray(val);
        return;
      } else if (isSpecificValue(val)) {
        target[key] = cloneSpecificValue(val);
        return;
      } else if (typeof src !== "object" || src === null || Array.isArray(src)) {
        target[key] = deepExtend({}, val);
        return;
      } else {
        target[key] = deepExtend(src, val);
        return;
      }
    });
  });
  return target;
};
function isSpecificValue(val) {
  return val instanceof Date || val instanceof RegExp ? true : false;
}
function cloneSpecificValue(val) {
  if (val instanceof Date) {
    return new Date(val.getTime());
  } else if (val instanceof RegExp) {
    return new RegExp(val);
  } else {
    throw new Error("cloneSpecificValue: Unexpected situation");
  }
}
function deepCloneArray(arr) {
  const clone = [];
  arr.forEach(function(item, index) {
    if (typeof item === "object" && item !== null) {
      if (Array.isArray(item)) {
        clone[index] = deepCloneArray(item);
      } else if (isSpecificValue(item)) {
        clone[index] = cloneSpecificValue(item);
      } else {
        clone[index] = deepExtend({}, item);
      }
    } else {
      clone[index] = item;
    }
  });
  return clone;
}
function getDeepFromObject(object = {}, name, defaultValue) {
  const keys = name.split(".");
  let level = deepExtend({}, object || {});
  keys.forEach((k) => {
    if (level && typeof level[k] !== "undefined") {
      level = level[k];
    } else {
      level = void 0;
    }
  });
  return typeof level === "undefined" ? defaultValue : level;
}
function urlBase64Decode(str) {
  let output = str.replace(/-/g, "+").replace(/_/g, "/");
  switch (output.length % 4) {
    case 0: {
      break;
    }
    case 2: {
      output += "==";
      break;
    }
    case 3: {
      output += "=";
      break;
    }
    default: {
      throw new Error("Illegal base64url string!");
    }
  }
  return b64DecodeUnicode(output);
}
function b64decode(str) {
  const chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
  let output = "";
  str = String(str).replace(/=+$/, "");
  if (str.length % 4 === 1) {
    throw new Error(`'atob' failed: The string to be decoded is not correctly encoded.`);
  }
  for (
    let bc = 0, bs, buffer, idx = 0;
    // get next character
    buffer = str.charAt(idx++);
    // character found in table? initialize bit storage and add its ascii value;
    ~buffer && (bs = bc % 4 ? bs * 64 + buffer : buffer, // and if not first of each 4 characters,
    // convert the first 8 bits to one ascii character
    bc++ % 4) ? output += String.fromCharCode(255 & bs >> (-2 * bc & 6)) : 0
  ) {
    buffer = chars.indexOf(buffer);
  }
  return output;
}
function b64DecodeUnicode(str) {
  return decodeURIComponent(Array.prototype.map.call(b64decode(str), (c) => {
    return "%" + ("00" + c.charCodeAt(0).toString(16)).slice(-2);
  }).join(""));
}
var NbAuthToken = class {
  constructor() {
    this.payload = null;
  }
  getName() {
    return this.constructor.NAME;
  }
  getPayload() {
    return this.payload;
  }
};
var NbAuthTokenNotFoundError = class extends Error {
  constructor(message) {
    super(message);
    Object.setPrototypeOf(this, new.target.prototype);
  }
};
var NbAuthIllegalTokenError = class extends Error {
  constructor(message) {
    super(message);
    Object.setPrototypeOf(this, new.target.prototype);
  }
};
var NbAuthEmptyTokenError = class extends NbAuthIllegalTokenError {
  constructor(message) {
    super(message);
    Object.setPrototypeOf(this, new.target.prototype);
  }
};
var NbAuthIllegalJWTTokenError = class extends NbAuthIllegalTokenError {
  constructor(message) {
    super(message);
    Object.setPrototypeOf(this, new.target.prototype);
  }
};
function nbAuthCreateToken(tokenClass, token, ownerStrategyName, createdAt) {
  return new tokenClass(token, ownerStrategyName, createdAt);
}
function decodeJwtPayload(payload) {
  if (payload.length === 0) {
    throw new NbAuthEmptyTokenError("Cannot extract from an empty payload.");
  }
  const parts = payload.split(".");
  if (parts.length !== 3) {
    throw new NbAuthIllegalJWTTokenError(`The payload ${payload} is not valid JWT payload and must consist of three parts.`);
  }
  let decoded;
  try {
    decoded = urlBase64Decode(parts[1]);
  } catch (e) {
    throw new NbAuthIllegalJWTTokenError(`The payload ${payload} is not valid JWT payload and cannot be parsed.`);
  }
  if (!decoded) {
    throw new NbAuthIllegalJWTTokenError(`The payload ${payload} is not valid JWT payload and cannot be decoded.`);
  }
  return JSON.parse(decoded);
}
var _NbAuthSimpleToken = class _NbAuthSimpleToken extends NbAuthToken {
  constructor(token, ownerStrategyName, createdAt) {
    super();
    this.token = token;
    this.ownerStrategyName = ownerStrategyName;
    this.createdAt = createdAt;
    try {
      this.parsePayload();
    } catch (err) {
      if (!(err instanceof NbAuthTokenNotFoundError)) {
        throw err;
      }
    }
    this.createdAt = this.prepareCreatedAt(createdAt);
  }
  parsePayload() {
    this.payload = null;
  }
  prepareCreatedAt(date) {
    return date ? date : /* @__PURE__ */ new Date();
  }
  /**
   * Returns the token's creation date
   * @returns {Date}
   */
  getCreatedAt() {
    return this.createdAt;
  }
  /**
   * Returns the token value
   * @returns string
   */
  getValue() {
    return this.token;
  }
  getOwnerStrategyName() {
    return this.ownerStrategyName;
  }
  /**
   * Is non empty and valid
   * @returns {boolean}
   */
  isValid() {
    return !!this.getValue();
  }
  /**
   * Validate value and convert to string, if value is not valid return empty string
   * @returns {string}
   */
  toString() {
    return !!this.token ? this.token : "";
  }
};
_NbAuthSimpleToken.NAME = "nb:auth:simple:token";
var NbAuthSimpleToken = _NbAuthSimpleToken;
var _NbAuthJWTToken = class _NbAuthJWTToken extends NbAuthSimpleToken {
  /**
   * for JWT token, the iat (issued at) field of the token payload contains the creation Date
   */
  prepareCreatedAt(date) {
    const decoded = this.getPayload();
    return decoded && decoded.iat ? new Date(Number(decoded.iat) * 1e3) : super.prepareCreatedAt(date);
  }
  /**
   * Returns payload object
   * @returns any
   */
  parsePayload() {
    if (!this.token) {
      throw new NbAuthTokenNotFoundError("Token not found. ");
    }
    this.payload = decodeJwtPayload(this.token);
  }
  /**
   * Returns expiration date
   * @returns Date
   */
  getTokenExpDate() {
    const decoded = this.getPayload();
    if (decoded && !decoded.hasOwnProperty("exp")) {
      return null;
    }
    const date = /* @__PURE__ */ new Date(0);
    date.setUTCSeconds(decoded.exp);
    return date;
  }
  /**
   * Is data expired
   * @returns {boolean}
   */
  isValid() {
    return super.isValid() && (!this.getTokenExpDate() || /* @__PURE__ */ new Date() < this.getTokenExpDate());
  }
};
_NbAuthJWTToken.NAME = "nb:auth:jwt:token";
var NbAuthJWTToken = _NbAuthJWTToken;
var prepareOAuth2Token = (data) => {
  if (typeof data === "string") {
    try {
      return JSON.parse(data);
    } catch (e) {
    }
  }
  return data;
};
var _NbAuthOAuth2Token = class _NbAuthOAuth2Token extends NbAuthSimpleToken {
  constructor(data = {}, ownerStrategyName, createdAt) {
    super(prepareOAuth2Token(data), ownerStrategyName, createdAt);
  }
  /**
   * Returns the token value
   * @returns string
   */
  getValue() {
    return this.token.access_token;
  }
  /**
   * Returns the refresh token
   * @returns string
   */
  getRefreshToken() {
    return this.token.refresh_token;
  }
  /**
   *  put refreshToken in the token payload
    * @param refreshToken
   */
  setRefreshToken(refreshToken) {
    this.token.refresh_token = refreshToken;
  }
  /**
   * Parses token payload
   * @returns any
   */
  parsePayload() {
    if (!this.token) {
      throw new NbAuthTokenNotFoundError("Token not found.");
    } else {
      if (!Object.keys(this.token).length) {
        throw new NbAuthEmptyTokenError("Cannot extract payload from an empty token.");
      }
    }
    this.payload = this.token;
  }
  /**
   * Returns the token type
   * @returns string
   */
  getType() {
    return this.token.token_type;
  }
  /**
   * Is data expired
   * @returns {boolean}
   */
  isValid() {
    return super.isValid() && (!this.getTokenExpDate() || /* @__PURE__ */ new Date() < this.getTokenExpDate());
  }
  /**
   * Returns expiration date
   * @returns Date
   */
  getTokenExpDate() {
    if (!this.token.hasOwnProperty("expires_in")) {
      return null;
    }
    return new Date(this.createdAt.getTime() + Number(this.token.expires_in) * 1e3);
  }
  /**
   * Convert to string
   * @returns {string}
   */
  toString() {
    return JSON.stringify(this.token);
  }
};
_NbAuthOAuth2Token.NAME = "nb:auth:oauth2:token";
var NbAuthOAuth2Token = _NbAuthOAuth2Token;
var _NbAuthOAuth2JWTToken = class _NbAuthOAuth2JWTToken extends NbAuthOAuth2Token {
  parsePayload() {
    super.parsePayload();
    this.parseAccessTokenPayload();
  }
  parseAccessTokenPayload() {
    const accessToken = this.getValue();
    if (!accessToken) {
      throw new NbAuthTokenNotFoundError("access_token key not found.");
    }
    this.accessTokenPayload = decodeJwtPayload(accessToken);
  }
  /**
   * Returns access token payload
   * @returns any
   */
  getAccessTokenPayload() {
    return this.accessTokenPayload;
  }
  /**
   * for Oauth2 JWT token, the iat (issued at) field of the access_token payload
   */
  prepareCreatedAt(date) {
    const payload = this.accessTokenPayload;
    return payload && payload.iat ? new Date(Number(payload.iat) * 1e3) : super.prepareCreatedAt(date);
  }
  /**
   * Is token valid
   * @returns {boolean}
   */
  isValid() {
    return this.accessTokenPayload && super.isValid();
  }
  /**
   * Returns expiration date :
   * - exp if set,
   * - super.getExpDate() otherwise
   * @returns Date
   */
  getTokenExpDate() {
    if (this.accessTokenPayload && this.accessTokenPayload.hasOwnProperty("exp")) {
      const date = /* @__PURE__ */ new Date(0);
      date.setUTCSeconds(this.accessTokenPayload.exp);
      return date;
    } else {
      return super.getTokenExpDate();
    }
  }
};
_NbAuthOAuth2JWTToken.NAME = "nb:auth:oauth2:jwt:token";
var NbAuthOAuth2JWTToken = _NbAuthOAuth2JWTToken;
var NB_AUTH_FALLBACK_TOKEN = new InjectionToken("Nebular Auth Options");
var _NbAuthTokenParceler = class _NbAuthTokenParceler {
  constructor(fallbackClass, tokenClasses) {
    this.fallbackClass = fallbackClass;
    this.tokenClasses = tokenClasses;
  }
  wrap(token) {
    return JSON.stringify({
      name: token.getName(),
      ownerStrategyName: token.getOwnerStrategyName(),
      createdAt: token.getCreatedAt().getTime(),
      value: token.toString()
    });
  }
  unwrap(value) {
    let tokenClass = this.fallbackClass;
    let tokenValue = "";
    let tokenOwnerStrategyName = "";
    let tokenCreatedAt = null;
    const tokenPack = this.parseTokenPack(value);
    if (tokenPack) {
      tokenClass = this.getClassByName(tokenPack.name) || this.fallbackClass;
      tokenValue = tokenPack.value;
      tokenOwnerStrategyName = tokenPack.ownerStrategyName;
      tokenCreatedAt = new Date(Number(tokenPack.createdAt));
    }
    return nbAuthCreateToken(tokenClass, tokenValue, tokenOwnerStrategyName, tokenCreatedAt);
  }
  // TODO: this could be moved to a separate token registry
  getClassByName(name) {
    return this.tokenClasses.find((tokenClass) => tokenClass.NAME === name);
  }
  parseTokenPack(value) {
    try {
      return JSON.parse(value);
    } catch (e) {
    }
    return null;
  }
};
_NbAuthTokenParceler.ɵfac = function NbAuthTokenParceler_Factory(t) {
  return new (t || _NbAuthTokenParceler)(ɵɵinject(NB_AUTH_FALLBACK_TOKEN), ɵɵinject(NB_AUTH_TOKENS));
};
_NbAuthTokenParceler.ɵprov = ɵɵdefineInjectable({
  token: _NbAuthTokenParceler,
  factory: _NbAuthTokenParceler.ɵfac
});
var NbAuthTokenParceler = _NbAuthTokenParceler;
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(NbAuthTokenParceler, [{
    type: Injectable
  }], () => [{
    type: void 0,
    decorators: [{
      type: Inject,
      args: [NB_AUTH_FALLBACK_TOKEN]
    }]
  }, {
    type: void 0,
    decorators: [{
      type: Inject,
      args: [NB_AUTH_TOKENS]
    }]
  }], null);
})();
var NbTokenStorage = class {
};
var _NbTokenLocalStorage = class _NbTokenLocalStorage extends NbTokenStorage {
  constructor(parceler) {
    super();
    this.parceler = parceler;
    this.key = "auth_app_token";
  }
  /**
   * Returns token from localStorage
   * @returns {NbAuthToken}
   */
  get() {
    const raw = localStorage.getItem(this.key);
    return this.parceler.unwrap(raw);
  }
  /**
   * Sets token to localStorage
   * @param {NbAuthToken} token
   */
  set(token) {
    const raw = this.parceler.wrap(token);
    localStorage.setItem(this.key, raw);
  }
  /**
   * Clears token from localStorage
   */
  clear() {
    localStorage.removeItem(this.key);
  }
};
_NbTokenLocalStorage.ɵfac = function NbTokenLocalStorage_Factory(t) {
  return new (t || _NbTokenLocalStorage)(ɵɵinject(NbAuthTokenParceler));
};
_NbTokenLocalStorage.ɵprov = ɵɵdefineInjectable({
  token: _NbTokenLocalStorage,
  factory: _NbTokenLocalStorage.ɵfac
});
var NbTokenLocalStorage = _NbTokenLocalStorage;
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(NbTokenLocalStorage, [{
    type: Injectable
  }], () => [{
    type: NbAuthTokenParceler
  }], null);
})();
var _NbTokenService = class _NbTokenService {
  constructor(tokenStorage) {
    this.tokenStorage = tokenStorage;
    this.token$ = new BehaviorSubject(null);
    this.publishStoredToken();
  }
  /**
   * Publishes token when it changes.
   * @returns {Observable<NbAuthToken>}
   */
  tokenChange() {
    return this.token$.pipe(filter((value) => !!value), share());
  }
  /**
   * Sets a token into the storage. This method is used by the NbAuthService automatically.
   *
   * @param {NbAuthToken} token
   * @returns {Observable<any>}
   */
  set(token) {
    this.tokenStorage.set(token);
    this.publishStoredToken();
    return of(null);
  }
  /**
   * Returns observable of current token
   * @returns {Observable<NbAuthToken>}
   */
  get() {
    const token = this.tokenStorage.get();
    return of(token);
  }
  /**
   * Removes the token and published token value
   *
   * @returns {Observable<any>}
   */
  clear() {
    this.tokenStorage.clear();
    this.publishStoredToken();
    return of(null);
  }
  publishStoredToken() {
    this.token$.next(this.tokenStorage.get());
  }
};
_NbTokenService.ɵfac = function NbTokenService_Factory(t) {
  return new (t || _NbTokenService)(ɵɵinject(NbTokenStorage));
};
_NbTokenService.ɵprov = ɵɵdefineInjectable({
  token: _NbTokenService,
  factory: _NbTokenService.ɵfac
});
var NbTokenService = _NbTokenService;
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(NbTokenService, [{
    type: Injectable
  }], () => [{
    type: NbTokenStorage
  }], null);
})();
var _NbAuthService = class _NbAuthService {
  constructor(tokenService, strategies) {
    this.tokenService = tokenService;
    this.strategies = strategies;
  }
  /**
   * Retrieves current authenticated token stored
   * @returns {Observable<any>}
   */
  getToken() {
    return this.tokenService.get();
  }
  /**
   * Returns true if auth token is present in the token storage
   * @returns {Observable<boolean>}
   */
  isAuthenticated() {
    return this.getToken().pipe(map((token) => token.isValid()));
  }
  /**
   * Returns true if valid auth token is present in the token storage.
   * If not, calls the strategy refreshToken, and returns isAuthenticated() if success, false otherwise
   * @returns {Observable<boolean>}
   */
  isAuthenticatedOrRefresh() {
    return this.getToken().pipe(switchMap((token) => {
      if (token.getValue() && !token.isValid()) {
        return this.refreshToken(token.getOwnerStrategyName(), token).pipe(switchMap((res) => {
          if (res.isSuccess()) {
            return this.isAuthenticated();
          } else {
            return of(false);
          }
        }));
      } else {
        return of(token.isValid());
      }
    }));
  }
  /**
   * Returns tokens stream
   * @returns {Observable<NbAuthSimpleToken>}
   */
  onTokenChange() {
    return this.tokenService.tokenChange();
  }
  /**
   * Returns authentication status stream
   * @returns {Observable<boolean>}
   */
  onAuthenticationChange() {
    return this.onTokenChange().pipe(map((token) => token.isValid()));
  }
  /**
   * Authenticates with the selected strategy
   * Stores received token in the token storage
   *
   * Example:
   * authenticate('email', {email: 'email@example.com', password: 'test'})
   *
   * @param strategyName
   * @param data
   * @returns {Observable<NbAuthResult>}
   */
  authenticate(strategyName, data) {
    return this.getStrategy(strategyName).authenticate(data).pipe(switchMap((result) => {
      return this.processResultToken(result);
    }));
  }
  /**
   * Registers with the selected strategy
   * Stores received token in the token storage
   *
   * Example:
   * register('email', {email: 'email@example.com', name: 'Some Name', password: 'test'})
   *
   * @param strategyName
   * @param data
   * @returns {Observable<NbAuthResult>}
   */
  register(strategyName, data) {
    return this.getStrategy(strategyName).register(data).pipe(switchMap((result) => {
      return this.processResultToken(result);
    }));
  }
  /**
   * Sign outs with the selected strategy
   * Removes token from the token storage
   *
   * Example:
   * logout('email')
   *
   * @param strategyName
   * @returns {Observable<NbAuthResult>}
   */
  logout(strategyName) {
    return this.getStrategy(strategyName).logout().pipe(switchMap((result) => {
      if (result.isSuccess()) {
        this.tokenService.clear().pipe(map(() => result));
      }
      return of(result);
    }));
  }
  /**
   * Sends forgot password request to the selected strategy
   *
   * Example:
   * requestPassword('email', {email: 'email@example.com'})
   *
   * @param strategyName
   * @param data
   * @returns {Observable<NbAuthResult>}
   */
  requestPassword(strategyName, data) {
    return this.getStrategy(strategyName).requestPassword(data);
  }
  /**
   * Tries to reset password with the selected strategy
   *
   * Example:
   * resetPassword('email', {newPassword: 'test'})
   *
   * @param strategyName
   * @param data
   * @returns {Observable<NbAuthResult>}
   */
  resetPassword(strategyName, data) {
    return this.getStrategy(strategyName).resetPassword(data);
  }
  /**
   * Sends a refresh token request
   * Stores received token in the token storage
   *
   * Example:
   * refreshToken('email', {token: token})
   *
   * @param {string} strategyName
   * @param data
   * @returns {Observable<NbAuthResult>}
   */
  refreshToken(strategyName, data) {
    return this.getStrategy(strategyName).refreshToken(data).pipe(switchMap((result) => {
      return this.processResultToken(result);
    }));
  }
  /**
   * Get registered strategy by name
   *
   * Example:
   * getStrategy('email')
   *
   * @param {string} provider
   * @returns {NbAbstractAuthProvider}
   */
  getStrategy(strategyName) {
    const found = this.strategies.find((strategy) => strategy.getName() === strategyName);
    if (!found) {
      throw new TypeError(`There is no Auth Strategy registered under '${strategyName}' name`);
    }
    return found;
  }
  processResultToken(result) {
    if (result.isSuccess() && result.getToken()) {
      return this.tokenService.set(result.getToken()).pipe(map((token) => {
        return result;
      }));
    }
    return of(result);
  }
};
_NbAuthService.ɵfac = function NbAuthService_Factory(t) {
  return new (t || _NbAuthService)(ɵɵinject(NbTokenService), ɵɵinject(NB_AUTH_STRATEGIES));
};
_NbAuthService.ɵprov = ɵɵdefineInjectable({
  token: _NbAuthService,
  factory: _NbAuthService.ɵfac
});
var NbAuthService = _NbAuthService;
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(NbAuthService, [{
    type: Injectable
  }], () => [{
    type: NbTokenService
  }, {
    type: void 0,
    decorators: [{
      type: Inject,
      args: [NB_AUTH_STRATEGIES]
    }]
  }], null);
})();
var NbAuthStrategy = class {
  // we should keep this any and validation should be done in `register` method instead
  // otherwise it won't be possible to pass an empty object
  setOptions(options) {
    this.options = deepExtend({}, this.defaultOptions, options);
  }
  getOption(key) {
    return getDeepFromObject(this.options, key, null);
  }
  createToken(value, failWhenInvalidToken) {
    const token = nbAuthCreateToken(this.getOption("token.class"), value, this.getName());
    if (failWhenInvalidToken && !token.isValid()) {
      throw new NbAuthIllegalTokenError("Token is empty or invalid.");
    }
    return token;
  }
  getName() {
    return this.getOption("name");
  }
  createFailResponse(data) {
    return new HttpResponse({
      body: {},
      status: 401
    });
  }
  createSuccessResponse(data) {
    return new HttpResponse({
      body: {},
      status: 200
    });
  }
  getActionEndpoint(action) {
    const actionEndpoint = this.getOption(`${action}.endpoint`);
    const baseEndpoint = this.getOption("baseEndpoint");
    return actionEndpoint ? baseEndpoint + actionEndpoint : "";
  }
  getHeaders() {
    const customHeaders = this.getOption("headers") ?? {};
    if (customHeaders instanceof HttpHeaders) {
      return customHeaders;
    }
    let headers = new HttpHeaders();
    Object.entries(customHeaders).forEach(([key, value]) => {
      headers = headers.append(key, value);
    });
    return headers;
  }
};
var NbAuthResult = class {
  // TODO: better pass object
  constructor(success, response, redirect, errors, messages, token = null) {
    this.success = success;
    this.response = response;
    this.redirect = redirect;
    this.errors = [];
    this.messages = [];
    this.errors = this.errors.concat([errors]);
    if (errors instanceof Array) {
      this.errors = errors;
    }
    this.messages = this.messages.concat([messages]);
    if (messages instanceof Array) {
      this.messages = messages;
    }
    this.token = token;
  }
  getResponse() {
    return this.response;
  }
  getToken() {
    return this.token;
  }
  getRedirect() {
    return this.redirect;
  }
  getErrors() {
    return this.errors.filter((val) => !!val);
  }
  getMessages() {
    return this.messages.filter((val) => !!val);
  }
  isSuccess() {
    return this.success;
  }
  isFailure() {
    return !this.success;
  }
};
var NbAuthStrategyOptions = class {
};
var NbDummyAuthStrategyOptions = class extends NbAuthStrategyOptions {
  constructor() {
    super(...arguments);
    this.token = {
      class: NbAuthSimpleToken
    };
    this.delay = 1e3;
    this.alwaysFail = false;
  }
};
var dummyStrategyOptions = new NbDummyAuthStrategyOptions();
var _NbDummyAuthStrategy = class _NbDummyAuthStrategy extends NbAuthStrategy {
  constructor() {
    super(...arguments);
    this.defaultOptions = dummyStrategyOptions;
  }
  static setup(options) {
    return [_NbDummyAuthStrategy, options];
  }
  authenticate(data) {
    return of(this.createDummyResult(data)).pipe(delay(this.getOption("delay")));
  }
  register(data) {
    return of(this.createDummyResult(data)).pipe(delay(this.getOption("delay")));
  }
  requestPassword(data) {
    return of(this.createDummyResult(data)).pipe(delay(this.getOption("delay")));
  }
  resetPassword(data) {
    return of(this.createDummyResult(data)).pipe(delay(this.getOption("delay")));
  }
  logout(data) {
    return of(this.createDummyResult(data)).pipe(delay(this.getOption("delay")));
  }
  refreshToken(data) {
    return of(this.createDummyResult(data)).pipe(delay(this.getOption("delay")));
  }
  createDummyResult(data) {
    if (this.getOption("alwaysFail")) {
      return new NbAuthResult(false, this.createFailResponse(data), null, ["Something went wrong."]);
    }
    try {
      const token = this.createToken("test token", true);
      return new NbAuthResult(true, this.createSuccessResponse(data), "/", [], ["Successfully logged in."], token);
    } catch (err) {
      return new NbAuthResult(false, this.createFailResponse(data), null, [err.message]);
    }
  }
};
_NbDummyAuthStrategy.ɵfac = /* @__PURE__ */ (() => {
  let ɵNbDummyAuthStrategy_BaseFactory;
  return function NbDummyAuthStrategy_Factory(t) {
    return (ɵNbDummyAuthStrategy_BaseFactory || (ɵNbDummyAuthStrategy_BaseFactory = ɵɵgetInheritedFactory(_NbDummyAuthStrategy)))(t || _NbDummyAuthStrategy);
  };
})();
_NbDummyAuthStrategy.ɵprov = ɵɵdefineInjectable({
  token: _NbDummyAuthStrategy,
  factory: _NbDummyAuthStrategy.ɵfac
});
var NbDummyAuthStrategy = _NbDummyAuthStrategy;
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(NbDummyAuthStrategy, [{
    type: Injectable
  }], null, null);
})();
var NbOAuth2ResponseType;
(function(NbOAuth2ResponseType2) {
  NbOAuth2ResponseType2["CODE"] = "code";
  NbOAuth2ResponseType2["TOKEN"] = "token";
})(NbOAuth2ResponseType || (NbOAuth2ResponseType = {}));
var NbOAuth2GrantType;
(function(NbOAuth2GrantType2) {
  NbOAuth2GrantType2["AUTHORIZATION_CODE"] = "authorization_code";
  NbOAuth2GrantType2["PASSWORD"] = "password";
  NbOAuth2GrantType2["REFRESH_TOKEN"] = "refresh_token";
})(NbOAuth2GrantType || (NbOAuth2GrantType = {}));
var NbOAuth2ClientAuthMethod;
(function(NbOAuth2ClientAuthMethod2) {
  NbOAuth2ClientAuthMethod2["NONE"] = "none";
  NbOAuth2ClientAuthMethod2["BASIC"] = "basic";
  NbOAuth2ClientAuthMethod2["REQUEST_BODY"] = "request-body";
})(NbOAuth2ClientAuthMethod || (NbOAuth2ClientAuthMethod = {}));
var NbOAuth2AuthStrategyOptions = class extends NbAuthStrategyOptions {
  constructor() {
    super(...arguments);
    this.baseEndpoint = "";
    this.clientId = "";
    this.clientSecret = "";
    this.clientAuthMethod = NbOAuth2ClientAuthMethod.NONE;
    this.redirect = {
      success: "/",
      failure: null
    };
    this.defaultErrors = ["Something went wrong, please try again."];
    this.defaultMessages = ["You have been successfully authenticated."];
    this.authorize = {
      endpoint: "authorize",
      responseType: NbOAuth2ResponseType.CODE,
      requireValidToken: true
    };
    this.token = {
      endpoint: "token",
      grantType: NbOAuth2GrantType.AUTHORIZATION_CODE,
      requireValidToken: true,
      class: NbAuthOAuth2Token
    };
    this.refresh = {
      endpoint: "token",
      grantType: NbOAuth2GrantType.REFRESH_TOKEN,
      requireValidToken: true
    };
  }
};
var auth2StrategyOptions = new NbOAuth2AuthStrategyOptions();
var _NbOAuth2AuthStrategy = class _NbOAuth2AuthStrategy extends NbAuthStrategy {
  static setup(options) {
    return [_NbOAuth2AuthStrategy, options];
  }
  get responseType() {
    return this.getOption("authorize.responseType");
  }
  get clientAuthMethod() {
    return this.getOption("clientAuthMethod");
  }
  constructor(http, route, window) {
    super();
    this.http = http;
    this.route = route;
    this.window = window;
    this.redirectResultHandlers = {
      [NbOAuth2ResponseType.CODE]: () => {
        return of(this.route.snapshot.queryParams).pipe(switchMap((params) => {
          if (params.code) {
            return this.requestToken(params.code);
          }
          return of(new NbAuthResult(false, params, this.getOption("redirect.failure"), this.getOption("defaultErrors"), []));
        }));
      },
      [NbOAuth2ResponseType.TOKEN]: () => {
        const module = "authorize";
        const requireValidToken = this.getOption(`${module}.requireValidToken`);
        return of(this.route.snapshot.fragment).pipe(map((fragment) => this.parseHashAsQueryParams(fragment)), map((params) => {
          if (!params.error) {
            return new NbAuthResult(true, params, this.getOption("redirect.success"), [], this.getOption("defaultMessages"), this.createToken(params, requireValidToken));
          }
          return new NbAuthResult(false, params, this.getOption("redirect.failure"), this.getOption("defaultErrors"), []);
        }), catchError((err) => {
          const errors = [];
          if (err instanceof NbAuthIllegalTokenError) {
            errors.push(err.message);
          } else {
            errors.push("Something went wrong.");
          }
          return of(new NbAuthResult(false, err, this.getOption("redirect.failure"), errors));
        }));
      }
    };
    this.redirectResults = {
      [NbOAuth2ResponseType.CODE]: () => {
        return of(this.route.snapshot.queryParams).pipe(map((params) => !!(params && (params.code || params.error))));
      },
      [NbOAuth2ResponseType.TOKEN]: () => {
        return of(this.route.snapshot.fragment).pipe(map((fragment) => this.parseHashAsQueryParams(fragment)), map((params) => !!(params && (params.access_token || params.error))));
      }
    };
    this.defaultOptions = auth2StrategyOptions;
  }
  authenticate(data) {
    if (this.getOption("token.grantType") === NbOAuth2GrantType.PASSWORD) {
      return this.passwordToken(data.email, data.password);
    } else {
      return this.isRedirectResult().pipe(switchMap((result) => {
        if (!result) {
          this.authorizeRedirect();
          return of(new NbAuthResult(true));
        }
        return this.getAuthorizationResult();
      }));
    }
  }
  getAuthorizationResult() {
    const redirectResultHandler = this.redirectResultHandlers[this.responseType];
    if (redirectResultHandler) {
      return redirectResultHandler.call(this);
    }
    throw new Error(`'${this.responseType}' responseType is not supported,
                      only 'token' and 'code' are supported now`);
  }
  refreshToken(token) {
    const module = "refresh";
    const url = this.getActionEndpoint(module);
    const requireValidToken = this.getOption(`${module}.requireValidToken`);
    return this.http.post(url, this.buildRefreshRequestData(token), {
      headers: this.getHeaders()
    }).pipe(map((res) => {
      return new NbAuthResult(true, res, this.getOption("redirect.success"), [], this.getOption("defaultMessages"), this.createRefreshedToken(res, token, requireValidToken));
    }), catchError((res) => this.handleResponseError(res)));
  }
  passwordToken(username, password) {
    const module = "token";
    const url = this.getActionEndpoint(module);
    const requireValidToken = this.getOption(`${module}.requireValidToken`);
    return this.http.post(url, this.buildPasswordRequestData(username, password), {
      headers: this.getHeaders()
    }).pipe(map((res) => {
      return new NbAuthResult(true, res, this.getOption("redirect.success"), [], this.getOption("defaultMessages"), this.createToken(res, requireValidToken));
    }), catchError((res) => this.handleResponseError(res)));
  }
  authorizeRedirect() {
    this.window.location.href = this.buildRedirectUrl();
  }
  isRedirectResult() {
    return this.redirectResults[this.responseType].call(this);
  }
  requestToken(code) {
    const module = "token";
    const url = this.getActionEndpoint(module);
    const requireValidToken = this.getOption(`${module}.requireValidToken`);
    return this.http.post(url, this.buildCodeRequestData(code), {
      headers: this.getHeaders()
    }).pipe(map((res) => {
      return new NbAuthResult(true, res, this.getOption("redirect.success"), [], this.getOption("defaultMessages"), this.createToken(res, requireValidToken));
    }), catchError((res) => this.handleResponseError(res)));
  }
  buildCodeRequestData(code) {
    const params = {
      grant_type: this.getOption("token.grantType"),
      code,
      redirect_uri: this.getOption("token.redirectUri"),
      client_id: this.getOption("clientId")
    };
    return this.urlEncodeParameters(this.cleanParams(this.addCredentialsToParams(params)));
  }
  buildRefreshRequestData(token) {
    const params = {
      grant_type: this.getOption("refresh.grantType"),
      refresh_token: token.getRefreshToken(),
      scope: this.getOption("refresh.scope"),
      client_id: this.getOption("clientId")
    };
    return this.urlEncodeParameters(this.cleanParams(this.addCredentialsToParams(params)));
  }
  buildPasswordRequestData(username, password) {
    const params = {
      grant_type: this.getOption("token.grantType"),
      username,
      password,
      scope: this.getOption("token.scope")
    };
    return this.urlEncodeParameters(this.cleanParams(this.addCredentialsToParams(params)));
  }
  buildAuthHeader() {
    if (this.clientAuthMethod === NbOAuth2ClientAuthMethod.BASIC) {
      if (this.getOption("clientId") && this.getOption("clientSecret")) {
        return new HttpHeaders({
          Authorization: "Basic " + btoa(this.getOption("clientId") + ":" + this.getOption("clientSecret"))
        });
      } else {
        throw Error("For basic client authentication method, please provide both clientId & clientSecret.");
      }
    }
    return void 0;
  }
  getHeaders() {
    let headers = super.getHeaders();
    headers = headers.append("Content-Type", "application/x-www-form-urlencoded");
    const authHeaders = this.buildAuthHeader();
    if (authHeaders === void 0) {
      return headers;
    }
    for (const headerKey of authHeaders.keys()) {
      for (const headerValue of authHeaders.getAll(headerKey)) {
        headers = headers.append(headerKey, headerValue);
      }
    }
    return headers;
  }
  cleanParams(params) {
    Object.entries(params).forEach(([key, val]) => !val && delete params[key]);
    return params;
  }
  addCredentialsToParams(params) {
    if (this.clientAuthMethod === NbOAuth2ClientAuthMethod.REQUEST_BODY) {
      if (this.getOption("clientId") && this.getOption("clientSecret")) {
        return __spreadProps(__spreadValues({}, params), {
          client_id: this.getOption("clientId"),
          client_secret: this.getOption("clientSecret")
        });
      } else {
        throw Error("For request body client authentication method, please provide both clientId & clientSecret.");
      }
    }
    return params;
  }
  handleResponseError(res) {
    let errors = [];
    if (res instanceof HttpErrorResponse) {
      if (res.error.error_description) {
        errors.push(res.error.error_description);
      } else {
        errors = this.getOption("defaultErrors");
      }
    } else if (res instanceof NbAuthIllegalTokenError) {
      errors.push(res.message);
    } else {
      errors.push("Something went wrong.");
    }
    return of(new NbAuthResult(false, res, this.getOption("redirect.failure"), errors, []));
  }
  buildRedirectUrl() {
    const params = __spreadValues({
      response_type: this.getOption("authorize.responseType"),
      client_id: this.getOption("clientId"),
      redirect_uri: this.getOption("authorize.redirectUri"),
      scope: this.getOption("authorize.scope"),
      state: this.getOption("authorize.state")
    }, this.getOption("authorize.params"));
    const endpoint = this.getActionEndpoint("authorize");
    const query = this.urlEncodeParameters(this.cleanParams(params));
    return `${endpoint}?${query}`;
  }
  parseHashAsQueryParams(hash) {
    return hash ? hash.split("&").reduce((acc, part) => {
      const item = part.split("=");
      acc[item[0]] = decodeURIComponent(item[1]);
      return acc;
    }, {}) : {};
  }
  urlEncodeParameters(params) {
    return Object.keys(params).map((k) => {
      return `${encodeURIComponent(k)}=${encodeURIComponent(params[k])}`;
    }).join("&");
  }
  createRefreshedToken(res, existingToken, requireValidToken) {
    const refreshedToken = this.createToken(res, requireValidToken);
    if (!refreshedToken.getRefreshToken() && existingToken.getRefreshToken()) {
      refreshedToken.setRefreshToken(existingToken.getRefreshToken());
    }
    return refreshedToken;
  }
  register(data) {
    throw new Error("`register` is not supported by `NbOAuth2AuthStrategy`, use `authenticate`.");
  }
  requestPassword(data) {
    throw new Error("`requestPassword` is not supported by `NbOAuth2AuthStrategy`, use `authenticate`.");
  }
  resetPassword(data = {}) {
    throw new Error("`resetPassword` is not supported by `NbOAuth2AuthStrategy`, use `authenticate`.");
  }
  logout() {
    return of(new NbAuthResult(true));
  }
};
_NbOAuth2AuthStrategy.ɵfac = function NbOAuth2AuthStrategy_Factory(t) {
  return new (t || _NbOAuth2AuthStrategy)(ɵɵinject(HttpClient), ɵɵinject(ActivatedRoute), ɵɵinject(NB_WINDOW));
};
_NbOAuth2AuthStrategy.ɵprov = ɵɵdefineInjectable({
  token: _NbOAuth2AuthStrategy,
  factory: _NbOAuth2AuthStrategy.ɵfac
});
var NbOAuth2AuthStrategy = _NbOAuth2AuthStrategy;
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(NbOAuth2AuthStrategy, [{
    type: Injectable
  }], () => [{
    type: HttpClient
  }, {
    type: ActivatedRoute
  }, {
    type: void 0,
    decorators: [{
      type: Inject,
      args: [NB_WINDOW]
    }]
  }], null);
})();
var NbPasswordAuthStrategyOptions = class extends NbAuthStrategyOptions {
  constructor() {
    super(...arguments);
    this.baseEndpoint = "/api/auth/";
    this.login = {
      alwaysFail: false,
      endpoint: "login",
      method: "post",
      requireValidToken: true,
      redirect: {
        success: "/",
        failure: null
      },
      defaultErrors: ["Login/Email combination is not correct, please try again."],
      defaultMessages: ["You have been successfully logged in."]
    };
    this.register = {
      alwaysFail: false,
      endpoint: "register",
      method: "post",
      requireValidToken: true,
      redirect: {
        success: "/",
        failure: null
      },
      defaultErrors: ["Something went wrong, please try again."],
      defaultMessages: ["You have been successfully registered."]
    };
    this.requestPass = {
      endpoint: "request-pass",
      method: "post",
      redirect: {
        success: "/",
        failure: null
      },
      defaultErrors: ["Something went wrong, please try again."],
      defaultMessages: ["Reset password instructions have been sent to your email."]
    };
    this.resetPass = {
      endpoint: "reset-pass",
      method: "put",
      redirect: {
        success: "/",
        failure: null
      },
      resetPasswordTokenKey: "reset_password_token",
      defaultErrors: ["Something went wrong, please try again."],
      defaultMessages: ["Your password has been successfully changed."]
    };
    this.logout = {
      alwaysFail: false,
      endpoint: "logout",
      method: "delete",
      redirect: {
        success: "/",
        failure: null
      },
      defaultErrors: ["Something went wrong, please try again."],
      defaultMessages: ["You have been successfully logged out."]
    };
    this.refreshToken = {
      endpoint: "refresh-token",
      method: "post",
      requireValidToken: true,
      redirect: {
        success: null,
        failure: null
      },
      defaultErrors: ["Something went wrong, please try again."],
      defaultMessages: ["Your token has been successfully refreshed."]
    };
    this.token = {
      class: NbAuthSimpleToken,
      key: "data.token",
      getter: (module, res, options) => getDeepFromObject(res.body, options.token.key)
    };
    this.errors = {
      key: "data.errors",
      getter: (module, res, options) => getDeepFromObject(res.error, options.errors.key, options[module].defaultErrors)
    };
    this.messages = {
      key: "data.messages",
      getter: (module, res, options) => getDeepFromObject(res.body, options.messages.key, options[module].defaultMessages)
    };
  }
};
var passwordStrategyOptions = new NbPasswordAuthStrategyOptions();
var _NbPasswordAuthStrategy = class _NbPasswordAuthStrategy extends NbAuthStrategy {
  static setup(options) {
    return [_NbPasswordAuthStrategy, options];
  }
  constructor(http, route) {
    super();
    this.http = http;
    this.route = route;
    this.defaultOptions = passwordStrategyOptions;
  }
  authenticate(data) {
    const module = "login";
    const method = this.getOption(`${module}.method`);
    const url = this.getActionEndpoint(module);
    const requireValidToken = this.getOption(`${module}.requireValidToken`);
    return this.http.request(method, url, {
      body: data,
      observe: "response",
      headers: this.getHeaders()
    }).pipe(map((res) => {
      if (this.getOption(`${module}.alwaysFail`)) {
        throw this.createFailResponse(data);
      }
      return res;
    }), map((res) => {
      return new NbAuthResult(true, res, this.getOption(`${module}.redirect.success`), [], this.getOption("messages.getter")(module, res, this.options), this.createToken(this.getOption("token.getter")(module, res, this.options), requireValidToken));
    }), catchError((res) => {
      return this.handleResponseError(res, module);
    }));
  }
  register(data) {
    const module = "register";
    const method = this.getOption(`${module}.method`);
    const url = this.getActionEndpoint(module);
    const requireValidToken = this.getOption(`${module}.requireValidToken`);
    return this.http.request(method, url, {
      body: data,
      observe: "response",
      headers: this.getHeaders()
    }).pipe(map((res) => {
      if (this.getOption(`${module}.alwaysFail`)) {
        throw this.createFailResponse(data);
      }
      return res;
    }), map((res) => {
      return new NbAuthResult(true, res, this.getOption(`${module}.redirect.success`), [], this.getOption("messages.getter")(module, res, this.options), this.createToken(this.getOption("token.getter")("login", res, this.options), requireValidToken));
    }), catchError((res) => {
      return this.handleResponseError(res, module);
    }));
  }
  requestPassword(data) {
    const module = "requestPass";
    const method = this.getOption(`${module}.method`);
    const url = this.getActionEndpoint(module);
    return this.http.request(method, url, {
      body: data,
      observe: "response",
      headers: this.getHeaders()
    }).pipe(map((res) => {
      if (this.getOption(`${module}.alwaysFail`)) {
        throw this.createFailResponse();
      }
      return res;
    }), map((res) => {
      return new NbAuthResult(true, res, this.getOption(`${module}.redirect.success`), [], this.getOption("messages.getter")(module, res, this.options));
    }), catchError((res) => {
      return this.handleResponseError(res, module);
    }));
  }
  resetPassword(data = {}) {
    const module = "resetPass";
    const method = this.getOption(`${module}.method`);
    const url = this.getActionEndpoint(module);
    const tokenKey = this.getOption(`${module}.resetPasswordTokenKey`);
    data[tokenKey] = this.route.snapshot.queryParams[tokenKey];
    return this.http.request(method, url, {
      body: data,
      observe: "response",
      headers: this.getHeaders()
    }).pipe(map((res) => {
      if (this.getOption(`${module}.alwaysFail`)) {
        throw this.createFailResponse();
      }
      return res;
    }), map((res) => {
      return new NbAuthResult(true, res, this.getOption(`${module}.redirect.success`), [], this.getOption("messages.getter")(module, res, this.options));
    }), catchError((res) => {
      return this.handleResponseError(res, module);
    }));
  }
  logout() {
    const module = "logout";
    const method = this.getOption(`${module}.method`);
    const url = this.getActionEndpoint(module);
    return of({}).pipe(switchMap((res) => {
      if (!url) {
        return of(res);
      }
      return this.http.request(method, url, {
        observe: "response",
        headers: this.getHeaders()
      });
    }), map((res) => {
      if (this.getOption(`${module}.alwaysFail`)) {
        throw this.createFailResponse();
      }
      return res;
    }), map((res) => {
      return new NbAuthResult(true, res, this.getOption(`${module}.redirect.success`), [], this.getOption("messages.getter")(module, res, this.options));
    }), catchError((res) => {
      return this.handleResponseError(res, module);
    }));
  }
  refreshToken(data) {
    const module = "refreshToken";
    const method = this.getOption(`${module}.method`);
    const url = this.getActionEndpoint(module);
    const requireValidToken = this.getOption(`${module}.requireValidToken`);
    return this.http.request(method, url, {
      body: data,
      observe: "response",
      headers: this.getHeaders()
    }).pipe(map((res) => {
      if (this.getOption(`${module}.alwaysFail`)) {
        throw this.createFailResponse(data);
      }
      return res;
    }), map((res) => {
      return new NbAuthResult(true, res, this.getOption(`${module}.redirect.success`), [], this.getOption("messages.getter")(module, res, this.options), this.createToken(this.getOption("token.getter")(module, res, this.options), requireValidToken));
    }), catchError((res) => {
      return this.handleResponseError(res, module);
    }));
  }
  handleResponseError(res, module) {
    let errors = [];
    if (res instanceof HttpErrorResponse) {
      errors = this.getOption("errors.getter")(module, res, this.options);
    } else if (res instanceof NbAuthIllegalTokenError) {
      errors.push(res.message);
    } else {
      errors.push("Something went wrong.");
    }
    return of(new NbAuthResult(false, res, this.getOption(`${module}.redirect.failure`), errors));
  }
};
_NbPasswordAuthStrategy.ɵfac = function NbPasswordAuthStrategy_Factory(t) {
  return new (t || _NbPasswordAuthStrategy)(ɵɵinject(HttpClient), ɵɵinject(ActivatedRoute));
};
_NbPasswordAuthStrategy.ɵprov = ɵɵdefineInjectable({
  token: _NbPasswordAuthStrategy,
  factory: _NbPasswordAuthStrategy.ɵfac
});
var NbPasswordAuthStrategy = _NbPasswordAuthStrategy;
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(NbPasswordAuthStrategy, [{
    type: Injectable
  }], () => [{
    type: HttpClient
  }, {
    type: ActivatedRoute
  }], null);
})();
var _NbAuthBlockComponent = class _NbAuthBlockComponent {
};
_NbAuthBlockComponent.ɵfac = function NbAuthBlockComponent_Factory(t) {
  return new (t || _NbAuthBlockComponent)();
};
_NbAuthBlockComponent.ɵcmp = ɵɵdefineComponent({
  type: _NbAuthBlockComponent,
  selectors: [["nb-auth-block"]],
  ngContentSelectors: _c0,
  decls: 1,
  vars: 0,
  template: function NbAuthBlockComponent_Template(rf, ctx) {
    if (rf & 1) {
      ɵɵprojectionDef();
      ɵɵprojection(0);
    }
  },
  styles: ["\n\n\n\n\n[_nghost-%COMP%]{display:block;width:100%;max-width:35rem}[_nghost-%COMP%]     form{width:100%}[_nghost-%COMP%]     .label{display:block;margin-bottom:.5rem}[_nghost-%COMP%]     .forgot-password{text-decoration:none;margin-bottom:.5rem}[_nghost-%COMP%]     .caption{margin-top:.5rem}[_nghost-%COMP%]     .alert{text-align:center}[_nghost-%COMP%]     .title{margin-top:0;margin-bottom:.75rem;text-align:center}[_nghost-%COMP%]     .sub-title{margin-bottom:2rem;text-align:center}[_nghost-%COMP%]     .form-control-group{margin-bottom:2rem}[_nghost-%COMP%]     .form-control-group.accept-group{display:flex;justify-content:space-between;margin:2rem 0}[_nghost-%COMP%]     .label-with-link{display:flex;justify-content:space-between}[_nghost-%COMP%]     .links{text-align:center;margin-top:1.75rem}[_nghost-%COMP%]     .links .socials{margin-top:1.5rem}[_nghost-%COMP%]     .links .socials a{margin:0 1rem;text-decoration:none;vertical-align:middle}[_nghost-%COMP%]     .links .socials a.with-icon{font-size:2rem}[_nghost-%COMP%]     .another-action{margin-top:2rem;text-align:center}[_nghost-%COMP%]     .sign-in-or-up{margin-top:2rem;display:flex;justify-content:space-between}[_nghost-%COMP%]     nb-alert .alert-title, [_nghost-%COMP%]     nb-alert .alert-message{margin:0 0 .5rem}[_nghost-%COMP%]     nb-alert .alert-message-list{list-style-type:none;padding:0;margin:0}"]
});
var NbAuthBlockComponent = _NbAuthBlockComponent;
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(NbAuthBlockComponent, [{
    type: Component,
    args: [{
      selector: "nb-auth-block",
      template: `
    <ng-content></ng-content>
  `,
      styles: ["/**\n * @license\n * Copyright Akveo. All Rights Reserved.\n * Licensed under the MIT License. See License.txt in the project root for license information.\n */:host{display:block;width:100%;max-width:35rem}:host ::ng-deep form{width:100%}:host ::ng-deep .label{display:block;margin-bottom:.5rem}:host ::ng-deep .forgot-password{text-decoration:none;margin-bottom:.5rem}:host ::ng-deep .caption{margin-top:.5rem}:host ::ng-deep .alert{text-align:center}:host ::ng-deep .title{margin-top:0;margin-bottom:.75rem;text-align:center}:host ::ng-deep .sub-title{margin-bottom:2rem;text-align:center}:host ::ng-deep .form-control-group{margin-bottom:2rem}:host ::ng-deep .form-control-group.accept-group{display:flex;justify-content:space-between;margin:2rem 0}:host ::ng-deep .label-with-link{display:flex;justify-content:space-between}:host ::ng-deep .links{text-align:center;margin-top:1.75rem}:host ::ng-deep .links .socials{margin-top:1.5rem}:host ::ng-deep .links .socials a{margin:0 1rem;text-decoration:none;vertical-align:middle}:host ::ng-deep .links .socials a.with-icon{font-size:2rem}:host ::ng-deep .another-action{margin-top:2rem;text-align:center}:host ::ng-deep .sign-in-or-up{margin-top:2rem;display:flex;justify-content:space-between}:host ::ng-deep nb-alert .alert-title,:host ::ng-deep nb-alert .alert-message{margin:0 0 .5rem}:host ::ng-deep nb-alert .alert-message-list{list-style-type:none;padding:0;margin:0}\n"]
    }]
  }], null, null);
})();
var _NbAuthComponent = class _NbAuthComponent {
  // showcase of how to use the onAuthenticationChange method
  constructor(auth, location) {
    this.auth = auth;
    this.location = location;
    this.destroy$ = new Subject();
    this.authenticated = false;
    this.token = "";
    this.subscription = auth.onAuthenticationChange().pipe(takeUntil(this.destroy$)).subscribe((authenticated) => {
      this.authenticated = authenticated;
    });
  }
  back() {
    this.location.back();
    return false;
  }
  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }
};
_NbAuthComponent.ɵfac = function NbAuthComponent_Factory(t) {
  return new (t || _NbAuthComponent)(ɵɵdirectiveInject(NbAuthService), ɵɵdirectiveInject(Location));
};
_NbAuthComponent.ɵcmp = ɵɵdefineComponent({
  type: _NbAuthComponent,
  selectors: [["nb-auth"]],
  decls: 10,
  vars: 0,
  consts: [[1, "navigation"], ["href", "#", "aria-label", "Back", 1, "link", "back-link", 3, "click"], ["icon", "arrow-back"]],
  template: function NbAuthComponent_Template(rf, ctx) {
    if (rf & 1) {
      ɵɵelementStart(0, "nb-layout")(1, "nb-layout-column")(2, "nb-card")(3, "nb-card-header")(4, "nav", 0)(5, "a", 1);
      ɵɵlistener("click", function NbAuthComponent_Template_a_click_5_listener() {
        return ctx.back();
      });
      ɵɵelement(6, "nb-icon", 2);
      ɵɵelementEnd()()();
      ɵɵelementStart(7, "nb-card-body")(8, "nb-auth-block");
      ɵɵelement(9, "router-outlet");
      ɵɵelementEnd()()()()();
    }
  },
  dependencies: [NbLayoutComponent, NbLayoutColumnComponent, NbCardComponent, NbCardBodyComponent, NbCardHeaderComponent, RouterOutlet, NbIconComponent, NbAuthBlockComponent],
  styles: [".visually-hidden[_ngcontent-%COMP%]{position:absolute!important;height:1px;width:1px;overflow:hidden;clip:rect(1px 1px 1px 1px);clip:rect(1px,1px,1px,1px)}.cdk-overlay-container[_ngcontent-%COMP%], .cdk-global-overlay-wrapper[_ngcontent-%COMP%]{pointer-events:none;top:0;left:0;height:100%;width:100%}.cdk-overlay-container[_ngcontent-%COMP%]{position:fixed;z-index:1000}.cdk-overlay-container[_ngcontent-%COMP%]:empty{display:none}.cdk-global-overlay-wrapper[_ngcontent-%COMP%]{display:flex;position:absolute;z-index:1000}.cdk-overlay-pane[_ngcontent-%COMP%]{position:absolute;pointer-events:auto;box-sizing:border-box;z-index:1000;display:flex;max-width:100%;max-height:100%}.cdk-overlay-backdrop[_ngcontent-%COMP%]{position:absolute;inset:0;z-index:1000;pointer-events:auto;-webkit-tap-highlight-color:rgba(0,0,0,0);transition:opacity .4s cubic-bezier(.25,.8,.25,1);opacity:0}.cdk-overlay-backdrop.cdk-overlay-backdrop-showing[_ngcontent-%COMP%]{opacity:1}.cdk-high-contrast-active[_ngcontent-%COMP%]   .cdk-overlay-backdrop.cdk-overlay-backdrop-showing[_ngcontent-%COMP%]{opacity:.6}.cdk-overlay-dark-backdrop[_ngcontent-%COMP%]{background:#00000052}.cdk-overlay-transparent-backdrop[_ngcontent-%COMP%]{transition:visibility 1ms linear,opacity 1ms linear;visibility:hidden;opacity:1}.cdk-overlay-transparent-backdrop.cdk-overlay-backdrop-showing[_ngcontent-%COMP%]{opacity:0;visibility:visible}.cdk-overlay-backdrop-noop-animation[_ngcontent-%COMP%]{transition:none}.cdk-overlay-connected-position-bounding-box[_ngcontent-%COMP%]{position:absolute;z-index:1000;display:flex;flex-direction:column;min-width:1px;min-height:1px}.cdk-global-scrollblock[_ngcontent-%COMP%]{position:fixed;width:100%;overflow-y:scroll}.nb-global-scrollblock[_ngcontent-%COMP%]{position:static;width:auto;overflow:hidden}\n\n\n\n\n\n\n\n\n\nhtml[_ngcontent-%COMP%]{box-sizing:border-box}*[_ngcontent-%COMP%], *[_ngcontent-%COMP%]:before, *[_ngcontent-%COMP%]:after{box-sizing:inherit}html[_ngcontent-%COMP%], body[_ngcontent-%COMP%]{margin:0;padding:0}html[_ngcontent-%COMP%]{line-height:1.15;-webkit-text-size-adjust:100%}body[_ngcontent-%COMP%]{margin:0}h1[_ngcontent-%COMP%]{font-size:2em;margin:.67em 0}hr[_ngcontent-%COMP%]{box-sizing:content-box;height:0;overflow:visible}pre[_ngcontent-%COMP%]{font-family:monospace,monospace;font-size:1em}a[_ngcontent-%COMP%]{background-color:transparent}abbr[title][_ngcontent-%COMP%]{border-bottom:none;text-decoration:underline;text-decoration:underline dotted}b[_ngcontent-%COMP%], strong[_ngcontent-%COMP%]{font-weight:bolder}code[_ngcontent-%COMP%], kbd[_ngcontent-%COMP%], samp[_ngcontent-%COMP%]{font-family:monospace,monospace;font-size:1em}small[_ngcontent-%COMP%]{font-size:80%}sub[_ngcontent-%COMP%], sup[_ngcontent-%COMP%]{font-size:75%;line-height:0;position:relative;vertical-align:baseline}sub[_ngcontent-%COMP%]{bottom:-.25em}sup[_ngcontent-%COMP%]{top:-.5em}img[_ngcontent-%COMP%]{border-style:none}button[_ngcontent-%COMP%], input[_ngcontent-%COMP%], optgroup[_ngcontent-%COMP%], select[_ngcontent-%COMP%], textarea[_ngcontent-%COMP%]{font-family:inherit;font-size:100%;line-height:1.15;margin:0}button[_ngcontent-%COMP%], input[_ngcontent-%COMP%]{overflow:visible}button[_ngcontent-%COMP%], select[_ngcontent-%COMP%]{text-transform:none}button[_ngcontent-%COMP%], [type=button][_ngcontent-%COMP%], [type=reset][_ngcontent-%COMP%], [type=submit][_ngcontent-%COMP%]{-webkit-appearance:button}button[_ngcontent-%COMP%]::-moz-focus-inner, [type=button][_ngcontent-%COMP%]::-moz-focus-inner, [type=reset][_ngcontent-%COMP%]::-moz-focus-inner, [type=submit][_ngcontent-%COMP%]::-moz-focus-inner{border-style:none;padding:0}button[_ngcontent-%COMP%]:-moz-focusring, [type=button][_ngcontent-%COMP%]:-moz-focusring, [type=reset][_ngcontent-%COMP%]:-moz-focusring, [type=submit][_ngcontent-%COMP%]:-moz-focusring{outline:1px dotted ButtonText}fieldset[_ngcontent-%COMP%]{padding:.35em .75em .625em}legend[_ngcontent-%COMP%]{box-sizing:border-box;color:inherit;display:table;max-width:100%;padding:0;white-space:normal}progress[_ngcontent-%COMP%]{vertical-align:baseline}textarea[_ngcontent-%COMP%]{overflow:auto}[type=checkbox][_ngcontent-%COMP%], [type=radio][_ngcontent-%COMP%]{box-sizing:border-box;padding:0}[type=number][_ngcontent-%COMP%]::-webkit-inner-spin-button, [type=number][_ngcontent-%COMP%]::-webkit-outer-spin-button{height:auto}[type=search][_ngcontent-%COMP%]{-webkit-appearance:textfield;outline-offset:-2px}[type=search][_ngcontent-%COMP%]::-webkit-search-decoration{-webkit-appearance:none}[_ngcontent-%COMP%]::-webkit-file-upload-button{-webkit-appearance:button;font:inherit}details[_ngcontent-%COMP%]{display:block}summary[_ngcontent-%COMP%]{display:list-item}template[_ngcontent-%COMP%]{display:none}[hidden][_ngcontent-%COMP%]{display:none}\n\n\n\n\n[_nghost-%COMP%]   nb-card[_ngcontent-%COMP%]{margin:0;height:calc(100vh - 5rem)}[_nghost-%COMP%]   .navigation[_ngcontent-%COMP%]   .link[_ngcontent-%COMP%]{display:inline-block;text-decoration:none}[_nghost-%COMP%]   .navigation[_ngcontent-%COMP%]   .link[_ngcontent-%COMP%]   nb-icon[_ngcontent-%COMP%]{font-size:2rem;vertical-align:middle}[_nghost-%COMP%]   .links[_ngcontent-%COMP%]   nb-icon[_ngcontent-%COMP%]{font-size:2.5rem}[_nghost-%COMP%]   nb-card-body[_ngcontent-%COMP%]{display:flex;width:100%}[_nghost-%COMP%]   nb-auth-block[_ngcontent-%COMP%]{margin:auto}@media (max-width: 767.98px){[_nghost-%COMP%]   nb-card[_ngcontent-%COMP%]{border-radius:0;height:100vh}}[_nghost-%COMP%]     nb-layout .layout .layout-container .content .columns nb-layout-column{padding:2.5rem}@media (max-width: 767.98px){[_nghost-%COMP%]     nb-layout .layout .layout-container .content .columns nb-layout-column{padding:0}}"]
});
var NbAuthComponent = _NbAuthComponent;
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(NbAuthComponent, [{
    type: Component,
    args: [{
      selector: "nb-auth",
      template: `
    <nb-layout>
      <nb-layout-column>
        <nb-card>
          <nb-card-header>
            <nav class="navigation">
              <a href="#" (click)="back()" class="link back-link" aria-label="Back">
                <nb-icon icon="arrow-back"></nb-icon>
              </a>
            </nav>
          </nb-card-header>
          <nb-card-body>
            <nb-auth-block>
              <router-outlet></router-outlet>
            </nb-auth-block>
          </nb-card-body>
        </nb-card>
      </nb-layout-column>
    </nb-layout>
  `,
      styles: [".visually-hidden{position:absolute!important;height:1px;width:1px;overflow:hidden;clip:rect(1px 1px 1px 1px);clip:rect(1px,1px,1px,1px)}.cdk-overlay-container,.cdk-global-overlay-wrapper{pointer-events:none;top:0;left:0;height:100%;width:100%}.cdk-overlay-container{position:fixed;z-index:1000}.cdk-overlay-container:empty{display:none}.cdk-global-overlay-wrapper{display:flex;position:absolute;z-index:1000}.cdk-overlay-pane{position:absolute;pointer-events:auto;box-sizing:border-box;z-index:1000;display:flex;max-width:100%;max-height:100%}.cdk-overlay-backdrop{position:absolute;inset:0;z-index:1000;pointer-events:auto;-webkit-tap-highlight-color:rgba(0,0,0,0);transition:opacity .4s cubic-bezier(.25,.8,.25,1);opacity:0}.cdk-overlay-backdrop.cdk-overlay-backdrop-showing{opacity:1}.cdk-high-contrast-active .cdk-overlay-backdrop.cdk-overlay-backdrop-showing{opacity:.6}.cdk-overlay-dark-backdrop{background:#00000052}.cdk-overlay-transparent-backdrop{transition:visibility 1ms linear,opacity 1ms linear;visibility:hidden;opacity:1}.cdk-overlay-transparent-backdrop.cdk-overlay-backdrop-showing{opacity:0;visibility:visible}.cdk-overlay-backdrop-noop-animation{transition:none}.cdk-overlay-connected-position-bounding-box{position:absolute;z-index:1000;display:flex;flex-direction:column;min-width:1px;min-height:1px}.cdk-global-scrollblock{position:fixed;width:100%;overflow-y:scroll}.nb-global-scrollblock{position:static;width:auto;overflow:hidden}/*\n * @license\n * Copyright Akveo. All Rights Reserved.\n * Licensed under the MIT License. See License.txt in the project root for license information.\n *//*!\n * @license\n * Copyright Akveo. All Rights Reserved.\n * Licensed under the MIT License. See License.txt in the project root for license information.\n */html{box-sizing:border-box}*,*:before,*:after{box-sizing:inherit}html,body{margin:0;padding:0}html{line-height:1.15;-webkit-text-size-adjust:100%}body{margin:0}h1{font-size:2em;margin:.67em 0}hr{box-sizing:content-box;height:0;overflow:visible}pre{font-family:monospace,monospace;font-size:1em}a{background-color:transparent}abbr[title]{border-bottom:none;text-decoration:underline;text-decoration:underline dotted}b,strong{font-weight:bolder}code,kbd,samp{font-family:monospace,monospace;font-size:1em}small{font-size:80%}sub,sup{font-size:75%;line-height:0;position:relative;vertical-align:baseline}sub{bottom:-.25em}sup{top:-.5em}img{border-style:none}button,input,optgroup,select,textarea{font-family:inherit;font-size:100%;line-height:1.15;margin:0}button,input{overflow:visible}button,select{text-transform:none}button,[type=button],[type=reset],[type=submit]{-webkit-appearance:button}button::-moz-focus-inner,[type=button]::-moz-focus-inner,[type=reset]::-moz-focus-inner,[type=submit]::-moz-focus-inner{border-style:none;padding:0}button:-moz-focusring,[type=button]:-moz-focusring,[type=reset]:-moz-focusring,[type=submit]:-moz-focusring{outline:1px dotted ButtonText}fieldset{padding:.35em .75em .625em}legend{box-sizing:border-box;color:inherit;display:table;max-width:100%;padding:0;white-space:normal}progress{vertical-align:baseline}textarea{overflow:auto}[type=checkbox],[type=radio]{box-sizing:border-box;padding:0}[type=number]::-webkit-inner-spin-button,[type=number]::-webkit-outer-spin-button{height:auto}[type=search]{-webkit-appearance:textfield;outline-offset:-2px}[type=search]::-webkit-search-decoration{-webkit-appearance:none}::-webkit-file-upload-button{-webkit-appearance:button;font:inherit}details{display:block}summary{display:list-item}template{display:none}[hidden]{display:none}/**\n * @license\n * Copyright Akveo. All Rights Reserved.\n * Licensed under the MIT License. See License.txt in the project root for license information.\n */:host nb-card{margin:0;height:calc(100vh - 5rem)}:host .navigation .link{display:inline-block;text-decoration:none}:host .navigation .link nb-icon{font-size:2rem;vertical-align:middle}:host .links nb-icon{font-size:2.5rem}:host nb-card-body{display:flex;width:100%}:host nb-auth-block{margin:auto}@media (max-width: 767.98px){:host nb-card{border-radius:0;height:100vh}}:host ::ng-deep nb-layout .layout .layout-container .content .columns nb-layout-column{padding:2.5rem}@media (max-width: 767.98px){:host ::ng-deep nb-layout .layout .layout-container .content .columns nb-layout-column{padding:0}}\n"]
    }]
  }], () => [{
    type: NbAuthService
  }, {
    type: Location
  }], null);
})();
var _NbLoginComponent = class _NbLoginComponent {
  constructor(service, options = {}, cd, router) {
    this.service = service;
    this.options = options;
    this.cd = cd;
    this.router = router;
    this.redirectDelay = 0;
    this.showMessages = {};
    this.strategy = "";
    this.errors = [];
    this.messages = [];
    this.user = {};
    this.submitted = false;
    this.socialLinks = [];
    this.rememberMe = false;
    this.redirectDelay = this.getConfigValue("forms.login.redirectDelay");
    this.showMessages = this.getConfigValue("forms.login.showMessages");
    this.strategy = this.getConfigValue("forms.login.strategy");
    this.socialLinks = this.getConfigValue("forms.login.socialLinks");
    this.rememberMe = this.getConfigValue("forms.login.rememberMe");
  }
  login() {
    this.errors = [];
    this.messages = [];
    this.submitted = true;
    this.service.authenticate(this.strategy, this.user).subscribe((result) => {
      this.submitted = false;
      if (result.isSuccess()) {
        this.messages = result.getMessages();
      } else {
        this.errors = result.getErrors();
      }
      const redirect = result.getRedirect();
      if (redirect) {
        setTimeout(() => {
          return this.router.navigateByUrl(redirect);
        }, this.redirectDelay);
      }
      this.cd.detectChanges();
    });
  }
  getConfigValue(key) {
    return getDeepFromObject(this.options, key, null);
  }
};
_NbLoginComponent.ɵfac = function NbLoginComponent_Factory(t) {
  return new (t || _NbLoginComponent)(ɵɵdirectiveInject(NbAuthService), ɵɵdirectiveInject(NB_AUTH_OPTIONS), ɵɵdirectiveInject(ChangeDetectorRef), ɵɵdirectiveInject(Router));
};
_NbLoginComponent.ɵcmp = ɵɵdefineComponent({
  type: _NbLoginComponent,
  selectors: [["nb-login"]],
  decls: 32,
  vars: 19,
  consts: [["form", "ngForm"], ["email", "ngModel"], ["password", "ngModel"], ["title", ""], ["id", "title", 1, "title"], [1, "sub-title"], ["outline", "danger", "role", "alert", 4, "ngIf"], ["outline", "success", "role", "alert", 4, "ngIf"], ["aria-labelledby", "title", 3, "ngSubmit"], [1, "form-control-group"], ["for", "input-email", 1, "label"], ["nbInput", "", "fullWidth", "", "name", "email", "id", "input-email", "pattern", ".+@.+\\..+", "placeholder", "Email address", "fieldSize", "large", "autofocus", "", 3, "ngModelChange", "ngModel", "status", "required"], [4, "ngIf"], [1, "label-with-link"], ["for", "input-password", 1, "label"], ["routerLink", "../request-password", 1, "forgot-password", "caption-2"], ["nbInput", "", "fullWidth", "", "name", "password", "type", "password", "id", "input-password", "placeholder", "Password", "fieldSize", "large", 3, "ngModelChange", "ngModel", "status", "required", "minlength", "maxlength"], [1, "form-control-group", "accept-group"], ["name", "rememberMe", 3, "ngModel", "ngModelChange", 4, "ngIf"], ["nbButton", "", "fullWidth", "", "status", "primary", "size", "large", 3, "disabled"], ["class", "links", "aria-label", "Social sign in", 4, "ngIf"], ["aria-label", "Register", 1, "another-action"], ["routerLink", "../register", 1, "text-link"], ["outline", "danger", "role", "alert"], [1, "alert-title"], [1, "alert-message-list"], ["class", "alert-message", 4, "ngFor", "ngForOf"], [1, "alert-message"], ["outline", "success", "role", "alert"], ["class", "caption status-danger", 4, "ngIf"], [1, "caption", "status-danger"], ["name", "rememberMe", 3, "ngModelChange", "ngModel"], ["aria-label", "Social sign in", 1, "links"], [1, "socials"], [4, "ngFor", "ngForOf"], [3, "routerLink", "with-icon", 4, "ngIf"], [3, "with-icon", 4, "ngIf"], [3, "routerLink"], [3, "icon", 4, "ngIf", "ngIfElse"], [3, "icon"]],
  template: function NbLoginComponent_Template(rf, ctx) {
    if (rf & 1) {
      const _r1 = ɵɵgetCurrentView();
      ɵɵelementStart(0, "h1", 4);
      ɵɵtext(1, "Login");
      ɵɵelementEnd();
      ɵɵelementStart(2, "p", 5);
      ɵɵtext(3, "Hello! Log in with your email.");
      ɵɵelementEnd();
      ɵɵtemplate(4, NbLoginComponent_nb_alert_4_Template, 6, 1, "nb-alert", 6)(5, NbLoginComponent_nb_alert_5_Template, 6, 1, "nb-alert", 7);
      ɵɵelementStart(6, "form", 8, 0);
      ɵɵlistener("ngSubmit", function NbLoginComponent_Template_form_ngSubmit_6_listener() {
        ɵɵrestoreView(_r1);
        return ɵɵresetView(ctx.login());
      });
      ɵɵelementStart(8, "div", 9)(9, "label", 10);
      ɵɵtext(10, "Email address:");
      ɵɵelementEnd();
      ɵɵelementStart(11, "input", 11, 1);
      ɵɵtwoWayListener("ngModelChange", function NbLoginComponent_Template_input_ngModelChange_11_listener($event) {
        ɵɵrestoreView(_r1);
        ɵɵtwoWayBindingSet(ctx.user.email, $event) || (ctx.user.email = $event);
        return ɵɵresetView($event);
      });
      ɵɵelementEnd();
      ɵɵtemplate(13, NbLoginComponent_ng_container_13_Template, 3, 2, "ng-container", 12);
      ɵɵelementEnd();
      ɵɵelementStart(14, "div", 9)(15, "span", 13)(16, "label", 14);
      ɵɵtext(17, "Password:");
      ɵɵelementEnd();
      ɵɵelementStart(18, "a", 15);
      ɵɵtext(19, "Forgot Password?");
      ɵɵelementEnd()();
      ɵɵelementStart(20, "input", 16, 2);
      ɵɵtwoWayListener("ngModelChange", function NbLoginComponent_Template_input_ngModelChange_20_listener($event) {
        ɵɵrestoreView(_r1);
        ɵɵtwoWayBindingSet(ctx.user.password, $event) || (ctx.user.password = $event);
        return ɵɵresetView($event);
      });
      ɵɵelementEnd();
      ɵɵtemplate(22, NbLoginComponent_ng_container_22_Template, 3, 2, "ng-container", 12);
      ɵɵelementEnd();
      ɵɵelementStart(23, "div", 17);
      ɵɵtemplate(24, NbLoginComponent_nb_checkbox_24_Template, 2, 1, "nb-checkbox", 18);
      ɵɵelementEnd();
      ɵɵelementStart(25, "button", 19);
      ɵɵtext(26, " Log In ");
      ɵɵelementEnd()();
      ɵɵtemplate(27, NbLoginComponent_section_27_Template, 4, 1, "section", 20);
      ɵɵelementStart(28, "section", 21);
      ɵɵtext(29, " Don't have an account? ");
      ɵɵelementStart(30, "a", 22);
      ɵɵtext(31, "Register");
      ɵɵelementEnd()();
    }
    if (rf & 2) {
      const form_r11 = ɵɵreference(7);
      const email_r5 = ɵɵreference(12);
      const password_r6 = ɵɵreference(21);
      ɵɵadvance(4);
      ɵɵproperty("ngIf", ctx.showMessages.error && (ctx.errors == null ? null : ctx.errors.length) && !ctx.submitted);
      ɵɵadvance();
      ɵɵproperty("ngIf", ctx.showMessages.success && (ctx.messages == null ? null : ctx.messages.length) && !ctx.submitted);
      ɵɵadvance(6);
      ɵɵtwoWayProperty("ngModel", ctx.user.email);
      ɵɵproperty("status", email_r5.dirty ? email_r5.invalid ? "danger" : "success" : "basic")("required", ctx.getConfigValue("forms.validation.email.required"));
      ɵɵattribute("aria-invalid", email_r5.invalid && email_r5.touched ? true : null);
      ɵɵadvance(2);
      ɵɵproperty("ngIf", email_r5.invalid && email_r5.touched);
      ɵɵadvance(7);
      ɵɵtwoWayProperty("ngModel", ctx.user.password);
      ɵɵproperty("status", password_r6.dirty ? password_r6.invalid ? "danger" : "success" : "basic")("required", ctx.getConfigValue("forms.validation.password.required"))("minlength", ctx.getConfigValue("forms.validation.password.minLength"))("maxlength", ctx.getConfigValue("forms.validation.password.maxLength"));
      ɵɵattribute("aria-invalid", password_r6.invalid && password_r6.touched ? true : null);
      ɵɵadvance(2);
      ɵɵproperty("ngIf", password_r6.invalid && password_r6.touched);
      ɵɵadvance(2);
      ɵɵproperty("ngIf", ctx.rememberMe);
      ɵɵadvance();
      ɵɵclassProp("btn-pulse", ctx.submitted);
      ɵɵproperty("disabled", ctx.submitted || !form_r11.valid);
      ɵɵadvance(2);
      ɵɵproperty("ngIf", ctx.socialLinks && ctx.socialLinks.length > 0);
    }
  },
  dependencies: [NgForOf, NgIf, NbCheckboxComponent, NbAlertComponent, NbInputDirective, NbButtonComponent, RouterLink, ɵNgNoValidate, DefaultValueAccessor, NgControlStatus, NgControlStatusGroup, RequiredValidator, MinLengthValidator, MaxLengthValidator, PatternValidator, NgModel, NgForm, NbIconComponent],
  encapsulation: 2,
  changeDetection: 0
});
var NbLoginComponent = _NbLoginComponent;
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(NbLoginComponent, [{
    type: Component,
    args: [{
      selector: "nb-login",
      changeDetection: ChangeDetectionStrategy.OnPush,
      template: `<h1 id="title" class="title">Login</h1>
<p class="sub-title">Hello! Log in with your email.</p>

<nb-alert *ngIf="showMessages.error && errors?.length && !submitted" outline="danger" role="alert">
  <p class="alert-title"><b>Oh snap!</b></p>
  <ul class="alert-message-list">
    <li *ngFor="let error of errors" class="alert-message">{{ error }}</li>
  </ul>
</nb-alert>

<nb-alert *ngIf="showMessages.success && messages?.length && !submitted" outline="success" role="alert">
  <p class="alert-title"><b>Hooray!</b></p>
  <ul class="alert-message-list">
    <li *ngFor="let message of messages" class="alert-message">{{ message }}</li>
  </ul>
</nb-alert>

<form (ngSubmit)="login()" #form="ngForm" aria-labelledby="title">

  <div class="form-control-group">
    <label class="label" for="input-email">Email address:</label>
    <input nbInput
           fullWidth
           [(ngModel)]="user.email"
           #email="ngModel"
           name="email"
           id="input-email"
           pattern=".+@.+\\..+"
           placeholder="Email address"
           fieldSize="large"
           autofocus
           [status]="email.dirty ? (email.invalid  ? 'danger' : 'success') : 'basic'"
           [required]="getConfigValue('forms.validation.email.required')"
           [attr.aria-invalid]="email.invalid && email.touched ? true : null">
    <ng-container *ngIf="email.invalid && email.touched">
      <p class="caption status-danger" *ngIf="email.errors?.required">
        Email is required!
      </p>
      <p class="caption status-danger" *ngIf="email.errors?.pattern">
        Email should be the real one!
      </p>
    </ng-container>
  </div>

  <div class="form-control-group">
    <span class="label-with-link">
      <label class="label" for="input-password">Password:</label>
      <a class="forgot-password caption-2" routerLink="../request-password">Forgot Password?</a>
    </span>
    <input nbInput
           fullWidth
           [(ngModel)]="user.password"
           #password="ngModel"
           name="password"
           type="password"
           id="input-password"
           placeholder="Password"
           fieldSize="large"
           [status]="password.dirty ? (password.invalid  ? 'danger' : 'success') : 'basic'"
           [required]="getConfigValue('forms.validation.password.required')"
           [minlength]="getConfigValue('forms.validation.password.minLength')"
           [maxlength]="getConfigValue('forms.validation.password.maxLength')"
           [attr.aria-invalid]="password.invalid && password.touched ? true : null">
    <ng-container *ngIf="password.invalid && password.touched ">
      <p class="caption status-danger" *ngIf="password.errors?.required">
        Password is required!
      </p>
      <p class="caption status-danger" *ngIf="password.errors?.minlength || password.errors?.maxlength">
        Password should contain
        from {{ getConfigValue('forms.validation.password.minLength') }}
        to {{ getConfigValue('forms.validation.password.maxLength') }}
        characters
      </p>
    </ng-container>
  </div>

  <div class="form-control-group accept-group">
    <nb-checkbox name="rememberMe" [(ngModel)]="user.rememberMe" *ngIf="rememberMe">Remember me</nb-checkbox>
  </div>

  <button nbButton
          fullWidth
          status="primary"
          size="large"
          [disabled]="submitted || !form.valid"
          [class.btn-pulse]="submitted">
    Log In
  </button>
</form>

<section *ngIf="socialLinks && socialLinks.length > 0" class="links" aria-label="Social sign in">
  or enter with:
  <div class="socials">
    <ng-container *ngFor="let socialLink of socialLinks">
      <a *ngIf="socialLink.link"
         [routerLink]="socialLink.link"
         [attr.target]="socialLink.target"
         [attr.class]="socialLink.icon"
         [class.with-icon]="socialLink.icon">
        <nb-icon *ngIf="socialLink.icon; else title" [icon]="socialLink.icon"></nb-icon>
        <ng-template #title>{{ socialLink.title }}</ng-template>
      </a>
      <a *ngIf="socialLink.url"
         [attr.href]="socialLink.url"
         [attr.target]="socialLink.target"
         [attr.class]="socialLink.icon"
         [class.with-icon]="socialLink.icon">
        <nb-icon *ngIf="socialLink.icon; else title" [icon]="socialLink.icon"></nb-icon>
        <ng-template #title>{{ socialLink.title }}</ng-template>
      </a>
    </ng-container>
  </div>
</section>

<section class="another-action" aria-label="Register">
  Don't have an account? <a class="text-link" routerLink="../register">Register</a>
</section>
`
    }]
  }], () => [{
    type: NbAuthService
  }, {
    type: void 0,
    decorators: [{
      type: Inject,
      args: [NB_AUTH_OPTIONS]
    }]
  }, {
    type: ChangeDetectorRef
  }, {
    type: Router
  }], null);
})();
var _NbRegisterComponent = class _NbRegisterComponent {
  constructor(service, options = {}, cd, router) {
    this.service = service;
    this.options = options;
    this.cd = cd;
    this.router = router;
    this.redirectDelay = 0;
    this.showMessages = {};
    this.strategy = "";
    this.submitted = false;
    this.errors = [];
    this.messages = [];
    this.user = {};
    this.socialLinks = [];
    this.redirectDelay = this.getConfigValue("forms.register.redirectDelay");
    this.showMessages = this.getConfigValue("forms.register.showMessages");
    this.strategy = this.getConfigValue("forms.register.strategy");
    this.socialLinks = this.getConfigValue("forms.login.socialLinks");
  }
  register() {
    this.errors = this.messages = [];
    this.submitted = true;
    this.service.register(this.strategy, this.user).subscribe((result) => {
      this.submitted = false;
      if (result.isSuccess()) {
        this.messages = result.getMessages();
      } else {
        this.errors = result.getErrors();
      }
      const redirect = result.getRedirect();
      if (redirect) {
        setTimeout(() => {
          return this.router.navigateByUrl(redirect);
        }, this.redirectDelay);
      }
      this.cd.detectChanges();
    });
  }
  getConfigValue(key) {
    return getDeepFromObject(this.options, key, null);
  }
};
_NbRegisterComponent.ɵfac = function NbRegisterComponent_Factory(t) {
  return new (t || _NbRegisterComponent)(ɵɵdirectiveInject(NbAuthService), ɵɵdirectiveInject(NB_AUTH_OPTIONS), ɵɵdirectiveInject(ChangeDetectorRef), ɵɵdirectiveInject(Router));
};
_NbRegisterComponent.ɵcmp = ɵɵdefineComponent({
  type: _NbRegisterComponent,
  selectors: [["nb-register"]],
  decls: 38,
  vars: 31,
  consts: [["form", "ngForm"], ["fullName", "ngModel"], ["email", "ngModel"], ["password", "ngModel"], ["rePass", "ngModel"], ["title", ""], ["id", "title", 1, "title"], ["outline", "danger", "role", "alert", 4, "ngIf"], ["outline", "success", "role", "alert", 4, "ngIf"], ["aria-labelledby", "title", 3, "ngSubmit"], [1, "form-control-group"], ["for", "input-name", 1, "label"], ["nbInput", "", "id", "input-name", "name", "fullName", "placeholder", "Full name", "autofocus", "", "fullWidth", "", "fieldSize", "large", 3, "ngModelChange", "ngModel", "status", "required", "minlength", "maxlength"], [4, "ngIf"], ["for", "input-email", 1, "label"], ["nbInput", "", "id", "input-email", "name", "email", "pattern", ".+@.+..+", "placeholder", "Email address", "fullWidth", "", "fieldSize", "large", 3, "ngModelChange", "ngModel", "status", "required"], ["for", "input-password", 1, "label"], ["nbInput", "", "type", "password", "id", "input-password", "name", "password", "placeholder", "Password", "fullWidth", "", "fieldSize", "large", 3, "ngModelChange", "ngModel", "status", "required", "minlength", "maxlength"], ["for", "input-re-password", 1, "label"], ["nbInput", "", "type", "password", "id", "input-re-password", "name", "rePass", "placeholder", "Confirm Password", "fullWidth", "", "fieldSize", "large", 3, "ngModelChange", "ngModel", "status", "required"], ["class", "form-control-group accept-group", 4, "ngIf"], ["nbButton", "", "fullWidth", "", "status", "primary", "size", "large", 3, "disabled"], ["class", "links", "aria-label", "Social sign in", 4, "ngIf"], ["aria-label", "Sign in", 1, "another-action"], ["routerLink", "../login", 1, "text-link"], ["outline", "danger", "role", "alert"], [1, "alert-title"], [1, "alert-message-list"], ["class", "alert-message", 4, "ngFor", "ngForOf"], [1, "alert-message"], ["outline", "success", "role", "alert"], ["class", "caption status-danger", 4, "ngIf"], [1, "caption", "status-danger"], [1, "form-control-group", "accept-group"], ["name", "terms", 3, "ngModelChange", "ngModel", "required"], ["href", "#", "target", "_blank"], ["aria-label", "Social sign in", 1, "links"], [1, "socials"], [4, "ngFor", "ngForOf"], [3, "routerLink", "with-icon", 4, "ngIf"], [3, "with-icon", 4, "ngIf"], [3, "routerLink"], [3, "icon", 4, "ngIf", "ngIfElse"], [3, "icon"]],
  template: function NbRegisterComponent_Template(rf, ctx) {
    if (rf & 1) {
      const _r1 = ɵɵgetCurrentView();
      ɵɵelementStart(0, "h1", 6);
      ɵɵtext(1, "Register");
      ɵɵelementEnd();
      ɵɵtemplate(2, NbRegisterComponent_nb_alert_2_Template, 6, 1, "nb-alert", 7)(3, NbRegisterComponent_nb_alert_3_Template, 6, 1, "nb-alert", 8);
      ɵɵelementStart(4, "form", 9, 0);
      ɵɵlistener("ngSubmit", function NbRegisterComponent_Template_form_ngSubmit_4_listener() {
        ɵɵrestoreView(_r1);
        return ɵɵresetView(ctx.register());
      });
      ɵɵelementStart(6, "div", 10)(7, "label", 11);
      ɵɵtext(8, "Full name:");
      ɵɵelementEnd();
      ɵɵelementStart(9, "input", 12, 1);
      ɵɵtwoWayListener("ngModelChange", function NbRegisterComponent_Template_input_ngModelChange_9_listener($event) {
        ɵɵrestoreView(_r1);
        ɵɵtwoWayBindingSet(ctx.user.fullName, $event) || (ctx.user.fullName = $event);
        return ɵɵresetView($event);
      });
      ɵɵelementEnd();
      ɵɵtemplate(11, NbRegisterComponent_ng_container_11_Template, 3, 2, "ng-container", 13);
      ɵɵelementEnd();
      ɵɵelementStart(12, "div", 10)(13, "label", 14);
      ɵɵtext(14, "Email address:");
      ɵɵelementEnd();
      ɵɵelementStart(15, "input", 15, 2);
      ɵɵtwoWayListener("ngModelChange", function NbRegisterComponent_Template_input_ngModelChange_15_listener($event) {
        ɵɵrestoreView(_r1);
        ɵɵtwoWayBindingSet(ctx.user.email, $event) || (ctx.user.email = $event);
        return ɵɵresetView($event);
      });
      ɵɵelementEnd();
      ɵɵtemplate(17, NbRegisterComponent_ng_container_17_Template, 3, 2, "ng-container", 13);
      ɵɵelementEnd();
      ɵɵelementStart(18, "div", 10)(19, "label", 16);
      ɵɵtext(20, "Password:");
      ɵɵelementEnd();
      ɵɵelementStart(21, "input", 17, 3);
      ɵɵtwoWayListener("ngModelChange", function NbRegisterComponent_Template_input_ngModelChange_21_listener($event) {
        ɵɵrestoreView(_r1);
        ɵɵtwoWayBindingSet(ctx.user.password, $event) || (ctx.user.password = $event);
        return ɵɵresetView($event);
      });
      ɵɵelementEnd();
      ɵɵtemplate(23, NbRegisterComponent_ng_container_23_Template, 3, 2, "ng-container", 13);
      ɵɵelementEnd();
      ɵɵelementStart(24, "div", 10)(25, "label", 18);
      ɵɵtext(26, "Repeat password:");
      ɵɵelementEnd();
      ɵɵelementStart(27, "input", 19, 4);
      ɵɵtwoWayListener("ngModelChange", function NbRegisterComponent_Template_input_ngModelChange_27_listener($event) {
        ɵɵrestoreView(_r1);
        ɵɵtwoWayBindingSet(ctx.user.confirmPassword, $event) || (ctx.user.confirmPassword = $event);
        return ɵɵresetView($event);
      });
      ɵɵelementEnd();
      ɵɵtemplate(29, NbRegisterComponent_ng_container_29_Template, 3, 2, "ng-container", 13);
      ɵɵelementEnd();
      ɵɵtemplate(30, NbRegisterComponent_div_30_Template, 6, 2, "div", 20);
      ɵɵelementStart(31, "button", 21);
      ɵɵtext(32, " Register ");
      ɵɵelementEnd()();
      ɵɵtemplate(33, NbRegisterComponent_section_33_Template, 4, 1, "section", 22);
      ɵɵelementStart(34, "section", 23);
      ɵɵtext(35, " Already have an account? ");
      ɵɵelementStart(36, "a", 24);
      ɵɵtext(37, "Log in");
      ɵɵelementEnd()();
    }
    if (rf & 2) {
      const form_r13 = ɵɵreference(5);
      const fullName_r5 = ɵɵreference(10);
      const email_r6 = ɵɵreference(16);
      const password_r7 = ɵɵreference(22);
      const rePass_r8 = ɵɵreference(28);
      ɵɵadvance(2);
      ɵɵproperty("ngIf", ctx.showMessages.error && (ctx.errors == null ? null : ctx.errors.length) && !ctx.submitted);
      ɵɵadvance();
      ɵɵproperty("ngIf", ctx.showMessages.success && (ctx.messages == null ? null : ctx.messages.length) && !ctx.submitted);
      ɵɵadvance(6);
      ɵɵtwoWayProperty("ngModel", ctx.user.fullName);
      ɵɵproperty("status", fullName_r5.dirty ? fullName_r5.invalid ? "danger" : "success" : "basic")("required", ctx.getConfigValue("forms.validation.fullName.required"))("minlength", ctx.getConfigValue("forms.validation.fullName.minLength"))("maxlength", ctx.getConfigValue("forms.validation.fullName.maxLength"));
      ɵɵattribute("aria-invalid", fullName_r5.invalid && fullName_r5.touched ? true : null);
      ɵɵadvance(2);
      ɵɵproperty("ngIf", fullName_r5.invalid && fullName_r5.touched);
      ɵɵadvance(4);
      ɵɵtwoWayProperty("ngModel", ctx.user.email);
      ɵɵproperty("status", email_r6.dirty ? email_r6.invalid ? "danger" : "success" : "basic")("required", ctx.getConfigValue("forms.validation.email.required"));
      ɵɵattribute("aria-invalid", email_r6.invalid && email_r6.touched ? true : null);
      ɵɵadvance(2);
      ɵɵproperty("ngIf", email_r6.invalid && email_r6.touched);
      ɵɵadvance(4);
      ɵɵtwoWayProperty("ngModel", ctx.user.password);
      ɵɵproperty("status", password_r7.dirty ? password_r7.invalid ? "danger" : "success" : "basic")("required", ctx.getConfigValue("forms.validation.password.required"))("minlength", ctx.getConfigValue("forms.validation.password.minLength"))("maxlength", ctx.getConfigValue("forms.validation.password.maxLength"));
      ɵɵattribute("aria-invalid", password_r7.invalid && password_r7.touched ? true : null);
      ɵɵadvance(2);
      ɵɵproperty("ngIf", password_r7.invalid && password_r7.touched);
      ɵɵadvance(4);
      ɵɵtwoWayProperty("ngModel", ctx.user.confirmPassword);
      ɵɵproperty("status", rePass_r8.dirty ? rePass_r8.invalid || password_r7.value != rePass_r8.value ? "danger" : "success" : "basic")("required", ctx.getConfigValue("forms.validation.password.required"));
      ɵɵattribute("aria-invalid", rePass_r8.invalid && rePass_r8.touched ? true : null);
      ɵɵadvance(2);
      ɵɵproperty("ngIf", rePass_r8.invalid && rePass_r8.touched);
      ɵɵadvance();
      ɵɵproperty("ngIf", ctx.getConfigValue("forms.register.terms"));
      ɵɵadvance();
      ɵɵclassProp("btn-pulse", ctx.submitted);
      ɵɵproperty("disabled", ctx.submitted || !form_r13.valid);
      ɵɵadvance(2);
      ɵɵproperty("ngIf", ctx.socialLinks && ctx.socialLinks.length > 0);
    }
  },
  dependencies: [NgForOf, NgIf, NbCheckboxComponent, NbAlertComponent, NbInputDirective, NbButtonComponent, RouterLink, ɵNgNoValidate, DefaultValueAccessor, NgControlStatus, NgControlStatusGroup, RequiredValidator, MinLengthValidator, MaxLengthValidator, PatternValidator, NgModel, NgForm, NbIconComponent],
  styles: ["\n\n\n\n\n[_nghost-%COMP%]   .title[_ngcontent-%COMP%]{margin-bottom:2rem}"],
  changeDetection: 0
});
var NbRegisterComponent = _NbRegisterComponent;
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(NbRegisterComponent, [{
    type: Component,
    args: [{
      selector: "nb-register",
      changeDetection: ChangeDetectionStrategy.OnPush,
      template: `<h1 id="title" class="title">Register</h1>

<nb-alert *ngIf="showMessages.error && errors?.length && !submitted" outline="danger" role="alert">
  <p class="alert-title"><b>Oh snap!</b></p>
  <ul class="alert-message-list">
    <li *ngFor="let error of errors" class="alert-message">{{ error }}</li>
  </ul>
</nb-alert>

<nb-alert *ngIf="showMessages.success && messages?.length && !submitted" outline="success" role="alert">
  <p class="alert-title"><b>Hooray!</b></p>
  <ul class="alert-message-list">
    <li *ngFor="let message of messages" class="alert-message">{{ message }}</li>
  </ul>
</nb-alert>

<form (ngSubmit)="register()" #form="ngForm" aria-labelledby="title">

  <div class="form-control-group">
    <label class="label" for="input-name">Full name:</label>
    <input nbInput
           [(ngModel)]="user.fullName"
           #fullName="ngModel"
           id="input-name"
           name="fullName"
           placeholder="Full name"
           autofocus
           fullWidth
           fieldSize="large"
           [status]="fullName.dirty ? (fullName.invalid  ? 'danger' : 'success') : 'basic'"
           [required]="getConfigValue('forms.validation.fullName.required')"
           [minlength]="getConfigValue('forms.validation.fullName.minLength')"
           [maxlength]="getConfigValue('forms.validation.fullName.maxLength')"
           [attr.aria-invalid]="fullName.invalid && fullName.touched ? true : null">
    <ng-container *ngIf="fullName.invalid && fullName.touched">
      <p class="caption status-danger" *ngIf="fullName.errors?.required">
        Full name is required!
      </p>
      <p class="caption status-danger" *ngIf="fullName.errors?.minlength || fullName.errors?.maxlength">
        Full name should contains
        from {{getConfigValue('forms.validation.fullName.minLength')}}
        to {{getConfigValue('forms.validation.fullName.maxLength')}}
        characters
      </p>
    </ng-container>
  </div>

  <div class="form-control-group">
    <label class="label" for="input-email">Email address:</label>
    <input nbInput
           [(ngModel)]="user.email"
           #email="ngModel"
           id="input-email"
           name="email"
           pattern=".+@.+..+"
           placeholder="Email address"
           fullWidth
           fieldSize="large"
           [status]="email.dirty ? (email.invalid  ? 'danger' : 'success') : 'basic'"
           [required]="getConfigValue('forms.validation.email.required')"
           [attr.aria-invalid]="email.invalid && email.touched ? true : null">
    <ng-container *ngIf="email.invalid && email.touched">
      <p class="caption status-danger" *ngIf="email.errors?.required">
        Email is required!
      </p>
      <p class="caption status-danger" *ngIf="email.errors?.pattern">
        Email should be the real one!
      </p>
    </ng-container>
  </div>

  <div class="form-control-group">
    <label class="label" for="input-password">Password:</label>
    <input nbInput
           [(ngModel)]="user.password"
           #password="ngModel"
           type="password"
           id="input-password"
           name="password"
           placeholder="Password"
           fullWidth
           fieldSize="large"
           [status]="password.dirty ? (password.invalid  ? 'danger' : 'success') : 'basic'"
           [required]="getConfigValue('forms.validation.password.required')"
           [minlength]="getConfigValue('forms.validation.password.minLength')"
           [maxlength]="getConfigValue('forms.validation.password.maxLength')"
           [attr.aria-invalid]="password.invalid && password.touched ? true : null">
    <ng-container *ngIf="password.invalid && password.touched">
      <p class="caption status-danger" *ngIf="password.errors?.required">
        Password is required!
      </p>
      <p class="caption status-danger" *ngIf="password.errors?.minlength || password.errors?.maxlength">
        Password should contain
        from {{ getConfigValue('forms.validation.password.minLength') }}
        to {{ getConfigValue('forms.validation.password.maxLength') }}
        characters
      </p>
    </ng-container>
  </div>

  <div class="form-control-group">
    <label class="label" for="input-re-password">Repeat password:</label>
    <input nbInput
           [(ngModel)]="user.confirmPassword"
           #rePass="ngModel"
           type="password"
           id="input-re-password"
           name="rePass"
           placeholder="Confirm Password"
           fullWidth
           fieldSize="large"
           [status]="rePass.dirty ? (rePass.invalid || password.value != rePass.value  ? 'danger' : 'success') : 'basic'"
           [required]="getConfigValue('forms.validation.password.required')"
           [attr.aria-invalid]="rePass.invalid && rePass.touched ? true : null">
    <ng-container *ngIf="rePass.invalid && rePass.touched">
      <p class="caption status-danger" *ngIf="rePass.errors?.required">
        Password confirmation is required!
      </p>
      <p class="caption status-danger" *ngIf="password.value != rePass.value && !rePass.errors?.required">
        Password does not match the confirm password.
      </p>
    </ng-container>
  </div>

  <div class="form-control-group accept-group" *ngIf="getConfigValue('forms.register.terms')">
    <nb-checkbox name="terms" [(ngModel)]="user.terms" [required]="getConfigValue('forms.register.terms')">
      Agree to <a href="#" target="_blank"><strong>Terms & Conditions</strong></a>
    </nb-checkbox>
  </div>

  <button nbButton
          fullWidth
          status="primary"
          size="large"
          [disabled]="submitted || !form.valid"
          [class.btn-pulse]="submitted">
    Register
  </button>
</form>

<section *ngIf="socialLinks && socialLinks.length > 0" class="links" aria-label="Social sign in">
  or enter with:
  <div class="socials">
    <ng-container *ngFor="let socialLink of socialLinks">
      <a *ngIf="socialLink.link"
         [routerLink]="socialLink.link"
         [attr.target]="socialLink.target"
         [attr.class]="socialLink.icon"
         [class.with-icon]="socialLink.icon">
        <nb-icon *ngIf="socialLink.icon; else title" [icon]="socialLink.icon"></nb-icon>
        <ng-template #title>{{ socialLink.title }}</ng-template>
      </a>
      <a *ngIf="socialLink.url"
         [attr.href]="socialLink.url"
         [attr.target]="socialLink.target"
         [attr.class]="socialLink.icon"
         [class.with-icon]="socialLink.icon">
        <nb-icon *ngIf="socialLink.icon; else title" [icon]="socialLink.icon"></nb-icon>
        <ng-template #title>{{ socialLink.title }}</ng-template>
      </a>
    </ng-container>
  </div>
</section>

<section class="another-action" aria-label="Sign in">
  Already have an account? <a class="text-link" routerLink="../login">Log in</a>
</section>
`,
      styles: ["/**\n * @license\n * Copyright Akveo. All Rights Reserved.\n * Licensed under the MIT License. See License.txt in the project root for license information.\n */:host .title{margin-bottom:2rem}\n"]
    }]
  }], () => [{
    type: NbAuthService
  }, {
    type: void 0,
    decorators: [{
      type: Inject,
      args: [NB_AUTH_OPTIONS]
    }]
  }, {
    type: ChangeDetectorRef
  }, {
    type: Router
  }], null);
})();
var _NbLogoutComponent = class _NbLogoutComponent {
  constructor(service, options = {}, router) {
    this.service = service;
    this.options = options;
    this.router = router;
    this.redirectDelay = 0;
    this.strategy = "";
    this.redirectDelay = this.getConfigValue("forms.logout.redirectDelay");
    this.strategy = this.getConfigValue("forms.logout.strategy");
  }
  ngOnInit() {
    this.logout(this.strategy);
  }
  logout(strategy) {
    this.service.logout(strategy).subscribe((result) => {
      const redirect = result.getRedirect();
      if (redirect) {
        setTimeout(() => {
          return this.router.navigateByUrl(redirect);
        }, this.redirectDelay);
      }
    });
  }
  getConfigValue(key) {
    return getDeepFromObject(this.options, key, null);
  }
};
_NbLogoutComponent.ɵfac = function NbLogoutComponent_Factory(t) {
  return new (t || _NbLogoutComponent)(ɵɵdirectiveInject(NbAuthService), ɵɵdirectiveInject(NB_AUTH_OPTIONS), ɵɵdirectiveInject(Router));
};
_NbLogoutComponent.ɵcmp = ɵɵdefineComponent({
  type: _NbLogoutComponent,
  selectors: [["nb-logout"]],
  decls: 2,
  vars: 0,
  template: function NbLogoutComponent_Template(rf, ctx) {
    if (rf & 1) {
      ɵɵelementStart(0, "div");
      ɵɵtext(1, "Logging out, please wait...");
      ɵɵelementEnd();
    }
  },
  encapsulation: 2
});
var NbLogoutComponent = _NbLogoutComponent;
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(NbLogoutComponent, [{
    type: Component,
    args: [{
      selector: "nb-logout",
      template: "<div>Logging out, please wait...</div>\n"
    }]
  }], () => [{
    type: NbAuthService
  }, {
    type: void 0,
    decorators: [{
      type: Inject,
      args: [NB_AUTH_OPTIONS]
    }]
  }, {
    type: Router
  }], null);
})();
var _NbRequestPasswordComponent = class _NbRequestPasswordComponent {
  constructor(service, options = {}, cd, router) {
    this.service = service;
    this.options = options;
    this.cd = cd;
    this.router = router;
    this.redirectDelay = 0;
    this.showMessages = {};
    this.strategy = "";
    this.submitted = false;
    this.errors = [];
    this.messages = [];
    this.user = {};
    this.redirectDelay = this.getConfigValue("forms.requestPassword.redirectDelay");
    this.showMessages = this.getConfigValue("forms.requestPassword.showMessages");
    this.strategy = this.getConfigValue("forms.requestPassword.strategy");
  }
  requestPass() {
    this.errors = this.messages = [];
    this.submitted = true;
    this.service.requestPassword(this.strategy, this.user).subscribe((result) => {
      this.submitted = false;
      if (result.isSuccess()) {
        this.messages = result.getMessages();
      } else {
        this.errors = result.getErrors();
      }
      const redirect = result.getRedirect();
      if (redirect) {
        setTimeout(() => {
          return this.router.navigateByUrl(redirect);
        }, this.redirectDelay);
      }
      this.cd.detectChanges();
    });
  }
  getConfigValue(key) {
    return getDeepFromObject(this.options, key, null);
  }
};
_NbRequestPasswordComponent.ɵfac = function NbRequestPasswordComponent_Factory(t) {
  return new (t || _NbRequestPasswordComponent)(ɵɵdirectiveInject(NbAuthService), ɵɵdirectiveInject(NB_AUTH_OPTIONS), ɵɵdirectiveInject(ChangeDetectorRef), ɵɵdirectiveInject(Router));
};
_NbRequestPasswordComponent.ɵcmp = ɵɵdefineComponent({
  type: _NbRequestPasswordComponent,
  selectors: [["nb-request-password-page"]],
  decls: 23,
  vars: 10,
  consts: [["requestPassForm", "ngForm"], ["email", "ngModel"], ["id", "title", 1, "title"], [1, "sub-title"], ["outline", "danger", "role", "alert", 4, "ngIf"], ["outline", "success", "role", "alert", 4, "ngIf"], ["aria-labelledby", "title", 3, "ngSubmit"], [1, "form-control-group"], ["for", "input-email", 1, "label"], ["nbInput", "", "id", "input-email", "name", "email", "pattern", ".+@.+\\..+", "placeholder", "Email address", "autofocus", "", "fullWidth", "", "fieldSize", "large", 3, "ngModelChange", "ngModel", "status", "required"], [4, "ngIf"], ["nbButton", "", "fullWidth", "", "status", "primary", "size", "large", 3, "disabled"], ["aria-label", "Sign in or sign up", 1, "sign-in-or-up"], ["routerLink", "../login", 1, "text-link"], ["routerLink", "../register", 1, "text-link"], ["outline", "danger", "role", "alert"], [1, "alert-title"], [1, "alert-message-list"], ["class", "alert-message", 4, "ngFor", "ngForOf"], [1, "alert-message"], ["outline", "success", "role", "alert"], ["class", "caption status-danger", 4, "ngIf"], [1, "caption", "status-danger"]],
  template: function NbRequestPasswordComponent_Template(rf, ctx) {
    if (rf & 1) {
      const _r1 = ɵɵgetCurrentView();
      ɵɵelementStart(0, "h1", 2);
      ɵɵtext(1, "Forgot Password");
      ɵɵelementEnd();
      ɵɵelementStart(2, "p", 3);
      ɵɵtext(3, "Enter your email address and we’ll send a link to reset your password");
      ɵɵelementEnd();
      ɵɵtemplate(4, NbRequestPasswordComponent_nb_alert_4_Template, 6, 1, "nb-alert", 4)(5, NbRequestPasswordComponent_nb_alert_5_Template, 6, 1, "nb-alert", 5);
      ɵɵelementStart(6, "form", 6, 0);
      ɵɵlistener("ngSubmit", function NbRequestPasswordComponent_Template_form_ngSubmit_6_listener() {
        ɵɵrestoreView(_r1);
        return ɵɵresetView(ctx.requestPass());
      });
      ɵɵelementStart(8, "div", 7)(9, "label", 8);
      ɵɵtext(10, "Enter your email address:");
      ɵɵelementEnd();
      ɵɵelementStart(11, "input", 9, 1);
      ɵɵtwoWayListener("ngModelChange", function NbRequestPasswordComponent_Template_input_ngModelChange_11_listener($event) {
        ɵɵrestoreView(_r1);
        ɵɵtwoWayBindingSet(ctx.user.email, $event) || (ctx.user.email = $event);
        return ɵɵresetView($event);
      });
      ɵɵelementEnd();
      ɵɵtemplate(13, NbRequestPasswordComponent_ng_container_13_Template, 3, 2, "ng-container", 10);
      ɵɵelementEnd();
      ɵɵelementStart(14, "button", 11);
      ɵɵtext(15, " Request password ");
      ɵɵelementEnd()();
      ɵɵelementStart(16, "section", 12)(17, "p")(18, "a", 13);
      ɵɵtext(19, "Back to Log In");
      ɵɵelementEnd()();
      ɵɵelementStart(20, "p")(21, "a", 14);
      ɵɵtext(22, "Register");
      ɵɵelementEnd()()();
    }
    if (rf & 2) {
      const requestPassForm_r6 = ɵɵreference(7);
      const email_r5 = ɵɵreference(12);
      ɵɵadvance(4);
      ɵɵproperty("ngIf", ctx.showMessages.error && (ctx.errors == null ? null : ctx.errors.length) && !ctx.submitted);
      ɵɵadvance();
      ɵɵproperty("ngIf", ctx.showMessages.success && (ctx.messages == null ? null : ctx.messages.length) && !ctx.submitted);
      ɵɵadvance(6);
      ɵɵtwoWayProperty("ngModel", ctx.user.email);
      ɵɵproperty("status", email_r5.dirty ? email_r5.invalid ? "danger" : "success" : "basic")("required", ctx.getConfigValue("forms.validation.email.required"));
      ɵɵattribute("aria-invalid", email_r5.invalid && email_r5.touched ? true : null);
      ɵɵadvance(2);
      ɵɵproperty("ngIf", email_r5.invalid && email_r5.touched);
      ɵɵadvance();
      ɵɵclassProp("btn-pulse", ctx.submitted);
      ɵɵproperty("disabled", ctx.submitted || !requestPassForm_r6.valid);
    }
  },
  dependencies: [NgForOf, NgIf, NbAlertComponent, NbInputDirective, NbButtonComponent, RouterLink, ɵNgNoValidate, DefaultValueAccessor, NgControlStatus, NgControlStatusGroup, RequiredValidator, PatternValidator, NgModel, NgForm],
  styles: ["\n\n\n\n\n[_nghost-%COMP%]   .form-group[_ngcontent-%COMP%]:last-of-type{margin-bottom:3rem}"],
  changeDetection: 0
});
var NbRequestPasswordComponent = _NbRequestPasswordComponent;
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(NbRequestPasswordComponent, [{
    type: Component,
    args: [{
      selector: "nb-request-password-page",
      changeDetection: ChangeDetectionStrategy.OnPush,
      template: `<h1 id="title" class="title">Forgot Password</h1>
<p class="sub-title">Enter your email address and we’ll send a link to reset your password</p>

<nb-alert *ngIf="showMessages.error && errors?.length && !submitted" outline="danger" role="alert">
  <p class="alert-title"><b>Oh snap!</b></p>
  <ul class="alert-message-list">
    <li *ngFor="let error of errors" class="alert-message">{{ error }}</li>
  </ul>
</nb-alert>

<nb-alert *ngIf="showMessages.success && messages?.length && !submitted" outline="success" role="alert">
  <p class="alert-title"><b>Hooray!</b></p>
  <ul class="alert-message-list">
    <li *ngFor="let message of messages" class="alert-message">{{ message }}</li>
  </ul>
</nb-alert>

<form (ngSubmit)="requestPass()" #requestPassForm="ngForm" aria-labelledby="title">

  <div class="form-control-group">
    <label class="label" for="input-email">Enter your email address:</label>
    <input nbInput
           [(ngModel)]="user.email"
           #email="ngModel"
           id="input-email"
           name="email"
           pattern=".+@.+\\..+"
           placeholder="Email address"
           autofocus
           fullWidth
           fieldSize="large"
           [status]="email.dirty ? (email.invalid  ? 'danger' : 'success') : 'basic'"
           [required]="getConfigValue('forms.validation.email.required')"
           [attr.aria-invalid]="email.invalid && email.touched ? true : null">
    <ng-container *ngIf="email.invalid && email.touched">
      <p class="caption status-danger" *ngIf="email.errors?.required">
        Email is required!
      </p>
      <p class="caption status-danger" *ngIf="email.errors?.pattern">
        Email should be the real one!
      </p>
    </ng-container>
  </div>

  <button nbButton
          fullWidth
          status="primary"
          size="large"
          [disabled]="submitted || !requestPassForm.valid"
          [class.btn-pulse]="submitted">
    Request password
  </button>
</form>

<section class="sign-in-or-up" aria-label="Sign in or sign up">
  <p><a class="text-link" routerLink="../login">Back to Log In</a></p>
  <p><a routerLink="../register" class="text-link">Register</a></p>
</section>
`,
      styles: ["/**\n * @license\n * Copyright Akveo. All Rights Reserved.\n * Licensed under the MIT License. See License.txt in the project root for license information.\n */:host .form-group:last-of-type{margin-bottom:3rem}\n"]
    }]
  }], () => [{
    type: NbAuthService
  }, {
    type: void 0,
    decorators: [{
      type: Inject,
      args: [NB_AUTH_OPTIONS]
    }]
  }, {
    type: ChangeDetectorRef
  }, {
    type: Router
  }], null);
})();
var _NbResetPasswordComponent = class _NbResetPasswordComponent {
  constructor(service, options = {}, cd, router) {
    this.service = service;
    this.options = options;
    this.cd = cd;
    this.router = router;
    this.redirectDelay = 0;
    this.showMessages = {};
    this.strategy = "";
    this.submitted = false;
    this.errors = [];
    this.messages = [];
    this.user = {};
    this.redirectDelay = this.getConfigValue("forms.resetPassword.redirectDelay");
    this.showMessages = this.getConfigValue("forms.resetPassword.showMessages");
    this.strategy = this.getConfigValue("forms.resetPassword.strategy");
  }
  resetPass() {
    this.errors = this.messages = [];
    this.submitted = true;
    this.service.resetPassword(this.strategy, this.user).subscribe((result) => {
      this.submitted = false;
      if (result.isSuccess()) {
        this.messages = result.getMessages();
      } else {
        this.errors = result.getErrors();
      }
      const redirect = result.getRedirect();
      if (redirect) {
        setTimeout(() => {
          return this.router.navigateByUrl(redirect);
        }, this.redirectDelay);
      }
      this.cd.detectChanges();
    });
  }
  getConfigValue(key) {
    return getDeepFromObject(this.options, key, null);
  }
};
_NbResetPasswordComponent.ɵfac = function NbResetPasswordComponent_Factory(t) {
  return new (t || _NbResetPasswordComponent)(ɵɵdirectiveInject(NbAuthService), ɵɵdirectiveInject(NB_AUTH_OPTIONS), ɵɵdirectiveInject(ChangeDetectorRef), ɵɵdirectiveInject(Router));
};
_NbResetPasswordComponent.ɵcmp = ɵɵdefineComponent({
  type: _NbResetPasswordComponent,
  selectors: [["nb-reset-password-page"]],
  decls: 29,
  vars: 17,
  consts: [["resetPassForm", "ngForm"], ["password", "ngModel"], ["rePass", "ngModel"], ["id", "title", 1, "title"], [1, "sub-title"], ["outline", "danger", "role", "alert", 4, "ngIf"], ["outline", "success", "role", "alert", 4, "ngIf"], ["aria-labelledby", "title", 3, "ngSubmit"], [1, "form-control-group"], ["for", "input-password", 1, "label"], ["nbInput", "", "type", "password", "id", "input-password", "name", "password", "placeholder", "New Password", "autofocus", "", "fullWidth", "", "fieldSize", "large", 1, "first", 3, "ngModelChange", "ngModel", "status", "required", "minlength", "maxlength"], [4, "ngIf"], [1, "form-group"], ["for", "input-re-password", 1, "label"], ["nbInput", "", "id", "input-re-password", "name", "rePass", "type", "password", "placeholder", "Confirm Password", "fullWidth", "", "fieldSize", "large", 1, "last", 3, "ngModelChange", "ngModel", "status", "required"], ["nbButton", "", "status", "primary", "fullWidth", "", "size", "large", 3, "disabled"], ["aria-label", "Sign in or sign up", 1, "sign-in-or-up"], ["routerLink", "../login", 1, "text-link"], ["routerLink", "../register", 1, "text-link"], ["outline", "danger", "role", "alert"], [1, "alert-title"], [1, "alert-message-list"], ["class", "alert-message", 4, "ngFor", "ngForOf"], [1, "alert-message"], ["outline", "success", "role", "alert"], ["class", "caption status-danger", 4, "ngIf"], [1, "caption", "status-danger"]],
  template: function NbResetPasswordComponent_Template(rf, ctx) {
    if (rf & 1) {
      const _r1 = ɵɵgetCurrentView();
      ɵɵelementStart(0, "h1", 3);
      ɵɵtext(1, "Change password");
      ɵɵelementEnd();
      ɵɵelementStart(2, "p", 4);
      ɵɵtext(3, "Please set a new password");
      ɵɵelementEnd();
      ɵɵtemplate(4, NbResetPasswordComponent_nb_alert_4_Template, 6, 1, "nb-alert", 5)(5, NbResetPasswordComponent_nb_alert_5_Template, 6, 1, "nb-alert", 6);
      ɵɵelementStart(6, "form", 7, 0);
      ɵɵlistener("ngSubmit", function NbResetPasswordComponent_Template_form_ngSubmit_6_listener() {
        ɵɵrestoreView(_r1);
        return ɵɵresetView(ctx.resetPass());
      });
      ɵɵelementStart(8, "div", 8)(9, "label", 9);
      ɵɵtext(10, "New Password:");
      ɵɵelementEnd();
      ɵɵelementStart(11, "input", 10, 1);
      ɵɵtwoWayListener("ngModelChange", function NbResetPasswordComponent_Template_input_ngModelChange_11_listener($event) {
        ɵɵrestoreView(_r1);
        ɵɵtwoWayBindingSet(ctx.user.password, $event) || (ctx.user.password = $event);
        return ɵɵresetView($event);
      });
      ɵɵelementEnd();
      ɵɵtemplate(13, NbResetPasswordComponent_ng_container_13_Template, 3, 2, "ng-container", 11);
      ɵɵelementEnd();
      ɵɵelementStart(14, "div", 12)(15, "label", 13);
      ɵɵtext(16, "Confirm Password:");
      ɵɵelementEnd();
      ɵɵelementStart(17, "input", 14, 2);
      ɵɵtwoWayListener("ngModelChange", function NbResetPasswordComponent_Template_input_ngModelChange_17_listener($event) {
        ɵɵrestoreView(_r1);
        ɵɵtwoWayBindingSet(ctx.user.confirmPassword, $event) || (ctx.user.confirmPassword = $event);
        return ɵɵresetView($event);
      });
      ɵɵelementEnd();
      ɵɵtemplate(19, NbResetPasswordComponent_ng_container_19_Template, 3, 2, "ng-container", 11);
      ɵɵelementEnd();
      ɵɵelementStart(20, "button", 15);
      ɵɵtext(21, " Change password ");
      ɵɵelementEnd()();
      ɵɵelementStart(22, "section", 16)(23, "p")(24, "a", 17);
      ɵɵtext(25, "Back to Log In");
      ɵɵelementEnd()();
      ɵɵelementStart(26, "p")(27, "a", 18);
      ɵɵtext(28, "Register");
      ɵɵelementEnd()()();
    }
    if (rf & 2) {
      const resetPassForm_r7 = ɵɵreference(7);
      const password_r5 = ɵɵreference(12);
      const rePass_r6 = ɵɵreference(18);
      ɵɵadvance(4);
      ɵɵproperty("ngIf", ctx.showMessages.error && (ctx.errors == null ? null : ctx.errors.length) && !ctx.submitted);
      ɵɵadvance();
      ɵɵproperty("ngIf", ctx.showMessages.success && (ctx.messages == null ? null : ctx.messages.length) && !ctx.submitted);
      ɵɵadvance(6);
      ɵɵtwoWayProperty("ngModel", ctx.user.password);
      ɵɵproperty("status", password_r5.dirty ? password_r5.invalid ? "danger" : "success" : "basic")("required", ctx.getConfigValue("forms.validation.password.required"))("minlength", ctx.getConfigValue("forms.validation.password.minLength"))("maxlength", ctx.getConfigValue("forms.validation.password.maxLength"));
      ɵɵattribute("aria-invalid", password_r5.invalid && password_r5.touched ? true : null);
      ɵɵadvance(2);
      ɵɵproperty("ngIf", password_r5.invalid && password_r5.touched);
      ɵɵadvance(4);
      ɵɵtwoWayProperty("ngModel", ctx.user.confirmPassword);
      ɵɵproperty("status", rePass_r6.touched ? rePass_r6.invalid || password_r5.value != rePass_r6.value ? "danger" : "success" : "basic")("required", ctx.getConfigValue("forms.validation.password.required"));
      ɵɵattribute("aria-invalid", rePass_r6.invalid && rePass_r6.touched ? true : null);
      ɵɵadvance(2);
      ɵɵproperty("ngIf", rePass_r6.touched);
      ɵɵadvance();
      ɵɵclassProp("btn-pulse", ctx.submitted);
      ɵɵproperty("disabled", ctx.submitted || !resetPassForm_r7.valid);
    }
  },
  dependencies: [NgForOf, NgIf, NbAlertComponent, NbInputDirective, NbButtonComponent, RouterLink, ɵNgNoValidate, DefaultValueAccessor, NgControlStatus, NgControlStatusGroup, RequiredValidator, MinLengthValidator, MaxLengthValidator, NgModel, NgForm],
  styles: [_c1],
  changeDetection: 0
});
var NbResetPasswordComponent = _NbResetPasswordComponent;
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(NbResetPasswordComponent, [{
    type: Component,
    args: [{
      selector: "nb-reset-password-page",
      changeDetection: ChangeDetectionStrategy.OnPush,
      template: `<h1 id="title" class="title">Change password</h1>
<p class="sub-title">Please set a new password</p>

<nb-alert *ngIf="showMessages.error && errors?.length && !submitted" outline="danger" role="alert">
  <p class="alert-title"><b>Oh snap!</b></p>
  <ul class="alert-message-list">
    <li *ngFor="let error of errors" class="alert-message">{{ error }}</li>
  </ul>
</nb-alert>

<nb-alert *ngIf="showMessages.success && messages?.length && !submitted" outline="success" role="alert">
  <p class="alert-title"><b>Hooray!</b></p>
  <ul class="alert-message-list">
    <li *ngFor="let message of messages" class="alert-message">{{ message }}</li>
  </ul>
</nb-alert>

<form (ngSubmit)="resetPass()" #resetPassForm="ngForm" aria-labelledby="title">

  <div class="form-control-group">
    <label class="label" for="input-password">New Password:</label>
    <input nbInput
           [(ngModel)]="user.password"
           #password="ngModel"
           type="password"
           id="input-password"
           name="password"
           class="first"
           placeholder="New Password"
           autofocus
           fullWidth
           fieldSize="large"
           [status]="password.dirty ? (password.invalid  ? 'danger' : 'success') : 'basic'"
           [required]="getConfigValue('forms.validation.password.required')"
           [minlength]="getConfigValue('forms.validation.password.minLength')"
           [maxlength]="getConfigValue('forms.validation.password.maxLength')"
           [attr.aria-invalid]="password.invalid && password.touched ? true : null">
    <ng-container *ngIf="password.invalid && password.touched">
      <p class="caption status-danger" *ngIf="password.errors?.required">
        Password is required!
      </p>
      <p class="caption status-danger" *ngIf="password.errors?.minlength || password.errors?.maxlength">
        Password should contains
        from {{getConfigValue('forms.validation.password.minLength')}}
        to {{getConfigValue('forms.validation.password.maxLength')}}
        characters
      </p>
    </ng-container>
  </div>

  <div class="form-group">
    <label class="label" for="input-re-password">Confirm Password:</label>
    <input nbInput
           [(ngModel)]="user.confirmPassword"
           #rePass="ngModel"
           id="input-re-password"
           name="rePass"
           type="password"
           class="last"
           placeholder="Confirm Password"
           fullWidth
           fieldSize="large"
           [status]="rePass.touched
               ? (rePass.invalid || password.value != rePass.value ? 'danger' : 'success')
               : 'basic'"
           [required]="getConfigValue('forms.validation.password.required')"
           [attr.aria-invalid]="rePass.invalid && rePass.touched ? true : null">
    <ng-container *ngIf="rePass.touched">
      <p class="caption status-danger" *ngIf="rePass.invalid && rePass.errors?.required">
        Password confirmation is required!
      </p>
      <p class="caption status-danger" *ngIf="password.value != rePass.value && !rePass.errors?.required">
        Password does not match the confirm password.
      </p>
    </ng-container>
  </div>

  <button nbButton
          status="primary"
          fullWidth
          size="large"
          [disabled]="submitted || !resetPassForm.valid"
          [class.btn-pulse]="submitted">
    Change password
  </button>
</form>

<section class="sign-in-or-up" aria-label="Sign in or sign up">
  <p><a class="text-link" routerLink="../login">Back to Log In</a></p>
  <p><a class="text-link" routerLink="../register">Register</a></p>
</section>
`,
      styles: ["/**\n * @license\n * Copyright Akveo. All Rights Reserved.\n * Licensed under the MIT License. See License.txt in the project root for license information.\n */:host .form-group:last-of-type{margin-bottom:3rem}\n"]
    }]
  }], () => [{
    type: NbAuthService
  }, {
    type: void 0,
    decorators: [{
      type: Inject,
      args: [NB_AUTH_OPTIONS]
    }]
  }, {
    type: ChangeDetectorRef
  }, {
    type: Router
  }], null);
})();
function nbStrategiesFactory(options, injector) {
  const strategies = [];
  options.strategies.forEach(([strategyClass, strategyOptions]) => {
    const strategy = injector.get(strategyClass);
    strategy.setOptions(strategyOptions);
    strategies.push(strategy);
  });
  return strategies;
}
function nbTokensFactory(strategies) {
  const tokens = [];
  strategies.forEach((strategy) => {
    tokens.push(strategy.getOption("token.class"));
  });
  return tokens;
}
function nbOptionsFactory(options) {
  return deepExtend(defaultAuthOptions, options);
}
function nbNoOpInterceptorFilter(req) {
  return true;
}
var _NbAuthModule = class _NbAuthModule {
  static forRoot(nbAuthOptions) {
    return {
      ngModule: _NbAuthModule,
      providers: [{
        provide: NB_AUTH_USER_OPTIONS,
        useValue: nbAuthOptions
      }, {
        provide: NB_AUTH_OPTIONS,
        useFactory: nbOptionsFactory,
        deps: [NB_AUTH_USER_OPTIONS]
      }, {
        provide: NB_AUTH_STRATEGIES,
        useFactory: nbStrategiesFactory,
        deps: [NB_AUTH_OPTIONS, Injector]
      }, {
        provide: NB_AUTH_TOKENS,
        useFactory: nbTokensFactory,
        deps: [NB_AUTH_STRATEGIES]
      }, {
        provide: NB_AUTH_FALLBACK_TOKEN,
        useValue: NbAuthSimpleToken
      }, {
        provide: NB_AUTH_INTERCEPTOR_HEADER,
        useValue: "Authorization"
      }, {
        provide: NB_AUTH_TOKEN_INTERCEPTOR_FILTER,
        useValue: nbNoOpInterceptorFilter
      }, {
        provide: NbTokenStorage,
        useClass: NbTokenLocalStorage
      }, NbAuthTokenParceler, NbAuthService, NbTokenService, NbDummyAuthStrategy, NbPasswordAuthStrategy, NbOAuth2AuthStrategy]
    };
  }
};
_NbAuthModule.ɵfac = function NbAuthModule_Factory(t) {
  return new (t || _NbAuthModule)();
};
_NbAuthModule.ɵmod = ɵɵdefineNgModule({
  type: _NbAuthModule,
  declarations: [NbAuthComponent, NbAuthBlockComponent, NbLoginComponent, NbRegisterComponent, NbRequestPasswordComponent, NbResetPasswordComponent, NbLogoutComponent],
  imports: [CommonModule, NbLayoutModule, NbCardModule, NbCheckboxModule, NbAlertModule, NbInputModule, NbButtonModule, RouterModule, FormsModule, NbIconModule],
  exports: [NbAuthComponent, NbAuthBlockComponent, NbLoginComponent, NbRegisterComponent, NbRequestPasswordComponent, NbResetPasswordComponent, NbLogoutComponent]
});
_NbAuthModule.ɵinj = ɵɵdefineInjector({
  imports: [CommonModule, NbLayoutModule, NbCardModule, NbCheckboxModule, NbAlertModule, NbInputModule, NbButtonModule, RouterModule, FormsModule, NbIconModule]
});
var NbAuthModule = _NbAuthModule;
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(NbAuthModule, [{
    type: NgModule,
    args: [{
      imports: [CommonModule, NbLayoutModule, NbCardModule, NbCheckboxModule, NbAlertModule, NbInputModule, NbButtonModule, RouterModule, FormsModule, NbIconModule],
      declarations: [NbAuthComponent, NbAuthBlockComponent, NbLoginComponent, NbRegisterComponent, NbRequestPasswordComponent, NbResetPasswordComponent, NbLogoutComponent],
      exports: [NbAuthComponent, NbAuthBlockComponent, NbLoginComponent, NbRegisterComponent, NbRequestPasswordComponent, NbResetPasswordComponent, NbLogoutComponent]
    }]
  }], null, null);
})();
var routes = [{
  path: "auth",
  component: NbAuthComponent,
  children: [{
    path: "",
    component: NbLoginComponent
  }, {
    path: "login",
    component: NbLoginComponent
  }, {
    path: "register",
    component: NbRegisterComponent
  }, {
    path: "logout",
    component: NbLogoutComponent
  }, {
    path: "request-password",
    component: NbRequestPasswordComponent
  }, {
    path: "reset-password",
    component: NbResetPasswordComponent
  }]
}];
var _NbAuthJWTInterceptor = class _NbAuthJWTInterceptor {
  constructor(injector, filter2) {
    this.injector = injector;
    this.filter = filter2;
  }
  intercept(req, next) {
    if (!this.filter(req)) {
      return this.authService.isAuthenticatedOrRefresh().pipe(switchMap((authenticated) => {
        if (authenticated) {
          return this.authService.getToken().pipe(switchMap((token) => {
            const JWT = `Bearer ${token.getValue()}`;
            req = req.clone({
              setHeaders: {
                Authorization: JWT
              }
            });
            return next.handle(req);
          }));
        } else {
          return next.handle(req);
        }
      }));
    } else {
      return next.handle(req);
    }
  }
  get authService() {
    return this.injector.get(NbAuthService);
  }
};
_NbAuthJWTInterceptor.ɵfac = function NbAuthJWTInterceptor_Factory(t) {
  return new (t || _NbAuthJWTInterceptor)(ɵɵinject(Injector), ɵɵinject(NB_AUTH_TOKEN_INTERCEPTOR_FILTER));
};
_NbAuthJWTInterceptor.ɵprov = ɵɵdefineInjectable({
  token: _NbAuthJWTInterceptor,
  factory: _NbAuthJWTInterceptor.ɵfac
});
var NbAuthJWTInterceptor = _NbAuthJWTInterceptor;
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(NbAuthJWTInterceptor, [{
    type: Injectable
  }], () => [{
    type: Injector
  }, {
    type: void 0,
    decorators: [{
      type: Inject,
      args: [NB_AUTH_TOKEN_INTERCEPTOR_FILTER]
    }]
  }], null);
})();
var _NbAuthSimpleInterceptor = class _NbAuthSimpleInterceptor {
  constructor(injector, headerName = "Authorization") {
    this.injector = injector;
    this.headerName = headerName;
  }
  intercept(req, next) {
    return this.authService.getToken().pipe(switchMap((token) => {
      if (token && token.getValue()) {
        req = req.clone({
          setHeaders: {
            [this.headerName]: token.getValue()
          }
        });
      }
      return next.handle(req);
    }));
  }
  get authService() {
    return this.injector.get(NbAuthService);
  }
};
_NbAuthSimpleInterceptor.ɵfac = function NbAuthSimpleInterceptor_Factory(t) {
  return new (t || _NbAuthSimpleInterceptor)(ɵɵinject(Injector), ɵɵinject(NB_AUTH_INTERCEPTOR_HEADER));
};
_NbAuthSimpleInterceptor.ɵprov = ɵɵdefineInjectable({
  token: _NbAuthSimpleInterceptor,
  factory: _NbAuthSimpleInterceptor.ɵfac
});
var NbAuthSimpleInterceptor = _NbAuthSimpleInterceptor;
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(NbAuthSimpleInterceptor, [{
    type: Injectable
  }], () => [{
    type: Injector
  }, {
    type: void 0,
    decorators: [{
      type: Inject,
      args: [NB_AUTH_INTERCEPTOR_HEADER]
    }]
  }], null);
})();
var NbUser = class {
  constructor(id, email, password, rememberMe, terms, confirmPassword, fullName) {
    this.id = id;
    this.email = email;
    this.password = password;
    this.rememberMe = rememberMe;
    this.terms = terms;
    this.confirmPassword = confirmPassword;
    this.fullName = fullName;
  }
};
export {
  NB_AUTH_FALLBACK_TOKEN,
  NB_AUTH_INTERCEPTOR_HEADER,
  NB_AUTH_OPTIONS,
  NB_AUTH_STRATEGIES,
  NB_AUTH_TOKENS,
  NB_AUTH_TOKEN_INTERCEPTOR_FILTER,
  NB_AUTH_USER_OPTIONS,
  NbAuthBlockComponent,
  NbAuthComponent,
  NbAuthEmptyTokenError,
  NbAuthIllegalJWTTokenError,
  NbAuthIllegalTokenError,
  NbAuthJWTInterceptor,
  NbAuthJWTToken,
  NbAuthModule,
  NbAuthOAuth2JWTToken,
  NbAuthOAuth2Token,
  NbAuthResult,
  NbAuthService,
  NbAuthSimpleInterceptor,
  NbAuthSimpleToken,
  NbAuthStrategy,
  NbAuthStrategyOptions,
  NbAuthToken,
  NbAuthTokenNotFoundError,
  NbAuthTokenParceler,
  NbDummyAuthStrategy,
  NbDummyAuthStrategyOptions,
  NbLoginComponent,
  NbLogoutComponent,
  NbOAuth2AuthStrategy,
  NbOAuth2AuthStrategyOptions,
  NbOAuth2ClientAuthMethod,
  NbOAuth2GrantType,
  NbOAuth2ResponseType,
  NbPasswordAuthStrategy,
  NbPasswordAuthStrategyOptions,
  NbRegisterComponent,
  NbRequestPasswordComponent,
  NbResetPasswordComponent,
  NbTokenLocalStorage,
  NbTokenService,
  NbTokenStorage,
  NbUser,
  auth2StrategyOptions,
  b64DecodeUnicode,
  b64decode,
  decodeJwtPayload,
  deepExtend,
  defaultAuthOptions,
  dummyStrategyOptions,
  getDeepFromObject,
  nbAuthCreateToken,
  nbNoOpInterceptorFilter,
  nbOptionsFactory,
  nbStrategiesFactory,
  nbTokensFactory,
  passwordStrategyOptions,
  routes,
  urlBase64Decode
};
/*! Bundled license information:

@nebular/auth/fesm2022/nebular-auth.mjs:
  (**
   * @license
   * Copyright Akveo. All Rights Reserved.
   * Licensed under the MIT License. See License.txt in the project root for license information.
   *)
*/
//# sourceMappingURL=@nebular_auth.js.map
