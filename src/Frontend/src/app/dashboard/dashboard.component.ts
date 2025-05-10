import { ChangeDetectionStrategy, Component, ChangeDetectorRef } from '@angular/core';
import { NbMenuItem, NbThemeService } from '@nebular/theme';
import { DashboardService } from './services/dashboard.service';
import { RouteModel } from './Models/route.model';
import { AppConfigurationService } from './services/app-config.service';
import { RouteService } from './services/route.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})

export class DashboardComponent {
  selectedTheme: string;
  isMobileView: boolean;
  themes: string[] = [
    'Default',
    'Evening',
    'Cosmic',
    'Corporate',
    'Midnight',
    'Aquamarine'
  ];
  displayName: string = '';
  displayRoles: string = '';

  menuOptions: NbMenuItem[] = [];

  constructor(
    chageDetector: ChangeDetectorRef,
    routeService: RouteService,
    private themeService: NbThemeService,
    appConfigService: AppConfigurationService,
  ) {
    this.displayName = appConfigService.UserModelData.userName;

    // If no theme has been cached, select default theme
    let preselectedTheme: string | null = localStorage.getItem('theme');
    this.selectedTheme = this.themes.find(x => x.toLowerCase() == preselectedTheme)? this.themes.find(x => x.toLowerCase() == preselectedTheme)! : this.themes[0];
    this.setTheme(this.selectedTheme);

    routeService.getMenuRoute()
      .subscribe((menuList: Array<RouteModel>) => {
        menuList = menuList.sort(x => x.sequence);
        let rootElements: { [key: number]: NbMenuItem } = {};

        menuList.forEach((menuItem: RouteModel) => {
          if (menuItem.parentRouteId == null || menuItem.parentRouteId == undefined) {
            rootElements[menuItem.routeId] = {
              title: menuItem.routeName,
              icon: menuItem.description,
              link: menuItem.routeValue
            };
          }
        });

        let childMenuList = menuList.filter((menuItem: any) => menuItem.parentRouteId != null && menuItem.parentRouteId != undefined);

        let itemsToRemove: number[] = [];
        childMenuList.forEach((menuItem: RouteModel) => {
          if (menuItem.parentRouteId != null && rootElements.hasOwnProperty(menuItem.parentRouteId)) {
            if (!rootElements[menuItem.parentRouteId].children)
              rootElements[menuItem.parentRouteId].children = [];

            rootElements[menuItem.parentRouteId].children?.push({
              title: menuItem.routeName,
              icon: menuItem.description,
              link: menuItem.routeValue
            });

            itemsToRemove.push(menuItem.routeId);
          }
        });

        menuList = [];

        menuList.forEach((menuItem: any) => {
          rootElements[menuItem.routeId] = {
            title: menuItem.routeName,
            icon: menuItem.description,
            link: menuItem.routeValue
          };
        });

        this.menuOptions = Object.values(rootElements);
        chageDetector.detectChanges();
      });

    this.isMobileView = false;
  }

  toggle(): void {
    let tempSide = document.getElementById('SideBar');
    let overlay = document.getElementById('Overlay');
    let sidebarElement: HTMLElement;
    if (tempSide == null || overlay == null)
      return;

    sidebarElement = tempSide;

    if (sidebarElement.classList.contains('expanded')) {
      sidebarElement.classList.remove('expanded');
      sidebarElement.classList.add('collapsed');
      overlay.classList.add('d-none');
      overlay.classList.remove('d-block');
    } else {
      sidebarElement.classList.remove('collapsed');
      sidebarElement.classList.add('expanded');
      overlay.classList.add('d-block');
      overlay.classList.remove('d-none');
    }
  }

  setTheme(theme: string): void {
    if (theme == 'Evening') {
      this.themeService.changeTheme('dark');
      localStorage.setItem('theme', 'dark');
    }
    else {
      this.themeService.changeTheme(theme.toLowerCase());
      localStorage.setItem('theme', theme.toLowerCase());
    }
  }

  flipProfilePanelVisibility(): void { this.isMobileView = !this.isMobileView; }

  logout(): void {
    localStorage.removeItem('auth_app_token');
    window.location.reload();
  }
}
